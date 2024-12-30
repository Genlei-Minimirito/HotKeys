using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using static MainGame.Program;
using static SelectGames.SelectGame;

namespace ThinkyT
{
    public class ThinkyType
    {
        static bool isInputReceived = false;

        public static void ThinkyMain()
        {
            SoundPlayer spacetypebgm = new SoundPlayer(@"C:\Users\Lei\source\repos\HotKeys\HotKeys\BGM&SFX\ILLIT_Magnetic_8_bit_cover_[_YTBMP3.org_].wav");
            spacetypebgm.PlayLooping();
            while (true)
            {
                ShowMenu();
            }
        }

        static void ShowMenu()
        {
            SoundPlayer spacetypebgm = new SoundPlayer(@"C:\Users\Lei\source\repos\HotKeys\HotKeys\BGM&SFX\ILLIT_Magnetic_8_bit_cover_[_YTBMP3.org_].wav");
            spacetypebgm.PlayLooping();
            Console.Clear();

            // ASCII art
            string[] titleArt = {
                "____________________________________________________________________",
                "  ______                                  ______                    ",
                "    /      /     ,         /                /                       ",
                "---/------/__--------__---/-__-------------/---------------__----__-",
                "  /      /   ) /   /   ) /(     /   /     /      /   /   /   ) /___)",
                "_/______/___/_/___/___/_/___\\__(___/_____/______(___/___/___/_(___ _",
                "                                  /                /   /            ",
                "                              (_ /             (_ /   /             "
            };

            // Menu options
            string[] menuOptions = { "Start Game", "Instructions", "Back to Select Game" };
            int selectedOption = 0;

            while (true)
            {
                Console.Clear();

                int windowHeight = Console.WindowHeight;
                int windowWidth = Console.WindowWidth;

                // Calculate vertical padding to center the menu
                int totalContentHeight = titleArt.Length + menuOptions.Length + 2; // 2 for spacing
                int verticalPadding = (windowHeight - totalContentHeight) / 2;

                // Add vertical padding
                for (int i = 0; i < verticalPadding; i++)
                {
                    Console.WriteLine();
                }

                // Display ASCII art centered
                foreach (string line in titleArt)
                {
                    CenterText(line, ConsoleColor.Cyan);
                }

                // Blank line for spacing between title and menu
                Console.WriteLine();

                // Display menu options centered, with the selected option highlighted
                for (int i = 0; i < menuOptions.Length; i++)
                {
                    if (i == selectedOption)
                    {
                        CenterText($"> {menuOptions[i]} <", ConsoleColor.Yellow);
                    }
                    else
                    {
                        CenterText(menuOptions[i], ConsoleColor.White);
                    }
                }

                // Add vertical padding after content
                for (int i = 0; i < verticalPadding; i++)
                {
                    Console.WriteLine();
                }

                // Handle user input for arrow key navigation
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedOption = (selectedOption - 1 + menuOptions.Length) % menuOptions.Length;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedOption = (selectedOption + 1) % menuOptions.Length;
                        break;
                    case ConsoleKey.Enter:
                        // Execute selected option
                        if (selectedOption == 0) StartGame();
                        else if (selectedOption == 1) ShowInstructions();
                        else if (selectedOption == 2) SelectGameMenu();
                        return;
                }
            }
        }

        static void ShowInstructions()
        {
            Console.Clear();

            int windowHeight = Console.WindowHeight;
            int totalLines = 6; // Number of instruction lines
            int padding = (windowHeight - totalLines) / 2;

            for (int i = 0; i < padding; i++) Console.WriteLine();

            CenterText("Instructions:", ConsoleColor.Yellow);
            CenterText("- A scrambled set of words will be displayed.");
            CenterText("- Type the correct sentence formed by those words.");
            CenterText("- Earn points for accuracy and speed.");
            CenterText("- Lose lives for mistakes or running out of time.");
            CenterText("- Difficulty levels determine the scrambling and time limit.");
            CenterText("");
            CenterText("Press any key to return to the menu...");
            Console.ReadKey();
        }

        static void StartGame()
        {
            int timeLimit = 30;
            int lives = 3;
            bool hardMode = false;

            string[] difficultyOptions = { "Easy (30 seconds, 3 lives)", "Medium (20 seconds, 3 lives)", "Hard (30 seconds, 2 lives, scrambled letters)", "Back to Menu" };
            int selectedDifficulty = 0;

            while (true)
            {
                Console.Clear();

                int windowHeight = Console.WindowHeight;
                int totalLines = difficultyOptions.Length + 2; // Difficulty options + title
                int padding = (windowHeight - totalLines) / 2;

                for (int i = 0; i < padding; i++) Console.WriteLine();

                CenterText("Select Difficulty:", ConsoleColor.Yellow);

                for (int i = 0; i < difficultyOptions.Length; i++)
                {
                    if (i == selectedDifficulty)
                    {
                        CenterText($"> {difficultyOptions[i]} <", ConsoleColor.Yellow);
                    }
                    else
                    {
                        CenterText(difficultyOptions[i]);
                    }
                }

                for (int i = 0; i < padding; i++) Console.WriteLine();

                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedDifficulty = (selectedDifficulty - 1 + difficultyOptions.Length) % difficultyOptions.Length;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedDifficulty = (selectedDifficulty + 1) % difficultyOptions.Length;
                        break;
                    case ConsoleKey.Enter:
                        switch (selectedDifficulty)
                        {
                            case 0:
                                timeLimit = 30;
                                lives = 3;
                                break;
                            case 1:
                                timeLimit = 20;
                                lives = 3;
                                break;
                            case 2:
                                timeLimit = 30;
                                lives = 2;
                                hardMode = true;
                                break;
                            case 3:
                                ShowMenu();
                                break;
                        }
                        PlayGame(timeLimit, lives, hardMode);
                        return;
                }
            }
        }

        static void PlayGame(int timeLimit, int lives, bool hardMode)
        {
            int points = 0;

            List<(string[], string)> wordSets = new List<(string[], string)>
    {
        (new string[] { "lamp", "chair", "jump", "over", "table" }, "Jump over the table and lamp"),
        (new string[] { "sky", "blue", "elephant", "runs", "fast", "the", "in", "the" }, "The elephant runs fast in the blue sky"),
        (new string[] { "phone", "calls", "every", "ring", "second", "the", "with", "a" }, "The phone rings every second with a call"),
        // Add more sentences as needed
    };

            Random random = new Random();

            while (lives > 0)
            {
                var (words, correctSentence) = wordSets[random.Next(wordSets.Count)];
                string[] scrambledWords = hardMode
                    ? ScrambleLettersInWords(words)
                    : ScrambleWords(words);

                while (true)
                {
                    Console.Clear();

                    // Prepare content
                    string[] content = {
                $"Lives: {lives} | Points: {points}",
                "Unscramble the words to form the correct sentence:",
                string.Join(" ", scrambledWords),
                "",
                "Type your input below and press Enter:"
            };

                    // Calculate vertical padding
                    int totalContentHeight = content.Length + 1; // +1 for input space
                    int verticalPadding = (Console.WindowHeight - totalContentHeight - 1) / 2;

                    // Add vertical padding before content
                    for (int i = 0; i < verticalPadding; i++) Console.WriteLine();

                    // Display content
                    foreach (string line in content)
                    {
                        CenterText(line, ConsoleColor.White);
                    }

                    // Add a blank line for spacing before input
                    Console.WriteLine();

                    // Center the user input cursor
                    int cursorLeft = (Console.WindowWidth - 50) / 2; // Assume max input width of 50
                    Console.SetCursorPosition(Math.Max(cursorLeft, 0), Console.CursorTop);
                    string userInput = ReadInputWithTimeout(timeLimit);

                    if (!isInputReceived)
                    {
                        CenterText("==========================================================");
                        CenterText("Time's up!", ConsoleColor.Red);
                        CenterText("==========================================================");
                        CenterText($"The correct sentence was: {correctSentence}");
                        lives--;
                        Console.ReadKey();
                        break;
                    }
                    else if (string.Equals(userInput.Trim(), correctSentence, StringComparison.OrdinalIgnoreCase))
                    {
                        CenterText("==========================================================");
                        CenterText("Correct!", ConsoleColor.Green);
                        CenterText("==========================================================");
                        points++;
                        break;
                    }
                    else
                    {
                        CenterText("==========================================================");
                        CenterText("Incorrect!", ConsoleColor.Red);
                        CenterText("==========================================================");
                        CenterText($"The correct sentence was: {correctSentence}");
                        lives--;
                        Console.ReadKey();
                        break;
                    }
                }
            }

            GameOver(points);
        }


        static void GameOver(int score)
        {
            SoundPlayer gameOverSound = new SoundPlayer(@"C:\Users\Lei\source\repos\HotKeys\HotKeys\BGM&SFX\Old Video Game Game Over Sound Effect FREE.wav");
            gameOverSound.Play();
            Console.Clear();

            // Game Over ASCII art
            string[] gameOverArt = {
        " ██████╗  █████╗ ███╗   ███╗███████╗     ██████╗ ██╗   ██╗███████╗██████╗ ",
        "██╔════╝ ██╔══██╗████╗ ████║██╔════╝    ██╔═══██╗██║   ██║██╔════╝██╔══██╗",
        "██║  ███╗███████║██╔████╔██║█████╗      ██║   ██║██║   ██║█████╗  ██████╔╝",
        "██║   ██║██╔══██║██║╚██╔╝██║██╔══╝      ██║   ██║╚██╗ ██╔╝██╔══╝  ██╔══██╗",
        "╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗    ╚██████╔╝ ╚████╔╝ ███████╗██║  ██║",
        " ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝     ╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═╝"
    };

            string[] content = {
        $"Your total score: {score}",
        "Press any key to return to the menu..."
    };

            // Calculate vertical padding
            int totalContentHeight = gameOverArt.Length + content.Length + 1; // +1 for spacing
            int verticalPadding = (Console.WindowHeight - totalContentHeight) / 2;

            // Add vertical padding before content
            for (int i = 0; i < verticalPadding; i++) Console.WriteLine();

            // Center the ASCII art
            foreach (string line in gameOverArt)
            {
                CenterText(line, ConsoleColor.Red);
            }

            // Blank line for spacing
            Console.WriteLine();

            // Center additional content
            foreach (string line in content)
            {
                CenterText(line, ConsoleColor.Yellow);
            }

            // Wait for user input
            Console.ReadKey();
        }


        static void CenterText(string text, ConsoleColor color = ConsoleColor.White)
        {
            int windowWidth = Console.WindowWidth;
            int textLength = text.Length;
            int leftPadding = (windowWidth - textLength) / 2;

            Console.ForegroundColor = color;
            Console.WriteLine(new string(' ', leftPadding) + text);
            Console.ResetColor();
        }

        static string ReadInputWithTimeout(int timeoutSeconds)
        {
            string input = string.Empty;
            isInputReceived = false;

            Task.Run(() =>
            {
                input = Console.ReadLine();
                isInputReceived = true;
            }).Wait(TimeSpan.FromSeconds(timeoutSeconds));

            return input ?? string.Empty;
        }

        static string[] ScrambleWords(string[] words)
        {
            return words.OrderBy(_ => Guid.NewGuid()).ToArray();
        }

        static string[] ScrambleLettersInWords(string[] words)
        {
            return words.Select(word => new string(word.OrderBy(_ => Guid.NewGuid()).ToArray())).ToArray();
        }
    }
}