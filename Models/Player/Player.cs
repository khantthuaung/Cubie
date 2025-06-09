using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
namespace Cubie;

public class Player : Sprite
{
    private Vector2 _position = new(100, 100);
    private readonly float _speed = 200f;
    private readonly Animation _animation;
    private List<Npc> _npcs;
    private InputManager _key;
    private KeyboardState _previous;
    public Player(string runTexture, string idleTexture, List<Npc> npcs) : base(idleTexture)
    {
        var run = Globals.Content.Load<Texture2D>(runTexture);
        var idle = Globals.Content.Load<Texture2D>(idleTexture);
        _animation = new Animation(idle, run, _position);
        _npcs = new List<Npc>();
        _key = new KeyboardManager();
    }
    public override void Update(GameTime gt, Map currentMap, List<Npc> npcs)
    {
        var current = Keyboard.GetState();
        _key.Update(current, _previous);
        _previous = current;

        Vector2 pos = Vector2.Zero;
        Globals.Direction currentDir = _animation.CurrentDirection;
        bool active = false;

        if (_key.IsHeld(Keys.W) || _key.IsHeld(Keys.Up)) { pos.Y -= 1; currentDir = Globals.Direction.Up; active = true; }
        if (_key.IsHeld(Keys.S) || _key.IsHeld(Keys.Down)) { pos.Y += 1; currentDir = Globals.Direction.Down; active = true; }
        if (_key.IsHeld(Keys.A) || _key.IsHeld(Keys.Left)) { pos.X -= 1; currentDir = Globals.Direction.Left; active = true; }
        if (_key.IsHeld(Keys.D) || _key.IsHeld(Keys.Right)) { pos.X += 1; currentDir = Globals.Direction.Right; active = true; }



        if (pos.Length() > 0)
        {
            pos.Normalize();
            Vector2 attemptedPosition = _animation.Position + pos * _speed * (float)gt.ElapsedGameTime.TotalSeconds;
            RectangleF attemptedBounds = new RectangleF(attemptedPosition.X + 8, attemptedPosition.Y + 48, 8, 8);

            bool collidesWithNpc = false;
            foreach (var npc in npcs)
            {
                if (attemptedBounds.Intersects(npc.GetBounds()))
                {
                    collidesWithNpc = true;
                    break;
                }
            }
            if (!currentMap.CheckCollision(attemptedBounds) && !collidesWithNpc)
            {
                _position = attemptedPosition;
                _animation.SetPosition(_position);
            }
        }
        else
        {
            _animation.SetPosition(_position);
            active = false;
        }
        _animation.Update(gt, active, currentDir);
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