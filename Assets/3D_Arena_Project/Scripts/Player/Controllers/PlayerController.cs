using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerModel _playerModel;
    [SerializeField] private PlayerView _playerView;

    [SerializeField] private ControlCollision _controlCollision;

    [SerializeField] private FloatingJoystick _joystickMove;
    [SerializeField] private FloatingJoystick _joystickAngular;

    private void OnEnable()
    {
        _controlCollision.TakeDamage += CollisionEnemy;
    }

    private void OnDisable()
    {
        _controlCollision.TakeDamage -= CollisionEnemy;
    }

    private void FixedUpdate()
    {
        SendJoystickPosition();
    }

    private void SendJoystickPosition()
    {
        _playerModel.NewVelocityVector(_joystickMove.Horizontal, _joystickMove.Vertical);

        _playerModel.NewQuaternion(_joystickAngular.Horizontal, _joystickAngular.Vertical);
    }

    private void CollisionEnemy(string typeDamage, GameObject enemy)
    {
        _playerModel.TakeDamage(typeDamage);
        _playerView.DestroyEnemy(enemy);   
    }
}
