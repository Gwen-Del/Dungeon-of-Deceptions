using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject spawnBasePrefab;
    public GameObject playerPrefab;
    public GameObject basicWallPrefab;
    public GameObject[] prefabs;

    void Start()
    {
        int BaseX = Random.Range(-4,5);
        int BaseZ = Random.Range(-4,5);

        Instantiate(spawnBasePrefab, new Vector3(BaseX*20-10,0,BaseZ*20-10), Quaternion.identity);
        GameObject player = Instantiate(playerPrefab, new Vector3(BaseX*20-10,0,BaseZ*20-10), Quaternion.identity);

        if(BaseX == -4)
        {
            Instantiate(basicWallPrefab, new Vector3(BaseX*20-20,2,BaseZ*20-10), Quaternion.identity).transform.Rotate(0.0f,90.0f,0.0f);
        }
        else if(BaseX == 5)
        {
            Instantiate(basicWallPrefab, new Vector3(BaseX*20,2,BaseZ*20-10), Quaternion.identity).transform.Rotate(0.0f,90.0f,0.0f);
        }
        if(BaseZ == -4)
        {
            Instantiate(basicWallPrefab, new Vector3(BaseX*20-10,2,BaseZ*20-20), Quaternion.identity);
        }
        else if(BaseZ == 5)
        {
            Instantiate(basicWallPrefab, new Vector3(BaseX*20-10,2,BaseZ*20), Quaternion.identity);
        }

        if(prefabs.Length-1 >= 0)
        {
            int SalleX = Random.Range(-4,5);
            int SalleZ = Random.Range(-4,5);

            while(SalleX == BaseX && SalleZ == BaseZ)
            {
                SalleX = Random.Range(-4,5);
                SalleZ = Random.Range(-4,5);
            }

            Instantiate(prefabs[Random.Range(0,prefabs.Length)], new Vector3(SalleX*20-10,0,SalleZ*20-10), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
