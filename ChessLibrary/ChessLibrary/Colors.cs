using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    enum Colors
    {
        white,
        black,
        zero
    }

   static class ColorChange // изменяем цвет после хода одной стороны
    {
        public static Colors FlipColor (this Colors color)
        {
            if (color == Colors.white) // смена цвета с первоначального
                return Colors.black;
            if (color == Colors.black)
                return Colors.white;
            return Colors.zero;
        }
    }
}
