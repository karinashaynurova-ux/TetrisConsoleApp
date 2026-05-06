using System;
using System.Threading;
class Program
{
    // Класс для фигуры
    public class Figure
    {
        public int[,] Shape { get; set; }
        public Figure(int[,] shape)
        {
            Shape = shape;
        }
        public void Rotate()
        {
            int n = Shape.GetLength(0);
            int m = Shape.GetLength(1);
            int[,] rotatedShape = new int[m, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    rotatedShape[j, n - 1 - i] = Shape[i, j];
            Shape = rotatedShape;
        }
    }
    // Класс игрового поля
    public class GameField
    {
        private int[,] field;
        public int Width { get; set; }
        public int Height { get; set; }
        public GameField(int width, int height)
        {
            Width = width;
            Height = height;
            field = new int[Height, Width];
        }
        public bool CanMove(Figure figure, int newX, int newY)
        {
            for (int i = 0; i < figure.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < figure.Shape.GetLength(1); j++)
                {
                    if (figure.Shape[i, j] == 1)
                    {
                        int x = newX + j;
                        int y = newY + i;
                        if (x < 0 || x >= Width || y < 0 || y >= Height || field[y, x] == 1)
                            return false;
                    }
                }
            }
            return true;
        }
        public void PlaceFigure(Figure figure, int x, int y)
        {
            for (int i = 0; i < figure.Shape.GetLength(0); i++)
                for (int j = 0; j < figure.Shape.GetLength(1); j++)
                    if (figure.Shape[i, j] == 1)
                        field[y + i, x + j] = 1;
        }
        // Отрисовка игрового поля
        public void Draw()
        {
            Console.SetCursorPosition(0, 0); // курсор в начало
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                    Console.Write(field[i, j] == 1 ? "■ " : "  "); // по 2 пробела для правильной формы
                Console.WriteLine();
            }
        }
        // Проверка Game Over
        public bool CheckGameOver()
        {
            for (int i = 0; i < Width; i++)
                if (field[0, i] == 1)
                    return true;
            return false;
        }
        // Метод для сброса поля
        public void Reset()
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    field[i, j] = 0;
        }
    }
    // Генерация случайной фигуры
    static Figure CreateRandomFigure()
    {
        Random rand = new Random();
        int choice = rand.Next
(0, 7);
        switch (choice)
        {
            case 0: return new Figure(new int[,] { { 1, 1, 1, 1 } }); // I
            case 1: return new Figure(new int[,] { { 1, 1 }, { 1, 1 } }); // O
            case 2: return new Figure(new int[,] { { 1, 1, 0 }, { 0, 1, 1 } }); // S
            case 3: return new Figure(new int[,] { { 0, 1, 1 }, { 1, 1, 0 } }); // Z
            case 4: return new Figure(new int[,] { { 1, 0, 0 }, { 1, 1, 1 } }); // L
            case 5: return new Figure(new int[,] { { 0, 0, 1 }, { 1, 1, 1 } }); // J
            case 6: return new Figure(new int[,] { { 0, 1, 0 }, { 1, 1, 1 } }); // T
            default: throw new InvalidOperationException();
        }
    }
    // Основной метод игры
    static void RunGame()
    {
        int width = 10;
        int height = 20;
        GameField gameField = new GameField(width, height);
        bool gameOver = false;
        while (!gameOver)
        {
            // Генерация новой фигуры при начале или после окончания игры
            Figure currentFigure = CreateRandomFigure();
            int x = width / 2;
            int y = 0;
           
DateTime lastUpdateTime = DateTime.Now
;
            while (!gameOver)
            {
                Console.Clear();
                // Отрисовываем поле
                gameField.Draw();
                // Рисуем текущую фигуру поверх поля
                for (int i = 0; i < currentFigure.Shape.GetLength(0); i++)
                    for (int j = 0; j < currentFigure.Shape.GetLength(1); j++)
                        if (currentFigure.Shape[i, j] == 1)
                        {
                            int drawX = x + j;
                            int drawY = y + i;
                            if (drawY >= 0 && drawY < height && drawX >= 0 && drawX < width)
                            {
                                Console.SetCursorPosition(drawX * 2, drawY);
                                Console.Write("■");
                            }
                        }
                // Автопадение фигуры каждые 500 мс
                if ((DateTime.Now
 - lastUpdateTime) > TimeSpan.FromMilliseconds(500))
                {
                    if (gameField.CanMove(currentFigure, x, y + 1))
                        y++;
                    else
                    {
                        // Фигура фиксируется
                        gameField.PlaceFigure(currentFigure, x, y);
                        gameOver = gameField.CheckGameOver(); // Game Over проверяется только после фиксации
                        break;
                    }
                    lastUpdateTime = DateTime.Now
;
                }
                // Управление клавишами
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.LeftArrow:
                            if (gameField.CanMove(currentFigure, x - 1, y)) x--;
                            break;
                        case ConsoleKey.RightArrow:
                            if (gameField.CanMove(currentFigure, x + 1, y)) x++;
                            break;
                        case ConsoleKey.DownArrow:
                            if (gameField.CanMove(currentFigure, x, y + 1)) y++;
                            break;
                        case ConsoleKey.UpArrow:
                            currentFigure.Rotate();
                            if (!gameField.CanMove(currentFigure, x, y))
                            {
                                // Отменяем вращение если не помещается
                                for (int r = 0; r < 3; r++) currentFigure.Rotate();
                            }
                            break;
                    }
                }
                Thread.Sleep(50); // уменьшение нагрузки на CPU
            }
            // Конец игры
            Console.SetCursorPosition(0, height + 1);
            if (gameOver)
            {
                Console.WriteLine("Game Over! Press any key to restart...");
                Console.ReadKey();
                gameField.Reset(); // Сброс поля
                Console.Clear();
            }
        }
    }
    // Точка входа в программу
    static void Main(string[] args)
    {
        while (true)
        {
            RunGame(); // Запуск игры
        }
    }
}