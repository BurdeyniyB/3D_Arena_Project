using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerModel : MonoBehaviour
{
    public Action EndGame;

    [SerializeField] private PlayerView _playerView;

    [SerializeField] private FactoryEnemy _factoryEnemy;
    [SerializeField] private PoolExample _bullet;
    [SerializeField] private Ulta _ulta;
    private AngleCorrection _angleCorrection = new AngleCorrection();

    [SerializeField] private Rigidbody _playerRigidBody;
    [SerializeField] private Collider _boundary;

    [SerializeField] private List<Transform> _enemyTransform;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotateHorizontalSpeed = 5f;
    [SerializeField] private float _rotateVerticalSpeed = 5f;

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
                MoveToRandomPositionOnPlatform();
            }
        }
    }

    public void MoveToRandomPositionOnPlatform()
    {
        if (_boundary != null && _enemyTransform.Count > 0)
        {
            Vector3 randomPosition = GetRandomPositionOnPlatform();
            Vector3 farthestPoint = FindFarthestPointFromEnemies(randomPosition);

            _playerRigidBody.MovePosition(farthestPoint);
        }
    }

    private Vector3 GetRandomPositionOnPlatform()
    {
        if (_boundary != null)
        {
            Bounds platformBounds = _boundary.bounds;

            float randomX = Random.Range(platformBounds.min.x, platformBounds.max.x);
            float randomZ = Random.Range(platformBounds.min.z, platformBounds.max.z);

            Vector3 randomPosition = new Vector3(randomX, transform.position.y, randomZ);

            randomPosition = ClampPositionWithinPlatform(randomPosition);

            return randomPosition;
        }

        return transform.position;
    }

    private Vector3 ClampPositionWithinPlatform(Vector3 position)
    {
        if (_boundary != null)
        {
            Vector3 closestPoint = _boundary.ClosestPoint(position);
            return closestPoint;
        }

        return position;
    }

    private Vector3 FindFarthestPointFromEnemies(Vector3 position)
    {
        Vector3 farthestPoint = position;
        float maxDistance = 0f;

        foreach (Transform enemyTransform in _enemyTransform)
        {
            if (enemyTransform != null)
            {
                float distance = Vector3.Distance(position, enemyTransform.position);

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthestPoint = position;
                }
            }
        }

        return farthestPoint;
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
