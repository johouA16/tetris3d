using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 定数を定義
public static class Define
{
    // 落ちるスピードが変化するミノの個数
    public const int SPEED_UP_NUM = 10;
}

public class Group : MonoBehaviour {
    // Time since last gravity tick
    float lastFall = 0;

    public static double fallTime = 1.0;

    bool isValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.roundVec2(child.position);

            // Not inside Border?
            if (!Grid.insideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (Grid.grid[(int)v.x, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    void updateGrid()
    {
        // Remove old children from grid
        for (int y = 0; y < Grid.h; ++y)
            for (int x = 0; x < Grid.w; ++x)
                if (Grid.grid[x, y] != null)
                    if (Grid.grid[x, y].parent == transform)
                        Grid.grid[x, y] = null;

        // Add new children to grid
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.roundVec2(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
        }
    }

    void fall_sequence()
    {
        // Move Left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Modify position
            transform.position += new Vector3(-1, 0, 0);

            // See if valid
            if (isValidGridPos())
                // It's valid. Update grid.
                updateGrid();
            else
                // It's not valid. revert.
                transform.position += new Vector3(1, 0, 0);
        }

        // Move Right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Modify position
            transform.position += new Vector3(1, 0, 0);

            // See if valid
            if (isValidGridPos())
                // It's valid. Update grid.
                updateGrid();
            else
                // It's not valid. revert.
                transform.position += new Vector3(-1, 0, 0);
        }
        
        //Hold 
        else if (Input.GetKeyDown(KeyCode.H) && Hold_Flag.second_ban == false)
        {
            GameObject.Destroy(gameObject);
        }

        // Rotate
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);

            // See if valid
            if (isValidGridPos())
                updateGrid();
            else
            {
                transform.position += new Vector3(-1, 0, 0);
                if (isValidGridPos())
                    updateGrid();
                else
                {
                    transform.position += new Vector3(-1, 0, 0);
                    if (isValidGridPos())
                        updateGrid();
                    else
                    {

                        transform.position += new Vector3(3, 0, 0);
                        if (isValidGridPos())
                            updateGrid();
                        else
                        {
                            transform.position += new Vector3(1, 0, 0);
                            if (isValidGridPos())
                                updateGrid();
                            else
                            {
                                // It's not valid. revert.
                                transform.Rotate(0, 0, 90);
                                transform.position += new Vector3(-2, 0, 0);
                            }
                        }
                    }
                }
            }
        }
        // Move Downwards and Fall
        else if ((Input.GetKey(KeyCode.DownArrow) && Time.time - lastFall >= 0.1) ||
                 Time.time - lastFall >= fallTime)
        {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // See if valid
            if (isValidGridPos())
            {
                // It's valid. Update grid.
                updateGrid();
            }
            else
            {
                // It's not valid. revert.
                transform.position += new Vector3(0, 1, 0);

                Hold_Flag.second_ban = false;

                // Clear filled horizontal lines
                Grid.deleteFullRows();

                // Spawn next Group
                FindObjectOfType<Spawner>().spawnNext();

                // 落ちるのが早くなるところ
                if (ScoreText.addFallBlocks() % Define.SPEED_UP_NUM == 0)
                {
                    fallTime -= 0.1;
                    Debug.Log("fallTime ->" + fallTime.ToString());
                }

                // Disable script
                enabled = false;
            }

            lastFall = Time.time;
        }
        // Fall
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            int score = 0;

            while (true)
            {

                transform.position += new Vector3(0, -1, 0);

                if (!isValidGridPos())
                {
                    // It's not valid. revert.
                    transform.position += new Vector3(0, 1, 0);
                    break;
                }
                score++;

            }
            updateGrid();

            Hold_Flag.second_ban = false;

            //スコア加算
            ScoreText.addScore(score);

            // Clear filled horizontal lines
            Grid.deleteFullRows();

            // Spawn next Group
            FindObjectOfType<Spawner>().spawnNext();

            // 落ちるのが早くなるところ
            if (ScoreText.addFallBlocks() % Define.SPEED_UP_NUM == 0)
            {
                fallTime -= 0.1;
                Debug.Log("fallTime ->" + fallTime.ToString());
            }

            // Disable script
            enabled = false;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (0 <= transform.position.x && transform.position.x <= 9)
        {
            if (!isValidGridPos())
            {
                Debug.Log("GAME OVER");
                Destroy(gameObject);
                GameOverFlag.End_flag = true;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(transform.position.x);
        //fall_sequence();

        if (0 <= transform.position.x && transform.position.x <= 9)
        {
            if (Time.timeScale > 0) {
                fall_sequence();
            }   

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Pause.GameStop();
            }
        }
    }
}
