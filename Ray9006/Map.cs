using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ray9006
{
    public class Map
    {
        // The map
        public int[,] MapI;
        // The width of the map
        public int MapWidth = 20;
        // The height of the map
        public int MapHeight = 20;
        
        // The map
        public static Map Map1;
        // player
        public static Player Player1 = new Player(1, 1);

        // Initialize the map
        public Map(int x, int y) {
            MapI = new int[x, y];
            MapWidth = x;
            MapHeight = y;
        }
        public void GenerateMap() {
            // Generate the map, all walls
            for (int x = 0; x < MapWidth; x++) {
                for (int y = 0; y < MapHeight; y++) {
                    // If the x or y is 0 or the width/height, make it a wall (1), else make it a floor (0)
                    if (x == 0 || y == 0 || x == MapWidth - 1 || y == MapHeight - 1) {
                        MapI[x, y] = 1;
                    } else {
                        MapI[x, y] = 0;
                    }
                }
            }
        }
    }
}