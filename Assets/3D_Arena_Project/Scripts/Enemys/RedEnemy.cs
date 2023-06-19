using System.Collections;
using UnityEngine;

public class RedEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] float _deltaUp, _speed;
    [SerializeField] private Transform _playerTransform;
    private Vector3 _upPosition;
    private bool _isUp;
    private bool _isAttack;
    private Rigidbody _rb;

    private void Start()
    {
        _upPosition = new Vector3(transform.position.x, transform.position.y + _deltaUp, transform.position.z);
        _isUp = true;
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (transform.position != _upPosition && _isUp)
        {
            Vector3 upDirection = (_upPosition - transform.position).normalized;
            _rb.velocity = upDirection * _speed;
            if (Vector3.Distance(transform.position, _upPosition) < 0.1f)
            {
                _isUp = false;
                _rb.velocity = Vector3.zero;
                StartCoroutine(ChangeDirection());
            }
        }

        if (_isAttack && _playerTransform)
        {
            Vector3 playerDirection = (_playerTransform.position - transform.position).normalized;
            _rb.velocity = playerDirection * _speed;
        }
    }

    private IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(1f);
        _isAttack = true;
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }
}
