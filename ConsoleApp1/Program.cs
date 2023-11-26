using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    public class Menu
    {

        public Menu(Player player)
        {
            this.player = player;
        }
        public Player player { get; set; }
        int topScore = 0;
        public void ShowStartMenu()
        {

            player.Score = 1000;
            Console.Clear();

            string title = "SNAKE GAME";
            string option1 = "Start Game";
            string option2 = "Choose Skin";
            string option3 = "Exit";
            string scoreString = $"Score: {topScore}";
            string topScoreString = $"Top Score: {topScore}";


            Console.ForegroundColor = ConsoleColor.Green;

            int centerX = (Console.WindowWidth - title.Length) / 2;
            int centerY = Console.WindowHeight / 2 - 2;

            Console.SetCursorPosition(centerX + 15, centerY / 10);
            Console.WriteLine("User: " + player.userName);

            Console.SetCursorPosition(centerX / 10, centerY / 10);
            Console.WriteLine(topScoreString);

            Console.SetCursorPosition(centerX, centerY);
            Console.WriteLine(title);

            Console.SetCursorPosition(centerX, centerY);
            Console.WriteLine(title);

            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(centerX, centerY + 1);
            Console.WriteLine(option1);

            Console.SetCursorPosition(centerX, centerY + 2);
            Console.WriteLine(option2);

            Console.SetCursorPosition(centerX, centerY + 3);
            Console.WriteLine(option3);

            int selectedOption = 1;

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.Enter)
                {
                    HandleStartMenuInput(selectedOption);
                    return;
                }

                if (key.Key == ConsoleKey.UpArrow)
                    selectedOption = Math.Max(selectedOption - 1, 1);
                else if (key.Key == ConsoleKey.DownArrow)
                    selectedOption = Math.Min(selectedOption + 1, 3);

                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(centerX, centerY);
                Console.WriteLine(title);


                Console.SetCursorPosition(centerX + 15, centerY / 10);
                Console.WriteLine("User: " + player.userName);

                Console.SetCursorPosition(centerX / 10, centerY / 10);
                Console.WriteLine(topScoreString);

                Console.ForegroundColor = ConsoleColor.White;

                Console.SetCursorPosition(centerX, centerY + 1);
                Console.WriteLine(selectedOption == 1 ? $"> {option1}" : option1);

                Console.SetCursorPosition(centerX, centerY + 2);
                Console.WriteLine(selectedOption == 2 ? $"> {option2}" : option2);

                Console.SetCursorPosition(centerX, centerY + 3);
                Console.WriteLine(selectedOption == 3 ? $"> {option3}" : option3);
            }
        }

        void HandleStartMenuInput(int selectedOption)
        {
            switch (selectedOption)
            {
                case 1:
                    Start();
                    ShowStartMenu();
                    break;
                case 2:
                    ChooseSkin();
                    ShowStartMenu();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
            }
        }

        void Start()
        {
            Game game = new Game(topScore);
            game.StartGame(player);
            topScore = game.getTopScore();
            player.Score += game.getScore();

        }

        void ChooseSkin()
        {
            Shop shopMenu = new Shop();
            shopMenu.ShowShop(player);
        }
    }

    class Program
    {
        static void Main(string[] Args)
        {
            Console.SetWindowSize(60, 30);
            Console.SetBufferSize(60, 30);
            Console.CursorVisible = false;

            Console.Write("Enter Your Name: ");
            var name = Console.ReadLine();

            var player = new Player(name);

            Menu i = new Menu(player);
            i.ShowStartMenu();
        }
    }

    public class Game
    {
        public static int MapWidth = 20;
        public static int MapHeight = 10;
        public static int ScreenWidth = MapWidth * 3;
        public static int ScreenHeight = MapHeight * 3;
        private static readonly Random Random = new Random();
        private const int FrameMilliseconds = 200;
        private const ConsoleColor BorderColor = ConsoleColor.Gray;
        private const ConsoleColor FoodColor = ConsoleColor.Green;
        private const ConsoleColor BodyColor = ConsoleColor.Red;
        private const ConsoleColor HeadColor = ConsoleColor.DarkBlue;
        static public int topScore;
        static public int score;

        public Game()
        {

        }

        public Game(int _topScore)
        {
            topScore = _topScore;
        }

        public int getTopScore()
        {
            return topScore;
        }
        public int getScore()
        {
            return score;
        }

        private readonly ConsoleColor[] skinColors = { ConsoleColor.Red, ConsoleColor.DarkBlue, ConsoleColor.Green };


        public void StartGame(Player player)
        {
            score = 0;

            Console.Clear();
            DrawBoard();

            Snake snake = new Snake(10, 5, skinColors[(int)player.SelectedSkin]);


            Pixel food = GenFood(snake);
            food.Draw();

            Direction currentMovement = Direction.Right;

            int lagMs = 0;
            var sw = new Stopwatch();

            while (true)
            {
                sw.Restart();
                Direction oldMovement = currentMovement;

                while (sw.ElapsedMilliseconds <= FrameMilliseconds - lagMs)
                {
                    if (currentMovement == oldMovement)
                        currentMovement = ReadMovement(currentMovement);
                }

                sw.Restart();

                if (snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true);
                    food = GenFood(snake);
                    food.Draw();

                    score++;

                    Task.Run(() => Console.Beep(1200, 200));
                }
                else
                {
                    snake.Move(currentMovement);
                }

                Console.SetCursorPosition(ScreenWidth / 12, ScreenHeight / 12);
                Console.WriteLine($"Score: {score}");

                if (snake.Head.X == MapWidth - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == MapHeight - 1
                    || snake.Head.Y == 0
                    || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                    break;

                lagMs = (int)sw.ElapsedMilliseconds;
            }

            if (topScore < score)
            {
                topScore = score;
            }

            snake.Clear();
            food.Clear();


        }

        static void DrawBoard()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                new Pixel(i, 0, BorderColor).Draw();
                new Pixel(i, MapHeight - 1, BorderColor).Draw();
            }

            for (int i = 0; i < MapHeight; i++)
            {
                new Pixel(0, i, BorderColor).Draw();
                new Pixel(MapWidth - 1, i, BorderColor).Draw();
            }
        }

        static Pixel GenFood(Snake snake)
        {
            Pixel food;

            do
            {
                food = new Pixel(Random.Next(1, MapWidth - 2), Random.Next(1, MapHeight - 2), FoodColor);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y ||
                     snake.Body.Any(b => b.X == food.X && b.Y == food.Y));

            return food;
        }

        static Direction ReadMovement(Direction currentDirection)
        {
            if (!Console.KeyAvailable)
                return currentDirection;

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    return (currentDirection != Direction.Down) ? Direction.Up : currentDirection;
                case ConsoleKey.DownArrow:
                    return (currentDirection != Direction.Up) ? Direction.Down : currentDirection;
                case ConsoleKey.LeftArrow:
                    return (currentDirection != Direction.Right) ? Direction.Left : currentDirection;
                case ConsoleKey.RightArrow:
                    return (currentDirection != Direction.Left) ? Direction.Right : currentDirection;
                default:
                    return currentDirection;
            }
        }
    }



    public class Snake
    {
        private readonly ConsoleColor _headColor;

        private readonly ConsoleColor _bodyColor;

        public Snake(int initialX,
            int initialY,
            ConsoleColor bodyColor,
            int bodyLength = 3)
        {
            _headColor = ConsoleColor.Blue;
            _bodyColor = bodyColor;

            Head = new Pixel(initialX, initialY, _headColor);

            for (int i = bodyLength; i >= 0; i--)
            {
                Body.Enqueue(new Pixel(Head.X - i - 1, initialY, _bodyColor));
            }

            Draw();
        }

        public Pixel Head { get; private set; }

        public Queue<Pixel> Body { get; } = new Queue<Pixel>();

        public void Move(Direction direction, bool eat = false)
        {
            Clear();

            Body.Enqueue(new Pixel(Head.X, Head.Y, _bodyColor));
            if (!eat)
                Body.Dequeue();

            switch (direction)
            {
                case Direction.Right:
                    Head = new Pixel(Head.X + 1, Head.Y, _headColor);
                    break;
                case Direction.Left:
                    Head = new Pixel(Head.X - 1, Head.Y, _headColor);
                    break;
                case Direction.Up:
                    Head = new Pixel(Head.X, Head.Y - 1, _headColor);
                    break;
                case Direction.Down:
                    Head = new Pixel(Head.X, Head.Y + 1, _headColor);
                    break;
                default:
                    break;
            }

            Draw();
        }

        public void Draw()
        {
            Head.Draw();

            foreach (Pixel pixel in Body)
            {
                pixel.Draw();
            }
        }

        public void Clear()
        {
            Head.Clear();

            foreach (Pixel pixel in Body)
            {
                pixel.Clear();
            }
        }
    }

    public readonly struct Pixel
    {
        private const char PixelChar = '█';

        public Pixel(int x, int y, ConsoleColor color, int pixelSize = 3)
        {
            X = x;
            Y = y;
            Color = color;
            PixelSize = pixelSize;
        }

        public int X { get; }

        public int Y { get; }

        public ConsoleColor Color { get; }

        public int PixelSize { get; }

        public void Draw()
        {
            Console.ForegroundColor = Color;
            for (int x = 0; x < PixelSize; x++)
            {
                for (int y = 0; y < PixelSize; y++)
                {
                    Console.SetCursorPosition(X * PixelSize + x, Y * PixelSize + y);
                    Console.Write(PixelChar);
                }
            }
        }

        public void Clear()
        {
            for (int x = 0; x < PixelSize; x++)
            {
                for (int y = 0; y < PixelSize; y++)
                {
                    Console.SetCursorPosition(X * PixelSize + x, Y * PixelSize + y);
                    Console.Write(' ');
                }
            }
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }




    public class Shop
    {
        private static List<int> purchasedSkins = new List<int>();

        public static bool access = true;

        public void ShowShop(Player player)
        {
            access = true;

            Console.Clear();
            Console.WriteLine("Welcome to the Shop!");
            Console.WriteLine($"Your Points: {player.Score}");

            int selectedOption = 1;

            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Shop!");
                Console.WriteLine($"Your Points: {player.Score}");
                Console.WriteLine("1. Blue Skin - 50 points" + (selectedOption == 1 ? " <-" : "") + (purchasedSkins.Contains(1) ? " (Owned)" : ""));
                Console.WriteLine("2. White Skin - 75 points" + (selectedOption == 2 ? " <-" : "") + (purchasedSkins.Contains(2) ? " (Owned)" : ""));
                Console.WriteLine("3. Green Skin - 100 points" + (selectedOption == 3 ? " <-" : "") + (purchasedSkins.Contains(3) ? " (Owned)" : ""));
                Console.WriteLine("4. Exit" + (selectedOption == 4 ? " <-" : ""));

                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedOption = Math.Max(1, selectedOption - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedOption = Math.Min(4, selectedOption + 1);
                        break;
                    case ConsoleKey.Enter:
                        ProcessSelection(player, selectedOption);
                        break;
                }

            } while (access);
        }

        private void ProcessSelection(Player player, int selectedOption)
        {
            if (purchasedSkins.Contains(selectedOption))
            {
                Console.WriteLine("You already own this skin!");
            }
            else
            {
                BuySkin(player, selectedOption);
            }
        }

        private void BuySkin(Player player, int selectedOption)
        {
            int cost = GetSkinCost(selectedOption);

            if (selectedOption == 4)
                return;

            if (player.Score >= cost)
            {
                player.Score -= cost;
                player.SelectedSkin = (SnakeSkin)selectedOption; // Convert selectedOption to SnakeSkin enum
                purchasedSkins.Add(selectedOption);
                Console.WriteLine($"You've successfully bought the skin {player.SelectedSkin}!");
            }
            else
            {
                Console.WriteLine("Not enough points to buy this skin!");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private int GetSkinCost(int selectedOption)
        {
            switch (selectedOption)
            {
                case 1:
                    return 50;
                case 2:
                    return 75;
                case 3:
                    return 100;
                default:
                    access = false;
                    break;
            }
            return 0;
        }
    }

}

public class Player
{
    public Player() { }
    public Player(string userName)
    {
        this.userName = userName;
    }

    public string userName { get; set; }
    public int Score { get; set; } = 0;
    public SnakeSkin SelectedSkin { get; set; }
}

public enum SnakeSkin
{
    Red,
    Blue,
    Green
}
