using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
   struct Squares
    {
        public static Squares zero = new Squares(-1, -1); // координата выходящая за рамки доски

        public int x { get; private set; } // координаты X и Y
        public int y { get; private set; }


        public Squares (int x, int y) // запись координат 
        {
            this.x = x;
            this.y = y;
        }

        public Squares(string a1) // обработка значения по названию клетки
        {
            if (a1.Length == 2 && a1[0] >= 'a' && a1[0] <= 'h' && a1[1] >= '1' && a1[1] <= '8')
            {
                x = a1[0] - 'a';
                y = a1[1] - '1';
            }
            else
            {
                this = zero;
            }
        }

        public bool PosOnBoard () // проверка координат клетки на доске
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }

        public string SqName { get { return ((char)('a' + x)).ToString() + (y + 1).ToString(); } } // возвращаем имя хода

        public static bool operator == (Squares a, Squares b) // оператор равенства
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Squares a, Squares b) // оператор неравенства
        {
            return a.x != b.x || a.y != b.y;
        }

       public static IEnumerable<Squares> SortSquares() // перебираем все клетки на доске
        {
            for (int y = 0; y < 8; y++)
         
                for (int x = 0; x < 8; x++)
                  yield return new Squares(x, y);              
        }
    }
}
