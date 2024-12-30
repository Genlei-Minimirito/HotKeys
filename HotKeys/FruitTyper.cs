using System;
using System.Collections.Generic;
using System.Media;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using static MainGame.Program;

namespace FruitTyping
{       
    public class FruitTyper
    {
        static List<FallingObject> fallingObjects = new List<FallingObject>();
        static Random rand = new Random();
        static int score = 0;
        static int highScore = 0; // Stores the highest score

        static int lives = 3;

        static bool gameOver = false;
        static bool isPaused = false; // Flag to check if game is paused

        static int playAreaWidth;
        static int playAreaHeight;

        // Fruit ASCII arts
        static string[] firstFruitArt = new string[]
        {
        @"    @@       @@  ",
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

            SoundPlayer spacetypebgm = new SoundPlayer(@"C:\Users\Lei\source\repos\HotKeys\HotKeys\BGM&SFX\Fruit_Ninja_Kinect_8-Bit_Music_Remix_[_YTBMP3.org_].wav");
            spacetypebgm.PlayLooping();

            // Game loop
            while (true) // Infinite loop to allow restarting the game
            {
                ResetGame();

                Console.CursorVisible = false;
                playAreaWidth = Console.WindowWidth - 2; // Adjust for border
                playAreaHeight = Console.WindowHeight - 4; // Adjust for border and info

                DrawBorder();
                ShowRules(); // Display Rules before the game starts

                // Start object generation
                Thread objectGenerator = new Thread(GenerateObjects);
                objectGenerator.Start();

                // Start player input monitoring
                Thread inputThread = new Thread(PlayerInput);
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
                // Stop all threads gracefully
                objectGenerator.Join();
                inputThread.Join();

                // Display game over message

                ShowGameOverFruit();

                // Wait for the player to press a key to restart
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

            static void GenerateObjects()
            {
                // List of different fruit ASCII arts
                List<string[]> fruitArts = new List<string[]>
        {
            firstFruitArt,
            bananaArt,
            watermelonArt
            // Add more fruits if desired
        };

                while (!gameOver)
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
                    Thread.Sleep(rand.Next(700, 1500)); // Adjusted spawn rate for better gameplay
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
                            if (Math.Abs(existingStart - newStart) < objectWidth || Math.Abs(existingEnd - newEnd) < objectWidth)
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

            static void PlayerInput()
            {
                while (!gameOver)
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
                            bool objectInteracted = false;
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
                                                GameOver();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            // Player sliced a fruit
                                            score++;
                                        }
                                        fallingObjects.RemoveAt(i);
                                        objectInteracted = true;
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
                        }
                        else if (pauseKeyInfo.Key == ConsoleKey.E)
                        {
                            gameOver = true;
                            validInput = true;
                        }
                    }
                    Thread.Sleep(50);
                }
            }
            static void ShowRules()
            {
                Console.Clear();
                DrawBorder();
                string[] rules = new string[]
                {
        "Welcome to Fruit Typer!",
        "Rules:",
        "1. Fruits will fall from the top of the screen.",
        "2. Type the letter shown below each fruit to slice it and gain points.",
        "3. Be careful! If you type the letter for a bomb, you will lose a life.",
        "4. If a fruit reaches the bottom of the screen, you also lose a life.",
        "5. The game ends when you run out of lives.",
        "",
        "Controls:",
        "1. Press the letter for a falling object to interact with it.",
        "2. Press 'Escape' to pause the game.",
        "",
        "Goodluck Keyboard Ninja!"
                };

                int startY = (playAreaHeight - rules.Length) / 2; // Center vertically
                int centerX = playAreaWidth / 2; // Center horizontally

                foreach (var line in rules)
                {
                    Console.SetCursorPosition(centerX - line.Length / 2, startY++);
                    Console.WriteLine(line);
                }

                Console.SetCursorPosition(centerX - 12, startY + 2); // Position the "Press any key" message
                Console.WriteLine("Press any key to start...");
                Console.ReadKey(true);
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
                                    GameOver();
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
            static void ResetGame()
            {

                lock (fallingObjects)
                {
                    fallingObjects.Clear(); // Clear all existing falling objects
                }

                score = 0;              // Reset score
                lives = 3;              // Reset lives
                gameOver = false;       // Reset game over flag
                isPaused = false;       // Reset paused flag
               
            }

            static void GameOver()
            {
                if (score > highScore)
                {
                    highScore = score;
                }

                gameOver = true;
                ShowGameOverFruit();

            }
            static void ShowGameOverFruit()
            {
                Console.Clear();
                Console.SetCursorPosition(playAreaWidth / 2 - 5, playAreaHeight / 2);
                Console.WriteLine("Game Over!");
                Console.SetCursorPosition(playAreaWidth / 2 - 7, playAreaHeight / 2 + 1);
                Console.WriteLine($"Your Score: {score}");
                Console.SetCursorPosition(playAreaWidth / 2 - 12, playAreaHeight / 2 + 2);
                Console.WriteLine("Press any key...");
                Console.ReadKey(true);
                ShowMainMenu();
            }
        }

       public class FallingObject
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
}
