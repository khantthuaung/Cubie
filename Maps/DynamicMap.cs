using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cubie;

public class DynamicMap : Map
{
    public DynamicMap(Globals.Location location) : base(GetMapName(location))
    {
    }
    public static string GetMapName(Globals.Location location)
    {
        switch (location)
        {
            case Globals.Location.Bedroom:
                return "bedroom";
            case Globals.Location.Training:
                return "training";
            case Globals.Location.Competition:
                return "competition";
            case Globals.Location.Register:
                return "register";
            default:
                return "bedroom";
        }
    }
    

}