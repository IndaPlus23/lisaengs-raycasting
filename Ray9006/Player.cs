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
        
        public Player(float x, float y)
        {
            px = x;
            py = y;
            // Start looking 45 degrees to the right
            pa = - MathF.PI / 4; // Why negative? Because the y axis is inverted.
            pdx = MathF.Cos(pa) * 5;
            pdy = MathF.Sin(pa) * 5;
        }

        public void Update(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.W))
            {
                px += pdx;
                py += pdy;
            }
            if (state.IsKeyDown(Keys.S))
            {
                px -= pdx;
                py -= pdy;
            }
            if (state.IsKeyDown(Keys.A))
            {
                pa -= 0.1f;
                if (pa < 0)
                {
                    pa += 2 * (float)Math.PI;
                }
                pdx = (float)Math.Cos(pa) * 5;
                pdy = (float)Math.Sin(pa) * 5;
            }
            if (state.IsKeyDown(Keys.D))
            {
                pa += 0.1f;
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