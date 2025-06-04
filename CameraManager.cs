using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Cubie
{
    public class CameraManager
    {
        private OrthographicCamera _camera;
        private ViewportAdapter _viewportAdapter;
        private int _mapWidth;
        private int _mapHeight;

        public CameraManager(GameWindow window, GraphicsDevice graphicsDevice, int screenWidth, int screenHeight)
        {
            _viewportAdapter = new BoxingViewportAdapter(window, graphicsDevice, screenWidth, screenHeight);
            _camera = new OrthographicCamera(_viewportAdapter);
        }

        public void SetMapBound(int width, int height)
        {
            _mapWidth = width;
            _mapHeight = height;
        }

        public void Follow(Vector2 target)
        {
            Vector2 viewSize = new Vector2(_camera.BoundingRectangle.Width, _camera.BoundingRectangle.Height);
            Vector2 halfViewSize = viewSize / 2f;

            // Clamp camera position to keep it within map bounds
            float minX = halfViewSize.X;
            float minY = halfViewSize.Y;
            float maxX = _mapWidth - halfViewSize.X;
            float maxY = _mapHeight - halfViewSize.Y;

            float clampedX = MathHelper.Clamp(target.X, minX, maxX);
            float clampedY = MathHelper.Clamp(target.Y, minY, maxY);

            _camera.LookAt(new Vector2(clampedX, clampedY));
        }

        public Matrix GetViewMatrix()
        {
            return _camera.GetViewMatrix();
        }

        public Vector2 GetPosition()
        {
            return _camera.Position;
        }
    }
}
