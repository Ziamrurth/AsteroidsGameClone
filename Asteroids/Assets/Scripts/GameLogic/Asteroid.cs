using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class Asteroid : Enemy, IDamageable {
    public delegate void OnAsteroidDestroyed(Vector2 pos, float size, float speed);
    public OnAsteroidDestroyed onAsteroidDestroyedCallback;

    public Asteroid(Vector2 position, float rotation, float size, float speed) : base(position, rotation, size)
    {
        this.speed = speed;
    }

    public void GetHit()
    {
        Destroyed = true;
        if (Size > 0.25f)
            onAsteroidDestroyedCallback?.Invoke(Position, Size * 0.5f, speed * 1.2f);
    }
}
