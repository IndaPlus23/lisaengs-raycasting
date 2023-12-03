using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Ray9006
{
    public class Player
    {
        public float px, py;
        public float pdx, pdy, pa;
        
        public Player(float[] pos)
        {
            px = pos[0];
            py = pos[1];
            // Start looking 45 degrees to the right
            pa = - MathF.PI / 4; // Why negative? Because the y axis is inverted.
            pdx = MathF.Cos(pa) * 5;
            pdy = MathF.Sin(pa) * 5;
        }

        public void Update(KeyboardState state, MapClass _map)
        {
            // Calculate current position on the map
            int mx = (int)(px / _map.mapSize);
            int my = (int)(py / _map.mapSize);
            if (state.IsKeyDown(Keys.W))
            {
                mx = (int)((px + pdx) / _map.mapSize);
                my = (int)((py + pdy) / _map.mapSize);
                if (_map.map[my * _map.mapWidth + mx] != 1)
                {
                    px += pdx;
                    py += pdy;
                }
            }
            if (state.IsKeyDown(Keys.S))
            {
                mx = (int)((px - pdx) / _map.mapSize);
                my = (int)((py - pdy) / _map.mapSize);
                if (_map.map[my * _map.mapWidth + mx] != 1)
                {
                    px -= pdx;
                    py -= pdy;
                }
            }
            if (state.IsKeyDown(Keys.D))
            {
                mx = (int)((px - pdy) / _map.mapSize);
                my = (int)((py + pdx) / _map.mapSize);
                if (_map.map[my * _map.mapWidth + mx] != 1)
                {
                    px -= pdy;
                    py += pdx;
                }
            }
            if (state.IsKeyDown(Keys.A))
            {
                mx = (int)((px + pdy) / _map.mapSize);
                my = (int)((py - pdx) / _map.mapSize);
                if (_map.map[my * _map.mapWidth + mx] != 1)
                {
                    px += pdy;
                    py -= pdx;
                }
            }
            if (state.IsKeyDown(Keys.Left))
            {
                pa -= 0.05f;
                if (pa < 0)
                {
                    pa += 2 * (float)Math.PI;
                }
                pdx = (float)Math.Cos(pa) * 5;
                pdy = (float)Math.Sin(pa) * 5;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                pa += 0.05f;
                if (pa > 2 * Math.PI)
                {
                    pa -= 2 * (float)Math.PI;
                }
                pdx = (float)Math.Cos(pa) * 5;
                pdy = (float)Math.Sin(pa) * 5;
            }
        }
    }
}