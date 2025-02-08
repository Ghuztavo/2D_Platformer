using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    [SerializeField] private Player1 player1;

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("platform"))
        {
            player1.LandedOnGround1();
        }
        if (collision.gameObject.CompareTag("player2"))
        {
            player1.LandedOnGround1();
        }
    }
}
