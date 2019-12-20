CREATE FUNCTION listar_portfolio (@NameP varchar(255))
RETURNS TABLE
AS
RETURN
	SELECT TB2.ISIN,TB2.ValorAtual,TB2.PercentagemVariacaoDiaria FROM 
	(SELECT Posicao.ISIN,Quantidade FROM Cliente
		INNER JOIN Posicao
			ON (Cliente.CC = Posicao.CC)
			INNER JOIN Registo
			ON(Posicao.ISIN = Registo.ISIN) 
	WHERE(Cliente.NomePortfolio = @NameP)) as TB1
	INNER JOIN (SELECT DISTINCT Instrumento_Financeiro.ISIN,Instrumento_Financeiro.ValorAtual,Instrumento_Financeiro.PercentagemVariacaoDiaria
				FROM Instrumento_Financeiro
				INNER JOIN Posicao
				ON Posicao.ISIN = Instrumento_Financeiro.ISIN) as TB2
	ON TB1.ISIN = TB2.ISIN 			
	




