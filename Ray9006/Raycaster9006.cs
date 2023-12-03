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
        float one_degree = (float)(Math.PI / 180);
        float dist = 0;
        float[] wall_color = new float[3] { 1f, 1f, 1f };

        public Raycaster9006(Player player)
        {
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

        public void Cast(int rays, Player player, MapClass _map, SpriteBatch _spriteBatch, Texture2D whiteRectangle)
        {
            ra = player.pa - one_degree * rays / 2;
            if (ra < 0)
            {
                ra += 2 * MathF.PI;
            }
            if (ra > 2 * MathF.PI)
            {
                ra -= 2 * MathF.PI;
            }

            for (r = 0; r < rays; r++)
            {
                // Check horizontal lines
                dof = 0;
                float disH = 1000000;
                float hx = player.px;
                float hy = player.py;
                float aTan = -1 / MathF.Tan(ra);
                if (ra > MathF.PI)
                {
                    ry = (((int)player.py >> 6) << 6) - 0.0001f;
                    rx = (player.py - ry) * aTan + player.px;
                    yo = -64;
                    xo = -yo * aTan;
                }
                if (ra < MathF.PI)
                {
                    ry = (((int)player.py >> 6) << 6) + 64;
                    rx = (player.py - ry) * aTan + player.px;
                    yo = 64;
                    xo = -yo * aTan;
                }
                if (ra == 0 || ra == MathF.PI)
                {
                    rx = player.px;
                    ry = player.py;
                    dof = 8;
                }
                while (dof < 8)
                {
                    mx = (int)(rx) >> 6;
                    my = (int)(ry) >> 6;
                    mp = my * _map.mapWidth + mx;
                    if (mp > 0 && mp < _map.mapWidth * _map.mapHeight && _map.map[mp] == 1)
                    {
                        hx = rx;
                        hy = ry;
                        disH = distance(player.px, player.py, hx, hy, ra);
                        dof = 8;
                    }
                    else
                    {
                        rx += xo;
                        ry += yo;
                        dof += 1;
                    }
                }
                // DrawLine(player, _map, _spriteBatch, whiteRectangle);
                // Check vertical lines
                dof = 0;
                float disV = 1000000;
                float vx = player.px;
                float vy = player.py;
                float nTan = -MathF.Tan(ra);
                if (ra > MathF.PI / 2 && ra < 3 * MathF.PI / 2)
                {
                    rx = (((int)player.px >> 6) << 6) - 0.0001f;
                    ry = (player.px - rx) * nTan + player.py;
                    xo = -64;
                    yo = -xo * nTan;
                }
                if (ra < MathF.PI / 2 || ra > 3 * MathF.PI / 2)
                {
                    rx = (((int)player.px >> 6) << 6) + 64;
                    ry = (player.px - rx) * nTan + player.py;
                    xo = 64;
                    yo = -xo * nTan;
                }
                if (ra == 0 || ra == MathF.PI)
                {
                    rx = player.px;
                    ry = player.py;
                    dof = 8;
                }
                while (dof < 8)
                {
                    mx = (int)(rx) >> 6;
                    my = (int)(ry) >> 6;
                    mp = my * _map.mapWidth + mx;
                    if (mp > 0 && mp < _map.mapWidth * _map.mapHeight && _map.map[mp] == 1)
                    {
                        vx = rx;
                        vy = ry;
                        disV = distance(player.px, player.py, vx, vy, ra);
                        dof = 8;
                    }
                    else
                    {
                        rx += xo;
                        ry += yo;
                        dof += 1;
                    }
                }

                if (disV < disH)
                {
                    rx = vx;
                    ry = vy;
                    dist = disV;
                    wall_color[0] = 0.8f;
                    wall_color[1] = 0.8f;
                    wall_color[2] = 0.8f;
                }
                else
                {
                    rx = hx;
                    ry = hy;
                    dist = disH;
                    wall_color[0] = 0.7f;
                    wall_color[1] = 0.7f;
                    wall_color[2] = 0.7f;
                }
                DrawLine(player, _map, _spriteBatch, whiteRectangle, dist);

                Draw(rays, player, _map, _spriteBatch, whiteRectangle);

                ra += one_degree;
            }
        }

        private void Draw(int rays, Player player, MapClass _map, SpriteBatch _spriteBatch, Texture2D whiteRectangle)
        {
            // Draw 3D walls
            float ca = player.pa - ra;
            if (ca < 0)
            {
                ca += 2 * MathF.PI;
            }
            if (ca > 2 * MathF.PI)
            {
                ca -= 2 * MathF.PI;
            }

            float windowWidth = Helper.GetWindowWidth();
            float windowHeight = Helper.GetWindowHeight();

            dist = dist * MathF.Cos(ca); // Fix fisheye
            float lineHeight = (_map.mapSize * windowHeight) / dist;
            if (lineHeight > windowHeight)
            {
                lineHeight = windowHeight;
            }
            float wallHeight = Math.Min(lineHeight, windowHeight); // Ensure the wall doesn't exceed window height
            float lineOff = (windowHeight - wallHeight) / 2;
            float columnWidth = windowWidth / rays; // Width of each column of the raycasting grid

            Color wallColor = new Color(wall_color[0], wall_color[1], wall_color[2]);
            _spriteBatch.Draw(whiteRectangle, new Vector2(r * columnWidth, lineOff), null,
                wallColor, 0f, Vector2.Zero, new Vector2(columnWidth, lineHeight),
                SpriteEffects.None, 0f);
        }

        public float distance(float ax, float ay, float bx, float by, float ang)
        {
            return (float)MathF.Sqrt((bx - ax) * (bx - ax) + (by - ay) * (by - ay));
        }

        public void DrawLine(Player player, MapClass _map, SpriteBatch _spriteBatch, Texture2D whiteRectangle, float dist)
        {
            // Draw line from player to wall (px, py) -> (rx, ry)
            _spriteBatch.Draw(whiteRectangle, new Vector2(player.px, player.py), null,
                Color.Red, ra, Vector2.Zero, new Vector2(dist, 2f),
                SpriteEffects.None, 0f);

        }
    }
}