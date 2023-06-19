using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _ratioToUnity;
    private IEnemy _ienemy;

    public GameObject GetEnemyPrefab()
    {
        return _enemyPrefab;
    }

    public int GetRatioToUnity()
    {
        return _ratioToUnity;
    }

    public void SetPlayerTransform(Transform player)
    {
        _ienemy = GetComponent<IEnemy>();
        _ienemy.SetPlayerTransform(player);
    }
}
