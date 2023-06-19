using System;
using UnityEngine;


public class ControlCollision : MonoBehaviour
{
    public Action<string, GameObject> TakeDamage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<RedEnemy>()) { TakeDamage.Invoke("Red Enemy", collision.gameObject); }
        if (collision.gameObject.GetComponent<BlueBullet>()) { TakeDamage.Invoke("Blue Bullet", collision.gameObject); }
    }
}
