using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float groundDrag;
    public float airDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;

    public Camera camera;
    public float cameraSize;
    public float zoomMultiplier;
    public float maxZoomSize;
    public float zoomLerpTime;

    public float playerWidth;
    public float playerHeight;
    public LayerMask groundCheckLayerMask;
    public bool grounded;

    private float horizontalInput;

    private Vector2 moveDirection;

    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        readyToJump = true;
    }

    private void Update()
    {

        GroundCheck();
        Inputs();
        SpeedControl();
        Visuals();
        CameraControl();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GroundCheck()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, playerHeight * 0.5f + 0.2f, groundCheckLayerMask) || Physics2D.Raycast(transform.position + new Vector3(playerWidth / 2, 0, 0), Vector2.down, playerHeight * 0.5f + 0.2f, groundCheckLayerMask) || Physics2D.Raycast(transform.position - new Vector3(playerWidth / 2, 0, 0), Vector2.down, playerHeight * 0.5f + 0.2f, groundCheckLayerMask))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    private void Inputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && readyToJump && grounded)
        {
            Debug.Log("JUMPED!");
            
            readyToJump = false;
            
            Jump();
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void Move()
    {
        moveDirection = Vector2.right * horizontalInput;
        
        if (grounded)
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode2D.Force);
        else
            rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode2D.Force);
    }

    private void SpeedControl()
    {
        Vector2 flatVel = new Vector2(rb.velocity.x, 0f);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector2 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector2(limitedVel.x, rb.velocity.y);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Visuals()
    {
        if (grounded)
        {
            if (horizontalInput != 0)
            {
                animator.SetBool("Moving", true);
                animator.SetBool("Jumping", false);
                animator.SetBool("Falling", false);
            }
            else
            {
                animator.SetBool("Moving", false);
                animator.SetBool("Jumping", false);
                animator.SetBool("Falling", false);
            }
        }
        else
        {
            animator.SetBool("Moving", false);

            if (rb.velocity.y > 0)
            {
                animator.SetBool("Jumping", true);
                animator.SetBool("Falling", false);
            }
            else
            {
                animator.SetBool("Jumping", false);
                animator.SetBool("Falling", true);
            }
        }

        if (horizontalInput < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else if (horizontalInput > 0)
            GetComponent<SpriteRenderer>().flipX = false;
    }

    private void CameraControl()
    {
        float targetSize = cameraSize + Mathf.Clamp(Mathf.Abs(rb.velocity.x) * zoomMultiplier, -maxZoomSize, maxZoomSize);

        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetSize, zoomLerpTime);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -(playerHeight / 2 + 0.2f), 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(playerWidth / -2, -(playerHeight / 2 + 0.2f), 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(playerWidth / 2, -(playerHeight / 2 + 0.2f), 0));
    }
}
