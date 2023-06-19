using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FactoryEnemy : MonoBehaviour
{
    public event Action<List<Transform>> EnemysTransform;

    [SerializeField] private Enemy[] _enemyData;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _deltaTimeBetweenSpans;

    private List<Transform> _enemyTransform = new List<Transform>();
    private float _timeSpawn = 5f, _timeSpawnMin = 2f;
    private int _countCreatedObject;


    private void Start()
    {
        StartCoroutine(CreateEnemy());
    }

    private IEnumerator CreateEnemy()
    {
        while (true)
        {
            GameObject createdPrefab = GetEnemyObject();
            Vector3 newPostion = new Vector3(GetRandomPosition(transform.position.x), createdPrefab.transform.localScale.y + 0.01f, GetRandomPosition(transform.position.z));
            GameObject enemy = Instantiate(createdPrefab, newPostion, Quaternion.identity, transform);
            enemy.GetComponent<Enemy>().SetPlayerTransform(_playerTransform);
            ChangeList(enemy);
            yield return new WaitForSeconds(_timeSpawn);
            CalculateTime();
        }
    }

    private GameObject GetEnemyObject()
    {
        _countCreatedObject++;
        GameObject enemyObject = _enemyData[0].GetEnemyPrefab();

        for (int i = 0; i < _enemyData.Length; i++)
        {
            if (_countCreatedObject % _enemyData[i].GetRatioToUnity() == 0)
            {
                enemyObject = _enemyData[i].GetEnemyPrefab();
            }
        }
        return enemyObject;
    }


    private float GetRandomPosition(float pos)
    {
        float newPosition = Random.Range(pos - 1.5f, pos + 1.5f);
        return newPosition;
    }

    private void CalculateTime()
    {
        if (_timeSpawn - _deltaTimeBetweenSpans > _timeSpawnMin) { _timeSpawnMin -= _deltaTimeBetweenSpans; }
        else { _timeSpawn = _timeSpawnMin; }
    }

    private void ChangeList(GameObject enemy)
    {
        _enemyTransform.RemoveAll(item => item == null);

        if (enemy)
        {
            _enemyTransform.Add(enemy.transform);
            EnemysTransform.Invoke(_enemyTransform);
        }
    }
}