using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cubie;

public static class Globals
{
    private static SpriteBatch _spriteBatch;
    private static ContentManager _content;
    public static float TotalSeconds;
    public static Texture2D Pixel;
    public enum Location
    {
        Bedroom,
        Training,
        Competition,
        Register,
        MainMenu
    }
    public enum Direction
    {
        Right = 0,
        Up = 1,
        Left = 2,
        Down = 3
    }
    //properties
    public static SpriteBatch SpriteBatch
    {
        get { return _spriteBatch; }
        set { _spriteBatch = value; }
    }
    public static ContentManager Content
    {
        get { return _content; }
        set { _content = value; }
    }
    public static void Update(GameTime gt){ TotalSeconds = (float)gt.ElapsedGameTime.TotalSeconds; }
    //readonly

}