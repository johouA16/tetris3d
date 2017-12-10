using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameOverFlag
{
    public static bool End_flag = false;
}

public class Game_Start_End : MonoBehaviour {
    public Text gametext;
    float start_time = 0;

    void Start()
    {
        start_time = Time.time;
    }

    // Update is called once per frame
    void Update () {
        if (Input.anyKeyDown || (Time.time - start_time > 0.5) )
        {
            gametext.text = " ";
        }

        if (GameOverFlag.End_flag == true)
        {
            gametext.text = "GAME OVER";
        }
    }
}
