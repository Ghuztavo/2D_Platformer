using UnityEngine;

public class Eagle : MonoBehaviour
{
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float speed = 1;
    [SerializeField] private Animator animator;


    // Update is called once per frame
    void Update()
    {
        // Move the eagle left and right
        if (transform.position.x > maxX || transform.position.x < minX)
        {
            speed *= -1;
        }

        animator.SetFloat("Direction", speed);
        transform.Translate(Vector2.right * speed * Time.deltaTime);

    }
}
