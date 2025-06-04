using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Cubie;

public class GameManager
{
    private Player _player;
    private List<Npc> _npcs;
    private CameraManager _camera;
    private Map _currentMap;
    private Globals.Location _currentLocation = Globals.Location.Bedroom;
    public void Init(CameraManager camera)
    {
        _camera = camera;
        _currentMap = new DynamicMap(_currentLocation);
    
       
        _npcs = new List<Npc>();
        _player = new("player_run", "player_idle",_npcs);

         //add npcs
        _npcs.Add(new Npc("edward", Globals.Location.Bedroom, new Vector2(704, 32), Globals.Direction.Down));
        _npcs.Add(new Npc("molly", Globals.Location.Register, new Vector2(832, 96), Globals.Direction.Left));
        _npcs.Add(new Npc("lucy", Globals.Location.Competition, new Vector2(64, 96), Globals.Direction.Down));
        _npcs.Add(new Npc("rob", Globals.Location.Training, new Vector2(800, 351), Globals.Direction.Left));
    }
    public void Load(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice)
    {
        _currentMap.LoadMap(Globals.Content, graphicsDevice, graphics);
        _camera.SetMapBound(_currentMap.MapWidthInPixels, _currentMap.MapHeightInPixels);
    }
    public void Update(GameTime gt, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice)
    {
        _camera.Follow(_player.Position);
        if (_currentMap.CheckForMapTransition(_player.GetBounds(), out Globals.Location newLocation, out Vector2 spawnPos))
        {
            SwitchMap(newLocation, graphics, graphicsDevice, spawnPos);
        }
        _player.Update(gt, _currentMap);
        foreach(Npc npc in _npcs)
        {
            npc.Update(gt, _currentLocation);
        }
        
    }
    public void Draw()
    {
        _currentMap.DrawMap(_camera.GetViewMatrix());
        _player.Draw(_player.Position);
        foreach(Npc npc in _npcs)
        {
            npc.Draw(_currentLocation);
        }
    }
    private void SwitchMap(Globals.Location newLocation,GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice,Vector2 spawnPos)
    {
        _currentLocation = newLocation;
        _currentMap = new DynamicMap(_currentLocation);
        _currentMap.LoadMap(Globals.Content, graphicsDevice, graphics);
        _camera.SetMapBound(_currentMap.MapWidthInPixels, _currentMap.MapHeightInPixels);
        _player.SetPosition(spawnPos);
    }
}