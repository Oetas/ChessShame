using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Board
    {
      public string fenchess { get; private set; }
      Figures[,] figures;
      public Colors ChangeColor { get; private set; } // цвет стороны, которая ходит
      public int MovingNumb { get; private set; } // номер хода
      
      public Board (string fenchess) // принимаем строку фен и создаём массив 8 на 8
      {
            this.fenchess = fenchess; 
            figures = new Figures[8, 8];
            Initial(); // вызываем метод     
      }
        
        void Initial () // метод инициализации расположения фигур     
        {
            

            string[] pieces = fenchess.Split(); //разделяем строку fenchess
            if (pieces.Length != 6) return; // проверяем на то, что она содержит 6 кусков
            InitializeFigures(pieces[0]); // инициализируем фигуры
            ChangeColor = (pieces[1] == "b") ? Colors.black : Colors.white; // смена цвета играющих сторон
            MovingNumb = int.Parse(pieces[5]); // парсинг пятого куска строки - значения хода
        } 

        void InitializeFigures (string data) // инициализация фигур
        {
            for (int j = 8; j >= 2; j --)
                data = data.Replace(j.ToString(), (j - 1).ToString() + "1"); // выводим строку, которая разделена слэшем и имеет восемь символов
            
            data = data.Replace("1", "."); // выводим точки на доске вместо единиц

            string[] lines = data.Split('/'); // разделяем строку слэшами
            for (int y = 7; y >= 0; y--) // располагаем фигуры на доске
                for (int x = 0; x < 8; x++)
                    figures[x, y] = lines[7-y][x] == '.' ? Figures.zero : (Figures)lines[7-y][x];                
        }
        
        public IEnumerable<FigureOnSquare> SortFigures() // перебираем фигуры
        {
            foreach (Squares square in Squares.SortSquares()) // перебираем все клетки
                if (GetFigureOn(square).GetColor() == ChangeColor) // если на клетке находитсья фигура цвета который ходит
                {
                    yield return new FigureOnSquare(GetFigureOn(square), square); // мы возвращаем её
                }
        }

        void GenFenChess () // генерация новой нотации фен для расположения фигур
        {
            fenchess = FFigures() + " " + (ChangeColor == Colors.white ? "w" : "b") + " - - 0 " + MovingNumb.ToString(); // меняем цвет играющей стороны после хода и увеличиваем кол-во сделанных ходов
        }

        string FFigures ()
        {
            StringBuilder nsb = new StringBuilder();  
            for (int y = 7; y >= 0; y--) // добавляем на доску фигуры
            {
                for (int x = 0; x < 8; x++)
                    nsb.Append(figures[x, y] == Figures.zero ? '1' : (char)figures[x, y]); // в случае, если фигуры существует на доске, ставим её на новую клетку

                if (y > 0)
                {
                    nsb.Append('/'); // если дошли до конца строчки, добавляем слэш для перехода на новую строку
                }            
            }

            string nline = "11111111"; 
            for (int j = 8; j >= 2; j--)
                nsb.Replace(nline.Substring(0, j), j.ToString()); // убираем единицы из строки fen, меняем количество единицы по итерации 
            return nsb.ToString();
        }
        
        public Figures GetFigureOn (Squares square) // определяем какая фигура на какой клетки находится
        {
            if (square.PosOnBoard()) // проверка присутствия клетки на доске
                return figures[square.x, square.y];
            
            return Figures.zero; // если клетка оказалась за доской
        }
        
        void SetFigureOn (Squares square, Figures figure) // метод для установки фигур
        {
            if (square.PosOnBoard())
            {
                figures[square.x, square.y] = figure;
            }
        }   
        
        public Board Moving (FiguresMovements figm) // создаём хода
        {
            Board next = new Board(fenchess); // создание доски
            next.SetFigureOn(figm.from, Figures.zero); // клетку, откуда фигура пошла, делаем пустой 
            next.SetFigureOn(figm.to, figm.transformation == Figures.zero ? figm.figure : figm.transformation); // в клетку, куда пошла фигура, добавляем фигуру с учётом превращения для пешки

            if (ChangeColor == Colors.black)
                next.MovingNumb = next.MovingNumb + 1; // увеличиваем количество ходов на 1
            next.ChangeColor = ChangeColor.FlipColor(); // меняем цвет
            next.GenFenChess(); // вызываем функцию генерации фигур по fen
            return next;
        }     
        
        bool KingEatPos() // короля можно есть
        {
            Squares enemyking = FindEnemyKing(); // находим местоположение вражеского короля
            Movements movements = new Movements(this); // создаем все ходы возможные на доске
            
            foreach(FigureOnSquare figs in SortFigures()) // перебираем все фиугуры на доске в список всех фигур
            {
                FiguresMovements figm = new FiguresMovements(figs, enemyking); // идём на клетку вражеского короля
                if (movements.MovementIsPos(figm)) // проверяем можем ли пойти на клетку вражеского короля
                {
                    return true;
                }            
            }
            return false;
        }

        private Squares FindEnemyKing() 
        {
            Figures enemyking = ChangeColor == Colors.black ? Figures.WKing : Figures.BKing; // определяем цвет вражеского короля
            foreach (Squares square in Squares.SortSquares()) // перебираем все клетки
                if (GetFigureOn(square) == enemyking) // проверяем искомая фигура равна ли вражескому королю
                    return square;
            return Squares.zero;

        }

        public bool IsCheck() // проверка на шах
        {
            Board after = new Board(fenchess); // создаём доску
            after.ChangeColor = ChangeColor.FlipColor(); // меняем цвет после хода
            return after.KingEatPos();
        }

        public bool IsCheckAftertM(FiguresMovements figm) // проверка на шах после хода
        {
            Board after = Moving(figm); // делаем ход
            return after.KingEatPos(); // проверяем можно ли съесть короля после хода
        }
    }
}
