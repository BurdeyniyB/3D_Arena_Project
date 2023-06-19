using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private PlayerModel _playerModel;

    [SerializeField] private Text _healthText, _powerText, _killedEnemyText;
    [SerializeField] private Image _healthBar, _powerBar;

    private Rigidbody _playerRigidBody;
    [SerializeField] private List<Transform> _enemyTransform;

    private void Start()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
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


    public void ChangePosition(Vector3 direction)
    {
        _playerRigidBody.velocity = direction;
        _playerModel.CrossingBoundary();
    }

    public void ChangeRotation(Quaternion rotation)
    {
        _playerRigidBody.MoveRotation(rotation);
        _playerModel.CrossingBoundary();
    }

    public void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }
}