using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void quit(){
        Application.Quit();
        Debug.Log("Quit");
    }

    public bool isShown = false;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isShown = !isShown;
            canvasGroup.alpha = isShown ? 1 : 0;
            canvasGroup.blocksRaycasts = isShown;
        }
      
    }
}
