using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessLibrary; // библиотека с запрограммированными шахматными правилами

public class ChessRules : MonoBehaviour
{
    FiguresDragAndDrop figuresdad;
    Chess1 newchess; // переменая для работы с шахматами

    public ChessRules() // конструктор для иниициализации класса перемещения фигур
    {
        figuresdad = new FiguresDragAndDrop();
        newchess = new Chess1(); // создаём шахматы
       
    }

	// Use this for initialization
	public void Start ()
    {
        PrintFigures(); // отображаем фигуры на доске 
        ShowWalkingFigures();
    }

	// Update is called once per frame
	void Update ()
    {
      if (figuresdad.Action()) // вызываем функцию перемещения фигур
        {
            string from = GetBoardSquare(figuresdad.GrabCoordinate); // положение откуда нужно двигаться 
            string to = GetBoardSquare(figuresdad.DropCoordinate); // положение куда нужно двигаться 
            string figure = newchess.GFigure((int)(figuresdad.GrabCoordinate.x / 2.0),(int)(figuresdad.GrabCoordinate.y / 2.0)).ToString(); // получаем фигуру с определенной клетки
            string move = figure + from + to; // формируем строчку хода
            Debug.Log(move);
            newchess = newchess.Moving(move); // вызываем движения фигур 
            PrintFigures();
            ShowWalkingFigures();
        }           
	}

    string GetBoardSquare(Vector2 coordinate) // функция принимающая вектор и возвращающая координату на которую сделан ход
    {
        int x = Convert.ToInt32(coordinate.x / 2.0); // конвертируем координату x и делим на 2
        int y = Convert.ToInt32(coordinate.y / 2.0); // конвертируем координату y и делим на 2
        return ((char)('a' + x)).ToString() + (y + 1).ToString(); // 
    }

    void PrintFigures() // функция рисующая шахматы
    {
        int newcount = 0; // счетчик
        for (int y = 0; y < 8; y++) // перебираем клетки на доске
            for (int x = 0; x < 8; x++)
            {
                string figure = newchess.GFigure(x, y).ToString(); // узнаём какая фигура находиться на конкретной клетке
                if (figure == ".") // если фигура пустая - пропускаем
                {
                    continue;
                }
                FigureOnBoard("cell" + newcount, figure, x, y); // размещаем нужную фигуру на указанных координатах
                newcount++; // увеличиваем счетчик после того как была задействована фигура
            }
        for (; newcount < 32; newcount++) // перебираем счётчик до 32, чтобы закинуть фигуру на 65 клетку
        {
            FigureOnBoard("cell" + newcount, "q", 9, 9); // перемещаем ферзя на клетку 9:9
        }
    }

    void ShowWalkingFigures()
    {
        for (int y = 0; y < 8; y++) // перебираем клетки на доске
            for (int x = 0; x < 8; x++)
            {
                PaintSquares(x, y, false);
            }

        foreach (string movements in newchess.GetAllMovements())
        {
            int x;
            int y;
            GetSquare(movements.Substring(1,2), out x, out y);
            PaintSquares(x, y, true);
        }
    }

    public void GetSquare(string a1, out int x, out int y) // обработка значения по названию клетки
    {
        x = 9;
        y = 9;

        if (a1.Length == 2 && a1[0] >= 'a' && a1[0] <= 'h' && a1[1] >= '1' && a1[1] <= '8')
        {
            x = a1[0] - 'a';
            y = a1[1] - '1';
        }
    }


    void FigureOnBoard (string сell, string figure, int x, int y) // метод размещения фигуры на своё место
    {
        //Debug.Log(сell + " " + figure + " " + x + y); // выводим на нужной ячейке выбранную фигуру на указанных координатах

        GameObject GameOCell = GameObject.Find(сell); // находим ячейку (cell) по его имени в которых отобразятся фигуры
        GameObject GameOFigure = GameObject.Find(figure); // находим фигуру по имени в нотации FEN
        GameObject GameOSquare = GameObject.Find("" + y + x); // находим клетку в которой будет находиться фигура

        var FigureSprite = GameOFigure.gameObject.GetComponent<SpriteRenderer>(); // рисуем на cell соответствующую фигуру
        var CellSPrite = GameOCell.gameObject.GetComponent<SpriteRenderer>(); // получаем спрайт ячейки на которой будет размещаться фигура
        CellSPrite.sprite = FigureSprite.sprite; // для спрайта ячейки указываем спрайт фигуры

        GameOCell.transform.position = GameOSquare.transform.position; // для ячейки указываем позицию на которой находиться клетка доски
    }

    void PaintSquares(int x, int y, bool SquareIsPainted)
    {
        string squarecolor = (x+y) % 2 == 0 ? "B" : "W"; 
        GameObject GameOSquare = GameObject.Find("" + y + x);
        GameObject storage;
        if (SquareIsPainted)
        {
            storage = GameObject.Find("SquareBIzm");
        }
        else
        {
           storage = GameObject.Find("Square"+squarecolor);
        }

        var SquareSprite = GameOSquare.GetComponent<SpriteRenderer>();
        var StorageSprite = storage.GetComponent<SpriteRenderer>();
        SquareSprite.sprite = StorageSprite.sprite;
         
    }

}

class FiguresDragAndDrop
{
    enum FiguresStates // список состояний фигур при перемещении
    {
        zero, // статичное состояние фигуры при котом она не двигается

        drag // состояние фигуры, когда её перемещают
    }

    public Vector2 GrabCoordinate { get; private set; } // местоположение где схватили объект
    public Vector2 DropCoordinate { get; private set; } // местоположение куда перетащили объект

    FiguresStates newstate; // переменная для смены состояний фигур
    GameObject subject; // предмет 
    Vector2 offset; // смещение


    public FiguresDragAndDrop()
    {
        newstate = FiguresStates.zero; // начальное состояние
        subject = null; // предмет перемещения
    }

    public bool Action() // функция перемещения фигур
    {
        switch (newstate) // перебираем состояния фигур
        {
            case FiguresStates.zero: 

                if (MousePressed()) // если нажали ЛКМ то захватываем объект
                {
                    FigureGrab(); // захватываем объект
                }

                break;

            case FiguresStates.drag:

                if (MousePressed()) // если ЛКМ уже нажали то перемещаем объект
                {
                    FigureDrag(); // перемещаем объект
                }

                else
                {
                    FigureDrop(); // отпускаем объект
                    return true;
                }

                break;
        }

        return false;
    }

    bool MousePressed() // проверяем нажата ли ЛКМ
    {
        return Input.GetMouseButton(0);
    } 

    void FigureGrab () // захват объектов
    {
        Vector2 clickCoordinate = GetClickCoordinate(); // сохраняем координату на которую кликнули 
        Transform clickedSubject = GetSubjectOn(clickCoordinate); // сохраняем объект на который кликнули

        if (clickedSubject == null) // проверяем не пустой ли объект
        {
            return;
        }

        GrabCoordinate = clickedSubject.position; // сохраняем позицию того объекта по которому мы кликнули
        subject = clickedSubject.gameObject; // записываем в subject объект на который кликнули
        newstate = FiguresStates.drag; // меняем состояние фигуры
        offset = GrabCoordinate - clickCoordinate; // сохраняем смещение при нажатии вычисляя координату на которую кликнули
        Debug.Log("It's grabed: " + subject.name); // выводим имя нужного объекта
    }

    Vector2 GetClickCoordinate () // возращаем координату на которую кликнули
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition); // переводим координаты с экрана в "мировые"
    }

    Transform GetSubjectOn(Vector2 coordinate) // функция узнающая какой объект был взят
    {
        RaycastHit2D[] figures =  Physics2D.RaycastAll(coordinate, coordinate, 0.5f); // узнаем на какой объект мы нажали. для этого получаем все колайдеры

        if (figures.Length == 0) // если кол-во элементов в массиве = ноль
        {
            return null;
        }
        return figures[0].transform; // возвращаем первый объект на который нажали
    }

    void FigureDrag() // перемещение объектов
    {
        subject.transform.position = GetClickCoordinate() + offset; // изменяем позицию объкта на которую нажали и сохраняем смещение
    }

    void FigureDrop() // функция отпускающая объект после переноса
    {
        DropCoordinate = subject.transform.position; // сохраняем позицию в которую попали после перетаскивания
        newstate = FiguresStates.zero; // меняем состояние фигуры
        subject = null; // прекращаем перетаскивание объекта
    }
}

