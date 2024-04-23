using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnThunderDrone : MonoBehaviour
{
    public GameObject[] enemy;
    public int len;
    public int enemySpawnnum;
    private bool collision = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !collision)
        {
            Camera.main.GetComponent<EnemySpawner>().ThunderSpawnEnemy(enemy[enemySpawnnum],len);
        }
    }
}
