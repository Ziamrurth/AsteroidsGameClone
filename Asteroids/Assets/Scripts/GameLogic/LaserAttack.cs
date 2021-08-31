using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class LaserAttack {
    public Vector2 StartPos { get; set; }
    public Vector2 EndPos { get; set; }

    public LaserAttack(Vector2 startPos, Vector2 endPos)
    {
        StartPos = startPos;
        EndPos = endPos;
    }

    public void CheckCollision(Enemy enemy)
    {
        float dx = EndPos.X - StartPos.X;
        float dy = EndPos.Y - StartPos.Y;

        float A = dx * dx + dy * dy;
        float B = 2 * (dx * (StartPos.X - enemy.Position.X) + dy * (StartPos.Y - enemy.Position.Y));
        float C = (StartPos.X - enemy.Position.X) * (StartPos.X - enemy.Position.X) +
            (StartPos.Y - enemy.Position.Y) * (StartPos.Y - enemy.Position.Y) -
            enemy.Size * enemy.Size;

        float det = B * B - 4 * A * C;
        
        if (det > 0)
        {
            IDamageable targetDamageable = (IDamageable)enemy;
            if (targetDamageable != null)
            {
                targetDamageable.GetHit();
            }
        }
    }
}
