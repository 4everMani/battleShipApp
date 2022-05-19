using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattelshipLite
{
    class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();

            PlayerInfoModel activePlayer = CreatePlayer("Player1");
            PlayerInfoModel opponent = CreatePlayer("Player2");
            PlayerInfoModel winner = null;
            do
            {
                // Display grid from active player on where they fired
                DisplayShotGrid(activePlayer);

                // Ask player 1 for a shot
                // Determine if it is a valid shot
                // Determine shot results
                RecordPlayerShot(activePlayer, opponent);

                // Determine if the game should continue
                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

                // if over, set player 1 as the winner
                if (doesGameContinue == true)
                {
                    // Swap using a temp variable

                    //PlayerInfoModel tempHolder = opponent;
                    //opponent = activePlayer;
                    //activePlayer = tempHolder;

                    // Swap using tuple
                    (activePlayer, opponent) = (opponent, activePlayer);

                }
                // else, swap positions (activePlayer to opponent)
                else
                {
                    winner = activePlayer;
                }

            } while (winner == null);

            IdentifyWinner(winner);
            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations to {winner.UserName} for winning");
            Console.WriteLine($"{ winner.UserName } took { GameLogic.GetShotCount(winner) }shots");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            bool isValidShot = false;
            string row = string.Empty;
            int column = 0;

            do
            {
                // Ask for a shot (we ask for "B2")
                string shot = AskForShot();

                //Determine what row and column that is - split ir apart
                (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);

                // Determine if that is a valid shot
                isValidShot = GameLogic.ValidateShot(row, column, activePlayer);

                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid shot location. Please try again. ");
                }
            } while (isValidShot == false);


            //Determine results
            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            //Record Result
            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);
        }

        private static string AskForShot()
        {
            Console.Write("Please enter your shot selection: ");
            string output = Console.ReadLine();
            return output;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].SpotLetter;
            foreach(var gridSpot in activePlayer.ShotGrid)
            {
                
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber}");
                }

                else if (gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X ");
                }

                else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O ");
                }

                else
                {
                    Console.Write(" ? ");
                }
            }
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Battleship Lite");
            Console.WriteLine("Created by Manish Jaiswal");
            Console.WriteLine();
        }

        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {

            Console.WriteLine($"Player information for { playerTitle }");

            PlayerInfoModel output = new PlayerInfoModel();

            // Ask the user for their name
            output.UserName = AskForUserName();

            // Load up the shot grid
            GameLogic.InitializeGrid(output);

            // Ask the user for their 5 ship placements
            PlaceShips(output);

            // clear
            Console.Clear();

            return output;

        }

        private static string AskForUserName()
        {
            Console.Write("What is your nmae: ");
            string output = Console.ReadLine();
            return output;
        }

        private static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.Write($"Where you want to place ship number {model.ShipLocations.Count + 1}: ");
                string location = Console.ReadLine();
                bool isValidLocation = GameLogic.PlaceShip(model, location);
            } while (model.ShipLocations.Count < 5);
        }
    }
}
