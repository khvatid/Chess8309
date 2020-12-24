using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLib
{
    struct Square
    {
        public static Square none = new Square(-1, -1);
        public int x { get;private set;}
        public int y { get; private set; }
        public Square (int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Square (string position)
        {
            if( position.Length == 2 &&
                position[0]>= 'a' && position[0]<= 'h' &&
                position[1]>= '1' && position[1]<= '8' )
            {
                x = position[0] - 'a';
                y = position[1] - '1';
            }
            else
                this = none;
        }

        public bool OnBoard() => x >= 0 && x < 8 &&
                                 y >= 0 && y < 8;

        public string Name { get { return((char)('a' + x)).ToString() + (y + 1).ToString(); } }

        //Я ИЗ-ЗА ЭТИХ ДВУХ ПЕРЕПИСЫВАЛ ВСЮ БИБЛИОТЕКУ БЛЯТЬ.. ПАЦАНЫ
        //ЕСЛИ ВЫ ЭТО ЧИТАЕТЕ НИКОГДА НЕ ЭКОНОМЬТЕ ВРЕМЯ НА ЭТОМ. ЛУЧШЕ СРАЗУ НАПИСАТЬ ПО ТЕОРЕМЕ ДЕ МОРГАНА и не парить голову
        public static bool operator ==(Square a, Square b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Square a, Square b) => a.x != b.x || a.y != b.y;

        public static IEnumerable<Square> YieldSquare()
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    yield return new Square(x,y);
            
        }
    }
}
