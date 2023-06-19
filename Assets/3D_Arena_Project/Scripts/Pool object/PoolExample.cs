using UnityEngine;

public class PoolExample : MonoBehaviour
{
    [SerializeField] private int _poolCount = 3;
    [SerializeField] private bool _autoExpand = false;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private GameObject _player;

    private PoolMono<Bullet> _pool;

    private void Start()
    {
        PlayerModel playerModel = _player.GetComponent<PlayerModel>();
        _pool = new PoolMono<Bullet>(_bulletPrefab, _poolCount, transform, playerModel);
        _pool.PutoExpand = _autoExpand;
    }

    public void CreateBullet()
    {
        var bullet = _pool.GetFreeElement();
        bullet.transform.position = transform.position;
        bullet.Shot();
    }
}
