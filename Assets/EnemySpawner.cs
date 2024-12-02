using QFSW.MOP2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public List<Transform> spawnPoints;
    public ObjectPool enemyPool;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void StartGame()
    {
        InvokeRepeating("SpawnEnemies", 1, 2f);
    }
    public void cancelSpawning()
    {
        CancelInvoke("SpawnEnemies");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnEnemies()
    {
        enemyPool.GetObject().transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
    }
}
