using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg;

    public GameObject Player;

    public float speed;
    public Transform target;

    public bool bulletAlive;

    public float cur = 0;
    float max = 5;

    void Update()
    {
        if (gameObject.tag.Equals("PlayerBulletD"))
        {
            //gameObject.transform.position = Player.transform.position;

            gameObject.transform.RotateAround(gameObject.transform.parent.position, this.transform.parent.forward, 90f * Time.deltaTime);
        }

        if ((gameObject.tag.Equals("PlayerBulletD")))
        {

        }
        else if ((gameObject.tag.Equals("PlayerBulletC"))){

        }
        else
        {
            bulletDel();
            Reload();
        }
    }

    void Reload()
    {
        cur += Time.deltaTime;
    }
    public void bulletDel()
    {
        if (cur < max)
            return;
        gameObject.SetActive(false);
        Debug.Log("불렛파괴");
        cur = 0;
    }

}
