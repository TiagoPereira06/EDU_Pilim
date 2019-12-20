
namespace Entities
{
    public class Instrumento
    {
        public string Isin { get; set; }
        public string CodigoMercado { get { return new Mercado().Codigo; } }
        public string Descricao { get; set; }
        public decimal ValorAtual { get; set; }
        public decimal ValorVariacaoDiaria { get; set; }
        public decimal PercentagemVariacaoDiaria { get; set; }
        public decimal ValorVariacao6Meses { get; set; }
        public decimal PercentagemVariacao6Meses { get; set; }
        public decimal Media6Meses { get; set; }
    }
}
