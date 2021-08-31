using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class Enemy : SpaceObject {
    public float Size { get; set; }

    public Enemy(Vector2 position, float rotation, float size) : base(position, rotation)
    {
        Size = size;
    }
}
