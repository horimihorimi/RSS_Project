using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float _Sec;
    public int _Min;
    GameManager gameManager;


    [SerializeField]Text _TimerText;

    private void Start()
    {
       gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        if(gameManager.PlayerAlive)
            _Timer();
    }

    void _Timer()
    {
        _Sec += Time.deltaTime;

        _TimerText.text = string.Format("{0:D2} : {1:D2}", _Min, (int)_Sec);

        if ((int)_Sec > 59)
        {
            _Sec = 0;
            _Min++;
        }
    }
}
