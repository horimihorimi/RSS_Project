using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMove : MonoBehaviour
{
    public GameObject gameOver;
    public AudioSorcePlay audioSorcePlay;

    public GameManager gameManager; // 플레이어 스킬 레벨 체크;
    
    public GameObject[] Axes;
    public GameObject AOEObj;
    bool AOELV2 = false;
    bool AOELV3 = false;
    public string LastKeyCord; // 좌우 방향

    public float speed;
    public int health = 100;
    public float roll = 3f;
    public int exp = 20;

    public float maxShotDelay;
    public float curShotDelay;
    public float curThrowDelay;
    public float maxThrowDelay;
    public float maxrollDelay;
    public float currollDelay;
    public float maxbeshotDelay;//플레이어가 몬스터에게 충돌당할 때 피격 딜레이
    public float curbeshotDelay;
    public float curAxesDelay;
    public float maxAxesDelay;
    public float curMobClearDelay;
    public float maxMobClearDelay;
    public float curFallingShotDelay;
    public float maxFallingShotDelay;

    public int Bulletdmg;

    private Collision2D enemycollision;
    public bool beshot;
    public bool beroll;

    public bool Alive;

    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;

    public float Moveh= 0, Movev = 0;

    public int collisionNum; // 불렛과 피격 1번, enemy와 피격 2번

    public int Pos = 0;
    public GameObject Bullet1;
    SpriteRenderer spriteRenderer;

    public PlayerState playerState;

    public Vector2 size;
    public LayerMask whatIsLayer;

    [Header("회전속도 조절")]
    [SerializeField] [Range(1f, 100f)] float rotateSpeed = 50f;

    Enemy enemy;

    public Animator anim;
    public Rigidbody2D rigid;

    public ObjectManager objectManager;

    void Start()
    {
        LastKeyCord = "RIGHT";
        Alive = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        
    }
    void Update()
    {
        rigid.velocity = Vector2.zero;
        Move();

        Fire();
        Reload0();
        WeaponThrow();
        Reload1();
        RotatingAxes();
        Reload3();
        Fallingshot();
        Reload4();
        AOE();
        MobClear();
        Reload5();
        ElapseTime();
        if(beroll)
            Roll();

        if (Input.GetKeyDown(KeyCode.Q)){
            playerState.CoinEventExp();
        }

        if (Input.GetKeyDown(KeyCode.Space)&& GameObject.FindWithTag("PlayerState").GetComponent<PlayerState>().currollDelay == 3f)
        {
            Debug.Log("스페이스바 누름");
            PlayerState playerstate = GameObject.FindWithTag("PlayerState").GetComponent<PlayerState>();
            playerstate.currentroll = 0;
            playerstate.roll = true;
            beroll = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)|| Input.GetKeyDown(KeyCode.A))
        {
            LastKeyCord = "LEFT";
            spriteRenderer.flipX = true;
            Debug.Log("왼쪽 키 " + LastKeyCord);
            anim.SetBool("IsWalking", true);

        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            LastKeyCord = "RIGHT";
            spriteRenderer.flipX = false;
            Debug.Log("오른쪽 키 "+LastKeyCord);
            anim.SetBool("IsWalking", true);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            anim.SetBool("IsWalking", true);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D)
            || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {

            }
            else
            {
                anim.SetBool("IsWalking", false);
            }

        }




    }
    void ElapseTime()
    {
        curbeshotDelay += Time.deltaTime;

        if (curbeshotDelay > maxbeshotDelay && beshot == true)
        {
            maxbeshotDelay = 1f;
            curbeshotDelay = 0f;
            OnDamaged(collisionNum);

        }
        else if (curbeshotDelay > maxbeshotDelay && beshot == false)
        {
            maxbeshotDelay = 1f;
            curbeshotDelay = 1f;

        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        else if (collision.gameObject.tag == "EnemyBullet")
        {
            collisionNum = 1;
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            Bulletdmg = bullet.dmg;
            OnDamaged(collisionNum);
        }

        else if (collision.gameObject.tag == "Coin")
        {
            collision.gameObject.SetActive(false);
            GameObject.FindWithTag("PlayerState").GetComponent<PlayerState>().SetCoinCount();
            

            //아이템 획득
        }
        else if (collision.gameObject.tag == "Emerald")
        {
            collision.gameObject.SetActive(false);
            GameObject.FindWithTag("PlayerState").GetComponent<PlayerState>().SetExpCount();


            //아이템 획득
        }

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
 
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy"|| collision.gameObject.tag == "EnemyBoss")
        {
            collisionNum = 2;
            Debug.Log("몹과 충돌");
            enemy = collision.gameObject.GetComponent<Enemy>();
            beshot = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        beshot = false;
    }

    // 몹과 피격시 데미지, 무적 처리
    void OnDamaged(int collisionNum)
    {
        // layer 값 변경
        gameObject.layer = 10;

        // 피격시 스프라이트 컬러 변경
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        
        
        if(collisionNum == 1)
        {
            OnHit(Bulletdmg);
        }
        if(collisionNum == 2)
        {
            OnHit(enemy.CollisionDmg);
        }
        
        Debug.Log("몹과 피격");

        Invoke("OffDamaged", 1);
    }

    void OffDamaged()
    {
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if((isTouchRight&&h == 1)||(isTouchLeft && h == -1))
        {
            h = 0;
        }
        float v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))
        {
            v = 0;
        }
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

    }

    // 스킬리스트
    // 1번 : Fire(해결)
    // 2번 : WeaponThrow(해결)
    // 3번 : AOE(해결)
    // 4번 : RotatingAxes(해결)
    // 5번 : FallingShot(해결)
    // 6번 : MobClear(해결)
    void Fire()
    {
        if (gameManager.SkillLevel[0] == 1)
        {
            if (curShotDelay < maxShotDelay)
                return;

            GameObject Bullet = objectManager.MakeObj("bulletPlayerA");
            Vector3 dirVec = transform.position; // 목표물 방향 = 목표물 위치 - 자신의 위치
            Rigidbody2D rigid = Bullet.GetComponent<Rigidbody2D>();
            if (LastKeyCord.Equals("RIGHT"))
            {
                Bullet.transform.position = transform.position;
                Bullet.transform.rotation = Quaternion.Euler(0, 0, 90);
                rigid.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
            }
            else if (LastKeyCord.Equals("LEFT"))
            {
                Bullet.transform.position = transform.position;
                Bullet.transform.rotation = Quaternion.Euler(0, 0, -90);
                rigid.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
            }
            
            curShotDelay = 0;
        }
        else if (gameManager.SkillLevel[0] == 2)
        {
            if (curShotDelay < maxShotDelay)
                return;

            GameObject[] Bullet = new GameObject[3];
            for (int i = 0; i < 3; i++)
            {
                Bullet[i] = objectManager.MakeObj("bulletPlayerA");
                Bullet[i].transform.position = transform.position;
                Rigidbody2D rigid = Bullet[i].GetComponent<Rigidbody2D>();
                Bullet[i].SetActive(false);
                switch (i)
                {
                    case 1:
                        Bullet[i].transform.eulerAngles = new Vector3(0, 0, 90);
                        Bullet[i].SetActive(true);
                        rigid.AddForce(Vector2.left * 20, ForceMode2D.Impulse);
                        break;
                    case 2:
                        Bullet[i].SetActive(true);
                        Bullet[i].transform.eulerAngles = new Vector3(0, 0, -90);
                        rigid.AddForce(Vector2.right * 20, ForceMode2D.Impulse);
                        break;
                }
            }
            
            curShotDelay = 0;
        }
        else if (gameManager.SkillLevel[0] == 3)
        {
            if (curShotDelay < maxShotDelay)
                return;

            GameObject[] Bullet = new GameObject[7];
            for(int i=0; i < 7; i++)
            {
                Bullet[i] = objectManager.MakeObj("bulletPlayerA");
                Bullet[i].transform.position = transform.position;
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
                }
                curShotDelay = 0;
            }

           
        }
        audioSorcePlay.AudioPlay(0);
    }

    void WeaponThrow()
    {
        if (gameManager.SkillLevel[1] == 1)
        {
            if (curThrowDelay < maxThrowDelay)
                return;

            GameObject hamer = objectManager.MakeObj("bulletPlayerB");
            hamer.transform.position = transform.position + new Vector3(0, 0.1f, 0);
            hamer.transform.rotation = Quaternion.identity;
            Rigidbody2D rigid = hamer.GetComponent<Rigidbody2D>();

            if (LastKeyCord.Equals("LEFT"))
            {
                rigid.AddForce((new Vector2(-5, 10)), ForceMode2D.Impulse);
            }
            else if (LastKeyCord.Equals("RIGHT"))
            {
                rigid.AddForce((new Vector2(5, 10)), ForceMode2D.Impulse);
            }

            curThrowDelay = 0;
        }
        else if (gameManager.SkillLevel[1] == 2)
        {
            if (curThrowDelay < maxThrowDelay)
                return;

            GameObject[] hamer = new GameObject[3];
            for (int i = 0; i < 3; i++)
            {
                hamer[i] = objectManager.MakeObj("bulletPlayerB");
                hamer[i].transform.position = transform.position + new Vector3(0, 0.1f, 0);
                hamer[i].transform.rotation = Quaternion.identity;
                Rigidbody2D rigid = hamer[i].GetComponent<Rigidbody2D>();
                switch (i)
                {
                    case 1:
                        rigid.AddForce((new Vector2(-5, 10)), ForceMode2D.Impulse);
                        break;
                    case 2:
                        rigid.AddForce((new Vector2(5, 10)), ForceMode2D.Impulse);
                        break;
                    case 0:
                        hamer[i].SetActive(false);
                        break;
                }
            }

            curThrowDelay = 0;
        }
        else if (gameManager.SkillLevel[1] == 3)
        {
            if (curThrowDelay < maxThrowDelay)
                return;

            GameObject[] hamer = new GameObject[5];
            for (int i = 0; i < 5; i++)
            {
                hamer[i] = objectManager.MakeObj("bulletPlayerB");
                hamer[i].transform.position = transform.position + new Vector3(0, 0.1f, 0);
                hamer[i].transform.rotation = Quaternion.identity;
                Rigidbody2D rigid = hamer[i].GetComponent<Rigidbody2D>();
                switch (i)
                {
                    case 1:
                        rigid.AddForce((new Vector2(-5, 10)), ForceMode2D.Impulse);
                        break;
                    case 2:
                        rigid.AddForce((new Vector2(5, 10)), ForceMode2D.Impulse);
                        break;
                    case 3:
                        rigid.AddForce((new Vector2(-3, 10)), ForceMode2D.Impulse);
                        break;
                    case 4:
                        rigid.AddForce((new Vector2(3, 10)), ForceMode2D.Impulse);
                        break;
                    case 0:
                        hamer[i].SetActive(false);
                        break;
                }
            }

            curThrowDelay = 0;
        }
        audioSorcePlay.AudioPlay(1);
    }
    void AOE()
    {
        if (gameManager.SkillLevel[2] == 1)
        {
            AOEObj.SetActive(true);

        }
        else if (gameManager.SkillLevel[2] == 2 && !AOELV2)
        {
            AOEObj.transform.localScale = new Vector3(AOEObj.transform.localScale.x + 0.02f, AOEObj.transform.localScale.y + 0.02f, AOEObj.transform.localScale.z);
            AOELV2 = true;
        }
        else if (gameManager.SkillLevel[2] == 3 && !AOELV3)
        {
            AOEObj.transform.localScale = new Vector3(AOEObj.transform.localScale.x + 0.02f, AOEObj.transform.localScale.y + 0.02f, AOEObj.transform.localScale.z);
            AOELV3 = true;
        }
    }
    void RotatingAxes()
    {
        if (gameManager.SkillLevel[3] == 1)
        {
            if (curAxesDelay < 4.5f)
            {
                Axes[0].SetActive(true);
                return;
            }
            else if (curAxesDelay > 4.5f && curAxesDelay < maxAxesDelay)
            {
                Axes[0].SetActive(false);
                Axes[0].transform.position = gameObject.transform.position + new Vector3(-2.5f, 0, 0);
                Axes[0].transform.rotation = Quaternion.Euler(0, 0, 0);
                return;
            }

            curAxesDelay = 0;
        }
        else if (gameManager.SkillLevel[3] == 2)
        {
            if (curAxesDelay < 4.5f)
            {
                Debug.Log("도끼 생성");
                Axes[0].SetActive(true);
                Axes[2].SetActive(true);
                return;
            }
            else if (curAxesDelay > 4.5f && curAxesDelay < maxAxesDelay)
            {
                Debug.Log("도끼 삭제");
                Axes[0].SetActive(false);
                Axes[2].SetActive(false);
                Axes[0].transform.position = gameObject.transform.position + new Vector3(-2.5f, 0, 0);
                Axes[2].transform.position = gameObject.transform.position + new Vector3(2.5f, 0, 0);
                Axes[0].transform.rotation = Quaternion.Euler(0, 0, 0);
                Axes[2].transform.rotation = Quaternion.Euler(0, 0, 180);
                return;
            }
            curAxesDelay = 0;
        }
        else if (gameManager.SkillLevel[3] == 3)
        {
            for(int i = 0; i < 4; i++)
            {
                if (!Axes[i].activeSelf)
                    Axes[i].SetActive(true);
            }
        }
    }

    void Fallingshot()
    {
        int count = 0;

        if (gameManager.SkillLevel[4] == 1)
            count = 2;
        else if (gameManager.SkillLevel[4] == 2)
            count = 4;
        else if (gameManager.SkillLevel[4] == 3)
            count = 8;

        if (curFallingShotDelay < maxFallingShotDelay)
            return;
        for (int i = 0; i < count/2; i++)
        {
            GameObject fallingBullet = objectManager.MakeObj("bulletPlayerE");
            fallingBullet.transform.position = transform.position + Vector3.up * 5f + Vector3.right * Random.Range(0f,4f);
            fallingBullet.transform.rotation = Quaternion.identity;
            Rigidbody2D rigid = fallingBullet.GetComponent<Rigidbody2D>();
        }
        for (int i = 0; i < count / 2; i++)
        {
            GameObject fallingBullet = objectManager.MakeObj("bulletPlayerE");
            fallingBullet.transform.position = transform.position + Vector3.up * 5f + Vector3.left * Random.Range(0f, 4f);
            fallingBullet.transform.rotation = Quaternion.identity;
            Rigidbody2D rigid = fallingBullet.GetComponent<Rigidbody2D>();
        }
        curFallingShotDelay = 0;
        audioSorcePlay.AudioPlay(4);
    }

    void MobClear()
    {
        if (gameManager.SkillLevel[5] == 1)
        {
            maxMobClearDelay = 90;
            setClear();
        }
        else if (gameManager.SkillLevel[5] == 2)
        {
            maxMobClearDelay = 70;
            setClear();
        }
        else if (gameManager.SkillLevel[5] == 3)
        {
            maxMobClearDelay = 50;
            setClear();
        }

        if (curMobClearDelay < maxMobClearDelay)
            return;
        
    }
    void setClear()
    {
        if (curMobClearDelay > maxMobClearDelay)
        {
            for (int i = 0; i < objectManager.enemy1.Length; i++)
            {
                if (objectManager.enemy1[i].activeSelf)
                    objectManager.enemy1[i].GetComponent<Enemy>().OnHit(100000);
            }
            for (int i = 0; i < objectManager.enemy2.Length; i++)
            {
                if (objectManager.enemy2[i].activeSelf)
                    objectManager.enemy2[i].GetComponent<Enemy>().OnHit(100000);
            }
            for (int i = 0; i < objectManager.enemy3.Length; i++)
            {
                if (objectManager.enemy3[i].activeSelf)
                    objectManager.enemy3[i].GetComponent<Enemy>().OnHit(100000);
            }
            for (int i = 0; i < objectManager.enemy4.Length; i++)
            {
                if (objectManager.enemy4[i].activeSelf)
                    objectManager.enemy4[i].GetComponent<Enemy>().OnHit(100000);
            }
            curMobClearDelay = 0;
            audioSorcePlay.AudioPlay(3);
        }
    }

    void Reload0() // 검 던지기 딜레이
    {
        curShotDelay += Time.deltaTime;
    }
    void Reload1() // 해머 던지기 딜레이
    {
        curThrowDelay += Time.deltaTime;
    }
    void Reload3() // 회전하는 도끼 딜레이
    {
        curAxesDelay += Time.deltaTime;
    }
    void Reload4() // 떨어지는 불렛 딜레이
    {
        curFallingShotDelay += Time.deltaTime;
    }
    void Reload5() // 몹클리어 딜레이
    {
        curMobClearDelay += Time.deltaTime;
    }
    void OnHit(int dmg)
    {
        health -= dmg;
        PlayerState playerstate = GameObject.FindWithTag("PlayerState").GetComponent<PlayerState>();
        playerstate.Onhit(dmg);
        //spriteRenderer.sprite = sprites[1];
        //Invoke("ReturnSprite", 0.1f);
        if (health <= 0)
        {
            gameObject.SetActive(false);
            GameManager manager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            manager.PlayerAlive = false;
            Alive = false;
            Time.timeScale = 0f;
            gameOver.SetActive(true);
        }
    }

    void ReturnSprite()
    {
       // spriteRenderer.sprite = sprites[0];
    }


    // 구르기(무적 판정)
    void Roll()
    {
        if (LastKeyCord.Equals("LEFT"))
        {
            currollDelay += Time.deltaTime;
            gameObject.layer = 10;
            this.transform.Rotate(0, 0, 90 * Time.deltaTime * rotateSpeed, Space.Self);
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);

            if (currollDelay > maxrollDelay && beroll == true)
            {
                gameObject.layer = 9;
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
                maxrollDelay = 0.4f;
                currollDelay = 0f;
                spriteRenderer.color = new Color(1, 1, 1, 1);
                beroll = false;

            }
        }
        if(LastKeyCord.Equals("RIGHT"))
        {
            currollDelay += Time.deltaTime;
            gameObject.layer = 10;
            this.transform.Rotate(0, 0, -90 * Time.deltaTime * rotateSpeed, Space.Self);
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);

            if (currollDelay > maxrollDelay && beroll == true)
            {
                gameObject.layer = 9;
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
                maxrollDelay = 0.4f;
                currollDelay = 0f;
                spriteRenderer.color = new Color(1, 1, 1, 1);
                beroll = false;

            }
        }
    }
}
