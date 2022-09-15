using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorV2 : MonoBehaviour
{

    public Transform Player;

    // 
    public float Speed = 1.5f;
    public float Distance, CloseDistance, DistanceToWalkPoint;

    //
    public float SightRange, AttackRange;
    public bool InSightRange, InDistanceRange, InAttackRange;
    
    // Player recognition
    [Range(0.0f,100.0f)]
    public float range = 30.0f;
    RaycastHit hit;

    // enemy's stat
    public float health;

    // patroling
    public Vector3 walkPoint;
    public bool SetWalkPoint;
    public float walkPointRange;

    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
    }

    
    void Update()
    {
        IsVisible();
        GetDistance();

        InDistanceRange = (Distance < SightRange) && (Distance > AttackRange);
        InAttackRange = (Distance < AttackRange) && (Distance > CloseDistance);

        if(InAttackRange && InSightRange) Attacking();
        else if(InDistanceRange && InSightRange) Chasing();
        else Patroling();
    }

    public void IsVisible()
    {
        Vector3 PlayerDirection = transform.position - Player.position;
        float angle = Vector3.Angle(transform.forward, PlayerDirection);
        float distance = Vector3.Distance(transform.position,Player.position);

        if(Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270 && distance <= range)
        {
            if(Physics.Linecast(transform.position, Player.position, out hit)){
                if(hit.transform.tag == "Player"){
                    Debug.DrawLine(transform.position, Player.position, Color.red);
                    // Debug.Log("visible");
                    //c'est ici qu'il le voit
                    InSightRange = true;

                }else{
                    InSightRange = false;
                }
            } else {
                    InSightRange = false;
            }
        } else {
                InSightRange = false;
        }
    }

    public void GetDistance()
    {
        Distance = Vector3.Distance(Player.transform.position, transform.position);
    }


/*----------------------------Chasing--------------------------------------*/

    public void Chasing()
    {
        Debug.Log("Chasing");
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Speed * Time.deltaTime);
        transform.forward = Player.transform.position - transform.position;
    }

/*----------------------------Attacking--------------------------------------*/
    public void Attacking()
    {
        Debug.Log("attack");  
    }

/*----------------------------Patroling--------------------------------------*/
    public void Patroling()
    {
        Debug.Log("patroling");
        if(!SetWalkPoint) SearchWalkPoint();
        else {
            transform.position = Vector3.MoveTowards(transform.position, walkPoint, 0.5f * Speed * Time.deltaTime);
            transform.forward = walkPoint - transform.position;
        }

        DistanceToWalkPoint = Vector3.Distance(transform.position, walkPoint);

        if(DistanceToWalkPoint < 0.5)
            SetWalkPoint = false;
    }

    public void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        SetWalkPoint = true;
    }
}
