using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
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
}
