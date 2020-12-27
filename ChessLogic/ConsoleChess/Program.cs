using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessLib;

namespace ConsoleChess
{
    class Program
    {
        static void Main(string[] args)
        {

            ChessClass chess = new ChessClass("r111k11r/1p111p11/8/8/8/8/1P1111P1/R111K11R w KQkq - 0 1");
            while(true)
            {
                Console.WriteLine(chess.fen);
                Console.WriteLine(ChessToAscii(chess));
                Console.WriteLine(chess.IsCheckAfter() ? "CHECK" :"---");
               // foreach (string moves in chess.GetAllMoves())
                   // Console.WriteLine(moves);
                Console.WriteLine();
                Console.Write("=> ");
                string move = Console.ReadLine();
                if (move == "") break;
                chess = chess.Move(move);
            }
        }
        static string ChessToAscii (ChessClass chess)
        {
            string text = "  +----------------+\n";
            for (int y = 7; y>=0;y--)
            {
                text += y + 1;
                text += " | ";
                for (int x = 0; x < 8; x++)
                    text += chess.GetFigureAt(x, y) + " ";
                text += "|\n";
            }
            text += "  +----------------+\n";
            text += "    a b c d e f g h\n";
            return text;
        }
    }
}
