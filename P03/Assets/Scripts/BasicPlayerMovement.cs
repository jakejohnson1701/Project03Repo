using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public float jumpHeight = 3f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;
    public ParticleSystem muzzleFlash;
    public AudioClip shootSound;
    public Camera fpsCam;
    public float range = 100f;

    // Update is called once per frame
    void Update()
    {
       
            Move();
            Fire();
        
    }

    public void Move()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        //if we hold left shift, activate sprinting by changing the move speed, whenever left shift is not pressed, reset to default
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 18f;
        }
        else
        {
            speed = 5f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    public void Fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            muzzleFlash.Play();
            AudioHelper.PlayClip2D(shootSound, 1);
            Shoot();
        }
    }

    //when called this function fires a raycast in the direction of the camera 
    //when another object is hit the function checks if the hit object was an enemy
    //if so, the player ability activation sound is played and the player ability is applied to the enemy (stun fied that disables enemy
    //the players score is increased by 10 for every enemy they stun
    
    public void Shoot()
    {
        RaycastHit hit;
       if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            BasicEnemyBehavior basicEnemyBehavior = hit.transform.GetComponent<BasicEnemyBehavior>();

            if(basicEnemyBehavior != null)
            {
                basicEnemyBehavior.Kill();
           
            }
        }
    }

    
}
