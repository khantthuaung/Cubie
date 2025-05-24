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
    private Map _currentMap;
    private DynamicMap.Location _currentLocation = DynamicMap.Location.Bedroom;
    private InputManager _input;
    private CameraManage _camera; 

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _input = new InputManager();
        _camera = new CameraManage(Window, GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, _input);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _currentMap = new DynamicMap(_currentLocation);
        
        _currentMap.LoadMap(Content, GraphicsDevice, _graphics);
        


        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _input.UpdateKey();
        _camera.UpdateCamera(gameTime);
        if (_input.IsKeyPressed(Keys.Space))
            SwitchMap(DynamicMap.Location.Training);
        if (_input.IsKeyPressed(Keys.Tab))
            SwitchMap(DynamicMap.Location.Register);
        if (_input.IsKeyPressed(Keys.E))
            SwitchMap(DynamicMap.Location.Bedroom);
        _currentMap?.UpdateMap(gameTime);

        // TODO: Add your update logic here
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
        _currentMap.DrawMap(_camera.GetViewMatrix());
        _spriteBatch.End();
        base.Draw(gameTime);
    }
    private void SwitchMap(DynamicMap.Location newLocation)
    {
        _currentLocation = newLocation;
        _currentMap = new DynamicMap(_currentLocation);
        _currentMap.LoadMap(Content, GraphicsDevice, _graphics);
        _camera.SetMapBound(_currentMap.MapWidthInPixels, _currentMap.MapHeightInPixels);
        
    }
}
