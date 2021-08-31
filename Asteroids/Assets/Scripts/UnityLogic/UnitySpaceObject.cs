using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitySpaceObject : MonoBehaviour {
    [SerializeField] public Sprite playerSprite;
    [SerializeField] public Sprite asteroidSprite;
    [SerializeField] public Sprite ufoSprite;
    [SerializeField] public Sprite bulletSprite;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    private SpaceObject _spaceObject;

    void Start()
    {
        
    }

    void Update()
    {
        CheckExistence();
        UpdateTransform();
    }

    public void SetSpaceObject(SpaceObject spaceObject)
    {
        _spaceObject = spaceObject;
        if (_spaceObject as Player != null)
        {
            _spriteRenderer.sprite = playerSprite;
        } 
        if (_spaceObject as Asteroid != null)
        {
            _spriteRenderer.sprite = asteroidSprite;
        }
        if(_spaceObject as Ufo != null)
        {
            _spriteRenderer.sprite = ufoSprite;
        }
        if(_spaceObject as Bullet != null)
        {
            _spriteRenderer.sprite = bulletSprite;
        }

        if(_spaceObject as Enemy != null)
        {
            transform.localScale = Vector3.one * (_spaceObject as Enemy).Size;
        }
        else
        {
            transform.localScale = Vector3.one * 0.1f;
        }
    }

    private void UpdateTransform()
    {
        transform.position = new Vector3(_spaceObject.Position.X, _spaceObject.Position.Y, 0);
        transform.rotation = Quaternion.Euler(0, 0, _spaceObject.Rotation);
    }

    private void CheckExistence()
    {
        if (_spaceObject.Destroyed)
        {
            Destroy(gameObject);
        }
    }
}
