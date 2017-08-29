using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public void ClickCoordinate()
        {
        }

        public void ResetBoard()
        {
            InitMapWithBlankPositionInfo();
            PlaceMines();
            CalculateNumberOfNeighbours();

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
            if (PosX < Map.GetLength(0) - 1)
            {
                PosX++;
            }
        }

        #endregion

        private void InitMapWithBlankPositionInfo()
        {
            Map = new PositionInfo[SizeX, SizeY];

            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    PositionInfo info = new PositionInfo();
                    info.X = x;
                    info.Y = y;

                    Map[x, y] = info;
                }
            }
        }

        private void CalculateNumberOfNeighbours()
        {
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    int numberOfMines = 0;

                    // loop through a 3x3 grid with the center on x and y
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

                    GetCoordinate(x, y).NrOfNeighbours = numberOfMines;
                }
            }
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
