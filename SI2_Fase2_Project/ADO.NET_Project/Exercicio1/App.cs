using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercicio1
{
    public class App
    {
        private static readonly string cs = ConfigurationManager.ConnectionStrings["TL51N_11"].ConnectionString;

        public static void Main(string[] args)
        {
            bool quit = false;
            while (!quit)
            {
                switch (Menu())
                {
                    case ConsoleKey.F:
                        Console.WriteLine("\nInserção de um Triplo!");
                        Console.Write("Inserir Instrumento existente: "); string isin1 = Console.ReadLine();
                        Console.Write("Inserir Valor: "); decimal valor = Int32.Parse(Console.ReadLine());
                        Console.Write("Inserir Data (yyyy-MM-dd HH:mm): "); string dtS = Console.ReadLine();
                        ExerciciosADO.ExercicioF(cs, isin1, valor, DateTimeInfo(dtS)); // "FR0004548873"
                        break;
                    case ConsoleKey.G:
                        Console.Write("\nInserir Instrumento existente: ");
                        string isin2 = Console.ReadLine();
                        ExerciciosADO.ExercicioG(cs, isin2); // "FR0004548873"
                        break;
                    case ConsoleKey.H:
                        Console.Write("\nInserir Instrumento existente: "); string isin3 = Console.ReadLine();
                        Console.Write("Inserir Valor: "); decimal valor2 = Int32.Parse(Console.ReadLine());
                        Console.Write("Inserir Data (yyyy-MM-dd HH:mm): "); string dtS2 = Console.ReadLine();
                        ExerciciosADO.ExercicioH(cs, isin3, DateTimeInfo(dtS2), valor2); // "FR0004548873"
                        break;
                    case ConsoleKey.I:
                        Console.WriteLine("\nInsira os seguintes requisitos:");
                        Console.Write("Nome do Portfolio: "); string port1 = Console.ReadLine();
                        Console.Write("Cartão Cidadão: "); string cc = Console.ReadLine();
                        Console.Write("NIF: "); string nif = Console.ReadLine();
                        Console.Write("Nome Cliente: "); string nCliente = Console.ReadLine();
                        ExerciciosADO.ExercicioI(port1, cc, nif, nCliente); //"Description7"
                        break;
                    case ConsoleKey.J:
                        Console.WriteLine("\nInsira os seguintes requisitos:");
                        Console.Write("ISIN: "); string isin5 = Console.ReadLine(); //"FR0004548873"
                        Console.Write("CC: "); string cc2 = Console.ReadLine(); //"125236521474"
                        Console.Write("Quantidade de instrumentos a somar: "); int quantidade = Int32.Parse(Console.ReadLine()); //200
                        ExerciciosADO.ExercicioJ(cc2, isin5, quantidade);
                        break;
                    case ConsoleKey.K:
                        Console.Write("\nNome do Portfolio: ");
                        string port2 = Console.ReadLine(); //"Description7"
                        ExerciciosADO.ExercicioK(cs, port2);
                        break;
                    case ConsoleKey.Escape: quit = true; break;
                    default: break;
                }
            }
        }
        private static ConsoleKey Menu()
        {
            Console.WriteLine("\n\nIndicar Exercicio: ");
            Console.WriteLine(
                "\tF: Aceder ao procedimento p_actualizaValorDiario\n\t" +
                "G: Calcular a media a 6 meses de um instrumento\n\t" +
                "H: Actualizar dados instrumento\n\t" +
                "I: Criar um portefolio\n\t" +
                "J: Actualizar o valor total do portfolio\n\t" +
                "K: Aceder a funcao para produzir a listagem de um portefolio\n" +
                "Esc: Sair"
            );
            return Console.ReadKey().Key;
        }

        private static DateTime DateTimeInfo(string stData)
        {
            DateTime data = new DateTime();
            try
            {
                data = DateTime.ParseExact(stData, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new FormatException("Erro! Tipo de data invalido");
            }
            return data;
        }
    }
}
