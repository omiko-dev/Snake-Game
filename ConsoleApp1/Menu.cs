using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
}
