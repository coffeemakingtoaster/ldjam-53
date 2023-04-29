using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{


    public void loadMainGame()
    {
        SceneManager.LoadScene(1);
    }

    public void openSettings()
    {
        Debug.Log("Not implemented yet");
    }

    public void exitGame()
    {
        Application.Quit();
    }

}
