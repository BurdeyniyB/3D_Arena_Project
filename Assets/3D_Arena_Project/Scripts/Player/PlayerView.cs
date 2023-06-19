using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private FactoryEnemy _factoryEnemy;
    [SerializeField] private Collider _boundary;
    [SerializeField] private Text _healthText, _powerText, _killedEnemyText;
    [SerializeField] private Image _healthBar, _powerBar;

    private Rigidbody _rb;
    [SerializeField] private List<Transform> _enemyTransform;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void ChangeBars(int health, int power)
    {
        _healthText.text = health.ToString();
        _powerText.text = power.ToString();

        _healthBar.fillAmount = (float)health / 100;
        _powerBar.fillAmount = (float)power / 100;
    }

    public void ChangeTextKilledEnemy(int killedEnemy)
    {
        _killedEnemyText.text = "Kills: " + killedEnemy.ToString();
    }

    private void OnEnable()
    {
        _factoryEnemy.EnemysTransform += GetEnemyList;
    }

    private void OnDisable()
    {
        _factoryEnemy.EnemysTransform -= GetEnemyList;
    }

    private void GetEnemyList(List<Transform> enemyTransform)
    {
        _enemyTransform = enemyTransform;
    }

    public void ChangePosition(Vector3 direction)
    {
        _rb.velocity = direction;
        CrossingBoundary();
    }

    public void ChangeRotation(Quaternion rotation)
    {
        Quaternion newRotation = _rb.rotation * rotation;

        Vector3 eulerRotation = newRotation.eulerAngles;
        eulerRotation.x = ClampAngle(eulerRotation.x, -20f, 20f);
        eulerRotation.z = 0f;

        newRotation = Quaternion.Euler(eulerRotation);

        _rb.MoveRotation(newRotation);
        CrossingBoundary();
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < 0f)
        {
            angle += 360f;
        }

        if (angle > 180f)
        {
            angle -= 360f;
        }

        return Mathf.Clamp(angle, min, max);
    }

    private void CrossingBoundary()
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

            _rb.MovePosition(farthestPoint);
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
            Debug.Log("ConsequenceOfDisplacement");
            character.GetComponent<BlueBullet>().SetPlayerTransform(transform.position, _boundary);
        }
    }
}