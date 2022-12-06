using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    public Enemy enemy;

    public int BossHP;

    // Update is called once per frame
    void Update()
    {
        BossHP = enemy.health;
        Clear();
    }
    void Clear()
    {
        if (BossHP <= 0)
            SceneManager.LoadScene("GameClear");
    }

}
