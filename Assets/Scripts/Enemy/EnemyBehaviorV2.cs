using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviorV2 : MonoBehaviour
{

    public Transform Player;
    public Image HealthBar;
    public GameObject Bullet;

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
    private const float MAX_HEALTH = 10f;
    public float health = MAX_HEALTH;

    // patroling
    public Vector3 walkPoint;
    public bool SetWalkPoint;
    public float walkPointRange;

    // attacking
    public bool AlreadyAttacked;
    public float TimeBetweenAttack;

    void Start()
    {
        // searching the player
        Player = GameObject.FindWithTag("Player").transform;
    }

    
    void Update()
    {
        IsVisible(); //check if the player is visible
        GetDistance(); //check the distance between the player and an enemy
        UpdateHealth(); //check the health


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

                    // the enemy sees the player
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

    // Distance between the player and enemy
    public void GetDistance()
    {
        Distance = Vector3.Distance(Player.transform.position, transform.position);
    }


/*----------------------------Chasing--------------------------------------*/

    // the enemy goes towards the player with a greater speed
    public void Chasing()
    {
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, 1.5f * Speed * Time.deltaTime);
        transform.forward = Player.transform.position - transform.position;
    }

/*----------------------------Attacking--------------------------------------*/
    
    // the enemy attacks each TimeBetweenAttack
    public void Attacking()
    { 
        transform.LookAt(Player);

        if(!AlreadyAttacked){
            Transform rb = Instantiate(Bullet, transform.position+(transform.forward*2), Quaternion.identity).transform;
            rb.Rotate(transform.localRotation.eulerAngles);

            AlreadyAttacked = true;
            Invoke(nameof(ResetAttack), TimeBetweenAttack);
        }
    }

    public void ResetAttack()
    {
        AlreadyAttacked = false;
    }

/*----------------------------Patroling--------------------------------------*/
    
    // when the player is not in the range, the enemy goes towards a random point
    public void Patroling()
    {
        if(!SetWalkPoint) SearchWalkPoint();
        else {
            transform.position = Vector3.MoveTowards(transform.position, walkPoint, 0.5f * Speed * Time.deltaTime);
            transform.forward = walkPoint - transform.position;
        }

        DistanceToWalkPoint = Vector3.Distance(transform.position, walkPoint);

        // the enemy finds its new position
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

/*----------------------------Health--------------------------------------*/
    public void UpdateHealth()
    {
        HealthBar.fillAmount = health / MAX_HEALTH;

        // it died
        if(health < 0) Destroy(gameObject);
    }

    // collision between a bullet and the enemy
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("bulletPlayer")){
            health -= 5f;
            Destroy(other);
            Debug.Log("enemy");
        }
    }
}
