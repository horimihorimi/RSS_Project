using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class LevelUP : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerState playerState;
    public PlayerMove playerMove;
    public Text[] text;
    public int LevelMax;

    public bool wakeup;

    void Awake()
    {
        //SkillUI[0].sprite = image;
    }

    // Update is called once per frame
    void Update()
    {
        //image[0].name
        if (gameObject.activeSelf&&!wakeup)
        {
            SetSkillImg();
            wakeup = true;
        }
    }

    public void Onclick(int num)
    {
        switch (num)
        {
            case 0:
                Debug.Log("1번 버튼 클릭");
                SkillUi(num);
                wakeup = false;
                break;
            case 1:
                Debug.Log("2번 버튼 클릭");
                SkillUi(num);
                wakeup = false;
                break;
            //case 3:
            //    Debug.Log("3번 버튼 클릭");
            //    SkillUi(num);
            //    break;
        }
        Time.timeScale = 1.0f;
        gameManager.LVUI.gameObject.SetActive(false);
        Debug.Log(num);

        
    }

    public void SetSkillImg()
    {
        //for(int i = 0; i < 2; i++)
        //{
        //    text[i].text = "";
        //}
        LevelMax = 0;
        SetSkillFlag();
        for (int i = 0; i < gameManager.imageSelect.Length; i++)
        {
            gameManager.RanNum = RandomNum();
            Debug.Log(gameManager.imageSelect.Length);
            while (true)
            {
                if (!gameManager.skillFlag[gameManager.RanNum])
                {
                    gameManager.imageSelect[i].sprite = gameManager.image[gameManager.RanNum];
                    text[i].text = gameManager.text[gameManager.RanNum].text;
                    gameManager.skillFlag[gameManager.RanNum] = true;
                    gameManager.SelectSkillNum[i] = gameManager.RanNum;
                    break;
                }
                else
                    gameManager.RanNum = RandomNum();
            }
        }
    }

    //SkillFlag값 false 초기화
    public void SetSkillFlag()
    {
        Debug.Log(gameManager.skillFlag.Length);
        for (int i = 0; i < gameManager.skillFlag.Length; i++)
        {
            gameManager.skillFlag[i] = false;
        }
    }

    int RandomNum()
    {
        return Random.Range(0, 7);
        //gameManager.image.Length
    }

    // 좌측 상단 스킬 UI 설정
    public void SkillUi(int num)
    {
        int i;
        
        for(i = 0; i < gameManager.SkillString.Length; i++)
        {
            if (gameManager.SkillString[i].Equals(gameManager.SkillList[gameManager.SelectSkillNum[num]]) && gameManager.SkillLevel[gameManager.SelectSkillNum[num]] < 3)
            {
                // 이미 스킬을 얻었을 경우 UI 갱신x, 레벨업만
                gameManager.SkillLevel[gameManager.SelectSkillNum[num]] += 1;
                gameManager.LevelText[i].text = gameManager.SkillLevel[i].ToString();
                Debug.Log("스킬 레벨업");
                return;
            }
            else if (gameManager.SkillString[i].Equals(gameManager.SkillList[gameManager.SelectSkillNum[num]])&&gameManager.SkillLevel[gameManager.SelectSkillNum[num]] ==3)
            {

                Debug.Log("체력회복1");
                playerMove.health += 30;
                return;
            }
        }
        

        if(i==6){
            for (int j = 0; j < gameManager.SkillString.Length; j++)
            {
                Debug.Log("스킬 get"+j);
                if (string.IsNullOrEmpty(gameManager.SkillString[j]) && gameManager.SkillLevel[j] < 3)
                {
                    gameManager.SkillUI[j].sprite = gameManager.image[gameManager.SelectSkillNum[num]];
                    gameManager.SkillLevel[gameManager.SelectSkillNum[num]] += 1;
                    gameManager.SkillString[j] = gameManager.SkillList[gameManager.SelectSkillNum[num]];
                    gameManager.LevelText[j].text = gameManager.SkillLevel[j].ToString();
                    return;
                }
                else if (string.IsNullOrEmpty(gameManager.SkillString[j]) && gameManager.SkillLevel[j] == 3)
                {
                    Debug.Log("체력회복2");
                    playerMove.health+=30;
                    return;
                }
            }
        }
    }
}
