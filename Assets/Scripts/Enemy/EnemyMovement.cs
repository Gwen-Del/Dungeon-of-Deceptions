using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject playerObject;
    Transform player;

    [Range(0.0f,100.0f)]
    public float range = 30.0f;

    RaycastHit hit;
    
    void Start()
    {
        player = playerObject.transform;
    }

    void Update()
    {
        isVisible();
    }

    bool isVisible()
    {
        Vector3 playerDirection = transform.position - player.position;
        float angle = Vector3.Angle(transform.forward, playerDirection);
        float distance = Vector3.Distance(transform.position,player.position);

        if(Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270 && distance <= range)
        {
            if(Physics.Linecast(transform.position, player.position, out hit)){
                if(hit.transform.tag == "Player"){
                    Debug.DrawLine(transform.position, player.position, Color.red);
                    Debug.Log("visible");
                    //c'est ici qu'il le voit
                    return true;
                }
            }
        }

        return false;
    }
}
