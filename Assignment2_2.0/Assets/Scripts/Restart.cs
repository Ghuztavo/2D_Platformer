using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    public void OnButtonClicked()
    {
        SceneManager.LoadScene("Level1");
    }
}
