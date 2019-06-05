using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    public class Chess1
    {
        public string fenchess { get; private set; }
        Board board; // шахматная доска
        Movements movement;
        List<FiguresMovements> AllMovements; // список всех ходов
      

        public Chess1 (string fenchess = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            this.fenchess = fenchess;
            board = new Board(fenchess); // создаём шахматную доску по нотации FEN    
            movement = new Movements(board); // создаем движения фигур на доске
        }

        Chess1 (Board board)
        {
            this.board = board; // генерация новой доски
            this.fenchess = board.fenchess; // указывает новой фен, который сформировался после хода
            movement = new Movements(board); // создаем движения фигур на доске
        }

        public Chess1 Moving (string move)
        {
            FiguresMovements figm = new FiguresMovements(move);// генерация хода
            if (!movement.MovementIsPos(figm)) // проверяем возможно ли сделать ход
                return this; // если ход сделать нельзя - позиция не изменяется 

            if (board.IsCheckAftertM(figm)) // проверяем есть ли шах после хода
            {
                return this;
            }          
                 
            Board newBoard = board.Moving(figm); // создаём доску после выполнения хода
            Chess1 newChess = new Chess1(newBoard); // создание шахмат от новой доски

            return newChess;
        }

        public char GFigure (int x, int y) // вычисляем местоположение каждой фигуры
        {
            Squares square = new Squares(x, y); // берём клетку, которая была передана
            Figures fig = board.GetFigureOn(square); // вычисляем фигуру

            return fig == Figures.zero ? '.' : (char)fig; // если фигура существует, возвращаем символ, иначе обнуляем
        }

        void FindAllMovements() // метод для поиска всех ходов
        {
            AllMovements = new List<FiguresMovements>(); // создаем пустой список
            foreach (FigureOnSquare figs in board.SortFigures()) // перебираем все фигуры на клетках доски того цвета который делает ход
                foreach (Squares to in Squares.SortSquares()) // перебираем все клетки куда можно пойти
                {
                    FiguresMovements figm = new FiguresMovements(figs, to); 
                    if (movement.MovementIsPos(figm)) // проверяем может ли быть выполнен конкретный ход
                    {
                        if (!board.IsCheckAftertM(figm))
                        {
                            AllMovements.Add(figm);
                        }
                    }
                }
        }

        public List<string> GetAllMovements() // вовзвращаем все ходы в виде строки
        {
            FindAllMovements();
            List<string> nlist = new List<string>();

            foreach (FiguresMovements figm in AllMovements)
                nlist.Add(figm.ToString());

            return nlist;
        }

        public bool IsCheck() // проверяем есть ли шах
        {
            return board.IsCheck();
        }

    }
}
