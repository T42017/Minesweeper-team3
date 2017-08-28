using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FloodFill
{
    class Program
    {
        public static Block[,] Map;

        public static int MapWidth;
        public static int MapHeight;

        public static Random Ran = new Random();

        public static int StartX = 3, StartY = 3;

        public static void Main(string[] args)
        {
            InitMap();

            MapWidth = Map.GetLength(0);
            MapHeight = Map.GetLength(1);

            PrintBombMap();
            Console.ReadLine();

            FloodFill(StartX, StartY);

            while (true)
            {
                Console.ReadLine();
            }
        }

        public static void InitMap()
        {
            Map = new Block[20, 40];

            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    Block b = new Block();

                    if (x != StartX && y != StartY)
                        b.CanWalkOn = Ran.Next(5) == 0 ? true : false;
                    
                    Map[x, y] = b;
                }
            }
        }

        public static void FloodFill(int x, int y)
        {
            if ((x < 0) || (x >= MapWidth)) return;
            if ((y < 0) || (y >= MapHeight)) return;

            if (!Map[x, y].HasBeenVisited && !Map[x, y].CanWalkOn)
            {
                Console.Clear();
                PrintVisitedMap();
                Console.ReadLine();

                Map[x, y].HasBeenVisited = true;
                
                FloodFill(x, y+1);
                FloodFill(x, y-1);
                FloodFill(x-1, y);
                FloodFill(x+1, y);
            }
        }

        public static void PrintBombMap()
        {
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    Block b = Map[x, y];
                    Console.Write(b.CanWalkOn ? "1" : "0");
                }
                Console.WriteLine();
            }
        }

        public static void PrintVisitedMap()
        {
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    Block b = Map[x, y];
                    Console.Write(b.HasBeenVisited ? "1" : "0");
                }
                Console.WriteLine();
            }
        }
    }
}
