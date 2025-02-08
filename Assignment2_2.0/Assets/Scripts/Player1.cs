using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player1 : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float defaultGravity = 3;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;

    private bool canJump = true;
    private bool isClimbing = false;
    private bool onLadder = false;
    private float horizontalMovement;

    private float deathDelay;


    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("Direction", 1.0f);
        animator.SetBool("Climbing", false);
    }

    void Update()
    {
        deathDelay -= Time.deltaTime;

        if (GameManager1.instance.nextStage)
        {
            SpawnOnCP();

        }

        if (!onLadder && !isClimbing)
        {
            if (Input.GetKeyDown(KeyCode.W) && canJump)
            {
                rb.AddForce(new Vector2(0, jumpForce));
                canJump = false;
                animator.SetBool("IsJumping", true);
            }
        }

        horizontalMovement = Input.GetAxis("Horizontal_P1") * speed;

        if (!onLadder)
        {
            isClimbing = false;
            if (horizontalMovement < 0)
            {
                animator.SetFloat("Direction", -1.0f);
            }
            else if (horizontalMovement > 0)
            {
                animator.SetFloat("Direction", 1.0f);
            }
        }
        if (onLadder)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                isClimbing = true;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                isClimbing = true;
            }
        }

        if (isClimbing)
        {
            //disable gravity
            rb.gravityScale = 0;
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(0, 1 * speed * Time.deltaTime, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(0, -1 * speed * Time.deltaTime, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }
        }
        else
        {
            //enable gravity
            rb.gravityScale = defaultGravity;
        }


        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
        animator.SetBool("IsFalling", (rb.linearVelocityY < -0.1f));
        animator.SetBool("Climbing", isClimbing);

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalMovement, rb.linearVelocityY);
    }

    public void LandedOnGround1()
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);
        canJump = true;
    }

    public void SpawnOnCP()
    {
        Vector2 respawnPos = GameManager1.instance.GetCheckPointPos();
        respawnPos.x -= 1;
        transform.position = respawnPos;
    }

    //collision of the player one with the cherry
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.CompareTag("cherry"))
        {
            GameManager1.instance.CherryCollected();
            Destroy(collision.gameObject);
        }

        //collision of the player with dead zone
        if (collision.gameObject.CompareTag("deadZone")
            || collision.gameObject.CompareTag("enemy")
            || collision.gameObject.CompareTag("spikes"))
        {

            if(deathDelay <= 0)
            {
                Debug.Log("dead");
                SpawnOnCP();
                GameManager1.instance.DecreaseLives(1);
                deathDelay = 0.1f;
            }
            
            
        }

        if (collision.gameObject.CompareTag("Ladder"))
        {
            onLadder = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("destination"))
        {
            GameManager1.instance.player1OnDestination = true;
        }

        if (collision.gameObject.CompareTag("platform"))
        {
            LandedOnGround1();
        }
        if (collision.gameObject.CompareTag("player2"))
        {
            LandedOnGround1();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            onLadder = false;
            animator.SetBool("Climbing", false);
            isClimbing = false;
        }
    }


}