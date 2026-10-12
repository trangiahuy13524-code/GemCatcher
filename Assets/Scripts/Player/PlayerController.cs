using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    [SerializeField] float currentVelocity = 1f;
    [SerializeField] BoxCollider2D groundDetectCollider;
    [SerializeField] Key dashKey = Key.LeftCtrl;
    [SerializeField] float dashSpeedMultiplier = 3f;
    [SerializeField] float dashDuration = 3f;
    public float walkSpeed = 1.2f;
    public float runSpeed = 3f;
    public float jumpForce = 20f;
    [SerializeField] bool isGrounded = false;
    bool isDashing;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void InGround()
    {
        isGrounded = true;
        animator.SetBool("grounded", true);
    }

    public void OffGround()
    {
        isGrounded = false;
        animator.SetBool("grounded", false);
    }

    public void Move(bool run = false, Direction dir = Direction.Left)
    {
        if (!isGrounded) return;
        float movementVelocity = isDashing ? currentVelocity * dashSpeedMultiplier : currentVelocity;
        if (dir == Direction.Right)
        {
            rb.linearVelocityX = movementVelocity;
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            rb.linearVelocityX = -movementVelocity;
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }

        animator.SetBool("walking", true);
        if (run)
        {
            animator.SetBool("running", true);
            currentVelocity = runSpeed;
        }
        else
        {
            animator.SetBool("running", false);
            currentVelocity = walkSpeed;
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocityY = jumpForce;
            animator.SetTrigger("Jump");
        }
    }

    public void Dash()
    {
        if (isDashing) return;

        StartCoroutine(DashProcess());
    }

    IEnumerator DashProcess()
    {
        isDashing = true;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }

    void StopWalking()
    {
        animator.SetBool("walking", false);
    }

    void AnimProcess()
    {
        
        bool falling = animator.GetBool("falling");
        if (falling) return;
        float vel = rb.linearVelocityY;
        if (vel < -0.5f)
        {
            animator.SetTrigger("Fall");
            animator.SetBool("falling", true);
        }
    }

    private void Update()
    {
        if (Keyboard.current[dashKey].wasPressedThisFrame)
        {
            Dash();
        }

        bool aPressed = Keyboard.current.aKey.isPressed;
        bool dPressed = Keyboard.current.dKey.isPressed;
        bool shiftPressed = Keyboard.current.leftShiftKey.isPressed;
        if (aPressed)
        {
            if (shiftPressed)
            {
                Move(true, Direction.Left);
            }
            else
            {
                Move(false, Direction.Left);
            }
        }
        if (dPressed)
        {
            if (shiftPressed)
            {
                Move(true, Direction.Right);
            }
            else
            {
                Move(false, Direction.Right);
            }
        }
        else
        {
            if (!aPressed)
            {
                StopWalking();
            }
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump();
        }

        AnimProcess();
    }

    
}
