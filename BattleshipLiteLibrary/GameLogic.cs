﻿using BattleshipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary
{
    public static class GameLogic
    {
        public static void InitializeGrid(PlayerInfoModel model)
        {
            List<string> letters = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E",
            };

            List<int> numbers = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            };

            foreach(string letter in letters)
            {
                foreach (int number in numbers)
                {
                    AddGridSpot(model, letter, number);
                }
            }
        }

        private static void AddGridSpot(PlayerInfoModel model, string letter, int number)
        {
            GridSpotModel spot = new GridSpotModel
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.Empty
            };
            model.ShotGrid.Add(spot);
        }

        public static bool PlayerStillActive(PlayerInfoModel player)
        {
            bool isActive = false;
            foreach(var ship in player.ShipLocations)
            {
                if (ship.Status != GridSpotStatus.Sunk)
                {
                    isActive = true;
                }
            }
            return isActive;
        }

        public static bool PlaceShip(PlayerInfoModel model, string location)
        {
            (string row, int column) = SplitShotIntoRowAndColumn(location);

            bool isValidLocation = ValidateGridLocation(model, row, column);

            bool isSpotOpen = ValidateShipLocation(model, row, column);

            if (isValidLocation && isSpotOpen)
            {
                model.ShipLocations.Add(new GridSpotModel
                {
                    SpotLetter = row,
                    SpotNumber = column,
                    Status = GridSpotStatus.Empty
                });
            }
            return isValidLocation && isSpotOpen;
        }

        private static bool ValidateShipLocation(PlayerInfoModel model, string row, int column)
        {
            bool isValidLocation = true;

            foreach(var ship in model.ShipLocations)
            {
                if (ship.SpotLetter.ToUpper() == row.ToUpper() && ship.SpotNumber == column)
                {
                    isValidLocation = false;
                }
            }

            return isValidLocation;
        }

        private static bool ValidateGridLocation(PlayerInfoModel model, string row, int column)
        {
            bool isValidLocation = false;

            foreach (var ship in model.ShipLocations)
            {
                if (ship.SpotLetter.ToUpper() == row.ToUpper() && ship.SpotNumber == column)
                {
                    isValidLocation = true;
                }
            }

            return isValidLocation;
        }

        public static int GetShotCount(PlayerInfoModel player)
        {
            int shotCount = 0;
            foreach(var shot in player.ShotGrid)
            {
                if (shot.Status != GridSpotStatus.Empty)
                {
                    shotCount += 1;
                }
            }
            return shotCount;
        }

        public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
        {
            string row = string.Empty;
            int column = 0;

            if (shot.Length != 2)
            {
                throw new ArgumentException("This was an invalid shot type", shot);
            }

            char[] shotArray = shot.ToArray();
            row = shotArray[0].ToString();
            column = int.Parse(shotArray[1].ToString());
            return (row, column);

        }

        public static bool ValidateShot(string row, int column, PlayerInfoModel activePlayer)
        {
            throw new NotImplementedException();
        }

        public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
        {
            throw new NotImplementedException();
        }

        public static void MarkShotResult(PlayerInfoModel activePlayer, string row, int column, bool isAHit)
        {
            throw new NotImplementedException();
        }
    }
}