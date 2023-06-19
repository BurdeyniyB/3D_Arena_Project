using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public Action EndGame;

    [SerializeField] private PlayerView _playerView;

    [SerializeField] private FactoryEnemy _factoryEnemy;
    [SerializeField] private PoolExample _bullet;
    [SerializeField] private Ulta _ulta;

    private MoveToRandomPositionOnPlatform _moveToRandomPositionOnPlatform;
    private AngleCorrection _angleCorrection = new AngleCorrection();

    [SerializeField] private Rigidbody _playerRigidBody;
    [SerializeField] private Collider _boundary;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotateHorizontalSpeed = 5f;
    [SerializeField] private float _rotateVerticalSpeed = 5f;

    private List<Transform> _enemyTransform;

    private int _health = 100;
    private int _power = 50;
    private int killedEnemy;

    private Vector3 _direction;

    private float _rotationHorizontal;
    private float _rotationVertical = 0f;

    private void OnEnable()
    {
        _ulta.UsedUlta += SetPower;
        _factoryEnemy.EnemysTransform += GetEnemyList;
    }

    private void OnDisable()
    {
        _ulta.UsedUlta -= SetPower;
        _factoryEnemy.EnemysTransform -= GetEnemyList;
    }

    private void GetEnemyList(List<Transform> enemyTransform)
    {
        _enemyTransform = enemyTransform;
    }

    public void NewVelocityVector(float horizontal, float vertical)
    {
        _direction = new Vector3(horizontal, 0, vertical);
        _direction = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * _direction;
        _direction = _direction.normalized * _moveSpeed;

        _playerView.ChangePosition(_direction);
    }

    public void NewQuaternion(float horizontal, float vertical)
    {
        _rotationHorizontal = horizontal;
        _rotationVertical -= vertical;

        _rotationHorizontal = _rotationHorizontal * _rotateHorizontalSpeed * Time.deltaTime;
        _rotationVertical = _rotationVertical * _rotateVerticalSpeed * Time.deltaTime;

        Quaternion rotation = Quaternion.Euler(_rotationVertical, _rotationHorizontal, 0f);

        rotation = _angleCorrection.Correction(rotation, _playerRigidBody);

        _playerView.ChangeRotation(rotation);
    }

    public void Shot()
    {
        _bullet.CreateBullet();
    }

    public void Ulta()
    {
        if (_power == 100) { _ulta.DestroyAllEnemy(); }
    }

    public void KillEnemy()
    {
        killedEnemy++;
        _playerView.ChangeTextKilledEnemy(killedEnemy);
    }

    public int GetHealth() => _health;

    public int GetPower() => _power;

    public void SetHealth(int health)
    {
        if (_health + health <= 100) { _health += health; } else { _health = 100; }
        _playerView.ChangeBars(_health, _power);
    }

    public void SetPower(int power)
    {
        if (_power + power <= 100) { _power += power; } else { _power = 100; }
        _playerView.ChangeBars(_health, _power);
    }

    public void TakeDamage(string typeDamage)
    {
        if (typeDamage == "Red Enemy") { _health -= 15; if (_health <= 0) { _health = 0; EndGame.Invoke(); } }
        if (typeDamage == "Blue Bullet") { _power -= 25; if (_power <= 0) { _power = 0; } }

        _playerView.ChangeBars(_health, _power);
    }

    public void CrossingBoundary()
    {
        if (_boundary)
        {
            Vector3 clampedPosition = _boundary.ClosestPoint(transform.position);
            clampedPosition.y = transform.position.y;
            if (transform.position != clampedPosition)
            {
                Debug.Log("ConsequenceOfDisplacement0");
                ConsequenceOfDisplacement();
                _moveToRandomPositionOnPlatform = new MoveToRandomPositionOnPlatform(_boundary, _playerRigidBody, transform, _enemyTransform);
                _moveToRandomPositionOnPlatform.Move();
            }
        }
    }



    private void ConsequenceOfDisplacement()
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Blue bullet");

        foreach (GameObject character in characters)
        {
            character.GetComponent<BlueBullet>().SetPlayerTransform(transform.position, _boundary);
        }
    }
}
