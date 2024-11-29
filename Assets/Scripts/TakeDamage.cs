using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int attackDamage = 30;

        private void OnTriggerEnter(Collider collision)
        {
            // Обрабатываем столкновение с врагом
            if (collision.CompareTag("Enemy"))
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                }
            }
        }
    }