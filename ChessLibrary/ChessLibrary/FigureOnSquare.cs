using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class FigureOnSquare
    {
        public Figures figure { get; private set; } // фигура
        public Squares square { get; private set; } // клетка на доске

        public FigureOnSquare (Figures figure, Squares square)
        {
            this.figure = figure;
            this.square = square;
        }
    }
}
