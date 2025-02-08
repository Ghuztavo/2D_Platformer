using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 instance;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject CP1;
    [SerializeField] private GameObject CP2;
    private GameObject currentCP;


    [SerializeField] private int lives;    
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private Image gemImage;
    [SerializeField] private Image cherryImage;
    public int stage;
    private float cameraXOnStage2 = 25.0f;
    public bool gemCollected;
    public bool cherryCollected;
    public bool doorOpened;
    private bool GameOver;
    public bool player1OnDestination;
    public bool player2OnDestination;
    public bool nextStage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            SetLives();
        }
        else
        {
            Destroy(gameObject);
        }
        this.stage = 1;
        this.lives = 5;
        this.stage = 1;
        this.gemCollected = false;
        this.cherryCollected = false;
        this.doorOpened = false;
        this.currentCP = CP1;
        this.nextStage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (lives <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }

        if (cherryCollected)
        {
            CherryCollected();
        }
        if (gemCollected)
        {
            GemCollected();
        }
        if (gemCollected && cherryCollected)
        {
            doorOpened = true;
        }

        if (player1OnDestination && player2OnDestination)
        {
            if (stage == 2)
            {
                //change the scene to the "Ending" scene
                SceneManager.LoadScene("Ending");
            }
            else
            {
                nextStage = true;
            }
        }
        if (nextStage)
        {
            gemCollected = false;
            cherryCollected = false;
            doorOpened = false;
            
            currentCP = CP2;
            nextStage = true;
            player1OnDestination = false;
            player2OnDestination = false;
            
            ResetCherry();
            ResetGem();
            
            //move the camera to the right
            if (mainCamera.transform.position.x < cameraXOnStage2)
            {
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + 25f * Time.deltaTime, mainCamera.transform.position.y, mainCamera.transform.position.z);
            }
            else
            {
                nextStage = false;
            }
            stage = 2;
        }

    }

    public void DecreaseLives(int amount)
    {
        this.lives -= amount;
        livesText.text = "Lives: " + lives;
    }

    public void SetLives()
    {
        lives = 5;
        livesText.text = "Lives: " + lives;

    }

    public void CherryCollected()
    {
        cherryImage.color = new Color(1, 1, 1, 1);
        cherryCollected = true;
    }
    public void GemCollected()
    {
        gemImage.color = new Color(1, 1, 1, 1);
        gemCollected = true;
    }
    public void ResetCherry()
    {
        cherryImage.color = new Color(1, 1, 1, 0.3921569f);
        cherryCollected = false;
    }
    public void ResetGem()
    {
        gemImage.color = new Color(1, 1, 1, 0.3921569f);
        gemCollected = false;
    }

    public Vector2 GetCheckPointPos()
    {
        return currentCP.transform.position;
    }

}
