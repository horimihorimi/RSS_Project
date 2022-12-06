using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinEvent : MonoBehaviour
{
    public PlayerState playerState;

    public GameObject coinEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnClick(int num)
    {
        if(num == 0)
        {
            playerState.CoinEventExp();
            coinEvent.SetActive(false);
        }

        else if (num == 1)
        {
            playerState.CoinEventHealth();
            coinEvent.SetActive(false);
            Time.timeScale = 1.0f;
        }   
    }
}
