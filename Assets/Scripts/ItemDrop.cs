using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public bool follow;
    public bool trace;
    Rigidbody2D rb;
    Transform target;
    [Header("추격속도")]
    [SerializeField] [Range(1f, 10f)] float moveSpeed = 3f;

    [Header("근접 거리")]
    [SerializeField] [Range(0f, 3f)] float contactDistance = 1f;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("player").GetComponent<Transform>();
    }

    void Update()
    {
        trace = GameObject.FindWithTag("GameManager").GetComponent<GameManager>().PlayerAlive;
        if (trace)
            traceTarget();
    }

    void traceTarget()
    {
        if (Vector2.Distance(transform.position, target.position) < contactDistance && target != null)
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        else
            rb.velocity = Vector2.zero;
    }


}
