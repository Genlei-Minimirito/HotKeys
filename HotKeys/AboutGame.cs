using System.Media;
using static MainGame.Program;

namespace AboutGame
{
    public class AboutGames
    {

        public static void AboutGameInfo()
        {
            Console.ForegroundColor = ConsoleColor.White;


            int selectedIndex = 0; // Index of the currently selected menu item
 
            string[] menu = new string[]
            {
            " █████  ██████   ██████  ██    ██ ████████     ████████ ██   ██ ███████      ██████   █████  ███    ███ ███████ ",
            "██   ██ ██   ██ ██    ██ ██    ██    ██           ██    ██   ██ ██          ██       ██   ██ ████  ████ ██      ",
            "███████ ██████  ██    ██ ██    ██    ██           ██    ███████ █████       ██   ███ ███████ ██ ████ ██ █████   ",
            "██   ██ ██   ██ ██    ██ ██    ██    ██           ██    ██   ██ ██          ██    ██ ██   ██ ██  ██  ██ ██      ",
            "██   ██ ██████   ██████   ██████     ██           ██    ██   ██ ███████      ██████  ██   ██ ██      ██ ███████ ",
            "                                                                                                                 ",
            "                                                                                                                 ",
            "Hot Keys is an engaging console-based typing game designed by BSCS 1B students ",
            "to enhance your typing speed and accuracy through fun and interactive gameplay. ",
            "",
            "The game features four distinct mini-games,each developed to improve different " ,
            "aspects of typingfrom mastering keyboard keys to increasing word-per-minute performance." ,
            "",
            "With its focus on both fun and skill-building, Hot Keys provides an interactive way for " ,
            "players of all levels to hone their typing abilities while enjoying a dynamic gaming experience. " ,
            "Whether you're a beginner or a seasoned typist, this game offers",
            "something for everyone!",
   
    

            };


            string[] options = {"Back to Main Menu" };

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

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;

                    string[] title = new string[]
                    {
                "Name: BREN JAREN F. MARTINEZ",
                "Contact:"
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

                    CenterDisplay(title);
                    Console.ReadKey();
                }
            }
        }
    }
}
