using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player2 : MonoBehaviour
{
    [SerializeField] private float speed2;
    [SerializeField] private float jumpForce2;
    [SerializeField] private float defaultGravity2 = 3;

    [SerializeField] private Rigidbody2D rb2;
    [SerializeField] private SpriteRenderer sprite2;
    [SerializeField] private Animator animator2;

    private bool canJump2 = true;
    private bool isClimbing2 = false;
    private bool onLadder2 = false;
    private float horizontalMovement2;

    private float deathDelay2;

    private void Start()
    {
        animator2 = GetComponent<Animator>();
        animator2.SetFloat("Direction", 1.0f);
        animator2.SetBool("Climbing", false);
    }

    // Update is called once per frame
    void Update()
    {

        deathDelay2 -= Time.deltaTime;
        if (GameManager1.instance.nextStage)
        {
            SpawnOnCP();

        }

        if (!onLadder2 && !isClimbing2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && canJump2)
            {
                rb2.AddForce(new Vector2(0, jumpForce2));
                canJump2 = false;
                animator2.SetBool("IsJumping", true);
            }
        }


        horizontalMovement2 = Input.GetAxis("Horizontal_P2") * speed2;

        if (!onLadder2)
        {
            isClimbing2 = false;
            if (horizontalMovement2 < 0)
            {
                animator2.SetFloat("Direction", -1.0f);
            }
            else if (horizontalMovement2 > 0)
            {
                animator2.SetFloat("Direction", 1.0f);
            }
        }
        if (onLadder2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isClimbing2 = true;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                isClimbing2 = true;
            }
        }

        if (isClimbing2)
        {
            //disable gravity
            rb2.gravityScale = 0;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += new Vector3(0, 1 * speed2 * Time.deltaTime, 0);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += new Vector3(0, -1 * speed2 * Time.deltaTime, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }
        }
        else
        {
            //enable gravity
            rb2.gravityScale = defaultGravity2;
        }

        animator2.SetFloat("Speed", Mathf.Abs(horizontalMovement2));
        animator2.SetBool("IsFalling", (rb2.linearVelocityY < -0.1f));
        animator2.SetBool("Climbing", isClimbing2);

    }

    private void FixedUpdate()
    {
        rb2.linearVelocity = new Vector2(horizontalMovement2, rb2.linearVelocityY);
    }

    public void LandedOnGround2()
    {
        animator2.SetBool("IsJumping", false);
        animator2.SetBool("IsFalling", false);
        canJump2 = true;
    }
    public void SpawnOnCP()
    {
        Vector2 respawnPos = GameManager1.instance.GetCheckPointPos();
        respawnPos.x += 1;
        transform.position = respawnPos;
    }

    //collision of the player two with the gem
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("gem"))
        {
            GameManager1.instance.GemCollected();
            Destroy(collision.gameObject);
        }

        //collision of the player with dead zone
        if (collision.CompareTag("deadZone")
            || collision.CompareTag("enemy")
            || collision.CompareTag("spikes"))
        {
            if (deathDelay2 <= 0)
            {
                Debug.Log("dead");
                SpawnOnCP();
                GameManager1.instance.DecreaseLives(1);
                deathDelay2 = 0.1f;
            }
        }

        if (collision.CompareTag("Ladder"))
        {
            onLadder2 = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("destination"))
        {
            GameManager1.instance.player2OnDestination = true;
        }

        if (collision.gameObject.CompareTag("platform"))
        {
            LandedOnGround2();
        }
        if (collision.gameObject.CompareTag("player1"))
        {
            LandedOnGround2();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            onLadder2 = false;
            animator2.SetBool("Climbing", false);
            isClimbing2 = false;
        }
    }
}
