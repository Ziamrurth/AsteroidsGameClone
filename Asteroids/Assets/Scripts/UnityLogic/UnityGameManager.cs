using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnityGameManager : MonoBehaviour {
    [SerializeField] public UnitySpaceObject unityObject;

    public Player player;

    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private PlayerInput _playerInput;

    void Start()
    {
        _spawnManager = SpawnManager.GetInstance();
        _spawnManager.onObjectCreatedCallback += CreateObject;

        _gameManager = GameManager.GetInstance();
        _gameManager.Start();

        _playerInput = PlayerInput.GetInstance();
        _playerInput.Acceleration = false;
        _playerInput.Rotation = Rotation.None;
    }

    void Update()
    {
        _gameManager.Update();

        float rotationInput = Input.GetAxis("Horizontal");
        bool accelerationInput = Input.GetKey(KeyCode.W);
        bool shootInput = Input.GetKey(KeyCode.Space);
        bool laserInput = Input.GetKeyDown(KeyCode.LeftControl);

        _playerInput.Shoot = shootInput;
        _playerInput.LaserAttack = laserInput;
        _playerInput.Acceleration = accelerationInput;
        if (rotationInput < 0)
            _playerInput.Rotation = Rotation.Left;
        if (rotationInput == 0)
            _playerInput.Rotation = Rotation.None;
        if (rotationInput > 0)
            _playerInput.Rotation = Rotation.Right;
        
    }

    public void CreateObject(SpaceObject spaceObject)
    {
        Vector3 position = new Vector3(spaceObject.Position.X, spaceObject.Position.Y, 0);
        Quaternion rotation = Quaternion.Euler(0, 0, spaceObject.Rotation);
        UnitySpaceObject unitySpaceObject = Instantiate(unityObject, position, rotation);
        unitySpaceObject.SetSpaceObject(spaceObject);

        if(spaceObject as Player != null)
        {
            (spaceObject as Player).onLaserAttackCallback += DrawLaserBeam;
            player = spaceObject as Player;
        }
    }
    
    private void DrawLaserBeam(LaserAttack laser)
    {
        DrawLine(new Vector3(laser.StartPos.X, laser.StartPos.Y), new Vector3(laser.EndPos.X, laser.EndPos.Y), Color.white, 0.1f);
    }

    private void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Standard"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}
