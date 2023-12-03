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
    private Map Map = new Map(10, 10);

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Map.GenerateMap();
        Console.WriteLine("Map generated");
        Console.WriteLine("Map dimensions: " + Map.MapWidth + "x" + Map.MapHeight);
        // Debug the map, print it to the console
        for (int x = 0; x < Map.MapWidth; x++) {
            for (int y = 0; y < Map.MapHeight; y++) {
                Console.Write(Map.MapI[x, y]);
            }
            Console.WriteLine();
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        // TODO: use this.Content to load your game content here
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
