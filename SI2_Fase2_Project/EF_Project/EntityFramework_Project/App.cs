using System;

namespace Si2_Fase2_EF
{
    class App
    {
        public static void Main(string[] args)
        {
            bool quit = false;
            while (!quit)
            {
                switch (Menu())
                {
                    case ConsoleKey.F:
                        Console.Write("\nInserir Instrumento: ");
                        string isin1 = Console.ReadLine();
                        Exercicios_EF.ExercicioF(isin1); // "FR0004548873"
                        break;
                    case ConsoleKey.G:
                        Console.Write("\nInserir Instrumento: ");
                        string isin2 = Console.ReadLine();
                        Exercicios_EF.ExercicioG(isin2); // "FR0004548873"
                        break;
                    case ConsoleKey.H:
                        Console.Write("\nInserir Instrumento: ");
                        string isin3 = Console.ReadLine();
                        Exercicios_EF.ExercicioH(isin3); // "FR0004548873"
                        break;
                    case ConsoleKey.I:
                        Console.Write("\nNome do Portfolio: ");
                        string port1 = Console.ReadLine();
                        Exercicios_EF.ExercicioI(port1); //"Description7"
                        break;
                    case ConsoleKey.J:
                        Console.WriteLine("\nInsira os seguintes requisitos:");
                        string isin4, cc; int val;
                        Console.Write("ISIN: "); isin4 = Console.ReadLine(); //"FR0004548873"
                        Console.Write("CC: "); cc = Console.ReadLine(); //"125236521474"
                        Console.Write("Valor: "); val = Int32.Parse(Console.ReadLine()); //200
                        Exercicios_EF.ExercicioJ(cc, isin4 , val);
                        break;
                    case ConsoleKey.K:
                        Console.Write("\nNome do Portfolio: ");
                        string port2 = Console.ReadLine(); //"Description7"
                        Exercicios_EF.ExercicioK(port2);
                        break;
                    case ConsoleKey.Escape: quit = true; break;
                    default: break;
                }
            }
        }
        private static ConsoleKey Menu()
        {
            Console.WriteLine("Indicar Exercicio: ");
            Console.WriteLine(
                "\tF: Aceder ao procedimento p_actualizaValorDiario\n\t" +
                "G: Calcular a media a 6 meses de um instrumento\n\t" +
                "H: Actualizar dados instrumento\n\t" +
                "I: Criar um portefolio\n\t" +
                "J: Actualizar o valor total do portfolio\n\t" +
                "K: Aceder a funcao para produzir a listagem de um portefolio\n\t" +
                "Esc: Sair"
            );
            return Console.ReadKey().Key;
        }
    }
}
