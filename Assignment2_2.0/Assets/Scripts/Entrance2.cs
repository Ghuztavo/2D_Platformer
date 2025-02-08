using UnityEngine;

public class Entrance2 : MonoBehaviour
{
    [SerializeField] private int type;
    private Vector2 originalPosition;
    private bool isMoving;
    private float speed = 2.0f;
    private float minYdistance = 5.0f;
    private float maxYdistance = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        BlocksStatus();
        if (GameManager1.instance.stage == 2 && type == 2)
        {
            if (isMoving)
            {
                float targetY = originalPosition.y - minYdistance;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, targetY), speed * Time.deltaTime);

                // Stop moving once it reaches the target position
                if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
                {
                    isMoving = false;
                }
            }
        }
    }

    public void BlocksStatus()
    {
        if (type == 2)
        {
            if (GameManager1.instance.doorOpened && GameManager1.instance.stage == 2)
            {
                Debug.Log("Blocks2 Moving");
                isMoving = true;
            }
        }
    }
}
