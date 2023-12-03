using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ray9006
{
    public class MapClass
    {
        public int mapSize = 64; 
        public int mapWidth = 8;
        public int mapHeight = 8;
        public int[] map = new int[8*8] {
            1,1,1,1,1,1,1,1,
            1,0,0,0,0,0,0,1,
            1,1,1,0,0,0,0,1,
            1,0,0,0,0,0,0,1,
            1,0,0,1,1,0,0,1,
            1,0,0,1,0,0,0,1,
            1,0,0,0,0,0,0,1,
            1,1,1,1,1,1,1,1
        };

        public MapClass()
        {

        }

        public int[] GetMap()
        {
            return map;
        }
        public static int GetMapSize()
        {
            return 64;
        }
    }
}