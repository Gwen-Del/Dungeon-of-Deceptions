using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomParameter : MonoBehaviour
{
    public GameObject[] walls;
    public bool[] status;

    void Start()
    {
        UpdateRoom(status);
    }

    public void UpdateRoom(bool[] status)
    {
        for (int i= 0; i < status.Length; i ++)
        {
            walls[i].SetActive(!status[i]);
        }
    }
}
