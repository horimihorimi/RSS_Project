using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        if (!gameManager.PlayerAlive)
            gameObject.SetActive(false);

        //불렛이 활성화되면 10초 뒤 삭제
        if (gameObject.activeSelf)
        {
            Invoke("Delete", 10);
        }
    }
    

    public void Delete()
    {
        gameObject.SetActive(false);
    }
}
