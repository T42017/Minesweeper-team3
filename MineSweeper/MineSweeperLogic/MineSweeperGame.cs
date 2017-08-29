﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperLogic
{
    public class MineSweeperGame
    {
        private IServiceBus _bus;

        public PositionInfo[,] GameBoard;

        public MineSweeperGame(int sizeX, int sizeY, int nrOfMines, IServiceBus bus)
        {
            _bus = bus;

            

            SizeX = sizeX;
            SizeY = sizeY;
            NumberOfMines = nrOfMines;
            this.Bus = bus;
            ResetBoard();

            GameBoard = new PositionInfo[SizeX, SizeY];

            //for (int i = 0; i < SizeY; i++)
            //{
            //    for (int j = 0; j < SizeX; j++)
            //    {
            //        GameBoard[i, j] = new PositionInfo(i, j, false);
            //    }
            //}

            
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
            if (x < 0 || x > SizeX ||
                y < 0 || y > SizeY) return null;
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

            PlaceMines();

            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    int numberOfMines = 0;
                    for (int x2 = x - 1; x2 < x + 1; x2++)
                    {
                        for (int y2 = y - 1; y2 < y + 1; y2++)
                        {
                            if (x2 == x && y2 == y) // ignore middle position
                                continue;
                            PositionInfo info = GetCoordinate(x2, y2);
                            if (info != null && info.HasMine)
                                numberOfMines++;
                        }
                    }
                    GetCoordinate(x, y).NrOfNeighbours = numberOfMines;
                }
            }

            State = GameState.Playing;
        }

        public void DrawBoard()
        {
            for (int i = 0; i < SizeY; i++)
            {
                for (int j = 0; j < SizeX; j++)
                {
                    _bus.Write(" " + GameBoard[i, j] + " ");
                }

                _bus.WriteLine();
            }

            if (GetCoordinate(PosX, PosY).IsOpen == false)
            {
                _bus.Write("?");
            }
            else if (GetCoordinate(PosX, PosY).IsOpen == true)
            {
                _bus.Write(" ");
            }

            if (GetCoordinate(PosX, PosY).HasMine == true)
            {
                _bus.Write("X", ConsoleColor.DarkCyan);
            }

            if (GetCoordinate(PosX, PosY).IsFlagged == true)
            {
                _bus.Write("!", ConsoleColor.DarkCyan);
            }
        }

        

        #region MoveCursor Methods

        public void MoveCursorUp()
        {
            
        }

        public void MoveCursorDown()
        {
        }

        public void MoveCursorLeft()
        {

        }

        public void MoveCursorRight()
        {
        }

        #endregion

        private void PlaceMines()
        {
            int numberOfMinesBeenPlaced = 0;

            while (numberOfMinesBeenPlaced < NumberOfMines)
            {
                int x = Bus.Next(SizeX);
                int y = Bus.Next(SizeY);

                PositionInfo positionToPlaceMine = GetCoordinate(x, y);

                if (positionToPlaceMine.HasMine)
                    continue;

                positionToPlaceMine.HasMine = true;

                numberOfMinesBeenPlaced++;
            }
        }
    }
}
