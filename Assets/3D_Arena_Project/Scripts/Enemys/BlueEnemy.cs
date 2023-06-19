using System.Collections;
using UnityEngine;

public class BlueEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject _blueBullet;

    [SerializeField] private Transform _spawnZoneTransform;
    [SerializeField] private Transform _playerTransform;
    private void Start()
    {
        StartCoroutine(CreateBullet());
    }

    private void Update()
    {
        if (_playerTransform != null)
        {
            Vector3 direction = _playerTransform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        
        _playerTransform = playerTransform;
    }

    private IEnumerator CreateBullet()
    {
        while (true)
        {
            GameObject bullet = Instantiate(_blueBullet, _spawnZoneTransform);
            bullet.GetComponent<BlueBullet>().SetPlayerTransform(_playerTransform);
            yield return new WaitForSeconds(1.3f);
        }
    }
}
