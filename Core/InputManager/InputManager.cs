using Microsoft.Xna.Framework.Input;

namespace Cubie;

public interface InputManager
{
    bool IsPressed(Keys k);
    bool IsHeld(Keys k);
    bool IsReleased(Keys k);
    void Update(KeyboardState current, KeyboardState previous);
}