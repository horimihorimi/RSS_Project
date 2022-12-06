using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public int leftPos;
    public int centerPos;
    public int rightPos;
    public Transform[] sprite;
    public Transform Player;
    public int curPosition;
    public bool Alive;


    float viewWidth;

    // Start is called before the first frame update
    void Start()
    {
        if (Alive)
        {
            Alive = GameObject.FindWithTag("GameManager").GetComponent<GameManager>().PlayerAlive;
            viewWidth = 16 * (Camera.main.orthographicSize) / 9;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Alive = GameObject.FindWithTag("GameManager").GetComponent<GameManager>().PlayerAlive;

        if (Alive)
        {
            //왼쪽
            Alive = GameObject.FindWithTag("GameManager").GetComponent<GameManager>().PlayerAlive;
            if (Player.position.x <= (sprite[curPosition].position.x - viewWidth))
            {
                Vector3 leftSpritepos = sprite[leftPos].localPosition;
                Vector3 centerSpritepos = sprite[centerPos].localPosition;
                Vector3 rightSpritepos = sprite[rightPos].localPosition;
                sprite[leftPos].transform.localPosition = leftSpritepos + Vector3.left * viewWidth * 2;
                sprite[centerPos].transform.localPosition = centerSpritepos + Vector3.left * viewWidth * 2;
                sprite[rightPos].transform.localPosition = rightSpritepos + Vector3.left * viewWidth * 2; ;
            }
            //오른쪽
            if (Player.position.x >= (sprite[curPosition].position.x + viewWidth))
            {
                Vector3 leftSpritepos = sprite[leftPos].localPosition;
                Vector3 centerSpritepos = sprite[centerPos].localPosition;
                Vector3 rightSpritepos = sprite[rightPos].localPosition;
                sprite[leftPos].transform.localPosition = leftSpritepos + Vector3.right * viewWidth * 2;
                sprite[centerPos].transform.localPosition = centerSpritepos + Vector3.right * viewWidth * 2;
                sprite[rightPos].transform.localPosition = rightSpritepos + Vector3.right * viewWidth * 2; ;
            }

        }
    }
}
