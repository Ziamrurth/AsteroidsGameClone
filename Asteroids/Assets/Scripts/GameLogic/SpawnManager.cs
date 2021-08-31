using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class SpawnManager {
    #region Singleton
    private static SpawnManager instance;

    private SpawnManager()
    { }

    public static SpawnManager GetInstance()
    {
        if (instance == null)
            instance = new SpawnManager();
        return instance;
    }
    #endregion

    public delegate void OnObjectCreated(SpaceObject spaceObject);
    public OnObjectCreated onObjectCreatedCallback;

    private int _asteroidsSpawnNumber = 5;
    private float _asteroidsSpawnRate = 20f;
    private float _ufoSpawnRate = 60f;

    private float _asteroidSpawnTimer;
    private float _ufoSpawnTimer;

    private Random _random;

    public void Start()
    {
        _random = new Random();

        _asteroidSpawnTimer = 0f;
        _ufoSpawnTimer = _ufoSpawnRate;

        SpawnPlayer();
    }

    public void Update()
    {
        _asteroidSpawnTimer -= GameManager.GetInstance().deltaTime;
        _ufoSpawnTimer -= GameManager.GetInstance().deltaTime;

        if (_asteroidSpawnTimer <= 0)
        {
            for (int i = 0; i < _asteroidsSpawnNumber; i++)
            {
                Vector2 spawnPosition = GetRandomPositionBorder(_random);
                SpawnAsteroid(spawnPosition, 1f, 1f);
            }

            _asteroidSpawnTimer = _asteroidsSpawnRate;
        }
        if (_ufoSpawnTimer <= 0)
        {
            Vector2 spawnPosition = GetRandomPositionBorder(_random);
            SpawnUfo(spawnPosition);

            _ufoSpawnTimer = _ufoSpawnRate;
        }
    }

    private void SpawnPlayer()
    {
        Vector2 startPlayerPos = Vector2.Zero;
        float startPlayerRot = 90f;
        SpaceObject player = new Player(startPlayerPos, startPlayerRot);
        onObjectCreatedCallback?.Invoke(player);

        (player as Player).onPlayerShootCallback += SpawnBullet;
        (player as Player).onLaserAttackCallback += GameManager.GetInstance().CheckLaserCollidions;
    }

    private void SpawnAsteroid(Vector2 pos, float size, float speed)
    {
        int asteroidsNumber = 1;
        if (size < 1f)
            asteroidsNumber = 2;
        for (int i = 0; i < asteroidsNumber; i++)
        {
            float rot = _random.Next(0, 360);
            SpaceObject asteroid = new Asteroid(pos, rot, size, speed);
            onObjectCreatedCallback?.Invoke(asteroid);
            (asteroid as Asteroid).onAsteroidDestroyedCallback += SpawnAsteroid;
        }
    }

    private void SpawnUfo(Vector2 pos)
    {
        float startUfoRot = 90f;
        float ufoSize = 0.2f;
        SpaceObject ufo = new Ufo(pos, startUfoRot, ufoSize);
        onObjectCreatedCallback?.Invoke(ufo);
    }

    private void SpawnBullet(Vector2 pos, float rotation)
    {
        SpaceObject bullet = new Bullet(pos, rotation);
        onObjectCreatedCallback?.Invoke(bullet);
    }

    private Vector2 GetRandomPositionBorder(Random random)
    {
        Vector2 randomPosition;

        Vector2 worldDim = GameManager.GetInstance().worldDimentions;
        float rndX = random.Next(-(int)worldDim.X / 2, (int)worldDim.X / 2) + (float)random.NextDouble();
        float rndY = random.Next(-(int)worldDim.Y / 2, (int)worldDim.Y / 2) + (float)random.NextDouble();

        if (random.NextDouble() > 0.5)
        {
            if (random.NextDouble() > 0.5)
                randomPosition = new Vector2(-worldDim.X / 2, rndY);
            else
                randomPosition = new Vector2(worldDim.X / 2, rndY);
        }
        else
        {
            if (random.NextDouble() > 0.5)
                randomPosition = new Vector2(rndX, -worldDim.Y / 2);
            else
                randomPosition = new Vector2(rndX, worldDim.Y / 2);
        }
        return randomPosition;
    }
}
