using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperLogic
{
    public class PositionInfo
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsOpen { get; set; }
        public bool HasMine { get; set; }
        public bool IsFlagged { get; set; }
        public int NrOfNeighbours { get; set; }

        //public PositionInfo(int i, int j, bool b)
        //{
        //    X = i;
        //    Y = j;
        //    IsOpen = b;

        //    MineSweeperGame.GameBoard[X, Y] = IsOpen;

        //}
    }
}
