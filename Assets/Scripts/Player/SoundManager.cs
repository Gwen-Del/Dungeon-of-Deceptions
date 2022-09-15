using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    List<List<GameObject>> sounds = new List<List<GameObject>>();

    List<GameObject> activeSounds = new List<GameObject>();

    void Start()
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            AudioSource ads = go.GetComponent<AudioSource>();

            if(ads != null)
            {
                bool find = false;
                int iter = 0;

                while(iter < sounds.Count && !find)
                {
                    if(sounds[iter][0].GetComponent<AudioSource>().clip == ads.clip)
                    {
                        go.GetComponent<AudioSource>().enabled = true;
                        go.SetActive(false);
                        sounds[iter].Add(go);
                        find = true;
                    }
                    iter++;
                }

                if(!find)
                {
                    sounds.Add( new List<GameObject>());
                    go.GetComponent<AudioSource>().enabled = true;
                    go.SetActive(true);
                    ads.Play();
                    sounds[iter].Add(go);
                    activeSounds.Add(go);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(List<GameObject> audiolist in sounds)
        {
            float bestDist = Vector3.Distance(audiolist[0].transform.position, transform.position);
            
            foreach(GameObject go in audiolist)
            {
                float dist = Vector3.Distance(go.transform.position, transform.position);

                if(dist < bestDist)
                {
                    bestDist = dist;

                    int iter = 0;
                    bool rep = false;

                    while(iter < activeSounds.Count && !rep)
                    {
                        GameObject goRep = activeSounds[iter];
                        if(goRep.GetComponent<AudioSource>().clip == go.GetComponent<AudioSource>().clip)
                        {
                            goRep.SetActive(false);
                            go.SetActive(true);
                            go.GetComponent<AudioSource>().Play();
                            activeSounds[iter] = go;
                            rep = true;
                        }
                        iter++;
                    }
                }
            }
        }
    }
}
