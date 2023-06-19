using System;
using UnityEngine;

public class Ulta : MonoBehaviour
{
    public Action<int> UsedUlta;

    public void DestroyAllEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        UsedUlta.Invoke(-100);
    }
}
