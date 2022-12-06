using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public Button LVUI;

    public Text CoinText;

    public Slider hpSlider;
    public Slider rollSlider;
    public Slider expSlider;

    public int currenthp;
    public float currentroll;
    public int currentexp;
    public int currentcoin;

    public float maxrollDelay;
    public float currollDelay;
    public bool roll;

    public PlayerMove playermove;

    public GameObject coinEvent;

    // Start is called before the first frame update
    void Start()
    {
        hpSlider.maxValue = playermove.health;
        rollSlider.maxValue = playermove.roll;
        expSlider.maxValue = 20;
        hpSlider.value = playermove.health;
        rollSlider.value = currentroll;
        expSlider.value = currentexp;
        CoinText.text = string.Format("{0:D3}", currentcoin);
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = playermove.health;
        
        Onroll();
    }

    public void Onhit(int dmg)
    {
        currenthp -= dmg;
        if (currenthp < 0)
            currenthp = 0;
        hpSlider.value -= currenthp;
    }

    public void Onroll()
    {
        currollDelay += Time.deltaTime;
        rollSlider.value = currollDelay;

        if (currollDelay > maxrollDelay && roll == true)
        {
            maxrollDelay = 3f;
            currollDelay = 0f;
            roll = false;

        }
        else if (currollDelay > maxrollDelay && roll == false)
        {
            maxrollDelay = 3f;
            currollDelay = 3f;

        }
    }

    public void SetCoinCount()
    {
        currentcoin += Random.Range(1, 11);
        CoinText.text = string.Format("{0:D3}", currentcoin);

        if (currentcoin >= 100)
        {
            currentcoin = 0;
            CoinText.text = string.Format("{0:D3}", currentcoin);
            CoinEvent();
        }
    }

    public void CoinEvent()
    {
        Time.timeScale=0;
        Debug.Log("아이템 선택");
        coinEvent.SetActive(true);
    }
    public void CoinEventExp()
    {
        expSlider.value = 0;
        playermove.exp = 0;
        currentexp = 0;
        ExpEvent();
    }
    public void CoinEventHealth()
    {
        hpSlider.value += 30;
        playermove.health += 30;
        currenthp += 30;
    }
    public void SetExpCount()
    {
        currentexp += 1;
        expSlider.value = currentexp;
        if (currentexp == 20)
        {
            expSlider.value = 0;
            currentexp = 0;
            ExpEvent();
        }
    }

    public void ExpEvent()
    {
        LVUI.gameObject.SetActive(true);
        Time.timeScale = 0;
        Debug.Log("스킬 선택");
    }
}
