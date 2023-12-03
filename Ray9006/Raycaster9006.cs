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
        int r, mx, my, mp;
        float rx, ry, ra;
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
            rx = 0;
            ry = 0;
        }

        public float[,] Cast(int rays, Player player, MapClass _map, SpriteBatch _spriteBatch, Texture2D whiteRectangle)
        {
            float[,] saved_rays = new float[rays, 2];
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
                for (var d = 0; d < 1000; d++)
                {
                    dist = distance(player.px, player.py, rx, ry, ra);
                    rx = player.px + d * MathF.Cos(ra);
                    ry = player.py + d * MathF.Sin(ra);
                    mx = (int)(rx / _map.mapSize);
                    my = (int)(ry / _map.mapSize);
                    mp = my * _map.mapWidth + mx;
                    if (mp > 0 && mp < _map.mapWidth * _map.mapHeight && _map.map[mp] == 1)
                    {
                        d = 1000;
                    }
                }
                // Color the walls based on distance, black if too far
                float limit = 750;
                if (dist > limit)
                {
                    dist = limit;
                }
                wall_color[0] = 1 - dist / limit;
                wall_color[1] = 1 - dist / limit;
                wall_color[2] = 1 - dist / limit;

                // DrawLine(player, _map, _spriteBatch, whiteRectangle, dist);
                saved_rays[r, 0] = dist;
                saved_rays[r, 1] = ra;

                Draw(rays, player, _map, _spriteBatch, whiteRectangle);

                ra += one_degree;
            }

            return saved_rays;
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

        public void DrawLine(Player player, MapClass _map, SpriteBatch _spriteBatch, Texture2D whiteRectangle, float dist, float ra)
        {
            // Draw line from player to wall (px, py) -> (rx, ry)
            _spriteBatch.Draw(whiteRectangle, new Vector2(player.px, player.py), null,
                Color.Red, ra, Vector2.Zero, new Vector2(dist, 2f),
                SpriteEffects.None, 0f);

        }
    }
}