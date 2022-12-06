using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{

    public Vector3 followPos;
    public int followDelay;
    public Transform PlayerTrans;
    public Queue<Vector3> PlayerPos;
    void Awake()
    {
        PlayerTrans = GameObject.FindWithTag("player").GetComponent<Transform>();
        PlayerPos = new Queue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        Watch();
        Follow();
    }

    void Watch()
    {
        PlayerPos.Enqueue(PlayerTrans.position);

        if(PlayerPos.Count > followDelay)
            followPos = PlayerPos.Dequeue();
    }

    void Follow()
    {
        transform.position = followPos;
    }
}
