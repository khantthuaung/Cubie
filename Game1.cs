using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Cubie;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;   
    private CameraManager _camera;
    private GameManager _game; 
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _camera = new CameraManager(Window, GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        Globals.Content = Content;
        Globals.GraphicsDevice = GraphicsDevice;
        _game = new();
        _game.Init(_camera);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Globals.GraphicsDevice);
        Globals.SpriteBatch = _spriteBatch;
        _game.Load(_graphics);

        Globals.Pixel = new Texture2D(GraphicsDevice, 1, 1);
        Globals.Pixel.SetData(new[] { Color.White });
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        Globals.Update(gameTime);
        _game.Update(gameTime,_graphics);
        // TODO: Add your update logic here
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
       
        // _player.Draw(_spriteBatch);
        _game.Draw();
        _spriteBatch.End();
        base.Draw(gameTime);
    }
    
}
