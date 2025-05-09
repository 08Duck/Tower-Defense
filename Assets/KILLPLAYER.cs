using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class KILLPLAYER : MonoBehaviour
{
    private Health dmgEnemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health dmgEnemy = collision.GetComponent<Health>();
            if (dmgEnemy != null)
            {
                int CritOrNAH = Random.Range(1, 10);
                if(CritOrNAH == 1)
                {
                    dmgEnemy.TakeDamage(50);
                }
                else
                {
                    dmgEnemy.TakeDamage(25);
                }
                ;
            }
            else
            {
                Debug.LogWarning("Health-komponent saknas på spelaren!");
            }
        }
    }
}
