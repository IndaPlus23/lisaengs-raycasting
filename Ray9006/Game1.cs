using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
// Raycasting 9006 B)

namespace Ray9006;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Player _player;
    Texture2D whiteRectangle;
    private MapClass _map;
    private Raycaster9006 _raycaster;
    private Boolean show_minimap = true;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Set the window size
        _graphics.PreferredBackBufferWidth = Helper.GetWindowWidth();
        _graphics.PreferredBackBufferHeight = Helper.GetWindowHeight();
        _graphics.ApplyChanges();

        _map = new MapClass(12);

        _player = new Player(Helper.GetMapSquareCenterPosition(_map.start[0], _map.start[1], _map.mapSize));
        _raycaster = new Raycaster9006(_player);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        // TODO: use this.Content to load your game content here

        // Create a 1px square rectangle texture that will be scaled to the
        // desired size and tinted the desired color at draw time
        whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
        whiteRectangle.SetData(new[] { Color.White });

    }

    protected override void UnloadContent()
    {
        base.UnloadContent();
        _spriteBatch.Dispose();
        // If you are creating your texture (instead of loading it with
        // Content.Load) then you must Dispose of it
        whiteRectangle.Dispose();
    }

    protected override void Update(GameTime gameTime)
    {
        // Poll for current keyboard state
        KeyboardState state = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
            Exit();
        // Toggle minimap with Spacebar
        if (state.IsKeyDown(Keys.Space))
        {
            show_minimap = true;
        } else {
            show_minimap = false;
        }

        // TODO: Add your update logic here
        _player.Update(state, _map);
        

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        float[,] rays = _raycaster.Cast(60, _player, _map, _spriteBatch, whiteRectangle);
        if (show_minimap) DrawMinimap(rays);

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void DrawMinimap(float[,] rays) {
        DrawMap();
        DrawPlayer();
        int player_sprite_offset = Helper.GetWindowWidth() / 2 - _map.mapWidth * _map.mapSize / 2;
        for (int i = 0; i < rays.GetLength(0); i++)
        {
            float dist = rays[i, 0];
            float ra = rays[i, 1];
            int x = (int)(dist * MathF.Cos(ra));
            int y = (int)(dist * MathF.Sin(ra));
            _spriteBatch.Draw(whiteRectangle, new Vector2(_player.px + player_sprite_offset, _player.py), null,
                Color.Red, ra, Vector2.Zero, new Vector2(dist, 2f),
                SpriteEffects.None, 0f);
        }
    }

    private void DrawPlayer()
    {
        // Draw a rectangle at the player's position
        // Option Two (if you have floating-point coordinates)
        int player_sprite_offset = Helper.GetWindowWidth() / 2 - _map.mapWidth * _map.mapSize / 2 - _map.mapSize / 4;
        _spriteBatch.Draw(whiteRectangle, new Vector2(_player.px + player_sprite_offset, _player.py - _map.mapSize / 4), null,
                Color.Red, 0f, Vector2.Zero, new Vector2(_map.mapSize/2, _map.mapSize/2),
                SpriteEffects.None, 0f);
        // Draw a line from the player's position to the player's direction
        _spriteBatch.Draw(whiteRectangle, new Vector2(_player.px + player_sprite_offset + _map.mapSize / 4, _player.py), null,
                Color.Red, _player.pa, Vector2.Zero, new Vector2(_map.mapSize, 2f),
                SpriteEffects.None, 0f);
    }

    private void DrawMap()
    {
        // Offset to center the map
        int offset = Helper.GetWindowWidth() / 2 - _map.mapWidth * _map.mapSize / 2;

        int[] map = _map.GetMap();
        for (int i = 0; i < _map.mapWidth; i++)
        {
            for (int j = 0; j < _map.mapHeight; j++)
            {
                Color color = Color.White;
                if (map[i * _map.mapWidth + j] == 1)
                {
                    color = Color.Black;
                }
                if (map[i * _map.mapWidth + j] == 2)
                {
                    color = Color.LightGreen;
                }
                if (map[i * _map.mapWidth + j] == 3)
                {
                    color = Color.Red;
                }
                _spriteBatch.Draw(whiteRectangle, new Vector2(j * _map.mapSize + offset, i * _map.mapSize), null,
                    color, 0f, Vector2.Zero, new Vector2(_map.mapSize, _map.mapSize),
                    SpriteEffects.None, 0f);
            }
        }
    }
}
