using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public float cameraSpeed = 5.0f;
    public bool Alive;
    public GameObject player;

    public void Awake()
    {
        Alive = GameObject.FindWithTag("GameManager").GetComponent<GameManager>().PlayerAlive;
    }
    private void Update()
    {
        Alive = GameObject.FindWithTag("GameManager").GetComponent<GameManager>().PlayerAlive;
        if (Alive)
        {
            Vector3 dir = player.transform.position - this.transform.position;


            Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f);

            
            this.transform.Translate(moveVector);
            
        }
    }
}