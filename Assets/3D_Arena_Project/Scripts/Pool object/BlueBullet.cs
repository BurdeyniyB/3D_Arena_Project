using UnityEngine;

public class BlueBullet : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] private Transform _playerTransform;
    private Vector3 _afterPlayerDisplacement;
    private Collider _boundary;
    private bool _playerDisplayment;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(_playerDisplayment)
        {
            ChangeVelocity(_afterPlayerDisplacement);
            CrossingBoundary();
        }
        else if (_playerTransform)
        {
            ChangeVelocity(_playerTransform.position);
        }
    }

    private void ChangeVelocity(Vector3 playerPosition)
    {
        Vector3 playerDirection = (playerPosition - transform.position).normalized;
        _rb.velocity = playerDirection * _speed;
    }


    public void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    public void SetPlayerTransform(Vector3 playerPosition, Collider boundary)
    {
        _afterPlayerDisplacement = playerPosition;
        _playerDisplayment = true;
        _boundary = boundary;
    }

    private void CrossingBoundary()
    {
        if (_boundary)
        {
            Vector3 clampedPosition = _boundary.ClosestPoint(transform.position);
            clampedPosition.y = transform.position.y;
            if (transform.position != clampedPosition)
            {
                Destroy(gameObject);
            }
        }
    }
}
