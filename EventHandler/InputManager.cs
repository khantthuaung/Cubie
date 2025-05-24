using Microsoft.Xna.Framework.Input;

namespace Cubie;

public class InputManager
{
    private KeyboardState _currentInput;
    private KeyboardState _prevInput;
    public void UpdateKey()
    {
        _prevInput = _currentInput;
        _currentInput = Keyboard.GetState();
    }
    public bool IsKeyPressed(Keys key)
    {
        return _currentInput.IsKeyDown(key) && !_prevInput.IsKeyDown(key);
    }
    public bool IsKeyHeld(Keys key)
    {
        return _currentInput.IsKeyDown(key);
    }
    public bool IsKeyReleased(Keys key)
    {
        return !_currentInput.IsKeyDown(key) && _prevInput.IsKeyDown(key);
    }
}