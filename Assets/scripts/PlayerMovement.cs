using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float gravityMultiplier;
    [SerializeField]
    private float jumpHorizontalSpeed;
    [SerializeField]
    private float jumpButtonGracePeriod;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    TextMeshProUGUI deathCounter_TMP;


    private Transform respawnPoint;
    public Transform respawn = null;

    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject cameraOnDeath;
    public int deaths;

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    private bool isJumping;
    private bool isGrounded;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        deaths = PlayerPrefs.GetInt("Deaths");
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {

            //   Movement Input
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Movement direction
            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
            float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);


            // Walk
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                inputMagnitude /= 2;
            }

            // Movement speed
            animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);
            movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
            movementDirection.Normalize();

            float gravity = Physics.gravity.y * gravityMultiplier;

            if (isJumping && ySpeed > 0 && Input.GetButton("Jump") == false)
            {
                gravity = gravity * 2;

            }
            ySpeed += gravity * Time.deltaTime;

            //** BEGIN JUMP BUFFER **//
            if (characterController.isGrounded)
            {
                lastGroundedTime = Time.time;

            }

            if (Input.GetButtonDown("Jump"))
            {
                jumpButtonPressedTime = Time.time;
            }

            if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
            {
                characterController.stepOffset = originalStepOffset;
                ySpeed = -0.5f;
                animator.SetBool("IsGrounded", true);
                isGrounded = true;
                animator.SetBool("IsJumping", false);
                isJumping = false;
                animator.SetBool("IsFalling", false);

                if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
                {
                    ySpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);
                    animator.SetBool("IsJumping", true);
                    isJumping = true;
                    jumpButtonPressedTime = null;
                    lastGroundedTime = null;
                }
            }
            else
            {
                characterController.stepOffset = 0;
                animator.SetBool("IsGrounded", false);
                isGrounded = false;

                // controls falling animation if falling off cliff or going downhill
                if ((isJumping && ySpeed < 0) || ySpeed < -3.5)
                {
                    animator.SetBool("IsFalling", true);
                }

            }
            //** END JUMP BUFFER **//


            if (movementDirection != Vector3.zero)
            {
                animator.SetBool("IsMoving", true);

                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
            if (isGrounded == false)
            {
                Vector3 velocity = movementDirection * inputMagnitude * jumpHorizontalSpeed;
                velocity = AdjustVelocityToSlope(velocity);
                velocity.y += ySpeed;

                characterController.Move(velocity * Time.deltaTime);

            }
        }
        //fall damage/animation
        if (ySpeed <= -25 && characterController.isGrounded)
        {
            isDead = true;
            animator.Play("Falling Forward Death");
            FindObjectOfType<GameManager>().EndGame();
            gameOverScreen.SetActive(true);

            Destroy(pauseScreen, 0f);
            Destroy(cameraOnDeath, 0f);

            StartCoroutine(ExecuteAfterTime(1.10f));
        }
    }

    // death on collision with water
    void OnTriggerEnter(Collider col)
    {
        float gravity = Physics.gravity.y * gravityMultiplier;

        if (col.gameObject.tag == "Water")
        {
            isDead = true;
            animator.Play("Falling Forward Death");

            Debug.Log("hit");
            FindObjectOfType<GameManager>().EndGame();
            gameOverScreen.SetActive(true);


            Destroy(pauseScreen, 0f);
            Destroy(cameraOnDeath, 0f);

            ySpeed += gravity * Time.deltaTime;

            StartCoroutine(ExecuteAfterTime(1.10f));
            // characterController.transform.position = respawnPoint.transform.position;
            // characterController.enabled = false;

        }
    }
    // uses root motion to move character
    private void OnAnimatorMove()
    {
        if (characterController.isGrounded)
        {
            Vector3 velocity = animator.deltaPosition;
            velocity = AdjustVelocityToSlope(velocity);
            velocity.y += ySpeed * Time.deltaTime;

            characterController.Move(velocity);
        }
    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        var ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.3f))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            isGrounded = true;

            if (adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }
        return velocity;
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        yield return null;
        Destroy(gameObject, 0f);

        if (respawn != null)
        {
            transform.position = respawn.position;
            transform.rotation = Quaternion.identity;
        }

    }

    void setDeathCounter()
    {
        deathCounter_TMP.text = deaths.ToString();
    }

    // makes mouse not visible during game
    /*
        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        */

}
