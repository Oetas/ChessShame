using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class FiguresMovements
    {
        public Figures figure { get; private set; } // фигура 
        public Squares from { get; private set; } // клетка откуда двигается фигура
        public Squares to { get; private set; } // куда двигается фигура
        public Figures transformation { get; private set; } // превращение пешки в другую фигуру

        public FiguresMovements(FigureOnSquare fos, Squares to, Figures transformation = Figures.zero) // информация о фигуре на клетке
        {
            this.figure = fos.figure; // исходная фигура
            this.from = fos.square; // стартовая позиция
            this.to = to; // позиция после сделанного хода
            this.transformation = transformation; // превращение для пешки
        }

        public FiguresMovements (string move) // превращение пешки в другую фигуру
        {
            this.figure = (Figures)move[0]; // фигура до превращения
            this.from = new Squares(move.Substring(1, 2)); // откуда фигура пошла

            this.to = new Squares(move.Substring(3, 2)); // куда сходила фигура
            this.transformation = (move.Length == 6) ? (Figures)move[5] : Figures.zero; // если длина строки равна 6 фигура превращается, если нет, обнуляем значение 
        }

        public int DeltaX { get { return to.x - from.x; } } // переменные для вычисления координат
        public int DeltaY { get { return to.y - from.y; } }

        public int AbsDeltaX { get { return Math.Abs(DeltaX); } } // переменные для вычисления модуля дельты X и Y
        public int AbsDeltaY { get { return Math.Abs(DeltaY); } }

        public int SignX { get { return Math.Sign(DeltaX); } } // переменыые для вычисления знака
        public int SignY { get { return Math.Sign(DeltaY); } }

        public override string ToString() // формируем ходы в виде строки
        {
            string text = (char)figure + from.SqName + to.SqName; // возвращаем символ и координату откуда совершается ход

            if (transformation != Figures.zero)
            {
                text += (char)transformation; // добаввляем фигуру, в которую превращались.
            }
            return text;
        }
    }
}
