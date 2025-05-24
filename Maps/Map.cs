using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
namespace Cubie;

public abstract class Map
{
    private TiledMap _mapTexture;
    private TiledMapRenderer _mapRenderer;
    private string _mapName;
    public Map(string name)
    {
        _mapName = name;
    }
    public virtual void LoadMap(ContentManager content, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphicsManager)
    {
        _mapTexture = content.Load<TiledMap>(_mapName);
        _mapRenderer = new TiledMapRenderer(graphicsDevice, _mapTexture);

        // Resize window to fit the map
        graphicsManager.PreferredBackBufferWidth = _mapTexture.WidthInPixels;
        graphicsManager.PreferredBackBufferHeight = _mapTexture.HeightInPixels;
        graphicsManager.ApplyChanges();
    }
    public virtual void UpdateMap(GameTime gameTime)
    {
        _mapRenderer.Update(gameTime);
    }
    public virtual void DrawMap(Matrix viewMatrix)
    {
        _mapRenderer.Draw(viewMatrix);
    }
    public int MapWidthInPixels => _mapTexture.WidthInPixels;
    public int MapHeightInPixels => _mapTexture.HeightInPixels;

}