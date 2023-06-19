using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _shotPower = 10f;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PlayerModel _playerModel;
    [SerializeField] private GameObject[] _enemies;

    private Transform _startPosition;
    private int _ricochet = 0;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Shot()
    {
        Vector3 vector3 = Vector3.forward * _shotPower;
        vector3 = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0f) * vector3;
        _rb.velocity = vector3;
    }

    public void SetData(Transform startTransform, PlayerModel playerModel)
    {
        _startPosition = startTransform;
        _playerModel = playerModel;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bonus(collision.gameObject);

        if (_ricochet == 0 && collision.gameObject.GetComponent<Enemy>())
        {
            DestroyEnemy(collision.gameObject);
            StartCoroutine(UpdateList());
        }
        else
        {
            if(collision.gameObject.GetComponent<Enemy>())
            {
                DestroyEnemy(collision.gameObject);
            }
            StartState();
        }
    }

    private void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
        _playerModel.KillEnemy();
    }

    private void Bonus(GameObject enemy)
    {
        if (enemy.GetComponent<RedEnemy>() && _ricochet == 0)
        {
            _playerModel.SetPower(15);
        }

        if (enemy.GetComponent<BlueEnemy>() && _ricochet == 0)
        {
            _playerModel.SetPower(50);
        }

        if(_ricochet == 1)
        {
            int random = Random.Range(0, 2);

            if(random == 0) { _playerModel.SetPower(35);  } else { _playerModel.SetHealth(20); }
        }
    }

    private IEnumerator UpdateList()
    {
        yield return new WaitForSeconds(0.1f);
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Ricochet();
    }

    private void Ricochet()
    {
        int health = _playerModel.GetHealth();
        int chance = 100;
        int random = Random.Range(0, 100) + 1;
        if (random <= chance)
        {
            Vector3 vector3 = FindNearestEnemy();
            if (vector3 != Vector3.zero)
            {
                vector3 = (vector3 - transform.position).normalized * _shotPower;
                _rb.velocity = vector3;
                _ricochet = 1;
            }
            else
            {
                StartState();
            }
        }

    }

    private void StartState()
    {
        _ricochet = 0;
        _rb.velocity = Vector3.zero;
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        transform.position = _startPosition.position;
        this.gameObject.SetActive(false);
    }

    private Vector3 FindNearestEnemy()
    {
        if (_enemies.Length == 0)
        {
            return Vector3.zero;
        }

        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in _enemies)
        {
            float distance = Vector3.Distance(currentPosition, enemy.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            return nearestEnemy.transform.position;
        }
        return Vector3.zero;
    }
}
