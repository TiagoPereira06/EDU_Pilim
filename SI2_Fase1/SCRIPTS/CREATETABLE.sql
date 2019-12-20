CREATE TABLE Mercado_Financeiro (
	Codigo VARCHAR(12),
	Nome VARCHAR(50),
	Descricao VARCHAR(255),
	PRIMARY KEY (Codigo)
);

CREATE TABLE Valores_Mercado (
	Dia DATE,
	Codigo VARCHAR(12),
	ValorIndice MONEY default 0, --
	ValorAbertura MONEY default 0, --
	VariacaoDiaria MONEY default 0, --
	FOREIGN KEY (Codigo) REFERENCES Mercado_Financeiro(Codigo),  --delete/update
	PRIMARY KEY(Dia, Codigo)
);

CREATE TABLE Instrumento_Financeiro(
	ISIN VARCHAR(12),
	CodigoMercado VARCHAR(12),
	Descricao VARCHAR(255),
	ValorAtual MONEY default 0,
	ValorVariacaoDiaria MONEY default 0, 
	PercentagemVariacaoDiaria MONEY default 0,
	ValorVariacao6Meses MONEY default 0,
	PercentagemVariacao6Meses DECIMAL(3,2) default 0,
	Media6Meses MONEY default 0,
	FOREIGN KEY (CodigoMercado) REFERENCES Mercado_Financeiro(Codigo),  --delete/update
	PRIMARY KEY (ISIN)
);

CREATE TABLE Registo(
	ISIN VARCHAR(12),
	Dia DATE,
	ValorAbertura MONEY,
	ValorFecho MONEY,
	ValorMaximo MONEY,
	ValorMinimo MONEY,
	HoraFecho DATETIME,
	FOREIGN KEY (ISIN) REFERENCES Instrumento_Financeiro(ISIN),  --delete/update
	PRIMARY KEY (ISIN, Dia)
);

CREATE TABLE Cliente(
	CC VARCHAR(12),
	NIF VARCHAR(9) UNIQUE NOT NULL,
	NomeCliente VARCHAR(255),
	NomePortfolio VARCHAR(255) UNIQUE NOT NULL,
	ValorTotalPortfolio MONEY default 0,
	PRIMARY KEY (CC)
);

CREATE TABLE Contacto(
	Codigo VARCHAR(12),
	CC VARCHAR(12),
	Descricao VARCHAR(255),
	FOREIGN KEY (CC) REFERENCES Cliente(CC), 
	PRIMARY KEY (Codigo)
);

CREATE TABLE Contacto_Telefonico(
	Numero NUMERIC(9),
	Indicativo NUMERIC(3),
	Codigo_Contacto VARCHAR(12),
	FOREIGN KEY (Codigo_Contacto) REFERENCES Contacto(Codigo),  --delete/update
	PRIMARY KEY (Numero)
)

CREATE TABLE Contacto_Email(
	Endereco VARCHAR(40),
	Codigo_Contacto VARCHAR(12),
	FOREIGN KEY (Codigo_Contacto) REFERENCES Contacto(Codigo),  --delete/update
	PRIMARY KEY (Endereco)
)

CREATE TABLE Posicao(
	ISIN VARCHAR(12), -- update?
	CC VARCHAR(12), -- update?
	Quantidade FLOAT,
	FOREIGN KEY (ISIN) REFERENCES Instrumento_Financeiro(ISIN), --delete/update
	FOREIGN KEY (CC) REFERENCES Cliente(CC),
	PRIMARY KEY(ISIN,CC)
);

CREATE TABLE Triplos(
	Identificacao VARCHAR(12), --ISIN 
	Dia DATETIME,
	Valor MONEY,
	Observado BIT default 0,
	PRIMARY KEY(Identificacao, Dia)
);



-- TRIGGERS



GO

CREATE TRIGGER updatePortfolioOnInsertPosicao ON Posicao 
FOR INSERT
AS
BEGIN 
	DECLARE @id VARCHAR(12) 
	DECLARE @cc VARCHAR(12)
	DECLARE @quantidadeInserida BIGINT
	DECLARE @precoInstrumento MONEY
	DECLARE @valorTotal MONEY
	
	SELECT @quantidadeInserida = inserted.Quantidade, @cc = inserted.CC, @id = inserted.ISIN FROM inserted
	SELECT @precoInstrumento = ValorAtual FROM Instrumento_Financeiro WHERE ISIN = @id

	SELECT @valorTotal = ValorTotalPortfolio FROM Cliente AS C WHERE C.CC = @cc
	
	SET @valorTotal = @valorTotal + ( @quantidadeInserida * @precoInstrumento )

	UPDATE Cliente SET ValorTotalPortfolio = @valorTotal WHERE CC = @cc
END

GO

CREATE TRIGGER updatePortfolioOnUpdatePosicao ON Posicao 
FOR UPDATE
AS
BEGIN 
	DECLARE @id VARCHAR(12) 
	DECLARE @cc VARCHAR(12)
	DECLARE @quantidadeAnterior BIGINT
	DECLARE @precoInstrumento MONEY
	DECLARE @valorTotal MONEY
	DECLARE @quantidadeNova BIGINT
	
	SELECT @quantidadeAnterior = deleted.Quantidade, @cc = deleted.CC, @id = deleted.ISIN FROM deleted

	SELECT @precoInstrumento = ValorAtual FROM Instrumento_Financeiro WHERE ISIN = @id
	SELECT @valorTotal = ValorTotalPortfolio FROM Cliente AS C WHERE C.CC = @cc
	
	SELECT @quantidadeNova = inserted.Quantidade FROM inserted

	SET @valorTotal = @valorTotal + ( (@quantidadeNova - @quantidadeAnterior) * @precoInstrumento)

	UPDATE Cliente SET ValorTotalPortfolio = @valorTotal WHERE CC = @cc
END

GO

CREATE TRIGGER updatePortfolioOnDeletePosicao ON Posicao 
FOR DELETE
AS
BEGIN 
	DECLARE @id VARCHAR(12) 
	DECLARE @cc VARCHAR(12)
	DECLARE @quantidadeAnterior BIGINT
	DECLARE @precoInstrumento MONEY
	DECLARE @valorTotal MONEY
	
	SELECT @quantidadeAnterior = deleted.Quantidade, @cc = deleted.CC, @id = deleted.ISIN FROM deleted

	SELECT @precoInstrumento = ValorAtual FROM Instrumento_Financeiro WHERE ISIN = @id
	SELECT @valorTotal = ValorTotalPortfolio FROM Cliente AS C WHERE C.CC = @cc

	SET @valorTotal = @valorTotal - ( @quantidadeAnterior * @precoInstrumento)

	UPDATE Cliente SET ValorTotalPortfolio = @valorTotal WHERE CC = @cc
END

GO

CREATE TRIGGER updatePortfolioOnUpdateInstrumento ON Instrumento_Financeiro 
FOR UPDATE
AS
BEGIN 
	DECLARE @id VARCHAR(12)
	DECLARE @precoAnterior MONEY
	DECLARE @precoNovo MONEY
	DECLARE @valorTotal MONEY
	DECLARE @quantidade BIGINT

	SELECT @id = deleted.ISIN, @precoAnterior = deleted.ValorAtual FROM deleted
	SELECT @precoNovo = inserted.ValorAtual FROM inserted

	DECLARE @cc VARCHAR(12)

	DECLARE @cursor CURSOR

	SET @cursor = CURSOR FOR 
		SELECT P.CC, P.Quantidade FROM Posicao AS P
		WHERE P.ISIN = @id

	OPEN @cursor
	FETCH NEXT FROM @cursor INTO @cc, @quantidade

	WHILE @@FETCH_STATUS = 0
		BEGIN 

			SELECT @valorTotal = C.ValorTotalPortfolio FROM CLIENTE AS C WHERE C.CC = @cc 

			SET @valorTotal = @valorTotal - (@precoAnterior * @quantidade) + (@precoNovo * @quantidade)

			UPDATE Cliente SET ValorTotalPortfolio = @valorTotal WHERE CC = @cc

		FETCH NEXT FROM @cursor INTO @cc, @quantidade
		END
		
	CLOSE @cursor
	DEALLOCATE @cursor

END

GO

-- PERCENTAGE VARIANCE
-- (|original - new|) / ( (original + new) / 2 )

-- VARIANCE
-- (new-original)


CREATE TRIGGER updateMercadoOnInsertRegisto -- É APENAS NECESSÁRIO NAS INSERÇÕES VISTO QUE O VALOR DE ABERTURA DE UM INSTRUMENTO NÃO É ATUALIZADO
ON Registo
FOR INSERT 
AS
BEGIN	

	DECLARE @isin VARCHAR(12)
	DECLARE @dia DATE
	DECLARE @codigoMercado VARCHAR(12)
	DECLARE @valorAberturaMercado MONEY
	DECLARE @variacaoDiaria MONEY
	DECLARE @valorIndice MONEY

	SELECT @isin = inserted.ISIN, @dia = inserted.Dia, @valorIndice = inserted.ValorAbertura FROM inserted
	
	SELECT @codigoMercado = CodigoMercado FROM Instrumento_Financeiro AS I WHERE I.ISIN = @isin
	
	SELECT * INTO #valoresMercado FROM Valores_Mercado WHERE Dia = @dia AND Codigo = @codigoMercado 

	IF NOT EXISTS(SELECT * FROM #valoresMercado) -- não existe valores de mercado ainda
		BEGIN
		
			SET @valorAberturaMercado = 0 -- caso não exista dia anterior o valor de Abertura é 0
			SELECT TOP 1 @valorAberturaMercado = ValorIndice FROM Valores_Mercado ORDER BY Dia DESC -- busca do último dia útil

			 SET @variacaoDiaria = (@valorIndice - @valorAberturaMercado)

			INSERT INTO Valores_Mercado (Dia, Codigo, ValorIndice, ValorAbertura, VariacaoDiaria) 
			VALUES (@dia, @codigoMercado, @valorIndice, @valorAberturaMercado, @variacaoDiaria)

		END
	ELSE -- já existem valores de mercado 
		BEGIN
			UPDATE Valores_Mercado SET ValorIndice = ValorIndice + @valorIndice WHERE Dia = @dia AND Codigo = @codigoMercado
			UPDATE Valores_Mercado SET VariacaoDiaria = ValorIndice - ValorAbertura WHERE Dia = @dia AND Codigo = @codigoMercado
		END

	DROP TABLE #valoresMercado

END

GO

CREATE TRIGGER updateInstrumentoOnInsertRegisto on Registo
FOR INSERT,UPDATE
AS
BEGIN
	DECLARE @VALOR_ACTUAL MONEY
	DECLARE @ISIN VARCHAR(12)
	DECLARE @VALOR_ANTERIOR MONEY
	DECLARE @VALOR_PERCENTAGEM MONEY
	DECLARE @VALOR_VARIACAO MONEY

	SELECT @VALOR_ACTUAL = ValorFecho, @ISIN=ISIN FROM INSERTED --NEW VALUES
	
	IF EXISTS (SELECT* FROM DELETED) --É UM UPDATE
		BEGIN
			SELECT @VALOR_ANTERIOR = ValorAtual FROM Instrumento_Financeiro AS R WHERE R.ISIN = @ISIN
			
			SET @VALOR_VARIACAO = @VALOR_ACTUAL - @VALOR_ANTERIOR
			
			SET @VALOR_PERCENTAGEM = (((@VALOR_ACTUAL-@VALOR_ANTERIOR)/((@VALOR_ACTUAL+@VALOR_ANTERIOR)/2))*100) 

			UPDATE Instrumento_Financeiro SET ValorAtual = @VALOR_ACTUAL, ValorVariacaoDiaria = @VALOR_VARIACAO, PercentagemVariacaoDiaria = @VALOR_PERCENTAGEM WHERE ISIN = @ISIN
		END
	
	ELSE --É UM INSERT
		BEGIN
			UPDATE Instrumento_Financeiro SET ValorAtual = @VALOR_ACTUAL WHERE ISIN=@ISIN
		END
END

