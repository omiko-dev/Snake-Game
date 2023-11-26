using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
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
                player.SelectedSkin = (SnakeSkin)selectedOption; 
                purchasedSkins.Add(selectedOption);
                Console.WriteLine($"You've successfully bought the skin {player.SelectedSkin}!");
            }
            else
            {
                Console.WriteLine("Not enough points to buy this skin!");
            }
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
