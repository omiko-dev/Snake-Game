using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
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
}




