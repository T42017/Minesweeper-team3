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
        private IServiceBus _bus;

        public PositionInfo[,] GameBoard;
        
        public MineSweeperGame(int sizeX, int sizeY, int nrOfMines, IServiceBus bus)
        {
            _bus = bus;

            

            SizeX = sizeX;
            SizeY = sizeY;
            NumberOfMines = nrOfMines;
            State = GameState.Playing;
            //ResetBoard();

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

        

        public PositionInfo GetCoordinate(int x, int y)
        {
            return null;
        }

        public void FlagCoordinate()
        {
            
        }

        public void ClickCoordinate()
        {
        }

        public void ResetBoard()
        {
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

    }
}
