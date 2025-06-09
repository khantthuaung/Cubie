using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cubie;

public abstract class Sprite
{

    private string _texture;
    public Sprite(string texture)
    {
        _texture = texture;
    }
    public abstract void Update(GameTime gt,Map currentMap,List<Npc> npcs);
    public abstract void Draw(Vector2 pos);
}