using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Animator anim;
    public CharacterController controller;
    public Transform cam;
    public Transform groundCheck;

    public float groundDistance = 0.3f;
    public float speed = 12f;
    public float gravity = -12f;
    public float turnSmoothTime = 0.1f;   
    public float jumpSpeed;
    private float ySpeed;

    float turnSmoothVelocity;
    public LayerMask groundMask;

    Vector3 velocity;
    bool grounded;

    void start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // make sure that the player is on the ground
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // horizontal between -1 and 1 with A,D
        // vertical between -1 and 1 with W,S
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Direction
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // camera direction in relation to the player
        if (direction.magnitude >= 0.1f)
        {

            // Atan 2 returns the angle between the x-axis and the vector starting at 0 and terminating at x,y
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;  
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if(controller.isGrounded){
            velocity.y = -2f;
        if(Input.GetKey(KeyCode.Space)){
            velocity.y = jumpSpeed;
            anim.SetBool("isJumping", true);
        } else {
            anim.SetBool("isJumping", false);
        }
        }

    }
}
