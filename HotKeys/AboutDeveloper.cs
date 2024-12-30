using System;
using System.Media;
using static MainGame.Program;

namespace AboutDev
{
    public class AboutDeveloper { 

    public static void AboutDevMenu()
    {
            Console.ForegroundColor = ConsoleColor.White;
            int selectedIndex = 0; // Index of the currently selected menu item
      
        string[] menu = new string[]
        {
    " █████  ██████   ██████  ██    ██ ████████     ██████  ███████ ██    ██ ███████ ██       ██████  ██████  ███████ ██████  ███████ ",
    "██   ██ ██   ██ ██    ██ ██    ██    ██        ██   ██ ██      ██    ██ ██      ██      ██    ██ ██   ██ ██      ██   ██ ██      ",
    "███████ ██████  ██    ██ ██    ██    ██        ██   ██ █████   ██    ██ █████   ██      ██    ██ ██████  █████   ██████  ███████ ",
    "██   ██ ██   ██ ██    ██ ██    ██    ██        ██   ██ ██       ██  ██  ██      ██      ██    ██ ██      ██      ██   ██      ██ ",
    "██   ██ ██████   ██████   ██████     ██        ██████  ███████   ████   ███████ ███████  ██████  ██      ███████ ██   ██ ███████ ",
    "                                                                                                                                 ",
     
        };
            
           
            string[] options = { "Martinez, Bren Jaren F.", "Minimirito, Genlei C.", "Nebreja, Abby Crystal R.", "Back to Main Menu" };

        while (true)
        {
            Console.Clear();

            // Center header text vertically
            int headerStartY = (Console.WindowHeight - (menu.Length + options.Length + 2)) / 2;
            int headerStartX = (Console.WindowWidth - menu[0].Length) / 2;

            // Display the header menu centered
            foreach (var line in menu)
            {
                Console.SetCursorPosition(headerStartX, headerStartY++);
                Console.WriteLine(line);
            }

            // Center the options menu
            int optionsStartY = headerStartY + 2; // Add spacing below header
            foreach (var option in options)
            {
                Console.SetCursorPosition((Console.WindowWidth - options[0].Length) / 2, optionsStartY++);
            }

            // Display options with highlight for selected index
            for (int i = 0; i < options.Length; i++)
            {
                int leftMargin = (Console.WindowWidth - options[i].Length) / 2;
                Console.SetCursorPosition(leftMargin, headerStartY + 2 + i);
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow; // Highlight color
                    Console.WriteLine($"> {options[i]} <");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray; // Normal color
                    Console.WriteLine($"  {options[i]}  ");
                }
            }

            // Reset cursor color
            Console.ForegroundColor = ConsoleColor.White;

            // Handle user input
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex - 1 + options.Length) % options.Length; // Wrap around to the last option
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex + 1) % options.Length; // Wrap around to the first option
                    break;
                case ConsoleKey.Enter:
                     if (selectedIndex == 0) // Display Bren's Info
                    {
                        BrenInfo();
                    }
                    else if (selectedIndex == 1) // Display Genlei's Info
                        {
                        GenleiInfo();
                    }
                    else if (selectedIndex == 2) // Display Abby's Info
                        {
                        AbbyInfo();
                    }
                    else
                    {
                        ShowMainMenu();
                        Console.ReadKey();
                    }
                    break;
            }
            static void BrenInfo()
            {
               
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;

                string[] title = new string[]
                {
                "Name: BREN JAREN F. MARTINEZ",
                "Contact: 0324-0586@lspu.edu.ph",
                "About Me:",
                 "Hi! I'm Bren Jaren F. Martinez born and residing at San Pablo City, Laguna.",
                  "Currently pursuing Bachelor of Science in Computer Science at",
                  "Laguna State Polytechnic University - San Pablo City Campus."
                };

                CenterDisplay(title);
                Console.ReadKey();
            }
                static void GenleiInfo()
                {

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;

                    string[] title = new string[]
                    {
                    "Name: GENLEI C. MINIMIRITO",
                    "Contact: 0324-0599@lspu.edu.ph",
                    "About Me: ",
                    "Hi! I'm Genlei C. Minimirito, a first-year student, studying Bachelor of Science in Computer Science (BSCS) ",
                    "in Laguna State Polytechnic University - San Pablo City Campus. ",
                    "I enjoy working on projects that are clear, simple, and effective."

                    };
                    CenterDisplay(title);
                    Console.ReadKey();
                }
                static void AbbyInfo()
                {

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;

                    string[] title = new string[]
                    {
                    "Name: ABBY CRYSTAL R. NEBREJA",
                    "Contact:0324-0600@lspu.edu.ph",
                    "About Me:",
                    "My name is Abby Crystal Nebreja, I am 18 year old, I live in Calauan, Laguna. " ,
                    "I am currently pursuing a Bachelor of Science in Computer Science at Laguna State " ,
                    "Polytechnic University - San Pablo City Campus."
                    };

                    CenterDisplay(title);
                    Console.ReadKey();
                }
            }
        }
    }
}
