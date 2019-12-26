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
	DECLARE @totalQuantidade FLOAT
	DECLARE @quantidade FLOAT
	DECLARE @isin VARCHAR(12)
	DECLARE @valorFecho MONEY
	DECLARE @percentagemVar MONEY
	DECLARE @ultimoDia DATE

	SELECT @valorPortfolioAtual = C.ValorTotalPortfolio, @cc = C.CC FROM Cliente AS C WHERE C.NomePortfolio = @nomeP

	SET @totalQuantidade = 0
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
			SET @totalQuantidade = @totalQuantidade + @quantidade
			FETCH NEXT FROM @cursor INTO @isin, @quantidade
		END

		SET @percentagemVar = ( ( @valorPortfolioAtual - @valorPortfolioPorCalcular ) / ( ( @valorPortfolioAtual + @valorPortfolioPorCalcular ) / 2 ) * 100)
	CLOSE @cursor
	DEALLOCATE @cursor

	INSERT INTO @portfolio VALUES( @isin, @totalQuantidade, @valorPortfolioAtual, @percentagemVar) 

	RETURN
END