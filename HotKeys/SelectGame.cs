using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using static MainGame.Program;
using static CodeMANIA.CodeMania;
using static FruitTyping.FruitTyper;
using static ThinkyT.ThinkyType;

namespace SelectGames 
{
    public class SelectGame
    {
        public static void SelectGameMenu()
        {
            SoundPlayer spacetypebgm = new SoundPlayer(@"C:\Users\Lei\source\repos\HotKeys\HotKeys\BGM&SFX\NewJeans_Supernatural_Chiptune_cover_8_Bit_Style_[_YTBMP3.org_].wav");
            spacetypebgm.PlayLooping();
            int selectedIndex = 0; // Index of the currently selected menu item
            Console.ForegroundColor = ConsoleColor.Red;
            string[] menu = new string[]
            {
        "   *************************************",
        "      )    )               )         ) (     ",
        "    ( /( ( /(   *   )    ( /(      ( /( )\\ )  ",
        "    )\\()))\\())` )  /(    )\\())(    )\\()|()/(  ",
        "   ((_\\((_\\)  ( )(_)) |((_\\ )\\  ((_\\ /(_)) ",
        "    _((_) ((_)(_(_())  |_ ((_|(_)__ ((_|_))   ",
        "   | || |/ _ \\|_   _|  | |/ /| __\\ \\ / / __|  ",
        "   | __ | (_) | | |      ' < | _| \\ V /\\__ \\  ",
        "   |_||_|\\___/  |_|     _|\\_\\|___| |_| |___/   ",
        "   *************************************",
            };

            string[] options = { "Space Typer", "Fruit Typer", "Code Mania", "Thinky Type", "Back to Main Menu" };

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
                        // Execute the selected option
                        if (selectedIndex == 0)
                        {
                            PlaySpaceTyper();
                        }
                        else if (selectedIndex == 1) // Play FruitTyper
                        {
                            PlayFruitTyper();
                        }
                        else if (selectedIndex == 2) // Play FruitTyper
                        {
                            CodeMain();
                        }
                        else if (selectedIndex == 3) // Play FruitTyper
                        {
                            ThinkyMain();
                        }
                        else
                        {
                            ShowMainMenu();
                            Console.ReadKey();
                        }
                        break;
                }

            }
        }
    }
}
