using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLib
{
    enum Color
    {
        none,
        white,
        black
    }
    static class ColorMethod
    {
        public static Color FlipColor(this Color color)
        {
            return 
                color == Color.none ?
                Color.none :
                color == Color.black ?
                Color.white:Color.black ;
        }
    }
}
