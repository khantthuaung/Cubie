using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Cubie;

public class CameraManage
{
    private OrthographicCamera _camera;
    private ViewportAdapter _viewportAdapter;
    private readonly InputManager _input;
    private int _mapWidth;
    private int _mapHeight;
    public CameraManage(GameWindow window, GraphicsDevice graphicsDevice, int width, int height, InputManager input)
    {
        _viewportAdapter = new BoxingViewportAdapter(window, graphicsDevice, width, height);
        _camera = new OrthographicCamera(_viewportAdapter);
        _input = input;
    }
    public void UpdateCamera(GameTime gameTime)
    {
        var direction = GetMovementDirection();
        const float speed = 200f;
        var newPosition = _camera.Position + direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _camera.Position = ClampToMapBounds(newPosition);

    }
    private Vector2 GetMovementDirection()
    {
        var direction = Vector2.Zero;

        if (_input.IsKeyHeld(Keys.Right)) direction.X += 1;
        if (_input.IsKeyHeld(Keys.Left)) direction.X -= 1;
        if (_input.IsKeyHeld(Keys.Down)) direction.Y += 1;
        if (_input.IsKeyHeld(Keys.Up)) direction.Y -= 1;

        return direction;
    }
    public void SetMapBound(int width, int height)
    {
        _mapWidth = width;
        _mapHeight = height;
    }
    public Vector2 ClampToMapBounds(Vector2 position)
    {
        var size = _camera.BoundingRectangle.Size;
        var viewSize = new Vector2(size.Width, size.Height);


        float clampedX = MathHelper.Clamp(position.X, 0, _mapWidth - viewSize.X);
        float clampedY = MathHelper.Clamp(position.Y, 0, _mapHeight - viewSize.Y);

        return new Vector2(clampedX, clampedY);
    }
    public Matrix GetViewMatrix() => _camera.GetViewMatrix();

    public void SetPosition(Vector2 position) => _camera.Position = position;

    public Vector2 GetPosition() => _camera.Position;
}