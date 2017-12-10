using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public GameObject[] Border;

    Vector3 StartPos;
    Vector3[] CamPos = { new Vector3(17, 15, -7), new Vector3(-7, 15, -7), new Vector3(-7, 15, 17), new Vector3(17, 15, 17) };
    float[] Angle = { -45.0F, 45.0F, 135.0F, -135.0F };

    double Timer = 0.0;
    double startTime;
    double time = 0.7;

    bool RotationFlag = false;
    int NowPos = 0;
    int NextPos = 1;

    // Use this for initialization
    void Start () {
        transform.position = CamPos[0];
        startTime = Time.timeSinceLevelLoad;
        Border[0].SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        //正回転
        if (Input.GetKeyDown(KeyCode.R) && RotationFlag == false)
        {
            NextPos = NowPos + 1;
            if (NowPos == 3)
            {
                NextPos = 0;
            }

            RotationFlag = true;
            startTime = Time.timeSinceLevelLoad;
        }
        //逆回転
        if (Input.GetKeyDown(KeyCode.T) && RotationFlag == false)
        {
            NextPos = NowPos - 1;
            if (NowPos == 0)
            {
                NextPos = 3;
            }

            RotationFlag = true;
            startTime = Time.timeSinceLevelLoad;
        }

        if (RotationFlag)
        {
            //移動
            double diff = Time.timeSinceLevelLoad - startTime;
            if (diff > time)
            {
                //カメラ正面のボーダーを消す
                Border[NowPos].SetActive(true);
                Border[NextPos].SetActive(false);

                NowPos = NextPos;
                RotationFlag = false;
                transform.position = CamPos[NowPos];
            }

            float rate = (float)diff / (float)time;

            transform.position = Vector3.Lerp(CamPos[NowPos], CamPos[NextPos], rate);

            //回転
            float angle = Mathf.LerpAngle(Angle[NowPos], Angle[NextPos], rate);
            transform.eulerAngles = new Vector3(30, angle, 0);
        }
    }
}
