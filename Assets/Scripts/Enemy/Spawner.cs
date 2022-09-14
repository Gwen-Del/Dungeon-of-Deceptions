using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnObject;
    Vector3[] spawnPoints;

    [Range(0,4)]
    public int minEnnemy = 0;
    [Range(0,4)]
    public int maxEnnemy = 4;

    int nbEnnemy;

    void Start()
    {
        spawnPoints = new [] { new Vector3(transform.position.x+1.0f,transform.position.y,transform.position.z), new Vector3(transform.position.x-1.0f,transform.position.y,transform.position.z), new Vector3(transform.position.x,transform.position.y,transform.position.z+1.0f), new Vector3(transform.position.x,transform.position.y,transform.position.z-1.0f)};
        if(minEnnemy>maxEnnemy)
        {
            nbEnnemy = Random.Range(maxEnnemy,minEnnemy);
        }
        else
        {
            nbEnnemy = Random.Range(minEnnemy,maxEnnemy);
        }
        Spawn();
    }

    private void Update()
    {

    }

    void Spawn()
    {
        for(int i=0; i<nbEnnemy; i++)
        {
            Instantiate(spawnObject, spawnPoints[i], Quaternion.identity);
        }
    }
}
