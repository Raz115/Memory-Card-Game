// Author: Raza Hussain Mirza
// File Name: main.cs
// Project Name: PASS1-Memory
// Creation Date: Sept. 15, 2022
// Modified Date: Oct. 15, 2022

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Console;

namespace PASS1_Memory
{
    class Program
    {
        //Store 6 states in which the game may be in 
        const int INSTRUCTIONS = 2;
        const int SINGLE_PLAYER = 3;
        const int MULTI_PLAYER = 4;
        const int STATISTICS = 5;
        const int EXIT = 6;
        const int MENU = 1;

        //Store current state the game is in 
        static int gameState = MENU;

        //Store whether game has ended 
        static bool gameOver = false;

        //Store random number generated
        static Random rng = new Random();

        //Store list containing deck of cards
        static List<int> deck = new List<int>();

        //Store the 2 indices of the list deck which are being swapped
        static int shuffleRng1;
        static int shuffleRng2;

        //Store shuffled deck in a 2D array
        static int[,] shuffledDeck = new int[4, 13];

        //Store whether card values in 2d array are in a visible state, invisible state or matched state
        static int[,] cardStatus = new int[4, 13];    

        //Store deck to display cards flipped backwards
        static string[,] invisDeck = new string[4, 13];
        
        //Store cards which have been chosen by user 
        static string[] cardChosen = new string[2];

        //Split chosen cards and store into row colum 
        static int[] chosenRow = new int[2];
        static int[] chosenColumn = new int[2];

        //Store row and columns in array based of cards
        static int[] cardOne = new int[2];
        static int[] cardTwo = new int[2];

        //Store whether user inpt is correct 
        static bool inputValid = true;

        //Prepares File IO 
        static StreamWriter outFile;
        static StreamReader inFile;

        //Store statistics used in File IO
        string fileName = "statistics";
        static int SpGamesPlayed;
        static int SpCardFlips;
        static int lowestFlipCount;
        static int MPGamesPlayed;
        static double P1Wins;
        static double P2Wins;

        //Stores statisitcs in array
        static string[] statDetails = new string [6];
        static void Main(string[] args)
        {
            //Sets up suit symbols
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            //Calculates what state the game curently is in and runs all other subprograms
            SwitchGameState();
        }

        //Pre: None
        //Post: None
        //Desc: Creates array with 52 indices storing values from 0 to 51
        private static void CreateDeck()
        {
            //Loop to add 52 cards to list 
            for (int deckCount = 0; deckCount < 52; deckCount++)
            {
                //Stores deckCount value in to a new index under the list deck
                deck.Add(deckCount);
            }
        }

        //Pre: None
        //Post: None
        //Desc: Shuffles deck array by swapping index values 
        private static void ShuffleDeck()
        {
            //Loop to swap different deck value index values to shuffle
            for (int i = 0; i < 1000; i++)
            {
                //Calculate 2 random numbers between 0 and 51 
                shuffleRng1 = rng.Next(0, 52);
                shuffleRng2 = rng.Next(0, 52);

                //Store previous deck value and swaps deck values in order to randomize deck
                int placeholder = deck[shuffleRng1];
                deck[shuffleRng1] = deck[shuffleRng2];
                deck[shuffleRng2] = placeholder;
            }
        }

        //Pre: None
        //Post: None
        //Desc: Stores shuffled deck in to a 2d array
        private static void DealCards()
        {
            //Loop to create 2D row array 
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 13; x++)
                {
                    //Stored vlaues from deck in to indices within a 2d array
                    shuffledDeck[y, x] = deck[(x * 4 + y)];
                }
            }
        }

        //Pre: None
        //Post: None
        //Desc: Displays menu allowing one to select game options
        private static void DisplayMenu()
        {
            //Store whether parse conversion worked or failed
            bool isValid;

            //Store user choice on menu page
            int convertValue;

            //Changes color
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkRed;

            //Display game title 
            Console.WriteLine("\n                                      ▄████▄   ▒█████   ███▄    █  ▄████▄  ▓█████▄▄▄█████▓ ██▀███   ▄▄▄     ▄▄▄█████▓ ██▓ ▒█████   ███▄    █\n                                     ▒██▀ ▀█  ▒██▒  ██▒ ██ ▀█   █ ▒██▀ ▀█  ▓█   ▀▓  ██▒ ▓▒▓██ ▒ ██▒▒████▄   ▓  ██▒ ▓▒▓██▒▒██▒  ██▒ ██ ▀█   █\n                                     ▒▓█    ▄ ▒██░  ██▒▓██  ▀█ ██▒▒▓█    ▄ ▒███  ▒ ▓██░ ▒░▓██ ░▄█ ▒▒██  ▀█▄ ▒ ▓██░ ▒░▒██▒▒██░  ██▒▓██  ▀█ ██▒\n                                     ▒▓▓▄ ▄██▒▒██   ██░▓██▒  ▐▌██▒▒▓▓▄ ▄██▒▒▓█  ▄░ ▓██▓ ░ ▒██▀▀█▄  ░██▄▄▄▄██░ ▓██▓ ░ ░██░▒██   ██░▓██▒  ▐▌██▒\n                                     ▒ ▓███▀ ░░ ████▓▒░▒██░   ▓██░▒ ▓███▀ ░░▒████▒ ▒██▒ ░ ░██▓ ▒██▒ ▓█   ▓██▒ ▒██▒ ░ ░██░░ ████▓▒░▒██░   ▓██░\n                                     ░ ░▒ ▒  ░░ ▒░▒░▒░ ░ ▒░   ▒ ▒ ░ ░▒ ▒  ░░░ ▒░ ░ ▒ ░░   ░ ▒▓ ░▒▓░ ▒▒   ▓▒█░ ▒ ░░   ░▓  ░ ▒░▒░▒░ ░ ▒░   ▒ ▒\n                                       ░  ▒     ░ ▒ ▒░ ░ ░░   ░ ▒░  ░  ▒    ░ ░  ░   ░      ░▒ ░ ▒░  ▒   ▒▒ ░   ░     ▒ ░  ░ ▒ ▒░ ░ ░░   ░ ▒░\n                                     ░        ░ ░ ░ ▒     ░   ░ ░ ░           ░    ░        ░░   ░   ░   ▒    ░       ▒ ░░ ░ ░ ▒     ░   ░ ░ \n                                     ░ ░          ░ ░           ░ ░ ░         ░  ░           ░           ░  ░         ░      ░ ░           ░");

            //Changes color
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            //Displays menu options
            Console.WriteLine("                                                             ___           _                   _   _\n                                                             |_ _|_ __  ___| |_ _ __ _   _  ___| |_(_) ___  _ __  ___\n                                                              | || '_ !/ __| __| '__| | | |/ __| __| |/ _ '| '_ '/ __|\n                                                              | || | | !__ | |_| |  | |_| | (__| |_| | (_) | | | !__ !       (Press 1)\n                                                             |___|_| |_|___/!__|_|   !__,_|!___|!__|_|!___/|_| |_|___/");
            Console.WriteLine("                                                            ____  _             _        ____  _\n                                                           / ___|(_)_ __   __ _| | ___  |  _ '| | __ _ _   _  ___ _ __\n                                                           !___ !| | '_ ' / _` | |/ _ ! | |_) | |/ _` | | | |/ _ | '__|\n                                                            ___) | | | | | (_| | |  __/ |  __/| | (_| | |_| |  __| |         (Press 2)\n                                                           |____/|_|_| |_|!__, |_|!___| |_|   |_|!__,_|'__, |!___|_|\n                                                                          |___/                        |___/");
            Console.WriteLine("                                                              _____                 ____  _\n                                                              |__  _;_      _____   |  _ '| | __ _ _   _  ___ _ __\n                                                                | | ' ' /' / / _ '  | |_) | |/ _` | | | |/ _ | '__|\n                                                                | |  | V  V | (_) | |  __/| | (_| | |_| |  __| |             (Press 3)\n                                                                |_|   '_/'_/ '___/  |_|   |_|'__,_|'__, |!___|_|\n                                                                                                   |___/");
            Console.WriteLine("                                                                     ____  _        _   _     _   _\n                                                                    / ___|| |_ __ _| |_(_)___| |_(_) ___ ___\n                                                                    !___ !| __/ _` | __| / __| __| |/ __/ __|\n                                                                     ___) | || (_| | |_| !__ | |_| | (__!__ !                (Press 4)\n                                                                    |____/ !__'__,_|!__|_|___/!__|_|'___|___/");
            Console.Write(" _____       _  _\n|  ___| _ _ |_|| |_\n|  ___||_'_|| ||  _|          (Press 5)\n|_____||_,_||_||_|");
            Console.ResetColor();
            Console.Write("                                                                   Enter choice: ");

            //Converts user input from string to integer
            isValid = int.TryParse(Console.ReadLine(), out convertValue);

            //Check whether input is valid and inside proper parameters
            if (isValid = true && convertValue > 0 && convertValue < 6)
            {
                //Change game state
                gameState = convertValue + 1;
            }

            //Loop when in;put is invalid or outside proper parameters
            while (isValid == false || !(convertValue > 0 && convertValue < 6))
             {
                //Display Invalid output
                Console.Write("\n                                                                              Incorrect Value Try Again: ");

                //Ask for input
                isValid = int.TryParse(Console.ReadLine(), out convertValue);

                //Check whether input is valid and inside proper parameters
                if (isValid == true && convertValue >= 1 && convertValue < 6)
                {
                    //Change game state and stop loop
                    gameState = convertValue + 1;
                    break;
                }

             }
            
            //Modify color
            Console.ResetColor();
        }
        //Pre: None
        //Post: None
        //Desc: Create blank cards in game board position
        private static void CreateBlankBoard()
        {
            //Loop to display 4 columns 
            for (int y = 0; y < 4; y++)
            {
                //Loop to display 13 rows
                for (int x = 0; x < 13; x++)
                {
                    //Display insvisible card in current location found via loops
                    invisDeck[y, x] = "|  |";
                }
            }
        }
        //Pre: None
        //Post: None
        //Desc: Displays game board in proper position
        private static void DisplayBoard()
        {
            //Create blank cards in game board position  
            CreateBlankBoard();

            //Display row values
            Console.WriteLine("    " + " A  " + " B  " + " C  " + " D  " + " E  " + " F  " + " G  " + " H  " + " I  " + " J  " + " K  " + " L  " + " M  ");

            //Loop to display 4 columns 
            for (int i = 0; i < 4; i++)
            {
                //Display cards tops
                Console.WriteLine("   " + " __ " + " __ " + " __ " + " __ " + " __ " + " __ " + " __ " + " __ " + " __ " + " __ " + " __ " + " __ " + " __ ");
                Console.Write(i + "  ");

                //Loop to display 12 row
                for (int x = 0; x < 13; x++)
                {
                    //Check card status is invisible
                    if (cardStatus[i, x] == 0)
                    {
                        //Display invisible card
                        Console.Write(invisDeck[i, x]);
                    }
                    //Check card status is visible
                    else if (cardStatus[i, x] == 1)
                    {
                        //Store row and columns of card selected
                        int row;
                        string column;
                        
                        //Calculates rows and columns of cards selected 
                        row = shuffledDeck[i, x] / 13;
                        column = Convert.ToString(shuffledDeck[i, x] % 13);

                        //Stores suits of cards based off row
                        string[] suits = { "♥", "♠", "♦", "♣" };

                       //Switch to speacial card types(Ace, King, Queen....) based off column 
                        switch (column)
                        {
                            case "0":
                                column = "A";
                                break;
                            case "1":
                                column = "2";
                                break;
                            case "2":
                                column = "3";
                                break;
                            case "3":
                                column = "4";
                                break;
                            case "4":
                                column = "5";
                                break;
                            case "5":
                                column = "6";
                                break;
                            case "6":
                                column = "7";
                                break;
                            case "7":
                                column = "8";
                                break;
                            case "8":
                                column = "9";
                                break;
                            case "9":
                                column = "T";
                                break;
                            case "10":
                                column = "J";
                                break;
                            case "11":
                                column = "Q";
                                break;
                            case "12":
                                column = "K";
                                break;
                        }
                        //Display card in visible status in proper format
                        Console.Write("|");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(suits[row] + column);
                        Console.ResetColor();
                        Console.Write("|");
                    }
                    //Check  card is matched
                    else if (cardStatus[i, x] == 2)
                    {
                        //Display card in matched status in proper format
                        Console.Write("|");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("XX");
                        Console.ResetColor();
                        Console.Write("|");
                    }
                }
                //Display card bottoms 
                Console.WriteLine("\n   " + "|__|" + "|__|" + "|__|" + "|__|" + "|__|" + "|__|" + "|__|" + "|__|" + "|__|" + "|__|" + "|__|" + "|__|" + "|__|");
            }
        }
        //Pre: None
        //Post: None
        //Desc: Checks card selected, finds status and checks match
        private static void GetBoardPosition()
        {
            //Store if current set of 52 cards is over 
            bool roundOver = false;

            //Store where it is player 1's turn
            bool addP1Turn = false;
            
            //Store current player turn
            string playerTurn = "Player 1: ";

            //Store total turn and indivsual turns per player
            int turn = 0;
            int p1Turn = 0;
            int p2Turn = 0;

            //Store scores
            int singlePlayerScore = 0;
            int playerOneScore = 0;
            int playerTwoScore = 0;

            //Stores how many of the two cards in every single turn of the game have been selected
            int i = 0;

            //Loop if current game is not over
            while (roundOver == false)
            {
                //Check whether game state is in multi player
                if (gameState == MULTI_PLAYER)
                {
                    //Check whether turn value is divisible by 2
                    if (turn % 2 == 0)
                    {
                        //Set player turn to player 1 and add turn to player 1
                        playerTurn = "Player 1: ";
                        addP1Turn = true;
                    }
                    else
                    {
                        //Set player turn to player w
                        playerTurn = "Player 2: ";
                    }
                }

                //Loop whwen 2 cards havent been selected
                while (i < 2)
                {
                    //Check whether it is the first card and the input is false
                    if (i == 0 || inputValid == false)
                    {
                        //Display board
                        DisplayBoard();

                        //Set input to valid 
                        inputValid = true;
                    }
                    
                    //Check whether game state is single play
                    if (gameState == SINGLE_PLAYER)
                    {
                        //Display in game statistic for single player
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write("\nPlayer One Flips: ");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(turn);
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write("\nPlayer One Matches: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(singlePlayerScore);
                    }
                    //Check whether game state is multi player 
                    else if (gameState == MULTI_PLAYER)
                    {
                        // Display in game statistics for multiplayer
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write("\nPlayer One Flips: ");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(p1Turn);
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write("                \nPlayer Two Flips: ");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write(p2Turn);
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write("\n\nPlayer One Matches: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(playerOneScore);
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write("                \nPlayer Two Matches: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(playerTwoScore);
                    }
                   
                    //Displays whos turn it is and current insturctions 
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n" + playerTurn);
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("Enter the column number and then row letter to select a card (Ex. 3B):  ");
                    Console.ResetColor();

                    //Ask and check card input
                    cardChosen[i] = Console.ReadLine();
                    cardChosen[i] = cardChosen[i].TrimEnd(Environment.NewLine.ToCharArray());
                    cardChosen[i] = cardChosen[i].ToUpper();

                    //Check whether card is empty or null
                    if (!(string.IsNullOrEmpty(cardChosen[i])))
                    {
                        //Check whether card length is not two
                        if (cardChosen[i].Length != 2)
                        {
                            //Display new instructions to user 
                            Console.WriteLine("Try Again From the First Card, Press Enter ");
                            Console.ReadLine();
                            Console.Clear();

                            //Set input validity to false
                            inputValid = false;

                            //Reset cards selected
                            cardStatus[chosenRow[0], chosenColumn[0]] = 0;
                            cardStatus[chosenRow[1], chosenColumn[1]] = 0;
                            cardChosen = new string[2];

                            //Resect card selected number to zero
                            i = 0;
                        }
                        //Check whether same card has been selected twice
                        else if (cardChosen[0] == cardChosen[1])
                        {
                            //Display new instructions
                            Console.WriteLine("Try Again From the First Card, Press Enter ");
                            Console.ReadLine();
                            Console.Clear();

                            //Set input validity to false
                            inputValid = false;

                            //Reset cards selected
                            cardStatus[chosenRow[0], chosenColumn[0]] = 0;
                            cardStatus[chosenRow[1], chosenColumn[1]] = 0;
                            cardChosen = new string[2];

                            //Resect card selected number to zero
                            i = 0;
                        }
                        else
                        {   
                            //Check what column has been selected 
                            if (cardChosen[i].Contains("0"))
                            {
                                chosenRow[i] = 0;
                            }
                            else if (cardChosen[i].Contains("1"))
                            {
                                chosenRow[i] = 1;
                            }
                            else if (cardChosen[i].Contains("2"))
                            {
                                chosenRow[i] = 2;
                            }
                            else if (cardChosen[i].Contains("3"))
                            {
                                chosenRow[i] = 3;
                            }

                            //Check what row has been selected
                            if (cardChosen[i].Contains("A"))
                            {
                                chosenColumn[i] = 0;
                            }
                            else if (cardChosen[i].Contains("B"))
                            {
                                chosenColumn[i] = 1;
                            }
                            else if (cardChosen[i].Contains("C"))
                            {
                                chosenColumn[i] = 2;
                            }
                            else if (cardChosen[i].Contains("D"))
                            {
                                chosenColumn[i] = 3;
                            }
                            else if (cardChosen[i].Contains("E"))
                            {
                                chosenColumn[i] = 4;
                            }
                            else if (cardChosen[i].Contains("F"))
                            {
                                chosenColumn[i] = 5;
                            }
                            else if (cardChosen[i].Contains("G"))
                            {
                                chosenColumn[i] = 6;
                            }
                            else if (cardChosen[i].Contains("H"))
                            {
                                chosenColumn[i] = 7;
                            }
                            else if (cardChosen[i].Contains("I"))
                            {
                                chosenColumn[i] = 8;
                            }
                            else if (cardChosen[i].Contains("J"))
                            {
                                chosenColumn[i] = 9;
                            }
                            else if (cardChosen[i].Contains("K"))
                            {
                                chosenColumn[i] = 10;
                            }
                            else if (cardChosen[i].Contains("L"))
                            {
                                chosenColumn[i] = 11;
                            }
                            else if (cardChosen[i].Contains("M"))
                            {
                                chosenColumn[i] = 12;
                            }
                            
                            //Store card one row and column value 
                            if (i == 0)
                            {
                                cardOne[0] = chosenRow[0];
                                cardOne[1] = chosenColumn[0];
                            }

                            //Store card two row and column 
                            else if (i == 1)
                            {
                                cardTwo[0] = chosenRow[1];
                                cardTwo[1] = chosenColumn[1];
                            }
                            
                            //Check whether cards is already matched card
                            if (cardStatus[chosenRow[i], chosenColumn[i]] == 2)
                            {
                                //Display new instructions
                                Console.WriteLine("Try Again From the First Card, Press Enter ");
                                Console.ReadLine();
                                Console.Clear();

                                //Loop to go through 4 columns
                                for (int a = 0; a < 4; a++)
                                {
                                    //Loop to go through 13 rows
                                    for (int c = 0; c < 13; c++)
                                    {
                                        //Check which card isn't in matched status
                                        if (cardStatus[a, c] != 2)
                                        {
                                            //Set card value to invisible
                                            cardStatus[a, c] = 0;
                                        }
                                    }
                                }
                                //Reset number of cards selected to zero
                                i = 0;
                            }
                            else
                            {
                                //Set card value to visible
                                cardStatus[chosenRow[i], chosenColumn[i]] = 1;
                            }

                            //Display board
                            Console.Clear();
                            DisplayBoard();
                            
                            //Add one to numebr of cards selceted
                            i++;
                        }
                    }
                }

                //Check whether number of cards selected is two 
                if (i == 2)
                {
                    //Check whether card column value is the same 
                    if (shuffledDeck[cardOne[0], cardOne[1]] % 13 == shuffledDeck[cardTwo[0], cardTwo[1]] % 13)
                    {
                        //Set both cards to matched status
                        cardStatus[cardOne[0], cardOne[1]] = 2;
                        cardStatus[cardTwo[0], cardTwo[1]] = 2;

                        //Display board adn cards (in matched status)
                        Console.Clear();
                        DisplayBoard();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Card Matched");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Press Enter to continue!");
                        Console.ResetColor();
                        Console.ReadLine();
                        Console.Clear();
                        DisplayBoard();

                        //Check whether game state is single player
                        if (gameState == SINGLE_PLAYER)
                        {
                            //Add to score
                            singlePlayerScore++;
                            Console.Clear();
                            if (singlePlayerScore >= 26)
                            {
                                //Dispay and set game over
                                Console.WriteLine("Game Over");
                                Console.WriteLine("It took " + turn + " turns");
                                Console.WriteLine("Press Enter to Continue");
                                Console.ReadLine();
                                roundOver = true;
                                gameState = STATISTICS;
                                SpGamesPlayed++;
                            }
                        }

                        //Check whether game state is multi player
                        else if (gameState == MULTI_PLAYER)
                        {
                            //Check whether turn is disivisble by 2 
                            if(turn % 2 == 0)
                            {
                                //Add to player one score
                                playerOneScore++;

                                //Remove turn so that player one can go again
                                turn--;
                            }
                            else
                            {
                                //Add to player two score
                                playerTwoScore++;

                                //Remove turn so the player two can go again
                                turn--;
                            }

                            //Check whether total mathced cards is 26
                            if (playerOneScore + playerTwoScore >= 26)
                            {
                                //Check whether player one matched more cards
                                if (playerOneScore > playerTwoScore)
                                {
                                    //Dispay player one won and and set game over 
                                    Console.Clear();
                                    Console.WriteLine("Player 1 Wins");
                                    Console.WriteLine("With a score of " + playerOneScore);
                                    Console.WriteLine("Press Enter to Continue");
                                    Console.ReadLine();
                                    roundOver = true;
                                    gameState = STATISTICS;
                                }
                                //Check whether player two matched more cards
                                else if (playerOneScore < playerTwoScore)
                                {
                                    //Dispay player two won and and set game over 
                                    Console.Clear();
                                    Console.WriteLine("Player 2 Wins");
                                    Console.WriteLine("With a score of " + playerTwoScore);
                                    Console.WriteLine("Press Enter to Continue");
                                    Console.ReadLine();
                                    roundOver = true;
                                    gameState = STATISTICS;
                                }
                                //Check whether player one and two matched the same number of cards
                                else if (playerOneScore == 13 && playerTwoScore == 13)
                                {
                                    //Dispay tie and set game over 
                                    Console.Clear();
                                    Console.WriteLine("Tie Game");
                                    Console.WriteLine("Press Enter to Continue");
                                    Console.ReadLine();
                                    roundOver = true;
                                    gameState = STATISTICS;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Loop through 4 columns
                        for (int a = 0; a < 4; a++)
                        {
                            //Loop through 13 rows
                            for (int c = 0; c < 13; c++)
                            {
                                //Check whether cards did not match
                                if (cardStatus[a, c] != 2)
                                {
                                    //Set cards to invisible
                                    cardStatus[a, c] = 0;
                                }
                            }
                        }
                        //Display no match and instructions
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Card didnt match");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Press Enter to continue!");
                        Console.ReadLine();
                    }

                    //Store turn with one more value 
                    turn++;
                    if(gameState == SINGLE_PLAYER)
                    {
                        SpCardFlips = turn;
                    }
                    //Check  game state is multiplayer
                    if (gameState == MULTI_PLAYER)
                    {
                        //Check whos turn just passed 
                        if (addP1Turn == true)
                        {
                            //Store player one turn plus one
                            p1Turn++;
                            
                            //Store to player two turn
                            addP1Turn = false;
                        }
                        else
                        {
                            //Store player two turn plus one
                            p2Turn++;
                        }
                    }
                    
                    //Reset console
                    Console.Clear();
                    Console.ResetColor();

                    //Reset chosen cards and number of chosen cards
                    cardChosen = new string[2];
                    i = 0;
                }
            }
        }
        //Pre: None
        //Post: None
        //Desc: Draws instructions game state
        private static void DrawInstructions()
        {
            //Displays instructions inside instructions game state
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n\nWelcome to Memory!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\nObejctive:");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Memory is a mathcing game consisting of 52 shuffled cards.These cards are placed face down on the table in 4 columns of 13 cards. The current player flips over two cards, one at a time. Once all cards have been flipped over the game ends.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\nInstructions:");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("1.To select a card enter column number and then row letter");
            Console.Write("2A.If the cards match, then the card will be replaced with");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" xx ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("and the player will get to take another turn");
            Console.WriteLine("2B.If the cards do not match, then the card will be fliped back over and the next turn will take place");
            Console.WriteLine("3.The game ends when all of the cards have been collected. The player with the most matching sets wins.  If both players have 13 points it will be a tied.");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\nPress ENTER to return to menu page");
            Console.ReadLine();

            //Set game state to menu
            gameState = MENU;
        }
        //Pre: None
        //Post: None
        //Desc: Write file io 
        private static void WriteStats(string fileName, int SpGamesPlayed, int SpCardFlips, int lowestFlipCount, int MPGamesPlayed, double P1Wins, double P2Wins)
        {
            try
            {
                outFile = File.CreateText(fileName);
                outFile.WriteLine(SpGamesPlayed + "," + SpCardFlips + "," + lowestFlipCount + "," + MPGamesPlayed + "," + P1Wins + "," + P2Wins);
                outFile.Close();
            }
            catch (FileNotFoundException fnf)
            {
                Console.WriteLine(fnf.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //Pre: None
        //Post: None
        //Desc: read file io 
        private static void ReadStats(string fileName, int SpGamesPlayed, int SpCardFlips, int lowestFlipCount, int MPGamesPlayed, double P1Wins, double P2Wins)
        {
            try
            {
                inFile = File.OpenText(fileName);

                string line = inFile.ReadLine();
                string [] data;
                
                int count = 0;

                while (!inFile.EndOfStream)
                {
                    line = inFile.ReadLine();
                    data = line.Split(',');

                    statDetails[count] = data[count];
                    count++;

                }

                inFile.Close();
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe.Message);
            }
            catch (FileNotFoundException fnf)
            {
                Console.WriteLine(fnf.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //Pre: None
        //Post: None
        //Desc: Draw statistics
        private static void DrawStatsScreen()
        {
            Console.WriteLine("Single Player Games Played " + SpGamesPlayed);
            Console.WriteLine("Single Player total flips" + SpCardFlips);
            Console.WriteLine("Single Player all time lowest flip count " + lowestFlipCount);
            Console.WriteLine("\nMulti Player Games Player " + MPGamesPlayed);
            Console.WriteLine("Multi Player, Player 1 win percentage" + (P1Wins/MPGamesPlayed) * 100 + "%");
            Console.WriteLine("Multi Player, Player 2 Player win percentage" + (P2Wins / MPGamesPlayed) * 100 + "%");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\nPress ENTER to return to menu page");
            Console.ReadLine();
            gameState = MENU;

        }
        //Pre: None
        //Post: None
        //Desc: Check what states game is in and runs subprograms 
        private static void SwitchGameState()
        {
            while (gameOver == false)
            {
                switch (gameState)
                {
                    case MENU:
                        Console.Clear();
                        DisplayMenu();
                        break;
                    case INSTRUCTIONS:
                        Console.Clear();
                        DrawInstructions();
                        break;

                    case SINGLE_PLAYER:
                        Console.Clear();
                        CreateDeck();
                        ShuffleDeck();
                        DealCards();
                        GetBoardPosition();
                        break;

                    case MULTI_PLAYER:
                        Console.Clear();
                        CreateDeck();
                        ShuffleDeck();
                        DealCards();
                        GetBoardPosition();
                        break;

                    case STATISTICS:
                        //WriteStats();
                        //ReadStats();
                        //DrawStatsScreen();
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("Statistics are currently under development");
                        Console.ResetColor();
                        Console.Write("Click enter to return to main menu:");
                        Console.ReadLine();
                        gameState = MENU;
                        break;

                    case EXIT:
                        gameOver = true;
                        break;
                }
            }
        }
    }
}
