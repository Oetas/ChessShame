using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    enum Figures // список всех фигур
    {
        zero,

        WKing = 'K',
        WQeen = 'Q',
        WKnight = 'N',     
        WBishop = 'B',
        WRook = 'R',      
        WPawn = 'P',
       

        BKing = 'k',
        BQeen = 'q',
        BKnight = 'n',
        BBishop = 'b',
        BRook = 'r',
        BPawn = 'p'
    }

    static class FiguresMethods // описываем особенности фигур
    {
        public static Colors GetColor(this Figures figure) // функция возвращающая цвет фигурам
        {
            if (figure == Figures.zero) 
            {
                return Colors.zero;
            }
            return (figure == Figures.WKing || figure == Figures.WQeen || figure == Figures.WKnight || figure == Figures.WBishop || figure == Figures.WRook || figure == Figures.WPawn) ? Colors.white : Colors.black; // если фигуры относятся к белым, возвращаем белый цвет и наоборот

        }
    }
}
