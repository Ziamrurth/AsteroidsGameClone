using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

public class GameManager {
    #region Singleton
    private static GameManager instance;

    private GameManager()
    { }

    public static GameManager GetInstance()
    {
        if (instance == null)
            instance = new GameManager();
        return instance;
    }
    #endregion

    public delegate void OnObjectDestroyed(SpaceObject spaceObject);
    public OnObjectDestroyed onObjectDestroyedCallback;

    public delegate void OnPlayerKilled();
    public OnPlayerKilled onPlayerKilledCallback;

    public List<SpaceObject> spaceObjects;

    public Vector2 worldDimentions = new Vector2(18f, 10f);

    public float deltaTime;

    private SpawnManager _spawnManager;
    private ScoreManager _scoreManager;

    private DateTime _timeSaved;
    private DateTime _timeNow;

    private List<SpaceObject> _spaceObjectsToCreate;

    private bool _game;

    public void Start()
    {
        _game = true;
        onPlayerKilledCallback += GameOver;

        _timeSaved = DateTime.Now;
        _timeNow = DateTime.Now;

        spaceObjects = new List<SpaceObject>();
        _spaceObjectsToCreate = new List<SpaceObject>();

        _spawnManager = SpawnManager.GetInstance();
        _spawnManager.onObjectCreatedCallback += AddSpaceObject;
        _spawnManager.Start();

        _scoreManager = ScoreManager.GetInstance();
        _scoreManager.Start();
    }

    public void Update()
    {
        if (!_game) return;

        CalculateDeltaTime();

        _spawnManager.Update();
        if (_spaceObjectsToCreate.Count != 0)
        {
            spaceObjects.AddRange(_spaceObjectsToCreate);
            _spaceObjectsToCreate = new List<SpaceObject>();
        }

        UpdateObjects();
        CheckCollisions();
        DestroyObjects();
        CheckPlayerAlive();
    }

    public void AddSpaceObject(SpaceObject spaceObject)
    {
        _spaceObjectsToCreate.Add(spaceObject);
    }

    public void Restart()
    {
        foreach (SpaceObject spaceObject in spaceObjects)
        {
            spaceObject.Destroyed = true;
        }
        foreach (SpaceObject spaceObject in _spaceObjectsToCreate)
        {
            spaceObject.Destroyed = true;
        }
        Start();
    }

    public SpaceObject GetPlayer()
    {
        List<SpaceObject> playerList = spaceObjects.Where(spaceObject => spaceObject as Player != null).ToList();
        if (playerList.Count == 0)
            return null;
        else return playerList[0];
    }

    public void CheckLaserCollidions(LaserAttack laser)
    {
        foreach (SpaceObject enemy in spaceObjects.Where(spaceObject => spaceObject as Enemy != null).ToList())
        {
            laser.CheckCollision(enemy as Enemy);
        }
    }

    private void UpdateObjects()
    {
        foreach (SpaceObject spaceObject in spaceObjects)
        {
            spaceObject.Update();
            WrapCoordinates(spaceObject);
        }
    }

    private void DestroyObjects()
    {
        List<SpaceObject> objectsToDestroy = new List<SpaceObject>();
        foreach (SpaceObject spaceObject in spaceObjects)
        {
            if (spaceObject.Destroyed)
            {
                objectsToDestroy.Add(spaceObject);
                onObjectDestroyedCallback?.Invoke(spaceObject);
            }

        }
        spaceObjects = spaceObjects.Except(objectsToDestroy).ToList();
    }

    private void CheckCollisions()
    {
        foreach (SpaceObject bullet in spaceObjects.Where(spaceObject => spaceObject as Bullet != null).ToList())
        {
            foreach (SpaceObject enemy in spaceObjects.Where(spaceObject => spaceObject as Enemy != null).ToList())
            {
                (bullet as Bullet).CheckCollisions(enemy as Enemy);
            }
        }

        foreach (SpaceObject enemy in spaceObjects.Where(spaceObject => spaceObject as Enemy != null).ToList())
        {
            foreach (SpaceObject player in spaceObjects.Where(spaceObject => spaceObject as Player != null).ToList())
            {
                (player as Player).CheckCollisions(enemy as Enemy);
            }
        }
    }

    private void CalculateDeltaTime()
    {
        _timeNow = DateTime.Now;
        deltaTime = (float)((_timeNow.Ticks - _timeSaved.Ticks) / 10000000.0);
        _timeSaved = _timeNow;
    }

    private void WrapCoordinates(SpaceObject spaceObject)
    {
        if (spaceObject.Position.X > worldDimentions.X / 2) spaceObject.Position -= new Vector2(worldDimentions.X, 0);
        if (spaceObject.Position.X < -worldDimentions.X / 2) spaceObject.Position += new Vector2(worldDimentions.X, 0);
        if (spaceObject.Position.Y > worldDimentions.Y / 2) spaceObject.Position -= new Vector2(0, worldDimentions.Y);
        if (spaceObject.Position.Y < -worldDimentions.Y / 2) spaceObject.Position += new Vector2(0, worldDimentions.Y);
    }

    private void CheckPlayerAlive()
    {
        SpaceObject player = GetPlayer();
        if (player == null)
            onPlayerKilledCallback?.Invoke();
    }

    private void GameOver()
    {
        _game = false;
    }
}
