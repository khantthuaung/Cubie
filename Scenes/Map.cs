using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace Cubie;
//base map class for all the map including game windows width and height based on the map size
public abstract class Map
{
    private TiledMap _mapTexture;
    private TiledMapRenderer _mapRenderer;
    private readonly string _mapName;
    private TiledMapTileLayer _wall, _objects, _objects2;
    private List<TiledMapTileLayer> _collidableLayers;
    public Map(string name)
    {
        _mapName = name;
    }
    public virtual void LoadMap(ContentManager content, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphicsManager)
    {
        _mapTexture = content.Load<TiledMap>(_mapName);
        _wall = _mapTexture.GetLayer<TiledMapTileLayer>("walls");
        _objects = _mapTexture.GetLayer<TiledMapTileLayer>("objects");
        _objects2 = _mapTexture.GetLayer<TiledMapTileLayer>("objects2");
        _collidableLayers = new List<TiledMapTileLayer> { _wall, _objects, _objects2 };
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
    public int MapWidthInPixels { get { return _mapTexture.WidthInPixels; } }
    public int MapHeightInPixels { get { return _mapTexture.HeightInPixels; } }

    public bool CheckCollision(RectangleF playerBounds)
    {
        foreach (var layer in _collidableLayers) // skip the floor layer
        {
            for (ushort y = 0; y < layer.Height; y++)
            {
                for (ushort x = 0; x < layer.Width; x++)
                {
                    if (layer.GetTile(x, y).GlobalIdentifier != 0)
                    {
                        RectangleF tileRect = new RectangleF(
                            x * _mapTexture.TileWidth,
                            y * _mapTexture.TileHeight,
                            _mapTexture.TileWidth,
                            _mapTexture.TileHeight
                        );

                        if (playerBounds.Intersects(tileRect))
                            return true;
                    }
                }
            }
        }
        return false;
    }

    public bool CheckForMapTransition(RectangleF playerBounds, out Globals.Location newLocation,out Vector2 spawnPos)
    {
        newLocation = Globals.Location.Bedroom; // default
        spawnPos = new Vector2(100, 100); // default

        var layer = _mapTexture.GetLayer<TiledMapTileLayer>("doors");
        if (layer == null)
            return false;

        for (ushort y = 0; y < layer.Height; y++)
        {
            for (ushort x = 0; x < layer.Width; x++)
            {
                var tile = layer.GetTile(x, y);
                if (tile.GlobalIdentifier == 0)
                    continue;

                var tileRect = new RectangleF(
                    x * _mapTexture.TileWidth,
                    y * _mapTexture.TileHeight,
                    _mapTexture.TileWidth,
                    _mapTexture.TileHeight
                );

                if (playerBounds.Intersects(tileRect))
                {
                    // Find the correct tileset for this tile
                    var tileset = _mapTexture.GetTilesetByTileGlobalIdentifier(tile.GlobalIdentifier);
                    if (tileset != null)
                    {
                        // Calculate the local tile ID within the tileset
                        int firstGid = _mapTexture.GetTilesetFirstGlobalIdentifier(tileset);
                        int localId = (int)tile.GlobalIdentifier - firstGid;

                        // Find the specific tile data that matches our door tile
                        var tileData = tileset.Tiles.FirstOrDefault(t => t.LocalTileIdentifier == localId);
                        if (tileData != null && tileData.Properties.TryGetValue("destination", out string destination))
                        {
                            if (Enum.TryParse(destination, out Globals.Location parsedLocation))
                            {
                                newLocation = parsedLocation;
                                if (tileData.Properties.TryGetValue("spawnX", out string spawnXStr) &&
                                    tileData.Properties.TryGetValue("spawnY", out string spawnYStr) &&
                                    float.TryParse(spawnXStr, out float spawnX) &&
                                    float.TryParse(spawnYStr, out float spawnY))
                                {
                                    spawnPos = new Vector2(spawnX, spawnY);
                                }

                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
}