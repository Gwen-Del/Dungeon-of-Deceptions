using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public GameObject weapon;

    public float health;

    // vision
    [Range(0.0f,100.0f)]
    public float range = 30.0f;
    RaycastHit hit;
    
    //patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking
    public float timeBetweenAttack;
    bool alreadyAttacked;

    //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool isPlayer;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
    }

    void Update()
    {

        playerInSightRange = Vector3.Distance(player.transform.position, transform.position) < sightRange;
        playerInAttackRange = Vector3.Distance(player.transform.position, transform.position) < attackRange;


        if(!playerInSightRange && !playerInAttackRange) Patrolling();
       // if(playerInSightRange && !playerInAttackRange) Chasing();
       // if(playerInSightRange && playerInAttackRange) Attacking();
    }

    private void Patrolling()
    {
        if(!walkPointSet) SearchWalkPoint();  

        if(walkPointSet)
            enemy.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
   
    }

    private void Chasing()
    {
        enemy.SetDestination(player.position);
    }

    private void Attacking()
    {
        enemy.SetDestination(transform.position);
        transform.LookAt(player);

        if(!alreadyAttacked){

            // projectiles
            Rigidbody rb = Instantiate(weapon, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttack);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0) 
            Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void IsVisible()
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
                    isPlayer = true;
                }
            }
        }

        isPlayer = false;
    }
}
