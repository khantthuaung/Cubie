using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Cubie;

public class GameManager
{
    private Player _player;
    private List<Npc> _npcs;
    private CameraManager _camera;
    private Map _currentMap;
    private EventManager _eventManager;
    private Globals.Location _currentLocation = Globals.Location.Bedroom;
    private static readonly Texture2D _exclamation = Globals.Content.Load<Texture2D>("exclamation");
    private float _distance;
    private Npc _nearbyNpc = null;
    private bool _isPlayerNearNpc = false;
    //events
    private bool _isEventTriggered = false;
    private InputManager keyboardManager;
    private KeyboardState previous;
    private MouseState prevMouse;
    public void Init(CameraManager camera)
    {
        _camera = camera;
        _currentMap = new DynamicMap(_currentLocation);

        //events
        keyboardManager = new KeyboardManager();
        _eventManager = new EventManager();
        _eventManager.CreateEvent();
        //
        _npcs = new List<Npc>();
        _player = new("player_run", "player_idle", _npcs);

        //add npcs
        _npcs.Add(new Npc("edward", Globals.Location.Bedroom, new Vector2(704, 32), Globals.Direction.Down));
        _npcs.Add(new Npc("molly", Globals.Location.Register, new Vector2(832, 96), Globals.Direction.Left));
        _npcs.Add(new Npc("lucy", Globals.Location.Competition, new Vector2(64, 96), Globals.Direction.Down));
        _npcs.Add(new Npc("rob", Globals.Location.Training, new Vector2(800, 351), Globals.Direction.Left));
    }
    public void Load(GraphicsDeviceManager graphics)
    {
        _currentMap.LoadMap(Globals.Content, Globals.GraphicsDevice, graphics);
        _camera.SetMapBound(_currentMap.MapWidthInPixels, _currentMap.MapHeightInPixels);
    }

    public void Update(GameTime gt, GraphicsDeviceManager graphics)
    {
        _camera.Follow(_player.Position);
        KeyboardState current = Keyboard.GetState();
        MouseState currMouse = Mouse.GetState();
        keyboardManager.Update(current, previous);
        previous = current;

        if (_currentMap.CheckForMapTransition(_player.GetBounds(), out Globals.Location newLocation, out Vector2 spawnPos))
        {
            SwitchMap(newLocation, graphics, spawnPos);
        }
        _player.Update(gt, _currentMap, _npcs);
        foreach (Npc npc in _npcs)
        {
            npc.Update(gt, _currentLocation);
            if (_currentLocation == npc.GetLocation)
            {
                Distance = Vector2.Distance(_player.Position, npc.Position);
                if (Distance < 60f)
                {
                    _isPlayerNearNpc = true;
                    _nearbyNpc = npc;
                    if (keyboardManager.IsPressed(Keys.E))
                    {
                        switch (npc.GetLocation)
                        {
                            case Globals.Location.Register:
                                _isEventTriggered = true;
                                break;
                            case Globals.Location.Competition:
                                break;
                            case Globals.Location.Training:
                                break;
                            default:
                                break;
                        }
                    }
                    if (_isEventTriggered &&
                    currMouse.LeftButton == ButtonState.Pressed &&
                    prevMouse.LeftButton == ButtonState.Released)
                    {
                        if (UIManager.CloseRect.Contains(currMouse.Position))
                        {
                            _isEventTriggered = false;
                            break;
                        }
                    }
                    if (keyboardManager.IsPressed(Keys.Space)) { _isEventTriggered = false; }
                    break;
                }
                else
                {
                    _isPlayerNearNpc = false;
                }

            }
        }
        prevMouse = currMouse;

    }
    public void Draw()
    {
        _currentMap.DrawMap(_camera.GetViewMatrix());
        _player.Draw(_player.Position);
        foreach (Npc npc in _npcs)
        {
            npc.Draw(_currentLocation);
        }
        if (_isPlayerNearNpc && _nearbyNpc != null)
        {
            Vector2 exMarkPos = _nearbyNpc.Position - new Vector2(-14, 8); //above head
            Rectangle boxRect = new Rectangle((int)exMarkPos.X, (int)exMarkPos.Y, 8, 16);

            Globals.SpriteBatch.Draw(_exclamation, boxRect, Color.White);
        }
        if (_isEventTriggered)
        {
            UIManager.DrawDailyEvents(_eventManager);
        }
    }
    private void SwitchMap(Globals.Location newLocation, GraphicsDeviceManager graphics, Vector2 spawnPos)
    {
        _currentLocation = newLocation;
        _currentMap = new DynamicMap(_currentLocation);
        _currentMap.LoadMap(Globals.Content, Globals.GraphicsDevice, graphics);
        _camera.SetMapBound(_currentMap.MapWidthInPixels, _currentMap.MapHeightInPixels);
        _player.SetPosition(spawnPos);
    }
    public float Distance
    {
        get { return _distance; }
        set { _distance = value; }
    }
}
