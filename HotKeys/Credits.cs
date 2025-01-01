using System.Media;
using static MainGame.Program;

namespace Credit
{
    public class Credits
    {

        public static void CreditsInfo()
        {
            


            int selectedIndex = 0; // Index of the currently selected menu item
            Console.ForegroundColor = ConsoleColor.Red;
            string[] menu = new string[]
            {
    " ██████ ██████  ███████ ██████  ██ ████████ ███████ ",
    "██      ██   ██ ██      ██   ██ ██    ██    ██      ",
    "██      ██████  █████   ██   ██ ██    ██    ███████ ",
    "██      ██   ██ ██      ██   ██ ██    ██         ██ ",
    " ██████ ██   ██ ███████ ██████  ██    ██    ███████ ",
    "                                                    ",
    "                                                    ",
    "Game Developers:",
    "Bren Martinez",
    "Genlei Minimirito",
    "Abby Crystal Nebreja",
    "",
    "Sound FX:",
    "Main Menu BGM: Ready Pixel One by Pix",
    "Space Typer BGM: Space Adventure by HydroGene",
    "Fruit Typer BGM: Fruit Ninja Kinect : 8-Bit Music Remix by Halfbrick Studios",
    "Code Mania BGM: Full Moon, Full Life 8-bit version from Persona 3",
    "Thinky Type BGM: Magnetic 8-bit version by ILLIT",
    "Select Game Menu BGM: Supernatural 8-bit version by NewJeans",
    "",
    "Special Thanks to:",
    "W3schools",
    "Guile Minimirito",
    "Bro Code",
    "GeeksforGeeks",
    "stackoverflow",
    "",
    "(C)2024-2025 MNM GAMES"

            }; Console.ForegroundColor = ConsoleColor.White;
            string[] options = { "Back to Main Menu" };

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
                Console.ForegroundColor = ConsoleColor.Red;

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
                            ShowMainMenu();
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

                    Console.ReadKey();
                }
                static void GenleiInfo()
                {

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;

                    string[] title = new string[]
                    {
                    "Name: GENLEI C. MINIMIRITO",
                    "Contact: 0324-0599@lspu.edu.ph"
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
                    "Contact:"
                    };

                }
            }
        
