using System;
using System.IO;
using System.Collections.Generic;

namespace TopTrumps
{
    class Program
    {
        static void Main()
        {
            Menu();
        }

        static void Menu()
        {
            string option;

            Console.Write("----Main Menu-----\n[1] Play Game\n[2] Quit\nPlease enter an option: ");
            option = Console.ReadLine();

            //-Option Check-
            if (option == "1")
            {
                Game();
            }
            else if (option == "2")
            {
                Console.Write("\nGoodbye!");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                Console.Write("\nInvalid option entered. Defaulting to option 2. Goodbye!");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        static void CardDisplay(bool user, List<string[]> deck)
        {
            //-Variable Declaration & Initialisation-
            string name, exercise, intelligence, friendliness, drool, title, lines;

            name = deck[0][0];
            exercise = deck[0][1];
            intelligence = deck[0][2];
            friendliness = deck[0][3];
            drool = deck[0][4];

            //-Displaying The Card-
            if (user)
            {
                title = "Your-Card";
                lines = new string('-', 21);
                Console.WriteLine($"{lines}{title}{lines}");
            }
            else
            {
                title = "Computer's-Card";
                lines = new string('-', 18);
                Console.WriteLine($"\n{lines}{title}{lines}");
            }

            Console.WriteLine("Name: | {0}", name);
            Console.WriteLine(new string('-', 51));
            Console.WriteLine("Exercise: | {0}", exercise);
            Console.WriteLine(new string('-', 51));
            Console.WriteLine("Intelligence: | {0}", intelligence);
            Console.WriteLine(new string('-', 51));
            Console.WriteLine("Friendliness: | {0}", friendliness);
            Console.WriteLine(new string('-', 51));
            Console.WriteLine("Drool: | {0}", drool);
            Console.WriteLine(new string('-', 51));
        }

        static void Result(bool win, List<string[]> uDeck, List<string[]> cDeck)
        {
            if (win)
            {
                Console.WriteLine("You won this round!\n");

                //-Placing Used/Won Cards To End Of User's Deck & Removing Lost Card From Computer's Deck
                uDeck.Add(cDeck[0]);
                uDeck.Add(uDeck[0]);
                uDeck.RemoveAt(0);
                cDeck.RemoveAt(0);
            }
            else
            {
                Console.WriteLine("You lost this round!\n");

                //-Placing Used/Won Cards To End Of Computer's Deck & Removing Lost Card From User's Deck
                cDeck.Add(uDeck[0]);
                cDeck.Add(cDeck[0]);
                cDeck.RemoveAt(0);
                uDeck.RemoveAt(0);
            }

            Console.WriteLine($"Number of cards you now have: {uDeck.Count}");
            Console.WriteLine($"Number of cards computer has: {cDeck.Count}");
        }

        static char GameState(List<string[]> uDeck, List<string[]> cDeck)
        {
            //-Checking If Either Deck Has No More Cards-
            if (uDeck.Count == 0)
            {
                return 'l';
            }
            else if (cDeck.Count == 0)
            {
                return 'w';
            }
            else
            {
                return 'o';
            }
        }

        static void Game()
        {
            //-Variable Declaration & Initialisation-
            string stringCardNum, name, exercise, intelligence, friendliness, drool, stringCategory;
            int cardNum, intExercise, intIntelligence, intFriendliness, intDrool, randomCard, roundCount, category;
            char gameState;
            bool gameOver;
            string[] card, categories;
            List<string[]> cards, userDeck, compDeck;

            roundCount = 0;
            gameOver = false;
            cards = new List<string[]>(30);
            Random rnd = new Random();
            categories = new string[] { "exercise", "intelligence", "friendliness", "drool" };

            //-File Setup-
            const string FILE_PATH = "/Users/jamesharrison/Desktop/Coding/C#/TopTrumps/TopTrumps/dogs.txt";
            StreamReader reader = new StreamReader(FILE_PATH);

            //-----GAME SETUP-----
            Console.Write("\n-----Game-----\nEnter the number of cards to be played: ");
            stringCardNum = Console.ReadLine();
            cardNum = int.Parse(stringCardNum);

            //-Card Number Validation-
            if (cardNum < 4 || cardNum > 30 || cardNum % 2 != 0)
            {
                Console.WriteLine("Invalid card number entered. Returning to menu.\n");
                Menu();
            }

            //-Stat Generation-
            while (!reader.EndOfStream)
            {
                card = new string[5];

                name = reader.ReadLine();
                card[0] = name;

                intExercise = rnd.Next(1, 6);
                exercise = intExercise.ToString();
                card[1] = exercise;

                intIntelligence = rnd.Next(1, 101);
                intelligence = intIntelligence.ToString();
                card[2] = intelligence;

                intFriendliness = rnd.Next(1, 11);
                friendliness = intFriendliness.ToString();
                card[3] = friendliness;

                intDrool = rnd.Next(1, 11);
                drool = intDrool.ToString();
                card[4] = drool;

                cards.Add(card);
            }
            reader.Close();

            userDeck = new List<string[]>();
            compDeck = new List<string[]>();

            //-Card Shuffle & Dealing To User & Computer-
            for (int i = 0; i < cardNum / 2; i++)
            {
                randomCard = rnd.Next(0, cards.Count);
                userDeck.Add(cards[randomCard]);
                cards.Remove(cards[randomCard]);

                randomCard = rnd.Next(0, cards.Count);
                compDeck.Add(cards[randomCard]);
                cards.Remove(cards[randomCard]);
            }

            //-----GAME LOOP-----
            while (!gameOver)
            {
                roundCount++;
                Console.WriteLine($"\n-----ROUND {roundCount}-----");
                CardDisplay(true, userDeck);

                //-Category Validation-
                bool redo = false;
                do
                {
                    if (redo)
                    {
                        Console.Write("\nInvalid category entered.");
                    }

                    Console.Write("\n[1] Exercise\n[2] Intelligence\n[3] Friendliness\n[4] Drool\nPlease enter your chosen category: ");
                    stringCategory = Console.ReadLine();

                    redo = true;
                } while (stringCategory != "1" && stringCategory != "2" && stringCategory != "3" && stringCategory != "4");

                category = int.Parse(stringCategory);
                CardDisplay(false, compDeck);

                Console.WriteLine($"\nYour card's {categories[category - 1]} value: {userDeck[0][category]}");
                Console.WriteLine($"Computer's card's {categories[category - 1]} value: {compDeck[0][category]}");

                //-Category Comparison-
                if (category == 4)
                {
                    if (int.Parse(userDeck[0][category]) <= int.Parse(compDeck[0][category]))
                    {
                        Result(true, userDeck, compDeck);
                    }
                    else
                    {
                        Result(false, userDeck, compDeck);
                    }
                }
                else
                {
                    if (int.Parse(userDeck[0][category]) >= int.Parse(compDeck[0][category]))
                    {
                        Result(true, userDeck, compDeck);
                    }
                    else
                    {
                        Result(false, userDeck, compDeck);
                    }
                }

                //-Game State Check-
                gameState = GameState(userDeck, compDeck);

                if (gameState == 'w')
                {
                    Console.WriteLine("\nCongratulations, you have won the game! Returning to the menu.\n");
                    gameOver = true;
                }
                else if (gameState == 'l')
                {
                    Console.WriteLine("\nYou lost this game! Better luck next time. Returning to the menu.\n");
                    gameOver = true;
                }
            }

            Menu();
        }
    }
}