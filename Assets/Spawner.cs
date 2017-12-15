using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupChecker
{
    public static int Next_Group = -1;
    public static int Current_Group = 1;

}

public class Spawner : MonoBehaviour {

    // Groups
    public GameObject[] groups;
    GameObject next;
    bool isExistNextZone = false;

    public void spawnNext()
    {
        if (isExistNextZone)
        {
            moveObject(next);
            next = createMino();
        }
        else
        {
            moveObject(createMino());
            next = createMino();
        }
    }

    void moveObject(GameObject fallObject)
    {
        GroupChecker.Current_Group = GroupChecker.Next_Group;
        fallObject.transform.position = new Vector3(4, 14, 0);
    }

    GameObject createMino()
    {
        // Random Index
        int i = Random.Range(0, groups.Length);
        GroupChecker.Next_Group = i;

        // Spawn Group at current Position
        return Instantiate(groups[i],
                    transform.position,
                    Quaternion.identity);
    }

    // Use this for initialization
    void Start()
    {
        spawnNext();
        isExistNextZone = true;
    }

    void LateUpdate()
    {
        if (Input.GetKeyUp(KeyCode.H) && Hold_Flag.hold_flag == false)
        {
            Hold_Flag.hold_flag = true;
            moveObject(next);
            next = createMino();

        }
    }


}
