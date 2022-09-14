using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject spawnBasePrefab;
    public GameObject playerPrefab;
    public GameObject basicWallPrefab;

    public GameObject[] prefabsMaze;
    [Range(0,30)]
    public int percentSpanwer = 30;
    public GameObject[] prefabsSpawner;
    [Range(0,30)]
    public int percentLoot = 30;
    public GameObject[] prefabsLoot;

    [Range(1,20)]
    public int nbSalleMin = 5;

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

        if(prefabsSpawner.Length-1 >= 0)
        {
            int SalleX = Random.Range(-4,5);
            int SalleZ = Random.Range(-4,5);

            while(SalleX == BaseX && SalleZ == BaseZ)
            {
                SalleX = Random.Range(-4,5);
                SalleZ = Random.Range(-4,5);
            }

            Instantiate(prefabsSpawner[Random.Range(0,prefabsSpawner.Length)], new Vector3(SalleX*20-10,0,SalleZ*20-10), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
