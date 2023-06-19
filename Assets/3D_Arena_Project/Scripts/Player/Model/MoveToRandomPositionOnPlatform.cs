using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveToRandomPositionOnPlatform
    {

    private Collider _boundary;
    private Rigidbody _playerRigidBody;
    private Transform _playerTransform;
    private List<Transform> _enemyTransform;

    public MoveToRandomPositionOnPlatform(Collider boundary,
                                          Rigidbody playerRigidBody,
                                          Transform playerTransform,
                                          List<Transform> enemyTransform)
    {
        _boundary = boundary;
        _playerRigidBody = playerRigidBody;
        _playerTransform = playerTransform;
        _enemyTransform = enemyTransform;
    }


    public void Move()
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

            Vector3 randomPosition = new Vector3(randomX, _playerTransform.position.y, randomZ);

            randomPosition = ClampPositionWithinPlatform(randomPosition);

            return randomPosition;
        }

        return _playerTransform.position;
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
}

