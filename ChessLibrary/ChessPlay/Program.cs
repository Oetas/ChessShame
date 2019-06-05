using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessLibrary;

namespace ChessPlay
{
    class Program
    {
        static void Main(string[] args)
        {
            Random nr = new Random();
            ChessLibrary.Chess1 chess = new ChessLibrary.Chess1();
            List<string> nlist; // список всех движений фигур
            while (true) // выводим шахматы
            {
               nlist = chess.GetAllMovements(); // получаем все возможные движения фигур
                Console.WriteLine(chess.fenchess);
                Console.WriteLine(ChessAscii(chess));
                Console.WriteLine(chess.IsCheck() ? "CHECK!!!" : ""); // проверяем есть ли шах

                foreach (string movements in nlist) // выводим все возможные ходы
                    Console.Write(movements + "\t");
                Console.WriteLine();
                Console.Write("> ");

                string move = Console.ReadLine();
                if (move == "q") break;
                if (move == "") move = nlist[nr.Next(nlist.Count)]; // создаем ход без ввода с клавиатуры
                chess = chess.Moving(move);
            }
        }

        static string ChessAscii (ChessLibrary.Chess1 chess)
        {
            string game = "  +-----------------+\n";
            for (int i = 7; i >= 0; i --)
            {
                game += i + 1;
                game += " | ";

                for (int j = 0; j < 8; j ++)
                    game += chess.GFigure(j, i) + " ";

                game += "|\n";
            }
            game += "  +-----------------+\n";
            game += "    a b c d e f g h\n";

            return game;
        }
    }
}
