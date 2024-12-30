using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floating
{
    class FloatingWord
    {

        public string Word { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }

        public FloatingWord(string word, int x, int y)
        {
            Word = word;
            X = x;
            Y = y;
        }

        public void MoveRandomly(int screenWidth, int screenHeight)
        {
            Random random = new Random();
            // Move the word randomly within the screen boundaries
            X += random.Next(-1, 2); // -1 (left), 0 (stay), 1 (right)
            Y += random.Next(-1, 2); // -1 (up), 0 (stay), 1 (down)

            // Ensure the word stays within the screen bounds
            if (X < 0) X = 0;
            if (X > screenWidth - Word.Length) X = screenWidth - Word.Length;

            if (Y < 0) Y = 0;
            if (Y > screenHeight - 1) Y = screenHeight - 1;
        }
    }

}
