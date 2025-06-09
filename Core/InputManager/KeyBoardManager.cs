using Microsoft.Xna.Framework.Input;

namespace Cubie;

public class KeyboardManager : InputManager
{
    private KeyboardState _current;
    private KeyboardState _previous;
    public void Update(KeyboardState current, KeyboardState previous)
    {
        _current = current;
        _previous = previous;
    }
    public bool IsHeld(Keys key)
    {
        return _current.IsKeyDown(key);
    }
    public bool IsPressed(Keys key)
    {
        return _current.IsKeyDown(key) && _previous.IsKeyUp(key);
    }
    public bool IsReleased(Keys key)
    {
        return _current.IsKeyUp(key) && _previous.IsKeyDown(key);
    }
 
}