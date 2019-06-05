using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Movements
    {
        FiguresMovements figm;
        Board board;

        public Movements(Board board)
        {
            this.board = board;
        }

        public bool MovementIsPos(FiguresMovements figm) // проверяем возможно ли движение для фигуры
        {
            this.figm = figm; // получаем и сохраняем ход
            return
                MovementPosFrom() && // проверяем возможность пойти с той клетки, на которой находимся
                MovementPosTo() && // проверяем возможность пойти на ту клетку, куда собираемся 
                MovementPosForFigure(); // проверяем возможность фигуры сделать ход
        }

        bool MovementPosFrom()
        {
            return figm.from.PosOnBoard() && figm.figure.GetColor() == board.ChangeColor; // проверяем нахождение клетки на доске и имеет ли фигура нужный цвет
        }

        bool MovementPosTo()
        {
            return figm.to.PosOnBoard() && figm.from != figm.to && board.GetFigureOn(figm.to).GetColor() != board.ChangeColor; // проверка нахождения клетки на доске и соответственного для фигуры цвета
        }

        bool MovementPosForFigure() // проверяем возможно ли движение для конкретной фигуры
        {
            switch (figm.figure) // получаем фигуру и вызываем метод проверки возможности хода
            {
                case Figures.WKing:
                case Figures.BKing:
                  return MovementPosForKing();

                case Figures.WQeen:                
                case Figures.BQeen:
                    return MovementStraightPos();

                case Figures.WKnight:
                case Figures.BKnight:
                    return MovementPosForKnight();

                case Figures.WBishop:
                case Figures.BBishop:
                    return (figm.SignX != 0 && figm.SignY != 0) && MovementStraightPos(); // прописываем движения для слона, двигается строго по диагонали

                case Figures.WRook:
                case Figures.BRook:
                    return (figm.SignX == 0 || figm.SignY == 0) && MovementStraightPos(); // прописываем движение для ладьи, двигается строго по вертикали или горизонтали

                case Figures.WPawn:
                case Figures.BPawn:
                    return MovementPosForPawn();


                default: return false;
            }        
        }

        private bool MovementPosForKing() 
        {
           if (figm.AbsDeltaX <= 1 && figm.AbsDeltaY <= 1) // вычисляем разницу координат по X и Y
            {
                return true;
            }
            return false;
        }

        private bool MovementStraightPos() // возможность двигаться прямо 
        {
            Squares on = figm.from; // исходная клетка откуда начинается движение
            do
            {
                on = new Squares(on.x + figm.SignX, on.y + figm.SignY); // создаем новую клетку со смещением на 1 

                if (on == figm.to) // если пришли на нужную клетку 
                {
                    return true;
                }
            }
            while (on.PosOnBoard() && board.GetFigureOn(on) == Figures.zero); // проверка на местонохождение клетки на доске и взятие фигуры

            return false;
        }
        

        private bool MovementPosForKnight()
        {
            if (figm.AbsDeltaX == 2 && figm.AbsDeltaY == 1)
            {
                return true;
            }
            else
                  if (figm.AbsDeltaX == 1 && figm.AbsDeltaY == 2)
            {
                return true;
            }
            return false;
        }

      bool MovementPosForPawn() // проверка может ли ходить пешка
        {
            if (figm.from.y < 1 || figm.from.y > 6) // проверяем находиться ли пешка там где она может находиться
            {
                return false;
            }

            int MoveY = figm.figure.GetColor() == Colors.white ? 1 : -1; // перемещение пешки вверх или вниз в зависимости от цвета

            return PawnGoPos(MoveY) || PawnGoDPos(MoveY) || PawnEatPos(MoveY); 
        }

        private bool PawnGoPos(int MoveY) // обычный ход пешки
        {
            if (board.GetFigureOn(figm.to) == Figures.zero) // проверка клетки на пустую, на которую собираемся сходить
                if (figm.DeltaX == 0) // пешка идёт прямо
                    if (figm.DeltaY == MoveY) // пешка идёт в верном направлении на 1 шаг
                        return true;
            return false;
        }

        private bool PawnGoDPos(int MoveY)
        {
            if (board.GetFigureOn(figm.to) == Figures.zero)
                if (figm.DeltaX == 0)
                    if (figm.DeltaY == 2 * MoveY) // смещение пешки на две клетки
                        if (figm.from.y == 1 || figm.from.y == 6) // проверяем ходит ли пешка с единицы для белых или шести для черных
                            if (board.GetFigureOn(new Squares(figm.from.x, figm.from.y + MoveY)) == Figures.zero) // проверяем не перепрыгивает ли пешка фигуру
                                return true;
            return false;      
        }


        private bool PawnEatPos(int MoveY)
        {
            if (board.GetFigureOn(figm.to) != Figures.zero) // проверяем ходит ли пешка не на пустую клетку
                if (figm.AbsDeltaX == 1) // проверяем сместилась ли она по X на 1
                    if (figm.DeltaY == MoveY) // проверяем идёт ли пешка в верном направлении
                        return true;           
            return false;
        }

    }
}
