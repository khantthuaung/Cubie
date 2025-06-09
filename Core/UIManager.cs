using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cubie;

public static class UIManager
{
    private static readonly Texture2D _uiBackground = Globals.Content.Load<Texture2D>("dialog");
    private static readonly SpriteFont _font = Globals.Content.Load<SpriteFont>("font");
    private static readonly Texture2D _registerButton = Globals.Content.Load<Texture2D>("button");
    private static readonly Texture2D _closeButton = Globals.Content.Load<Texture2D>("close");
    private static Rectangle _closeRectangle;
    public static void DrawDailyEvents(EventManager eventManager)
    {
        int boxX = (Globals.GraphicsDevice.Viewport.Width - 600) / 2;
        int boxY = (Globals.GraphicsDevice.Viewport.Height - 150) / 2;

        Rectangle boxRect = new Rectangle(boxX, boxY, 600, 150);
        CloseRect = new Rectangle(boxRect.Right - 64, boxRect.Top + 10, 48, 24);
        Globals.SpriteBatch.Draw(_uiBackground, boxRect, Color.White);
        Globals.SpriteBatch.Draw(_closeButton, CloseRect, Color.White);


        for (int i = 0; i < eventManager.TodayEvents.Count; i++)
        {
            int eventY = boxRect.Top + 40 + i * 40;
            var e = eventManager.TodayEvents[i];
            string text = $"{e.EventName} --- {e.Difficulty}   <<{e.GetFormattedTime()}>>";
            Globals.SpriteBatch.DrawString(_font, text, new Vector2(boxRect.X + 20, eventY), Color.Black);

            // Draw a simple "Register" box next to each event
            Globals.SpriteBatch.Draw(_registerButton, new Rectangle(boxRect.Right - 120, eventY - 5, 80, 30), Color.White);
            Globals.SpriteBatch.DrawString(_font, "Register", new Vector2(boxRect.Right -108, eventY), Color.White);
        }
    }
    public static void DrawTraining() { }
    public static void DrawCompetition() { }
    public static Rectangle CloseRect
    {
        get { return _closeRectangle; }
        private set { _closeRectangle = value; }
    }
}