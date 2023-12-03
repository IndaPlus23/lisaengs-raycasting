using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ray9006
{
    public class Raycaster9006
    {
        int r, mx, my, mp, dof;
        float rx, ry, ra, xo, yo;

        public Raycaster9006(Player player) {
            ra = player.pa;
            // Initialize variables
            r = 0;
            mx = 0;
            my = 0;
            mp = 0;
            dof = 0;
            rx = 0;
            ry = 0;
            xo = 0;
            yo = 0;
        }

        public void Cast(Player player, MapClass _map, SpriteBatch _spriteBatch, Texture2D whiteRectangle) {
            ra = player.pa;
            for (r = 0; r < 1; r++) {
                for (float disH = 0; disH < 10000; disH += 0.01f) {
                    // Calculate line coordinates
                    mx = (int)(player.px + MathF.Cos(ra) * disH);
                    my = (int)(player.py + MathF.Sin(ra) * disH);
                    // Calculate map position
                    mp = (my / _map.mapSize) * _map.mapWidth + (mx / _map.mapSize);
                    // Check if the ray is out of bounds
                    if (mp > 0 && mp < _map.mapWidth * _map.mapHeight && _map.map[mp] == 1) {
                        // Calculate distance to wall
                        dof = 1;
                        rx = mx;
                        ry = my;
                        break;
                    }
                    DrawLine(player, _map, _spriteBatch, whiteRectangle);
                }
            }
        }

        public void DrawLine(Player player, MapClass _map, SpriteBatch _spriteBatch, Texture2D whiteRectangle) {

            // Calculate distance to wall
            float disH = (float)MathF.Sqrt((rx - player.px) * (rx - player.px) + (ry - player.py) * (ry - player.py));
            // Draw line from player to wall (px, py) -> (rx, ry)
            _spriteBatch.Draw(whiteRectangle, new Vector2(player.px, player.py), null,
                Color.Red, ra, Vector2.Zero, new Vector2(disH, 1f),
                SpriteEffects.None, 0f);
            
        }
    }
}