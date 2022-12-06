using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public AudioSorcePlay audioSorcePlay;

    public GameObject[] itemObjs; // 드랍 아이템 리스트

    public bool bulletTime; // 불렛 생성 지속시간

    public float speed;
    public int health;
    public Sprite[] sprites;
    public float maxShotDelay;
    public float curShotDelay;
    public GameObject player;
    public int CollisionDmg;

    public GameObject[] audioSources;

    public bool Boss;

    public bool OnAoE; // 플레이어 스킬 중 장판에 들어왔는가 체크
    public float maxOnAoEDelay;//몬스터가 장판 안에 들어왔을 때 피격 딜레이
    public float curOnAoEDelay;

    public GameManager gameManager;

    public GameObject Bullet1;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;

    public GameObject enemyTag;
    public ObjectManager objectManager;

    public string EnemyName;

    // 보스 공격 패턴 위한 변수
    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    int num1 = 0, num2 = 4;
    //

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        objectManager = GameObject.FindWithTag("ObjectManager").GetComponent<ObjectManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.down * speed;
        audioSorcePlay = GameObject.FindWithTag("Sound").GetComponent<AudioSorcePlay>();
        
    }
    void Update()
    {
        
        if(GameObject.FindWithTag("GameManager").GetComponent<GameManager>().PlayerAlive == true 
            && gameObject.GetComponent<Enemy>().EnemyName.Equals("3")&& gameObject.activeSelf)
        {
            Debug.Log("몹 불렛 발사");
            Fire();
            Reload();
        }
        
        Direction();
        if (gameObject.tag.Equals("EnemyBoss")&&gameManager.BossSpawn&&!Boss)
        {
            Debug.Log("보스 패턴");
            Boss = true;
            Invoke("think", 2);
        }
    }

    //걸어다니는 애니메이션의 방향
    void Direction()
    {
        if(player.transform.position.x < this.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        if (player.transform.position.x > this.transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }
    
    void bulletDel()
    {
        gameObject.SetActive(false);
        Debug.Log("적 불렛 삭제");
    }

    void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;

        GameObject _Bullet = objectManager.MakeObj("bulletEnemyA");
        _Bullet.transform.position = transform.position;
        Rigidbody2D rigid = _Bullet.GetComponent<Rigidbody2D>();
        _Bullet.transform.rotation = Quaternion.identity;

        Vector3 dirVec = player.transform.position - transform.position;// 목표물 방향 = 목표물 위치 - 자신의 위치
        rigid.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);
        
        curShotDelay = 0;
    }
    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
    
    public void OnHit(int dmg)
    {
        health -= dmg;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        if (health <= 0)
        {
            Debug.Log("몹죽음");
            if (gameObject.tag == ("EnemyBoss"))
            {
                Debug.Log("보스몹죽음");
                SceneManager.LoadScene("GameClear");
            }
            gameObject.SetActive(false);
            DropItem();
        }
        Invoke("ReturnSprite", 0.1f);
        
    }
    
    void OnDead()
    {
        
    }

    void ReturnSprite()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
            gameObject.SetActive(false);
        else if(collision.gameObject.tag == "PlayerBullet")
        {
            Debug.Log("플레이어 불렛A와 충돌");
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
        }
        else if (collision.gameObject.tag == "PlayerBulletB")
        {
            Debug.Log("플레이어 불렛B와 충돌");
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
        }
        else if (collision.gameObject.tag == "PlayerBulletC")
        {
            OnAoE = true; // 몹이 장판안으로 들어옴
        }
        if (collision.gameObject.tag == "PlayerBulletD")
        {
            Debug.Log("플레이어 불렛D와 충돌");
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
            audioSorcePlay.AudioPlay(2);
        }
        else if (collision.gameObject.tag == "PlayerBulletE")
        {
            Debug.Log("플레이어 불렛E와 충돌");
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerBulletC")
        {
            ElapseTime();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBulletC")
            OnAoE = false;
    }


    void ElapseTime()
    {
        curOnAoEDelay += Time.deltaTime;

        if (curOnAoEDelay > maxOnAoEDelay && OnAoE == true && gameObject.activeSelf)
        {
            OnHit(1);
            audioSorcePlay.AudioPlay(5);
            maxOnAoEDelay = 1f;
            curOnAoEDelay = 0f;
        }
        else if (curOnAoEDelay > maxOnAoEDelay && OnAoE == false && gameObject.activeSelf)
        {
            maxOnAoEDelay = 1f;
            curOnAoEDelay = 1f;

        }
    }
    void DropItem()
    {
        int ranItem = Random.Range(0, 100);
        if (ranItem % 2 == 0)
        {
            //COIN
            GameObject coinitem = Instantiate(itemObjs[0],
            this.transform.position,
            this.transform.rotation);// 랜덤으로 정해진 적 프리펩, 생성 위치로 생성 로직 작성

            Rigidbody2D rigid = coinitem.GetComponent<Rigidbody2D>();
        }
        if (ranItem % 2 == 1)
        {
            GameObject Emeralditem = Instantiate(itemObjs[1],
            this.transform.position,
            this.transform.rotation);

            Rigidbody2D rigid = Emeralditem.GetComponent<Rigidbody2D>();
        }

    }
    

    // 보스 패턴
    void think()
    {
        patternIndex = Random.Range(0, 4);

        switch (patternIndex)
        {
            case 0:
                FireWard();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireAroundVer1();
                break;
            case 3:
                FireAroundVer2();
                break;
        }
    }

    void FireWard()
    {
        Debug.Log("6방향으로 적불렛 발사");
        GameObject[] Bullet = new GameObject[7];
        for (int i = 0; i < 7; i++)
        {
            Bullet[i] = objectManager.MakeObj("bulletEnemyA");
            Bullet[i].transform.position = transform.position;
            transform.rotation = Quaternion.identity;
            Rigidbody2D rigid = Bullet[i].GetComponent<Rigidbody2D>();
            Bullet[i].SetActive(false);
            switch (i)
            {
                case 1:
                    Bullet[i].transform.rotation = Quaternion.Euler(0, 0, 90);
                    Bullet[i].SetActive(true);
                    rigid.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
                    break;
                case 2:
                    Bullet[i].transform.rotation = Quaternion.Euler(0, 0, -65);
                    Bullet[i].SetActive(true);
                    rigid.AddForce(new Vector2(10, 3), ForceMode2D.Impulse);
                    // 오른쪽 대각선 위
                    break;
                case 3:
                    Bullet[i].SetActive(true);
                    Bullet[i].transform.rotation = Quaternion.Euler(0, 0, -115);
                    rigid.AddForce(new Vector2(10, -3), ForceMode2D.Impulse);
                    //오른쪽 대각선 아래
                    break;
                case 4:
                    Bullet[i].SetActive(true);
                    Bullet[i].transform.rotation = Quaternion.Euler(0, 0, -90);
                    rigid.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                    break;
                case 5:
                    Bullet[i].transform.rotation = Quaternion.Euler(0, 0, 115);
                    Bullet[i].SetActive(true);
                    rigid.AddForce(new Vector2(-10, -3), ForceMode2D.Impulse);
                    //왼쪽 대각선 아래
                    break;
                case 6:
                    Bullet[i].transform.rotation = Quaternion.Euler(0, 0, 65);
                    Bullet[i].SetActive(true);
                    rigid.AddForce(new Vector2(-10, 3), ForceMode2D.Impulse);
                    //왼쪽 대각선 위
                    break;
                case 7:
                    Bullet[i].SetActive(false);
                    break;
            }
        }
        Invoke("think", 2);
    }

    void FireShot()
    {
        Debug.Log("플레이어 방향으로 샷건");

        for(int index = 0; index < 5; index++)
        {
            GameObject bullet = objectManager.MakeObj("bulletEnemyA");
            bullet.transform.position = transform.position;
            transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = player.transform.position - transform.position;// 플레이어 방향으로
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        }


        Invoke("think", 2);
    }

    void FireAroundVer1()
    {
        Debug.Log("원 형태 발사");

        int roundA = 50;

        for(int index = 0; index < roundA; index++)
        {
            GameObject bullet = objectManager.MakeObj("bulletEnemyA");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundA),
                Mathf.Sin(Mathf.PI * 2 * index / roundA));
            rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        }
        

        Invoke("think", 2);
    }


    void FireAroundVer2()
    {
        Debug.Log("원 형태 연속 발사");

        int roundA = 50;
        int roundB = 40;
        int roundNum = num1 % 2 == 0 ? roundA : roundB;

        for (int index = 0; index < roundNum; index++)
        {
            GameObject bullet = objectManager.MakeObj("bulletEnemyA");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundNum),
                Mathf.Sin(Mathf.PI * 2 * index / roundNum));
            rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * index / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }

        num1++;

        if (num1 < num2)
            Invoke("FireAroundVer2", 0.7f);
        else
            Invoke("think", 3);
    }
}
