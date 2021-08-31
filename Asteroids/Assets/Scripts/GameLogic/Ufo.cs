using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class Ufo : Enemy, IDamageable {
    public Ufo(Vector2 position, float rotation, float size) : base(position, rotation, size)
    {
        speed = 2f;
    }

    public void GetHit()
    {
        Destroyed = true;
    }

    public override void Move()
    {
        SpaceObject player = GameManager.GetInstance().GetPlayer();
        if (player != null)
        {
            Vector2 direction = player.Position - Position;
            float s = (float)(Math.Sqrt(Math.Pow(Velocity.X, 2) + Math.Pow(Velocity.Y, 2)));
            float factor = (float)(s / Math.Sqrt(Math.Pow(direction.X, 2) + Math.Pow(direction.Y, 2)));

            Velocity = direction * factor;
        }
        Position += Velocity * GameManager.GetInstance().deltaTime * speed;
    }
}
