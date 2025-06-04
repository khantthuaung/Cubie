using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace Cubie;

public class Player : Sprite
{
    private Vector2 _position = new(100, 100);
    private readonly float _speed = 200f;
    private readonly Animation _animation;
    private List<Npc> _npcs = new List<Npc>();
    public Player(string runTexture, string idleTexture, List<Npc> npcs) : base(idleTexture)
    {
        var run = Globals.Content.Load<Texture2D>(runTexture);
        var idle = Globals.Content.Load<Texture2D>(idleTexture);
        _animation = new Animation(idle, run, _position);
        _npcs = npcs;
    }
    public override void Update(GameTime gt, Map currentMap)
    {
        _animation.Update(ref _position, _speed, gt, currentMap,_npcs);
    }

    public override void Draw(Vector2 position)
    {
        _animation.Draw(position);
    }
    public Vector2 Position
    {
        get { return _position; }
        set { _position = value; }
    }
    public void SetPosition(Vector2 position)
    {
        _position = position;
        _animation.SetPosition(position);
    }
    public RectangleF GetBounds()
    {
            return new RectangleF(
                _position.X + 8,
                _position.Y + 48,
                16,
                16
            );   
    }

}