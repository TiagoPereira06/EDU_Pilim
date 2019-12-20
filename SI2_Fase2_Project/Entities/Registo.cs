using System;

namespace Entities
{
    public class Registo
    {
        public string Isin { get { return new Instrumento().Isin; } }
        public DateTime Dia { get; set; }
        public decimal ValorAbertura { get; set; }
        public decimal ValorFecho { get; set; }
        public decimal ValorMaximo { get; set; }
        public decimal ValorMinimo { get; set; }
        public DateTime HoraFecho { get; set; }
    }
}
