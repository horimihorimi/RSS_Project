using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public string[] enemyObjs;
    //public GameObject[] enemyObjs;

    public float maxSpawnDelay;
    public float curSpawnDelay;
    public GameObject player;
    public bool PlayerAlive;

    public ObjectManager objectManager;

    public Transform sprite; // sprite(카메라위치 + 랜덤 값  = 스폰

    public Timer timer;
    public bool BossSpawn;

    public Text[] LevelText;

    // 레벨업 이벤트에서 사용되는 변수들
    public Sprite[] image;
    public int[] SkillLevel; // 스킬레벨
    public string[] SkillList; // 게임시스템에 있는 스킬리스트

    public Button[] button;
    public Text[] text;
    public Button LVUI;

    public Image[] imageSelect; // 출력되는 3개 스킬 이미지
    public int[] SelectSkillNum;// 선택지에 저장된 스킬 번호
    public bool[] skillFlag; // UI에 동일한 스킬이 출력되는 것을 방지

    public Image[] SkillUI;
    public string[] SkillString; // 레벨업을 통해 얻은 스킬들
    public int RanNum;
    //


    void Awake()
    {
        enemyObjs = new string[] { "enemy1", "enemy2", "enemy3", "enemy4", "EnemyBoss" };
    }

    void Start()
    {
        PlayerAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerAlive)
        {
            curSpawnDelay += Time.deltaTime;

            if (curSpawnDelay > maxSpawnDelay)
            {
                SpawnEnemy();
                Debug.Log("몹소환");
                maxSpawnDelay = Random.Range(0.1f, 1.2f);
                curSpawnDelay = 0;
            }
        }
    }

    void SpawnEnemy()
    {
        if (PlayerAlive)
        {
            GameObject enemy;
            if (timer._Min >= 5 && timer._Sec >= 0&&!BossSpawn)
            {
                Debug.Log("보스생성");
                Vector3 spritepos = sprite.localPosition;
                spritepos.x = spritepos.x + Random.Range((16 * (Camera.main.orthographicSize) / 9) * (-1), 16 * (Camera.main.orthographicSize) / 9);
                spritepos.y = spritepos.y + Random.Range((Camera.main.orthographicSize) * (-1), Camera.main.orthographicSize);
                BossSpawn = true;
                enemy = objectManager.MakeObj(enemyObjs[4]);
                enemy.transform.position = spritepos;
                enemy.GetComponent<Enemy>().health = 1000;
                //GameObject enemy = Instantiate(enemyObjs[ranEnemy],
                //    spritepos,
                //    Quaternion.identity);// 랜덤으로 정해진 적 프리펩, 생성 위치로 생성 로직 작성


            }
            else
            {
                Vector3 spritepos = sprite.localPosition;
                spritepos.x = spritepos.x + Random.Range((16 * (Camera.main.orthographicSize) / 9) * (-1), 16 * (Camera.main.orthographicSize) / 9);
                spritepos.y = spritepos.y + Random.Range((Camera.main.orthographicSize) * (-1), Camera.main.orthographicSize);
                int ranEnemy = Random.Range(0, 4);
                //int ranPoint = Random.Range(0, 4);
                enemy = objectManager.MakeObj(enemyObjs[ranEnemy]);
                enemy.transform.position = spritepos;
                switch (ranEnemy)
                {
                    case 1:
                        enemy.GetComponent<Enemy>().health = 10;
                        break;
                    case 2:
                        enemy.GetComponent<Enemy>().health = 100;
                        break;
                    case 3:
                        enemy.GetComponent<Enemy>().health = 30;
                        break;
                    case 0:
                        enemy.GetComponent<Enemy>().health = 10;
                        break;
                }

                //GameObject enemy = Instantiate(enemyObjs[ranEnemy],
                //    spritepos,
                //    Quaternion.identity);// 랜덤으로 정해진 적 프리펩, 생성 위치로 생성 로직 작성


            }
            Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
            Enemy enemylogic = enemy.GetComponent<Enemy>();
            enemylogic.player = player;





        }
    }

    
}
