using System;
using System.Diagnostics;
using System.Media;
using static MainGame.Program;
using static SelectGames.SelectGame;

namespace CodeMANIA
{
    public class CodeMania
    {
        static int Score = 0;
        static int CorrectCharacters = 0; // New variable to track only correct characters
        static int CurrentLineIndex = 0;
        static int CurrentPosition = 0;
        static int CurrentLevel = 0;

        static string[][] Levels = new[]
        {
        new[]
        {
            "using System;",
            "class HelloWorld",
            "{",
            "    static void Main()",
            "    {",
            "        Console.WriteLine(\"Hello, World!\");",
            "    }",
            "}",
        },
        new[]
        {
            "using System;",
            "class Calculator",
            "{",
            "    static int Add(int a, int b)",
            "    {",
            "        return a + b;",
            "    }",
            "}",
        },
        new[]
        {
            "using System;",
            "class Loops",
            "{",
            "    static void PrintNumbers()",
            "    {",
            "        for (int i = 1; i <= 10; i++)",
            "        {",
            "            Console.WriteLine(i);",
            "        }",
            "    }",
            "}",
        },
        new[]
        {
            "using System;",
            "class Greetings",
            "{",
            "    static void SayHello(string name)",
            "    {",
            "        Console.WriteLine($\"Hello, {name}!\");",
            "    }",
            "}",
        },
        new[]
        {
            "using System;",
            "class Multiplier",
            "{",
            "    static int Multiply(int x, int y)",
            "    {",
            "        return x * y;",
            "    }",
            "    static void Main()",
            "    {",
            "        Console.WriteLine(Multiply(3, 4));",
            "    }",
            "}",
        },
    };

        static Stopwatch Stopwatch = new();

       public static void CodeMain()
        {
            SoundPlayer spacetypebgm = new SoundPlayer(@"C:\Users\Lei\source\repos\HotKeys\HotKeys\BGM&SFX\8bit_Persona_3_Reload_Full_Moon_Full_Life_Chiptune_Remix_Cover_Persona3Reload_[_YTBMP3.org_].wav");
            spacetypebgm.PlayLooping();
            ShowMenu();
        }

        static void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            int selectedIndex = 0; // To track the currently selected menu option

            while (true)
            {
                Console.Clear();

                // ASCII art title
                string[] title = {
                @" ██████╗ ██████╗ ██████╗ ███████╗    ███╗   ███╗ █████╗ ███╗   ██╗██╗ █████╗ ",
                @"██╔════╝██╔═══██╗██╔══██╗██╔════╝    ████╗ ████║██╔══██╗████╗  ██║██║██╔══██╗",
                @"██║     ██║   ██║██║  ██║█████╗      ██╔████╔██║███████║██╔██╗ ██║██║███████║",
                @"██║     ██║   ██║██║  ██║██╔══╝      ██║╚██╔╝██║██╔══██║██║╚██╗██║██║██╔══██║",
                @"╚██████╗╚██████╔╝██████╔╝███████╗    ██║ ╚═╝ ██║██║  ██║██║ ╚████║██║██║  ██║",
                @" ╚═════╝ ╚═════╝ ╚═════╝ ╚══════╝    ╚═╝     ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝╚═╝  ╚═╝"
            };
                Console.ForegroundColor = ConsoleColor.White;
                // Menu options
                string[] menuOptions = {
                "Start Game",
                "Instructions",
                "Back to Select Game Menu"
            };

                // Calculate vertical centering
                int totalLines = title.Length + menuOptions.Length + 2; // Title + Menu + spacer
                int startY = (Console.WindowHeight - totalLines) / 2;

                // Render title centered vertically and horizontally
                Console.SetCursorPosition(0, startY);
                foreach (var line in title)
                {
                    int startX = (Console.WindowWidth - line.Length) / 2;
                    Console.SetCursorPosition(startX, Console.CursorTop);
                    Console.WriteLine(line);
                }

                Console.WriteLine(); // Spacer between title and menu

                // Render menu centered vertically and horizontally
                for (int i = 0; i < menuOptions.Length; i++)
                {
                    string option = menuOptions[i];
                    int startX = (Console.WindowWidth - option.Length) / 2;

                    Console.SetCursorPosition(startX, Console.CursorTop);
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow; // Highlight selected option
                        Console.WriteLine($"> {option} <");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(option);
                    }
                }
                
               
                // Input handling for navigation
                var input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.UpArrow)
                {
                    selectedIndex = (selectedIndex - 1 + menuOptions.Length) % menuOptions.Length; // Wrap around
                }
                else if (input == ConsoleKey.DownArrow)
                {
                    selectedIndex = (selectedIndex + 1) % menuOptions.Length; // Wrap around
                }
                else if (input == ConsoleKey.Enter)
                {
                    if (selectedIndex == 0)
                    {
                        StartGame();
                    }
                    else if (selectedIndex == 1)
                    {
                        ShowInstructions();
                    }
                    else if (selectedIndex == 2)
                    {
                        Console.Clear();
                        SelectGameMenu();
                    }
                }
            }
        }

        static void ShowInstructions()
        {
            Console.Clear();
            string[] instructions = {
            "=== Instructions ===",
            "",
            "1. Type the code shown on the screen as accurately as possible.",
            "2. Use Tab for indentation where needed.",
            "3. Your Words Per Minute (WPM) and score will be displayed at the end.",
            "4. Complete all levels to win the game.",
            "",
            "Press any key to return to the menu..."
        };

            int startY = (Console.WindowHeight - instructions.Length) / 2;
            Console.SetCursorPosition(0, startY);
            foreach (var line in instructions)
            {
                int startX = (Console.WindowWidth - line.Length) / 2;
                Console.SetCursorPosition(startX, Console.CursorTop);
                Console.WriteLine(line);
            }

            Console.ReadKey(true);
        }

        static void StartGame()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Score = 0;
            CorrectCharacters = 0; // Reset correct characters
            CurrentLevel = 0;

            Console.WriteLine("Get ready to type the code blocks!");
            Console.WriteLine("Press any key to start...");
            Console.ReadKey(true);

            Stopwatch.Restart();
            LoadLevel();
        }

        static void LoadLevel()
        {
            if (CurrentLevel >= Levels.Length)
            {
                EndGame();
                return;
            }

            CurrentLineIndex = 0;
            CurrentPosition = 0;

            Console.Clear();
            Console.WriteLine($"Level {CurrentLevel + 1}");
            Console.WriteLine("Press any key to start...");
            Console.ReadKey(true);

            PlayGame();
        }

        static void RenderCode()
        {
            Console.Clear();

            string[] CodeBlock = Levels[CurrentLevel];
            int codeStartX = (Console.WindowWidth - CodeBlock[0].Length) / 2;
            int codeStartY = (Console.WindowHeight - CodeBlock.Length) / 2;

            for (int i = 0; i < CodeBlock.Length; i++)
            {
                Console.SetCursorPosition(codeStartX, codeStartY + i);

                if (i < CurrentLineIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green; // Completed lines
                    Console.Write(CodeBlock[i]);
                }
                else if (i == CurrentLineIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(CodeBlock[i].Substring(0, CurrentPosition));

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(CodeBlock[i].Substring(CurrentPosition));
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(CodeBlock[i]);
                }
            }

            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Score: {Score}");
        }

        static void PlayGame()
        {
            string[] CodeBlock = Levels[CurrentLevel];
            while (CurrentLineIndex < CodeBlock.Length)
            {
                RenderCode();
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    EndGame();
                    return;
                }

                string currentLine = CodeBlock[CurrentLineIndex];

                if (key.Key == ConsoleKey.Tab)
                {
                    string tabSpaces = "    ";
                    if (currentLine.Substring(CurrentPosition).StartsWith(tabSpaces))
                    {
                        CurrentPosition += tabSpaces.Length;
                        Score += tabSpaces.Length;
                        CorrectCharacters += tabSpaces.Length; // Increment correct characters for valid Tab input
                    }
                }
                else if (key.KeyChar == currentLine[CurrentPosition])
                {
                    CurrentPosition++;
                    Score++;
                    CorrectCharacters++; // Increment only for correct keystrokes

                    if (CurrentPosition >= currentLine.Length)
                    {
                        CurrentLineIndex++;
                        CurrentPosition = 0;
                    }
                }
            }

            CurrentLevel++;
            LoadLevel();
        }

        static void EndGame()
        {
            Stopwatch.Stop();
            double totalTimeInMinutes = Stopwatch.Elapsed.TotalMinutes;

            double words = CorrectCharacters / 5.0;
            double wpm = words / totalTimeInMinutes;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Congratulations! You've completed the game.");
            Console.WriteLine($"Your final score: {Score}");
            Console.WriteLine($"Your typing speed: {wpm:F2} WPM");
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey(true);
            ShowMenu();
        }
    }
}
