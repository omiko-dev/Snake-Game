using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
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
}
