using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class Bullet : SpaceObject {
    private float bulletLifetime = 3f;

    public Bullet(Vector2 position, float rotation) : base(position, rotation)
    {
        speed = 5f;
    }

    public override void Update()
    {
        base.Update();
        CheckLifetime();
    }

    public void CheckCollisions(Enemy target)
    {
        if(Math.Sqrt(Math.Pow(target.Position.X-Position.X,2)+ Math.Pow(target.Position.Y - Position.Y, 2)) < target.Size)
        {
            IDamageable targetDamageable = (IDamageable)target;
            if(targetDamageable != null)
            {
                targetDamageable.GetHit();
                Destroyed = true;
            }
        }
    }

    private void CheckLifetime()
    {
        bulletLifetime -= GameManager.GetInstance().deltaTime;
        if (bulletLifetime <= 0f)
            Destroyed = true;
    }
}
