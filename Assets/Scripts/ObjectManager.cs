using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    public GameObject enemy3Prefab;
    public GameObject enemy4Prefab;
    public GameObject enemyBossPrefab;
    public GameObject itemCoinPrefab;
    public GameObject itemEmeraldPrefab;
    public GameObject bulletPlayerAPrefab;
    public GameObject bulletPlayerBPrefab;
    public GameObject bulletPlayerEPrefab;
    public GameObject bulletEnemyAPrefab;


    public GameObject[] enemy1;
    public GameObject[] enemy2;
    public GameObject[] enemy3;
    public GameObject[] enemy4;
    GameObject[] EnemyBoss;

    GameObject[] itemCoin;
    GameObject[] itemEmerald;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletPlayerE;
    GameObject[] bulletEnemyA;

    GameObject[] targetPool;
    
    // Start is called before the first frame update
    void Awake()
    {
        enemy1 = new GameObject[200];
        enemy2 = new GameObject[200];
        enemy3 = new GameObject[200];
        enemy4 = new GameObject[200];
        EnemyBoss = new GameObject[1];

        itemCoin = new GameObject[500];
        itemEmerald = new GameObject[500];

        bulletPlayerA = new GameObject[200];
        bulletPlayerB = new GameObject[200];
        bulletPlayerE = new GameObject[100];
        bulletEnemyA = new GameObject[1000];

        Generate();
    }
    
    void Generate()
    {
        //#1. enemy
        for(int index = 0; index < enemy1.Length; index++)
        {
            enemy1[index] = Instantiate(enemy1Prefab);
            enemy1[index].SetActive(false);
        }
        for (int index = 0; index < enemy2.Length; index++)
        {
            enemy2[index] = Instantiate(enemy2Prefab);
            enemy2[index].SetActive(false);
        }
        for (int index = 0; index < enemy3.Length; index++)
        {
            enemy3[index] = Instantiate(enemy3Prefab);
            enemy3[index].SetActive(false);
        }
        for (int index = 0; index < enemy4.Length; index++)
        {
            enemy4[index] = Instantiate(enemy4Prefab);
            enemy4[index].SetActive(false);
        }
        EnemyBoss[0] = Instantiate(enemyBossPrefab);
        EnemyBoss[0].SetActive(false);
        //#2. Item
        for (int index = 0; index < itemCoin.Length; index++)
        {
            itemCoin[index] = Instantiate(itemCoinPrefab);
            itemCoin[index].SetActive(false);
        }
        for (int index = 0; index < itemEmerald.Length; index++)
        {
            itemEmerald[index] = Instantiate(itemEmeraldPrefab);
            itemEmerald[index].SetActive(false);
        }

        //#3. Bullet
        for (int index = 0; index < bulletPlayerA.Length; index++)
        {
            bulletPlayerA[index] = Instantiate(bulletPlayerAPrefab);
            bulletPlayerA[index].SetActive(false);
        }
        for (int index = 0; index < bulletPlayerB.Length; index++)
        {
            bulletPlayerB[index] = Instantiate(bulletPlayerBPrefab);
            bulletPlayerB[index].SetActive(false);
        }
        for (int index = 0; index < bulletPlayerE.Length; index++)
        {
            bulletPlayerE[index] = Instantiate(bulletPlayerEPrefab);
            bulletPlayerE[index].SetActive(false);
        }
        for (int index = 0; index < bulletEnemyA.Length; index++)
        {
            bulletEnemyA[index] = Instantiate(bulletEnemyAPrefab);
            bulletEnemyA[index].SetActive(false);
        }
    }


    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "enemy1":
                targetPool = enemy1;
                break;
            case "enemy2":
                targetPool = enemy2;
                break;
            case "enemy3":
                targetPool = enemy3;
                break;
            case "enemy4":
                targetPool = enemy4;
                break;
            case "EnemyBoss":
                targetPool = EnemyBoss;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
            case "itemEmerald":
                targetPool = itemEmerald;
                break;
            case "bulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "bulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "bulletPlayerE":
                targetPool = bulletPlayerE;
                break;
            case "bulletEnemyA":
                targetPool = bulletEnemyA;
                break;
        }

        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }
        return null;
    }

            

    // Update is called once per frame
    void Update()
    {
        
    }
}
