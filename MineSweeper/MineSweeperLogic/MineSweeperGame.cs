using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using MineSweeperLogic;

namespace MineSweeperLogic
{
    public class MineSweeperGame
    {

        public MineSweeperGame(int sizeX, int sizeY, int nrOfMines, IServiceBus bus)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            NumberOfMines = nrOfMines;
            this.Bus = bus;
            State = GameState.Playing;
            ResetBoard();
        }

        public int PosX { get; private set; }
        public int PosY { get; private set; }
        public int SizeX { get; }
        public int SizeY { get; }
        public int NumberOfMines { get; }
        public GameState State { get; private set; }
        private PositionInfo[,] Map;
        private readonly IServiceBus Bus;

        public PositionInfo GetCoordinate(int x, int y)
        {
            return Map[x, y];
        }

        public void FlagCoordinate()
        {
            PositionInfo position = GetCoordinate(PosX, PosY);
            if (position.IsOpen)
                return;
            
            position.IsFlagged = !position.IsFlagged;
        }

        public void ClickCoordinate()
        {
            PositionInfo position = GetCoordinate(PosX, PosY);

            if (position.HasMine)
            {
                ShowAllMines();
                State = GameState.Lost;
                return;
            }

            if (!position.IsFlagged)
            {
                position.IsOpen = true;
                OpenSuroundingPositions();

                if (IsAllSafePositionsOpened())
                {
                    State = GameState.Won;
                }
            }

        }

        public void ResetBoard()
        {
            InitMapWithBlankPositionInfo();
            PlaceMines();
            CalculateNumberOfNeighbours();


            FlagCoordinate();
            State = GameState.Playing;
        }

        public void DrawBoard()
        {
        }

        #region MoveCursor Methods

        public void MoveCursorUp()
        {

            if (PosY > 0)
            {
                PosY--;
            }

        }

        public void MoveCursorDown()
        {
            if (PosY < Map.GetLength(1) - 1)
            {
                PosY++;
            }
            
        }

        public void MoveCursorLeft()
        {
            if (PosX > 0)
            {
                PosX--;
            }
        }

        public void MoveCursorRight()
        {
            if(PosX < Map.GetLength(0) - 1)
            PosX++;
        }

        #endregion

        private void ShowAllMines()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    if (Map[x, y].HasMine)
                        Map[x, y].IsOpen = true;
                }
            }
        }

        private void OpenSuroundingPositions()
        {
            bool[,] mapIsVisited = new bool[SizeX, SizeY];
            
            FloodFill(mapIsVisited, PosX, PosY);
        }

        private void FloodFill(bool[,] mapIsVisited, int x, int y)
        {
            if ((x < 0) || (x >= SizeX)) return;
            if ((y < 0) || (y >= SizeY)) return;
            if (Map[x, y].HasMine || mapIsVisited[x, y]) return;

            mapIsVisited[x, y] = true;
            Map[x, y].IsOpen = true;

            if (Map[x, y].NrOfNeighbours <= 1)
            {
                FloodFill(mapIsVisited, x, y + 1);
                FloodFill(mapIsVisited, x, y - 1);
                FloodFill(mapIsVisited, x - 1, y);
                FloodFill(mapIsVisited, x + 1, y);
            }
        }

        private bool IsAllSafePositionsOpened()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for(int y = 0; y < SizeY; y++)
                {
                    PositionInfo position = GetCoordinate(x, y);

                    if (!position.IsOpen && !position.HasMine)
                        return false;
                }
            }
            return true;
        }

        private void InitMapWithBlankPositionInfo()
        {
            Map = new PositionInfo[SizeX, SizeY];

            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    PositionInfo info = new PositionInfo();
                    info.X = x;
                    info.Y = y;
                    info.IsFlagged = false;
                    info.IsOpen = false;

                    Map[x, y] = info;
                }
            }
        }

        private void CalculateNumberOfNeighbours()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    GetCoordinate(x, y).NrOfNeighbours = GetNumberOfNeighbours(x, y);
                }
            }
        }

        private int GetNumberOfNeighbours(int x, int y)
        {
            int numberOfMines = 0;

            // Loop through a 3x3 grid with the center being x and y
            for (int posX = x - 1; posX <= x + 1; posX++)
            {
                for (int posY = y - 1; posY <= y + 1; posY++)
                {

                    if (posX == x && posY == y) // ignore middle position
                        continue;

                    if (posX < 0 || posX >= SizeX ||
                        posY < 0 || posY >= SizeY) // is outside map
                        continue;

                    if (GetCoordinate(posX, posY).HasMine)
                        numberOfMines++;
                }
            }

            return numberOfMines;
        }

        private void PlaceMines()
        {
            int numberOfMinesBeenPlaced = 0;

            while (numberOfMinesBeenPlaced < NumberOfMines)
            {
                int x = Bus.Next(SizeX - 1);
                int y = Bus.Next(SizeY - 1);

                PositionInfo positionToPlaceMine = GetCoordinate(x, y);

                if (positionToPlaceMine.HasMine)
                    continue;

                positionToPlaceMine.HasMine = true;

                numberOfMinesBeenPlaced++;
            }
        }
    }
}
