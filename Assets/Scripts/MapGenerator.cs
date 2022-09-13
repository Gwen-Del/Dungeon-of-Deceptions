using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject spawnBase;
    public GameObject player;

    void Start()
    {
        int x = Random.Range(-4,4);
        int z = Random.Range(-4,4);

        Instantiate(spawnBase, new Vector3(x*20,1,z*20), Quaternion.identity);
        Instantiate(player, new Vector3(x*20,1,z*20), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
