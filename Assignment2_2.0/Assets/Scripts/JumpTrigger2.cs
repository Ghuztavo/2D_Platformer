using UnityEngine;

public class JumpTrigger2 : MonoBehaviour
{
    [SerializeField] private Player2 player2;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("platform"))
        {
            player2.LandedOnGround2();
        }
        if (collision.gameObject.CompareTag("player1"))
        {
            player2.LandedOnGround2();
        }
    }
}
