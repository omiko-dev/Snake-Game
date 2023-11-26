using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
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
}
