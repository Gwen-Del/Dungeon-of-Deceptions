using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    public Image HealthBar;
    public GameObject Bullet;

    public float speed = 6.0f;
    public float gravity = 1000.0f;

    private Vector3 moveDirection = Vector3.zero;

    private const float MAX_HEALTH = 10f;
    public float health = MAX_HEALTH;

    public float TimeBetweenAttack; 
    public bool AlreadyAttacked; 

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
         UpdateHealth();

        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

        if (Input.GetButton("Fire1"))
        {
            Attack();
        }
    }

    public void UpdateHealth()
    {
        HealthBar.fillAmount = health / MAX_HEALTH;

        if(health <= 0){
           SceneManager.LoadScene("deathMenu", LoadSceneMode.Single);
        }
    }

    public void Attack()
    {
        if(!AlreadyAttacked){

            Transform rb = Instantiate(Bullet, transform.position+(transform.forward*1.5f), Quaternion.identity).transform;
            rb.Rotate(transform.localRotation.eulerAngles);

            AlreadyAttacked = true;
            Invoke(nameof(ResetAttack), TimeBetweenAttack);
        }
    }

    public void ResetAttack()
    {
        AlreadyAttacked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("bulletEnemy")){
            health -= 0.5f;
            Destroy(other);
            Debug.Log("player");
        }
    }
}
