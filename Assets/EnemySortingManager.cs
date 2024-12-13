using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySortingManager : MonoBehaviour
{
    public Transform cam;
    public string[] sortingLayers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        enemies = enemies.OrderBy(enemy => Vector3.Distance(cam.position, enemy.transform.position)).Reverse().ToArray();
        for (int i = 0; i < enemies.Length && i < sortingLayers.Length; i++)
        {
            enemies[i].Setsortinglayer(sortingLayers[i]);
        }
    }
}
