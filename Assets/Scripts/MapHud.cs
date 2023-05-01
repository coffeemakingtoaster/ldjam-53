using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapHud : MonoBehaviour
{
  public bool isShown = false;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.M))
        {
            isShown = !isShown;
            canvasGroup.alpha = isShown ? 1 : 0;
            canvasGroup.blocksRaycasts = isShown;
        }
    }
}
