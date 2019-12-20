GO

CREATE FUNCTION Media6Meses(@isin VARCHAR(12))
RETURNS MONEY
AS 
BEGIN
	
	DECLARE @media MONEY
	DECLARE @valor MONEY

	DECLARE @today DATE
	DECLARE @days INT


	SET @days = 0
	SET @media = 0
	SET @today = convert(DATE, GETDATE())

	DECLARE @cursor CURSOR
	
	SET @cursor = CURSOR FOR
		SELECT R.ValorFecho FROM Registo AS R
		WHERE R.ISIN = @isin AND R.Dia BETWEEN CONVERT(DATE, DATEADD(month, -6, @today)) AND @today

	OPEN @cursor
	FETCH NEXT FROM @cursor INTO @valor

	WHILE @@FETCH_STATUS=0
		BEGIN 
			SET @days = @days + 1
			SET @media = @media + @valor
			FETCH NEXT FROM @cursor INTO @valor
		END

	CLOSE @cursor
	DEALLOCATE @cursor

	SET @media = @media / @days
	RETURN @media
END

GO

CREATE FUNCTION listar_portfolio (@nomeP varchar(255))
RETURNS @portfolio TABLE (
        isdn VARCHAR(12),
        quantidade FLOAT,
        ValorAtual MONEY,
        PercentagemVariacao MONEY
    )
AS
BEGIN
	DECLARE @cc VARCHAR(12)
	DECLARE @valorPortfolioAtual MONEY
	DECLARE @valorPortfolioPorCalcular MONEY
	DECLARE @quantidade FLOAT
	DECLARE @isin VARCHAR(12)
	DECLARE @valorFecho MONEY
	DECLARE @percentagemVar MONEY
	DECLARE @ultimoDia DATE

	SELECT @valorPortfolioAtual = C.ValorTotalPortfolio, @cc = C.CC FROM Cliente AS C WHERE C.NomePortfolio = @nomeP

	SET @valorPortfolioPorCalcular = 0

	DECLARE @cursor CURSOR
	SET @cursor = CURSOR FOR
		SELECT P.ISIN, P.Quantidade FROM Posicao AS P WHERE P.CC = @cc


	OPEN @cursor
	FETCH NEXT FROM @cursor INTO @isin, @quantidade

	WHILE @@FETCH_STATUS=0
		BEGIN 
			-- último dia útil
			SELECT TOP 1 @ultimoDia = R.Dia FROM Registo AS R 
			WHERE R.ISIN = @isin
			ORDER BY R.Dia DESC

			SELECT TOP 1 @valorFecho = R.ValorFecho FROM Registo AS R 
			WHERE R.ISIN = @isin AND R.Dia != @ultimoDia
			ORDER BY R.Dia DESC


			SET @valorPortfolioPorCalcular = @valorPortfolioPorCalcular + (@valorFecho * @quantidade)

			FETCH NEXT FROM @cursor INTO @isin, @quantidade
		END

		SET @percentagemVar = ( ( @valorPortfolioAtual - @valorPortfolioPorCalcular ) / ( ( @valorPortfolioAtual + @valorPortfolioPorCalcular ) / 2 ) * 100)
	CLOSE @cursor
	DEALLOCATE @cursor

	INSERT INTO @portfolio VALUES( @isin, @quantidade, @valorPortfolioAtual, @percentagemVar) 

	RETURN
END