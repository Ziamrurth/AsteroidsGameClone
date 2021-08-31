using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class Player : SpaceObject {
    public delegate void OnPlayerShoot(Vector2 pos, float rotation);
    public OnPlayerShoot onPlayerShootCallback;

    public delegate void OnLaserAttack(LaserAttack laser);
    public OnLaserAttack onLaserAttackCallback;

    public float laserRaload;
    public int laserCharges;
    public float instantSpeed;

    private float _rotationSpeed = 120f;
    private float _attackReloadTime = 0.3f;
    private float _laserLength = 20f;
    private float _laserChargeTime = 10f;
    private int _maxLaserCharges = 3;

    private float _attackReload;

    public Player(Vector2 position, float rotation) : base(position, rotation)
    {
        Velocity = Vector2.Zero;
        _attackReload = _attackReloadTime;
        laserCharges = 0;
        laserRaload = _laserChargeTime;
    }

    public void CheckCollisions(Enemy enemy)
    {
        if (Math.Sqrt(Math.Pow(enemy.Position.X - Position.X, 2) + Math.Pow(enemy.Position.Y - Position.Y, 2)) < enemy.Size)
        {
            Destroyed = true;
        }
    }

    public override void Update()
    {
        base.Update();
        Shoot();
        LaserAttack();

        instantSpeed = (float)(Math.Sqrt(Math.Pow(Velocity.X, 2) + Math.Pow(Velocity.Y, 2)));
    }

    public override void Move()
    {
        PlayerInput playerInput = PlayerInput.GetInstance();
        switch (playerInput.Rotation)
        {
            case global::Rotation.Left:
                {
                    Rotation += _rotationSpeed * GameManager.GetInstance().deltaTime;
                    break;
                }
            case global::Rotation.None:
                {
                    break;
                }
            case global::Rotation.Right:
                {
                    Rotation -= _rotationSpeed * GameManager.GetInstance().deltaTime;
                    break;
                }
        }

        if (playerInput.Acceleration)
        {
            Velocity += new Vector2((float)Math.Cos(Math.PI * Rotation / 180.0), (float)Math.Sin(Math.PI * Rotation / 180.0)) * GameManager.GetInstance().deltaTime;
        }
        Position += Velocity * GameManager.GetInstance().deltaTime;
    }

    private void Shoot()
    {
        _attackReload -= GameManager.GetInstance().deltaTime;

        if (PlayerInput.GetInstance().Shoot && _attackReload <= 0)
        {
            onPlayerShootCallback?.Invoke(Position, Rotation);
            _attackReload = _attackReloadTime;
        }
    }

    private void LaserAttack()
    {
        if(laserCharges < _maxLaserCharges)
        {
            laserRaload -= GameManager.GetInstance().deltaTime;

            if (laserRaload <= 0)
            {
                laserCharges++;
                laserRaload = _laserChargeTime;
            }
        }

        if (PlayerInput.GetInstance().LaserAttack && laserCharges > 0)
        {
            Vector2 laserStart = new Vector2(Position.X, Position.Y);
            Vector2 laserRotation = new Vector2((float)Math.Cos(Math.PI * Rotation / 180.0), (float)Math.Sin(Math.PI * Rotation / 180.0));
            Vector2 laserEnd = new Vector2(Position.X + laserRotation.X * _laserLength, Position.Y + laserRotation.Y * _laserLength);
            
            LaserAttack laser = new LaserAttack(laserStart, laserEnd);
            onLaserAttackCallback?.Invoke(laser);

            laserCharges--;
        }
    }
}
