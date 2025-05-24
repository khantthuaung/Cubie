using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cubie;

public class DynamicMap : Map
{
    public enum Location
    {
        Bedroom,
        Training,
        Competition,
        Register,
        MainMenu
    }
    public DynamicMap(Location location) : base(GetMapName(location))
    {
    }
    private static string GetMapName(Location location)
    {
        return location switch
        {
            Location.Bedroom => "bedroom",
            Location.Training => "training",
            Location.Competition => "competition",
            Location.Register => "register",
            _ => "bedroom"
        };
    }

}