using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Cubie;

public class Npc : Sprite
{
    private Texture2D _texture;
    private Globals.Location _location;
    private Vector2 _position;
    private Globals.Direction _direction;
    private readonly Dictionary<Globals.Direction, Rectangle[]> _npcFrames = new();
    private float _animationTimer = 0f;
    private float _frameTime = 0.2f;
    private int _currentFrame = 0;
    public Npc(string texture, Globals.Location location, Vector2 position, Globals.Direction direction) : base(texture)
    {
        _texture = Globals.Content.Load<Texture2D>(texture);
        _location = location;
        _position = position;
        _direction = direction;

        this.LoadFrames();
    }

    private void LoadFrames()
    {
        int frameWidth = 32;
        int frameHeight = 64;
        for (int d = 0; d < 4; d++) // 4 directions
        {
            Rectangle[] npcFrames = new Rectangle[6];
            for (int i = 0; i < 6; i++)
            {

                npcFrames[i] = new Rectangle((i + d * 6) * frameWidth, 0, frameWidth, frameHeight);
            }
            _npcFrames[(Globals.Direction)d] = npcFrames;
        }
    }
    public void Update(GameTime gt, Globals.Location location)
    {
        if (_location != location) return;
        _animationTimer += (float)gt.ElapsedGameTime.TotalSeconds;
        if (_animationTimer >= _frameTime)
        {
            _animationTimer = 0f;
            _currentFrame = (_currentFrame + 1) % 6;
        }

    }
    public void Draw(Globals.Location location)
    {
        if (location != _location) return;
        Rectangle source = _npcFrames[_direction][_currentFrame];
        Globals.SpriteBatch.Draw(_texture, _position, source, Color.White);
    }
    public override void Draw(Vector2 pos)
    {
        throw new System.NotImplementedException();
    }

    public override void Update(GameTime gt, Map currentMap,List<Npc> npcs)
    {
        throw new System.NotImplementedException();
    }
    public Vector2 Position
    {
        get{ return _position; }   
    }
    public Globals.Location GetLocation
    {
        get { return _location; }
    }
    public Rectangle GetBounds()
    {
        return new Rectangle((int)_position.X, (int)_position.Y + 48, 32, 48);
    }
}