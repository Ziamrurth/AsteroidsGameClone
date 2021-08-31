using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public abstract class SpaceObject {
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Rotation { get; set; }
    public bool Destroyed { get; set; }

    public float speed = 1f;
    

    public SpaceObject(Vector2 position, float rotation)
    {
        Position = position;
        Rotation = rotation;
        Velocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
        Destroyed = false;
    }

    public virtual void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        Velocity = new Vector2((float)Math.Cos(Math.PI * Rotation / 180.0), (float)Math.Sin(Math.PI * Rotation / 180.0));
        Position += Velocity * GameManager.GetInstance().deltaTime * speed;
    }
}
