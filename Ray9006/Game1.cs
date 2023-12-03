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
    private int[] _window_dimensions = new int[2] { 1920, 1080 };

    private Player _player;
    Texture2D whiteRectangle;
    private MapClass _map;
    private Raycaster9006 _raycaster;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Set the window size
        _graphics.PreferredBackBufferWidth = _window_dimensions[0];
        _graphics.PreferredBackBufferHeight = _window_dimensions[1];
        _graphics.ApplyChanges();

        _map = new MapClass();

        _player = new Player((64+32)*3, 64+32);
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

        // TODO: Add your update logic here
        _player.Update(state);
        

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        DrawMap();
        // Draw a rectangle at the player's position
        DrawPlayer();
        _raycaster.Cast(_player, _map, _spriteBatch, whiteRectangle);

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void DrawPlayer()
    {
        // Draw a rectangle at the player's position
        // Option Two (if you have floating-point coordinates)
        _spriteBatch.Draw(whiteRectangle, new Vector2(_player.px-15f, _player.py-15f), null,
                Color.Red, 0f, Vector2.Zero, new Vector2(30f, 30f),
                SpriteEffects.None, 0f);
        // Draw a line from the player's position to the player's direction
        _spriteBatch.Draw(whiteRectangle, new Vector2(_player.px, _player.py), null,
                Color.Yellow, _player.pa, Vector2.Zero, new Vector2(100f, 1f),
                SpriteEffects.None, 0f);
        // Draw a line from the player's position to walls
        // _raycaster.DrawLine(_player, _map, _spriteBatch, whiteRectangle);
    }

    private void DrawMap()
    {
        int[] map = _map.GetMap();
        for (int i = 0; i < _map.mapWidth; i++)
        {
            for (int j = 0; j < _map.mapHeight; j++)
            {
                if (map[i * _map.mapWidth + j] == 1)
                {
                    _spriteBatch.Draw(whiteRectangle, new Vector2(j * _map.mapSize, i * _map.mapSize), null,
                        Color.Black, 0f, Vector2.Zero, new Vector2(_map.mapSize, _map.mapSize),
                        SpriteEffects.None, 0f);
                }
            }    
        }
    }
}
