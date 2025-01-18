using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEnemy : MonoBehaviour
{
    public Transform player; // —сылка на игрока

    void Update()
    {
        if (player != null)
        {
            // ѕолучаем позицию врага и игрока
            float enemyPosX = transform.position.x;
            float playerPosX = player.position.x;

            // ѕровер€ем, находитс€ ли игрок слева от врага
            if (playerPosX < enemyPosX)
            {
                // ѕоворачиваем врага на 180 градусов по оси Y
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                // ¬раг смотрит в стандартное положение (лицо к игроку)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}