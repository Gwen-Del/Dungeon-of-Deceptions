using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int nbEnemies; 
    List<GameObject> taggedObjects;


    // Start is called before the first frame update
    void Start()
    {
        nbEnemies = 0;
        taggedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        taggedObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g=>g.tag=="enemy").ToList();
        nbEnemies = taggedObjects.Count();
        LooseCondition();
    }

    public void LooseCondition(){
        if(nbEnemies <= 2){
            SceneManager.LoadScene("winMenu", LoadSceneMode.Single);
        }
    }
}
