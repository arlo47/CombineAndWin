using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
 * - Good use of game object containing deck, hand and all methods?
 */

namespace CombineAndWin {
    class Program {

        static void Main(string[] args) {

            Game game = new Game();

            int option = -1;
            const int EXIT_VALUE = 4;

            string headerMessage = "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n" +
                                    "|                                 COMBINE & WIN                                |\n" +
                                    "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";
            Console.WriteLine(headerMessage);

            do {

                Console.WriteLine("Select an option: \n" +
                                    "\t1. Play\n" +
                                    "\t2. Manual Testing\n" +
                                    "\t3. Reset Game\n" +
                                   $"\t{EXIT_VALUE}. Exit");

                option = GetIntegerInput(Console.ReadLine(), 1, EXIT_VALUE);

                switch (option) {
                    case 1:
                        game.FillHand();
                        DiscardingRounds(game);
                        break;
                    case 2:
                        ManualTestMenu(game);
                        break;
                    case 3:
                        Console.WriteLine("Discarding hand and reseting points...");
                        game = new Game();
                        break;
                    case EXIT_VALUE:
                        Console.WriteLine("\nGoodbye!");
                        break;
                    default:
                        Console.WriteLine("Default break statement in main menu...");
                        break;
                }

            } while (option != EXIT_VALUE);



            Console.ReadKey();
        }

        #region Play Game

        static void DiscardingRounds(Game game) {

            int roundsOption = 10;
            int cardsDiscardedOption = 10;
            int discardRounds = 0;
            int cardsDiscarded = 0;

            while (discardRounds < 3) {

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"You have: {game.PrintHand() }");
                Console.ResetColor();

                Console.WriteLine($"\nWould you like to discard more cards?\n" +
                                  $"\t1. Discard cards\n" +
                                  $"\t2. Continue to point round");

                roundsOption = GetIntegerInput(Console.ReadLine(), 1, 2);

                if (roundsOption == 2)
                    break;

                cardsDiscarded = 0;             //reset cards discarded for new discard round
                cardsDiscardedOption = 10;      //reset menu option, as 0 breaks out of the loop

                while (cardsDiscarded < 4 && cardsDiscardedOption != 0) {
                    
                    Console.WriteLine($"\nDiscard Round: {discardRounds}\n" +
                                      $"Cards Discarded: {cardsDiscarded}");

                    Console.WriteLine("Pick up to 4 cards to discard. Press 0 to continue.");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n1. {game.GetHand()[0]}   " +
                                        $"2. {game.GetHand()[1]}   " +
                                        $"3. {game.GetHand()[2]}   " +
                                        $"4. {game.GetHand()[3]}   " +
                                        $"5. {game.GetHand()[4]}\n");
                    Console.ResetColor();

                    cardsDiscardedOption = GetIntegerInput(Console.ReadLine(), 0, 5);
                    int indexToDiscards = cardsDiscardedOption - 1;

                    try {
                        if (cardsDiscardedOption == 0)
                            break;                                                  //end discard round
                        else if (game.DiscardCard(indexToDiscards)) {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Card successfully removed.");
                            Console.ResetColor();

                            cardsDiscarded++;
                        }
                    }
                    catch (Exception ex) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                    }
                }
                discardRounds++;
                game.FillHand();
            }

            CheckCombination(game);

        }

        static void CheckCombination(Game game) {

            int pointsGained;
            int previousPoints = game.GetPoints();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"You have: {game.PrintHand() }");
            Console.ResetColor();

            if (game.IsHighFive(game.GetHand())) {
                pointsGained = game.GetPoints() - previousPoints;
                Console.WriteLine($"You got a high five! You gained {pointsGained} points.");
            }
            else if (game.IsSequence(game.GetHand())) {
                pointsGained = game.GetPoints() - previousPoints;
                Console.WriteLine($"You got a sequence! You gained {pointsGained} points.");
            }
            else if (game.IsQuadruplets(game.GetHand())) {
                pointsGained = game.GetPoints() - previousPoints;
                Console.WriteLine($"You got a quadruplet! You gained {pointsGained} points.");
            }
            else if (game.IsFamily(game.GetHand())) {
                pointsGained = game.GetPoints() - previousPoints;
                Console.WriteLine($"You got a family! You gained {pointsGained} points.");
            }
            else if (game.IsMixedSequence(game.GetHand())) {
                pointsGained = game.GetPoints() - previousPoints;
                Console.WriteLine($"You got a mixed sequence! You gained {pointsGained} points.");
            }
            else if (game.IsDoubleTwins(game.GetHand())) {
                pointsGained = game.GetPoints() - previousPoints;
                Console.WriteLine($"You got double twins! You gained {pointsGained} points.");
            }
            else {
                game.SubtractPoints(1000);
                Console.WriteLine($"You did not get a winning combination. You lost 1000 points.");
            }
                       
            Console.WriteLine($"You have {game.GetPoints()} points");

            if (game.GetPoints() > 0)
                ReplayMenu(game);
            else
                GameOver(game);
        }

        static void ReplayMenu(Game game) {

            int option = -1;
            const int EXIT_VALUE = 2;

            do {

                Console.WriteLine($"Would you like to play another round?\n" +
                                  $"\t1. Play another round.\n" +
                                  $"\t2. Back to main menu.");

                option = GetIntegerInput(Console.ReadLine(), 1, EXIT_VALUE);

                switch (option) {
                    case 1:
                        game.DrawNewHand();
                        DiscardingRounds(game);
                        break;
                    case EXIT_VALUE:
                        Main(null);
                        break;
                }

            } while (option != EXIT_VALUE);

        }

        static void GameOver(Game game) {

            int option = -1;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"You lose :(");
            Console.ResetColor();

            Console.WriteLine($"\t1. Play Again\n" +
                              $"\t2. Back to main menu");

            option = GetIntegerInput(Console.ReadLine(), 1, 2);

            switch (option) {
                case 1:
                    game = new Game();
                    DiscardingRounds(game);
                    break;
                case 2:
                    Main(null);
                    break;
            }
        }
        #endregion

        #region Manual Testing
        static void ManualTestMenu(Game game) {

            int option = -1;
            const int EXIT_VALUE = 2;
            string selectedSuit = "";
            int selectedNumber = -1;

            do {
                Console.WriteLine("Manual Testing Menu:\n" +
                                  "  1. Select a card\n" +
                                  "  2. Back to main menu");

                option = GetIntegerInput(Console.ReadLine(), 1, EXIT_VALUE);

                switch (option) {
                    case 1:
                        selectedSuit = SelectSuitManually();
                        selectedNumber = SelectNumberManually();
                        break;
                    case 2:
                        Main(null);
                        break;
                    default:
                        Console.WriteLine("Default break statement in manual testing menu...");
                        break;
                }

                Card cardToAdd = new Card(selectedNumber, selectedSuit);
                game.AddToHand(cardToAdd);
                
                Console.WriteLine(game.ToString());

            } while (option != EXIT_VALUE && game.GetNumberOfCardsInHand() < 5);

            SelectWinningCombinationManually(game);
        }

        static void SelectWinningCombinationManually(Game game) {

            int option = -1;
            const int EXIT_VALUE = 7;

            do {
                Console.WriteLine("Pick a winning combination to test:\n" +
                                  "  1. High Five\n" +
                                  "  2. Sequence\n" +
                                  "  3. Quadruplets\n" +
                                  "  4. Family\n" +
                                  "  5. Mixed Sequence\n" +
                                  "  6. Double Twins\n" +
                                  "  7. Back to main menu");

                option = GetIntegerInput(Console.ReadLine(), 1, EXIT_VALUE);

                switch (option) {
                    case 1:
                        Console.WriteLine($"Is it High Five? {game.IsHighFive(game.GetHand())}");
                        break;
                    case 2:
                        Console.WriteLine($"Is it Sequence? {game.IsSequence(game.GetHand())}");
                        break;
                    case 3:
                        Console.WriteLine($"Is Quadruplets? {game.IsQuadruplets(game.GetHand())}");
                        break;
                    case 4:
                        Console.WriteLine($"Is it Family? {game.IsFamily(game.GetHand())}");
                        break;
                    case 5:
                        Console.WriteLine($"Is it Mixed Sequence? {game.IsMixedSequence(game.GetHand())}");
                        break;
                    case 6:
                        Console.WriteLine($"Is it Double Twins? {game.IsDoubleTwins(game.GetHand())}");
                        break;
                    default:
                        Console.WriteLine("Default break statement in manual testing menu...");
                        break;
                }
                
            } while (option != EXIT_VALUE);


        }

        static string SelectSuitManually() {

            int option = -1;
            string selectedSuit = "";

            Console.WriteLine("Select Suit:\n" +
                              "  1. Hearts\n" +
                              "  2. Diamonds\n" +
                              "  3. Spades\n" +
                              "  4. Clubs");

            option = GetIntegerInput(Console.ReadLine(), 1, 4);

            switch (option) {
                case 1:
                    selectedSuit = "hearts";
                    break;
                case 2:
                    selectedSuit = "diamonds";
                    break;
                case 3:
                    selectedSuit = "spades";
                    break;
                case 4:
                    selectedSuit = "clubs";
                    break;
                default:
                    Console.WriteLine("Default break statement in main menu...");
                    break;
            }
            return selectedSuit;
        }

        static int SelectNumberManually() {
            int selectedNumber = -1;

            Console.WriteLine("Select A number between 1 and 13");
            selectedNumber = GetIntegerInput(Console.ReadLine(), 1, 13);

            return selectedNumber;
        }
        #endregion

        #region Helper Methods
        static int GetIntegerInput(string toParse, int min, int max) {
            int num = -1;

            while(!(int.TryParse(toParse, out num)) || num < min || num > max) {
                Console.WriteLine($"Only numeric values between {min} and {max} accepted. Try again.");
                toParse = Console.ReadLine();
            }
            return num;
        }

        #endregion
    }
}