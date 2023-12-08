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
        public int[] map;

        public int[] start = new int[2] { 1, 1 };

        public MapClass(int size)
        {
            mapWidth = size;
            mapHeight = size;
            mapSize = Helper.GetWindowHeight() / mapHeight;
            map = GenerateMap(size);
        }

        public int[] GenerateMap(int size) {
            char[,] maze = GenerateMaze(size, size);
            int[,] map = new int[size, size];
            // Convert the maze to a map, c = 0, w = 1, s = 2, e = 3
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++){
                    if (maze[i, j] == 'c') {
                        map[i, j] = 0;
                    }
                    if (maze[i, j] == 'w') {
                        map[i, j] = 1;
                    }
                    if (maze[i, j] == 's') {
                        map[i, j] = 2;
                    }
                    if (maze[i, j] == 'e') {
                        map[i, j] = 3;
                    }
                }
            }
            // Flatten the map to a 1D array
            int[] flat_map = new int[size * size];
            int index = 0;
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++){
                    flat_map[index] = map[j, i];
                    index++;
                }
            }
            return flat_map;
        }

        public char[,] GenerateMaze(int width, int height)
        {
            char[,] generated_maze = new char[width, height];
            // Start the grid full of unvisited tiles ('u')
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    generated_maze[i, j] = 'u';
                }
            }
            // pick random spot to start maze at
            Random rnd = new Random();
            int starting_height = rnd.Next(1, height - 1);
            int starting_width = rnd.Next(1, width - 1);
            // mark spot as path and add surrounding walls to wall list/array
            List<int[]> walls = new List<int[]>();
            generated_maze[starting_width, starting_height] = 'c';
            walls.Add(new int[] {starting_width - 1, starting_height});
            walls.Add(new int[] {starting_width, starting_height - 1});
            walls.Add(new int[] {starting_width, starting_height + 1});
            walls.Add(new int[] {starting_width + 1, starting_height});
            // denote the above walls as walls
            generated_maze[starting_width - 1, starting_height] = 'w';
            generated_maze[starting_width, starting_height - 1] = 'w';
            generated_maze[starting_width, starting_height + 1] = 'w';
            generated_maze[starting_width + 1, starting_height] = 'w';
            // step 3, algorithm
            // while walls pick random wall
            int i_lim = 0;
            while (walls.Count > 0 && i_lim < 100000)
            {
                // pick random wall from list
                int[] random_wall = walls[(int)rnd.Next(0, walls.Count - 1)];
                int x = (int)random_wall[0];
                int y = (int)random_wall[1];
                // Console.WriteLine("x: " + x + " y: " + y + " walls: " + walls.Count + " i_lim: " + i_lim);
                i_lim++;

                // Check if its a left wall
                if (y != 0)
                {
                    // if (maze[rand_wall[0]][rand_wall[1]-1] == 'u' and maze[rand_wall[0]][rand_wall[1]+1] == 'c'):
                    if (generated_maze[x, y - 1] == 'u' && generated_maze[x, y + 1] == 'c')
                    {
                        int s_cells = SurroundingCells(random_wall, generated_maze);
                        if (s_cells < 2)
                        {
                            generated_maze[x, y] = 'c';
                            // Mark new walls
                            // Upper cell
                            if (x != 0)
                            {
                                if (generated_maze[x - 1, y] != 'c')
                                {
                                    generated_maze[x - 1, y] = 'w';
                                }
                                if (!walls.Contains(new int[] {x - 1, y}))
                                {
                                    walls.Add(new int[] {x - 1, y});
                                }
                            }
                            // Bottom cell
                            if (x != (width - 1))
                            {
                                if (generated_maze[x + 1, y] != 'c')
                                {
                                    generated_maze[x + 1, y] = 'w';
                                }
                                if (!walls.Contains(new int[] {x + 1, y}))
                                {
                                    walls.Add(new int[] {x + 1, y});
                                }
                            }
                            // Leftmost wall
                            if (y != 0)
                            {
                                if (generated_maze[x, y - 1] != 'c')
                                {
                                    generated_maze[x, y - 1] = 'w';
                                }
                                if (!walls.Contains(new int[] {x, y - 1}))
                                {
                                    walls.Add(new int[] {x, y - 1});
                                }
                            }
                        }
                        // walls.Remove(new int[] {x, y});
                        walls = RemoveWall(new int[] {x, y}, walls);
                        continue;
                    }
                }
                // Check if its an upper wall
                if (x != 0)
                {
                    if (generated_maze[x - 1, y] == 'u' && generated_maze[x + 1, y] == 'c')
                    {
                        int s_cells = SurroundingCells(random_wall, generated_maze);
                        if (s_cells < 2)
                        {
                            generated_maze[x, y] = 'c';
                            // Upper cell
                            if (x != 0)
                            {
                                if (generated_maze[x - 1, y] != 'c')
                                {
                                    generated_maze[x - 1, y] = 'w';
                                }
                                if (!walls.Contains(new int[] {x - 1, y}))
                                {
                                    walls.Add(new int[] {x - 1, y});
                                }
                            }
                            // Leftmost wall
                            if (y != 0)
                            {
                                if (generated_maze[x, y - 1] != 'c')
                                {
                                    generated_maze[x, y - 1] = 'w';
                                }
                                if (!walls.Contains(new int[] {x, y - 1}))
                                {
                                    walls.Add(new int[] {x, y - 1});
                                }
                            }
                            // Rightmost cell
                            if (y != (height - 1))
                            {
                                if (generated_maze[x, y + 1] != 'c')
                                {
                                    generated_maze[x, y + 1] = 'w';
                                }
                                if (!walls.Contains(new int[] {x, y + 1}))
                                {
                                    walls.Add(new int[] {x, y + 1});
                                }
                            }
                        }
                        // walls.Remove(new int[] {x, y});
                        walls = RemoveWall(new int[] {x, y}, walls);
                        continue;
                    }
                }
                // Check the bottom wall
                if (x != (width - 1))
                {
                    if (generated_maze[x + 1, y] == 'u' && generated_maze[x - 1, y] == 'c')
                    {
                        int s_cells = SurroundingCells(random_wall, generated_maze);
                        if (s_cells < 2)
                        {
                            generated_maze[x, y] = 'c';
                            // Mark new walls
                            // Bottom cell
                            if (x != (width - 1))
                            {
                                if (generated_maze[x + 1, y] != 'c')
                                {
                                    generated_maze[x + 1, y] = 'w';
                                }
                                if (!walls.Contains(new int[] {x + 1, y}))
                                {
                                    walls.Add(new int[] {x + 1, y});
                                }
                            }
                            // Leftmost wall
                            if (y != 0)
                            {
                                if (generated_maze[x, y - 1] != 'c')
                                {
                                    generated_maze[x, y - 1] = 'w';
                                }
                                if (!walls.Contains(new int[] {x, y - 1}))
                                {
                                    walls.Add(new int[] {x, y - 1});
                                }
                            }
                            // Rightmost cell
                            if (y != (height - 1))
                            {
                                if (generated_maze[x, y + 1] != 'c')
                                {
                                    generated_maze[x, y + 1] = 'w';
                                }
                                if (!walls.Contains(new int[] {x, y + 1}))
                                {
                                    walls.Add(new int[] {x, y + 1});
                                }
                            }
                        }
                        // walls.Remove(new int[] {x, y});
                        walls = RemoveWall(new int[] {x, y}, walls);
                        continue;
                    }
                }
                // Check the right wall
                if (y != (height - 1))
                {
                    if (generated_maze[x, y + 1] == 'u' && generated_maze[x, y - 1] == 'c')
                    {
                        int s_cells = SurroundingCells(random_wall, generated_maze);
                        if (s_cells < 2)
                        {
                            generated_maze[x, y] = 'c';
                            // Mark the new walls
                            // Rightmost cell
                            if (y != (height - 1))
                            {
                                if (generated_maze[x, y + 1] != 'c')
                                {
                                    generated_maze[x, y + 1] = 'w';
                                }
                                if (!walls.Contains(new int[] {x, y + 1}))
                                {
                                    walls.Add(new int[] {x, y + 1});
                                }
                            }
                            // Bottom cell
                            if (x != (width - 1))
                            {
                                if (generated_maze[x + 1, y] != 'c')
                                {
                                    generated_maze[x + 1, y] = 'w';
                                }
                                if (!walls.Contains(new int[] {x + 1, y}))
                                {
                                    walls.Add(new int[] {x + 1, y});
                                }
                            }
                            // Upper cell
                            if (x != 0)
                            {
                                if (generated_maze[x - 1, y] != 'c')
                                {
                                    generated_maze[x - 1, y] = 'w';
                                }
                                if (!walls.Contains(new int[] {x - 1, y}))
                                {
                                    walls.Add(new int[] {x - 1, y});
                                }
                            }
                        }
                        // walls.Remove(new int[] {x, y});
                        walls = RemoveWall(new int[] {x, y}, walls);
                        continue;
                    }
                }
                walls.Remove(new int[] {x, y});
            }
            // Mark remaining unvisited cells as walls
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (generated_maze[i, j] == 'u')
                    {
                        generated_maze[i, j] = 'w';
                    }
                }
            }
            // Set entrance and exit (maybe randomize which side the entence and exists should be :)
            for (int i = 0; i < width; i++)
            {
                if (generated_maze[i, (height - 2)] == 'c')
                {
                    //generated_maze[i, (height - 1)] = 's';
                    generated_maze[i, (height - 2)] = 's';
                    start[0] = i;
                    start[1] = (height - 2);
                    break;
                }
            }
            for (int i = (width - 1); i >= 0; i--)
            {
                if (generated_maze[i, 1] == 'c')
                {
                    // generated_maze[i, 0] = 'e';
                    generated_maze[i, 1] = 'e';
                    break;
                }
            }

            return generated_maze;
        }

        public int SurroundingCells(int[] rand_wall, char[,] maze)
        {
            int s_cells = 0;
            if (maze[(int)rand_wall[0] - 1, (int)rand_wall[1]] == 'c')
            {
                s_cells += 1;
            }
            if (maze[(int)rand_wall[0] + 1, (int)rand_wall[1]] == 'c')
            {
                s_cells += 1;
            }
            if (maze[(int)rand_wall[0], (int)rand_wall[1] - 1] == 'c')
            {
                s_cells += 1;
            }
            if (maze[(int)rand_wall[0], (int)rand_wall[1] + 1] == 'c')
            {
                s_cells += 1;
            }
            return s_cells;
        }

        private List<int[]> RemoveWall(int[] wall, List<int[]> walls)
        {
            List<int[]> new_walls = new List<int[]>();
            // Console.WriteLine("walls: " + walls.Count);
            foreach (int[] w in walls)
            {
                if (w[0] != wall[0] || w[1] != wall[1])
                {
                    // Console.WriteLine("w: " + w[0] + " " + w[1] + " wall: " + wall[0] + " " + wall[1]);
                    new_walls.Add(w);
                }
            }
            // Console.WriteLine("new_walls: " + new_walls.Count);
            return new_walls;
        }

        public int[] GetMap()
        {
            return map;
        }
    }
}