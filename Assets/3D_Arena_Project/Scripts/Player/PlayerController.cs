using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerModel _playerModel;

    [SerializeField] private FloatingJoystick _joystickMove;
    [SerializeField] private FloatingJoystick _joystickAngular;


    private void FixedUpdate()
    {
        SendJoystickPosition();
    }

    private void SendJoystickPosition()
    {
        _playerModel.NewVelocityVector(_joystickMove.Horizontal, _joystickMove.Vertical);

        _playerModel.NewQuaternion(_joystickAngular.Horizontal, _joystickAngular.Vertical);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<RedEnemy>()) { _playerModel.TakeDamage("Red Enemy"); Destroy(collision.gameObject); }
        if (collision.gameObject.GetComponent<BlueBullet>()) { _playerModel.TakeDamage("Blue Bullet"); Destroy(collision.gameObject); }
    }
}
