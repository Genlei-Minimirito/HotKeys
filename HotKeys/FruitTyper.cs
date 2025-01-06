using System;
using System.Collections.Generic;
using System.Media;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using static SelectGames.SelectGame;
using static MainGame.Program;
using static System.Reflection.Metadata.BlobBuilder;

namespace FruitTyping
{

   public class FruitTyper
    {
        static List<FallingObject> fallingObjects = new List<FallingObject>();
        static Random rand = new Random();
        static int score = 0;
        static int lives = 3;
        static bool gameOver = false;
        static bool isPaused = false; // Flag to check if the game is paused

        static int playAreaWidth;
        static int playAreaHeight;

        // New variables for tracking letters typed and game time
        static int totalLettersTyped = 0;
        static DateTime gameStartTime;
        static TimeSpan totalPausedDuration = TimeSpan.Zero;

        // Fruit ASCII arts
        static string[] firstFruitArt = new string[]
        {
        @"                 @@       @@  ",
        @"                @@@@@@@@%%%@  ",
        @"                @@@@@@%%%@@   ",
        @"         @@@@%%@@@@%%%@@@@    ",
        @"      @@@%%%%%%%%@@@@%%%%%@@  ",
        @"     @@%####%%%%##########%@@@",
        @"    @@%####################%@@",
        @"    @%%####################%%@",
        @"    @%#####################%%@",
        @"    @%%####################%@@",
        @"    @%%####################%@@",
        @"     @%%##################%@@ ",
        @"      @%%################%@@  ",
        @"       @@%%############%%@@   ",
        @"         @@@%%%@@@@%%%%@@     "
        };

        static string[] bananaArt = new string[]
        {
        @"         ==--       ",
        @"         ==---      ",
        @"         ==----     ",
        @"         ===---     ",
        @"         -====-     ",
        @"   ----=-=====---:: ",
        @" -=---==-======= ---",
        @"====  ===-==----- =-",
        @" =   -=--==+=-----==",
        @"    ==--===+ ====-  ",
        @"   ==--====  =====  ",
        @"  ==-=====  ====    ",
        @" @%++==     ==       "
        };

        static string[] watermelonArt = new string[]
        {
        @"          +*++          ",
        @"     +++++++#+++++++     ",
        @"   ++*+++++++++++++**+   ",
        @" %##+++*++++*+++*#++#%*+ ",
        @"%%%*+*#++++**+++++#++*@@@",
        @"@@*+%%%++++*#+++++##++%@@",
        @"@#++@@*++++%%++++%@@*++%@",
        @"@#++@@@++++@@%++**@%+++*@",
        @"@#+*%@%%+++#@@++*@@#+++%@",
        @"@%+++@@@#+++@@+++@@#++%@@",
        @" @##+*@@#+++#@++%@+++*@@ ",
        @"   @#*++%#++#*+#+++*%@   ",
        @"      %+++*++++++#%      "
        };

        // Bomb ASCII art
        static string[] bombArt = new string[]
        {
        @"                  ***  ** ",
        @"                 ***-==:=**",
        @"                +**-....-* ",
        @"             *****+.....-+*",
        @"           ***   ***+++**  ",
        @"          ***        *     ",
        @"         -*#@              ",
        @"         -#@@@             ",
        @"      @@@@@@@@@@           ",
        @"    @@@@%%%@@@@@@@         ",
        @"  @@@#**+++*#%@@@@@@       ",
        @" @@%#+=-:::-+*%@@@@@@      ",
        @" @@#+=:    :=+#@@@@@@@     ",
        @" @@#+=:.  .:=*#@@@@@@@     ",
        @" @@%#+=----=*#%@@@@@@@     ",
        @" @@@%##****#%@@@@@@@@      ",
        @"  @@@@@@@@@@@@@@@@@@@      ",
        @"   @@@@@@@@@@@@@@@@        ",
        @"     @@@@@@@@@@@@          "
        };

        public static void PlayFruitTyper()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;

            while (true)
            {
                // Show main menu
                int menuChoice = FruitTyperMenu();

                if (menuChoice == 1)
                {
                    StartGame();
                }
                else if (menuChoice == 2)
                {
                    ShowRules();
                }
                else if (menuChoice == 3)
                {
                    // Exit the game
                    SelectGameMenu();
                    break;
                }
            }
        }

        public static int FruitTyperMenu()
        {
            SoundPlayer spacetypebgm = new SoundPlayer(@"C:\Users\Lei\source\repos\HotKeys\HotKeys\BGM&SFX\Fruit_Ninja_Kinect_8-Bit_Music_Remix_[_YTBMP3.org_].wav");
            spacetypebgm.PlayLooping();
            int selectedOption = 0; // Index of the currently selected option
            string[] options = { "Start Game", "View Rules", "Back to Select Game Menu" };

            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                string title = "Welcome to Fruit Typer!";
                Console.SetCursorPosition((Console.WindowWidth - title.Length) / 2, Console.WindowHeight / 2 - 6);
                Console.WriteLine(title);

                // Display the menu options
                for (int i = 0; i < options.Length; i++)
                {
                    int posY = Console.WindowHeight / 2 - 2 + i;
                    Console.SetCursorPosition((Console.WindowWidth - options[i].Length) / 2 - 3, posY);

                    if (i == selectedOption)
                    {
                        // Highlight the selected option
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("> " + options[i] + " <");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("   " + options[i]);
                    }
                }

                // Wait for user input
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedOption = (selectedOption == 0) ? options.Length - 1 : selectedOption - 1;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedOption = (selectedOption + 1) % options.Length;
                }
                else if (keyPressed == ConsoleKey.Escape)
                {
                    selectedOption = options.Length - 1; // Set to 'Exit' option
                    break;
                }

            } while (keyPressed != ConsoleKey.Enter);

            return selectedOption + 1; // Return 1-based index for consistency
        }

        static void StartGame()
        {
            do
            {
                // Reset game variables
                score = 0;
                lives = 3;
                gameOver = false;
                isPaused = false;
                fallingObjects.Clear();
                totalLettersTyped = 0; // Reset total letters typed
                totalPausedDuration = TimeSpan.Zero; // Reset total paused duration
                gameStartTime = DateTime.Now; // Record the start time

                playAreaWidth = Console.WindowWidth - 2; // Adjust for border
                playAreaHeight = Console.WindowHeight - 4; // Adjust for border and info

                DrawBorder();

                // Cancellation token for thread management
                using (CancellationTokenSource cts = new CancellationTokenSource())
                {
                    // Start object generation
                    Thread objectGenerator = new Thread(() => GenerateObjects(cts.Token));
                    objectGenerator.Start();

                    // Start player input monitoring
                    Thread inputThread = new Thread(() => PlayerInput(cts.Token));
                    inputThread.Start();

                    // Game loop
                    while (!gameOver)
                    {
                        if (!isPaused)
                        {
                            UpdateObjects();
                            DrawObjects();
                        }
                        else
                        {
                            ShowPauseMenu();
                        }
                        Thread.Sleep(100);
                    }

                    // Cancel threads
                    cts.Cancel();

                    // Wait for threads to finish
                    objectGenerator.Join();
                    inputThread.Join();
                }

                // Calculate letters per minute, excluding paused time
                TimeSpan totalGameTime = (DateTime.Now - gameStartTime) - totalPausedDuration;
                double totalTimeInMinutes = totalGameTime.TotalMinutes;
                double lettersPerMinute = totalTimeInMinutes > 0 ? totalLettersTyped / totalTimeInMinutes : 0;

                // Display game over message
                Console.Clear();
                Console.SetCursorPosition(playAreaWidth / 2 - 5, playAreaHeight / 2 - 2);
                Console.WriteLine("Game Over!");
                Console.SetCursorPosition(playAreaWidth / 2 - 7, playAreaHeight / 2 - 1);
                Console.WriteLine($"Your Score: {score}");
                Console.SetCursorPosition(playAreaWidth / 2 - 12, playAreaHeight / 2);
                Console.WriteLine($"Letters Per Minute: {lettersPerMinute:F2}");
                Console.SetCursorPosition(playAreaWidth / 2 - 18, playAreaHeight / 2 + 2);
                Console.WriteLine("Press 'R' to Replay or any other key to return to Main Menu.");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key != ConsoleKey.R)
                {
                    break; // Exit to main menu
                }

            } while (true);
        }

        static void ShowRules()
        {
            Console.Clear();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            string[] title = new string[]
            {
                "FRUIT TYPER RULES",
                "",
                "1. Fruits and bombs will fall from the top of the screen.",
                "2. Type the letter shown on each falling object to slice it.",
                "3. Slice fruits to gain points.",
                "4. Avoid slicing bombs, as they will cost you a life.",
                "5. If you lose all your lives, the game is over.",
                "6. Press 'Escape' to pause the game.",
                "In the pause menu, press 'C' to continue or 'E' to exit",
                "",
                "Press any key to return to the Main Menu..."
            };

            CenterDisplay(title);
            Console.ReadKey(true);
        }

        static void DrawBorder()
        {
            Console.Clear();
            // Top border
            Console.SetCursorPosition(0, 1);
            Console.Write("+" + new string('-', playAreaWidth) + "+");

            // Side borders
            for (int i = 2; i <= playAreaHeight + 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("|");
                Console.SetCursorPosition(playAreaWidth + 1, i);
                Console.Write("|");
            }

            // Bottom border
            Console.SetCursorPosition(0, playAreaHeight + 2);
            Console.Write("+" + new string('-', playAreaWidth) + "+");
        }

        static void GenerateObjects(CancellationToken token)
        {
            // List of different fruit ASCII arts
            List<string[]> fruitArts = new List<string[]>
        {
            firstFruitArt,
            bananaArt,
            watermelonArt
            // Add more fruits if desired
        };

            while (!gameOver && !token.IsCancellationRequested)
            {
                if (!isPaused)
                {
                    int objectType = rand.Next(0, 10); // Adjusted bomb spawn rate
                    FallingObject fallingObject;

                    if (objectType == 0)
                    {
                        // Generate a bomb (10% chance)
                        string[] art = bombArt;
                        fallingObject = CreateFallingObject(art, isBomb: true);
                    }
                    else
                    {
                        // Generate a fruit
                        string[] fruitArt = fruitArts[rand.Next(fruitArts.Count)];
                        fallingObject = CreateFallingObject(fruitArt);
                    }

                    if (fallingObject != null)
                    {
                        lock (fallingObjects)
                        {
                            fallingObjects.Add(fallingObject);
                        }
                    }
                }
                Thread.Sleep(rand.Next(700, 1500)); // Adjust spawn rate for better gameplay
            }
        }

        static FallingObject CreateFallingObject(string[] art, bool isBomb = false)
        {
            int objectWidth = art.Max(line => line.Length);
            int xPosition = rand.Next(1, playAreaWidth - objectWidth + 1);
            char letter = (char)rand.Next(65, 91); // Random uppercase letter

            // Ensure the new object doesn't overlap with existing ones
            bool positionFound = false;
            int attempts = 0;

            while (!positionFound && attempts < 10)
            {
                positionFound = true;
                lock (fallingObjects)
                {
                    foreach (var obj in fallingObjects)
                    {
                        // Check horizontal overlap
                        int existingStart = obj.XPosition;
                        int existingEnd = obj.XPosition + obj.Art[0].Length;

                        int newStart = xPosition;
                        int newEnd = xPosition + objectWidth;

                        // If objects are too close horizontally
                        if (newStart < existingEnd && newEnd > existingStart)
                        {
                            positionFound = false;
                            break;
                        }
                    }
                }

                if (!positionFound)
                {
                    // Try a new random position
                    xPosition = rand.Next(1, playAreaWidth - objectWidth + 1);
                    attempts++;
                }
            }

            if (positionFound)
            {
                return new FallingObject(art, xPosition, letter, isBomb);
            }
            else
            {
                // Could not find a suitable position; skip spawning this object
                return null;
            }
        }

        static void PlayerInput(CancellationToken token)
        {
            while (!gameOver && !token.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        isPaused = true;
                    }
                    else if (!isPaused)
                    {
                        char keyChar = char.ToUpper(keyInfo.KeyChar);

                        // Increment total letters typed
                        totalLettersTyped++;

                        lock (fallingObjects)
                        {
                            for (int i = 0; i < fallingObjects.Count; i++)
                            {
                                if (fallingObjects[i].Letter == keyChar)
                                {
                                    if (fallingObjects[i].IsBomb)
                                    {
                                        // Player hit a bomb
                                        lives--;
                                        if (lives <= 0)
                                        {
                                            gameOver = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        // Player sliced a fruit
                                        score++;
                                    }
                                    fallingObjects.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(50);
            }
        }

        static void ShowPauseMenu()
        {
            DateTime pauseStartTime = DateTime.Now;

            Console.Clear();
            DrawBorder();
            string pauseMessage = "Game Paused";
            string optionsMessage = "Press 'C' to Continue or 'E' to Exit";
            Console.SetCursorPosition((playAreaWidth - pauseMessage.Length) / 2, playAreaHeight / 2);
            Console.Write(pauseMessage);
            Console.SetCursorPosition((playAreaWidth - optionsMessage.Length) / 2, (playAreaHeight / 2) + 1);
            Console.Write(optionsMessage);

            bool validInput = false;
            while (!validInput)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pauseKeyInfo = Console.ReadKey(true);
                    if (pauseKeyInfo.Key == ConsoleKey.C)
                    {
                        isPaused = false;
                        validInput = true;
                        Console.Clear();
                        DrawBorder();

                        // Update total paused duration
                        DateTime pauseEndTime = DateTime.Now;
                        totalPausedDuration += pauseEndTime - pauseStartTime;
                    }
                    else if (pauseKeyInfo.Key == ConsoleKey.E)
                    {
                        gameOver = true;
                        validInput = true;

                        // Update total paused duration
                        DateTime pauseEndTime = DateTime.Now;
                        totalPausedDuration += pauseEndTime - pauseStartTime;
                    }
                }
                Thread.Sleep(50);
            }
        }

        static void UpdateObjects()
        {
            lock (fallingObjects)
            {
                for (int i = 0; i < fallingObjects.Count; i++)
                {
                    fallingObjects[i].YPosition++;
                    if (fallingObjects[i].YPosition > playAreaHeight - fallingObjects[i].Art.Length)
                    {
                        // Object reached the bottom
                        if (!fallingObjects[i].IsBomb)
                        {
                            lives--;
                            if (lives <= 0)
                            {
                                gameOver = true;
                                break;
                            }
                        }
                        fallingObjects.RemoveAt(i);
                        i--; // Adjust index after removal
                    }
                }
            }
        }

        static void DrawObjects()
        {
            // Clear inside play area without erasing the border
            for (int i = 2; i <= playAreaHeight + 1; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.Write(new string(' ', playAreaWidth));
            }

            lock (fallingObjects)
            {
                foreach (var obj in fallingObjects)
                {
                    // Set color based on object type
                    ConsoleColor objectColor = ConsoleColor.Green; // Default to green for watermelon

                    if (obj.Art == firstFruitArt)
                        objectColor = ConsoleColor.Red; // First fruit is red
                    else if (obj.Art == bananaArt)
                        objectColor = ConsoleColor.Yellow; // Banana is yellow
                    else if (obj.Art == watermelonArt)
                        objectColor = ConsoleColor.Green; // Watermelon remains green
                    else if (obj.IsBomb)
                        objectColor = ConsoleColor.DarkRed; // Bomb is dark red

                    for (int i = 0; i < obj.Art.Length; i++)
                    {
                        int posY = obj.YPosition + i + 2; // Adjust for border and score line
                        if (posY >= 2 && posY < playAreaHeight + 2)
                        {
                            string line = obj.Art[i];
                            // Ensure the line doesn't overflow the play area width
                            if (obj.XPosition + line.Length > playAreaWidth + 1)
                            {
                                line = line.Substring(0, playAreaWidth + 1 - obj.XPosition);
                            }
                            Console.SetCursorPosition(obj.XPosition, posY);
                            Console.ForegroundColor = objectColor;
                            Console.Write(line);
                        }
                    }
                    // Display the letter to type, ensuring it doesn't hit the border
                    int letterPosX = obj.XPosition + obj.Art.Max(line => line.Length) / 2;
                    int letterPosY = obj.YPosition + obj.Art.Length + 2; // Adjust for border
                    if (letterPosX >= playAreaWidth)
                    {
                        letterPosX = playAreaWidth - 1;
                    }
                    if (letterPosY >= 2 && letterPosY < playAreaHeight + 2)
                    {
                        Console.SetCursorPosition(letterPosX, letterPosY);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(obj.Letter);
                    }
                }
            }
            // Reset color and display score and lives
            Console.ResetColor();
            Console.SetCursorPosition(1, 0);
            Console.Write($"Score: {score}  Lives: {lives}");
        }
    }

    class FallingObject
    {
        public string[] Art { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public char Letter { get; set; }
        public bool IsBomb { get; set; }

        public FallingObject(string[] artLines, int xPosition, char letter, bool isBomb = false)
        {
            Art = artLines;
            XPosition = xPosition;
            YPosition = 0;
            Letter = letter;
            IsBomb = isBomb;
        }
    }
}
