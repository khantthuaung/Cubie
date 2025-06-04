using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Cubie;

public class Animation
{


    private Texture2D _idleTexture;
    private Texture2D _runTexture;
    private Dictionary<Globals.Direction, Rectangle[]> _idleFrames = new();
    private Dictionary<Globals.Direction, Rectangle[]> _animations = new();
    private Globals.Direction _currentDirection = Globals.Direction.Down;
    private int _currentFrame = 0;
    private float _animationTimer = 0f;
    private float _frameTime = 0.1f;
    private bool _active = false;
    private Vector2 _position;
    public Animation(Texture2D idle, Texture2D run, Vector2 position)
    {
        _position = position;
        this.Load(run, idle);
    }
    public void Load(Texture2D run, Texture2D idle)
    {
        _runTexture = run;
        _idleTexture = idle;

        int frameWidth = 32;  // run.png is 768px wide / 24 frames = 32px
        int frameHeight = 64; // full height

        for (int d = 0; d < 4; d++) // 4 directions
        {
            Rectangle[] run_frames = new Rectangle[6];
            Rectangle[] idle_frames = new Rectangle[6];
            for (int i = 0; i < 6; i++)
            {
                run_frames[i] = new Rectangle((i + d * 6) * frameWidth, 0, frameWidth, frameHeight);
                idle_frames[i] = new Rectangle((i + d * 6) * frameWidth, 0, frameWidth, frameHeight);
            }
            _animations[(Globals.Direction)d] = run_frames;
            _idleFrames[(Globals.Direction)d] = idle_frames;
        }
    }
    public void Update(ref Vector2 position, float speed, GameTime gameTime, Map currentMap, List<Npc> npcs)
    {
        KeyboardState key = Keyboard.GetState();
        Vector2 pos = Vector2.Zero;

        _active = false;


        if (key.IsKeyDown(Keys.W) || key.IsKeyDown(Keys.Up)) { pos.Y -= 1; _currentDirection = Globals.Direction.Up; _active = true; }
        if (key.IsKeyDown(Keys.S) || key.IsKeyDown(Keys.Down)) { pos.Y += 1; _currentDirection = Globals.Direction.Down; _active = true; }
        if (key.IsKeyDown(Keys.A) || key.IsKeyDown(Keys.Left)) { pos.X -= 1; _currentDirection = Globals.Direction.Left; _active = true; }
        if (key.IsKeyDown(Keys.D) || key.IsKeyDown(Keys.Right)) { pos.X += 1; _currentDirection = Globals.Direction.Right; _active = true; }

        if (pos.Length() > 0)
        {
            pos.Normalize();
            Vector2 attemptedPosition = _position + pos * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            RectangleF attemptedBounds = new RectangleF(attemptedPosition.X + 8, attemptedPosition.Y + 48, 8, 8); // adjust size if needed

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
                position = _position;
            }
        }
        else { position = _position; }

        if (_active)
        {
            _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_animationTimer >= _frameTime)
            {
                _animationTimer = 0f;
                _currentFrame = (_currentFrame + 1) % 6;
            }
        }
        else
        {
            _currentFrame = 0;
        }
    }

    public void Draw(Vector2 position)
    {
        Rectangle sourceRect;
        if (_active)
        {
            sourceRect = _animations[_currentDirection][_currentFrame];
            Globals.SpriteBatch.Draw(_runTexture, position, sourceRect, Color.White);
        }
        else
        {
            sourceRect = _idleFrames[_currentDirection][_currentFrame];
            Globals.SpriteBatch.Draw(_idleTexture, position, sourceRect, Color.White);
        }
    }
    public void SetPosition(Vector2 position)
    {
        _position = position;
    }


}