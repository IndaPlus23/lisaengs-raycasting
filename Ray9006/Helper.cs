using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ray9006
{
    public class Helper
    {
        // NOTE: Generic helpers
        public static int[] _window_dimensions = new int[2] { 1920, 1080 };
        public static int GetWindowWidth()
        {
            return _window_dimensions[0];
        }
        public static int GetWindowHeight()
        {
            return _window_dimensions[1];
        }
        public static int[] GetWindowCenter()
        {
            return new int[2] { _window_dimensions[0] / 2, _window_dimensions[1] / 2 };
        }

        // NOTE: Map helpers
        public static float[] GetMapSquareCenterPosition(int x, int y, int mapSize)
        {
            float[] position = new float[2] { x * mapSize + mapSize / 2, y * mapSize + mapSize / 2 };
            return position;
        }
    }
}