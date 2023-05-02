using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public bool isShown = false;

    public GameObject gameGod;

    public GameObject player;
    private CanvasGroup canvasGroup;

    private int activeButtonCount = 3;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.H))
        {
            isShown = !isShown;
            canvasGroup.alpha = isShown ? 1 : 0;
            canvasGroup.blocksRaycasts = isShown;
        }
        if (activeButtonCount == 0){
            canvasGroup.transform.Find("EmptyShop").transform.gameObject.SetActive(true);
        }
    }

    public void buyHonk()
    {
        if (gameGod.GetComponent<GameGod>().buySomething(0))
        {
            player.GetComponent<DriftController>().ActivateHonk();
            Debug.Log("Buying Bumpers");
             canvasGroup.transform.Find("Honk").transform.gameObject.SetActive(false);
             activeButtonCount--;
        }
    }

    public void buyJumpers()
    {
        if (gameGod.GetComponent<GameGod>().buySomething(0))
        {
            player.GetComponent<DriftController>().ActivateJumper();
            Debug.Log("Buying Jumpers");
             canvasGroup.transform.Find("Jumpers").transform.gameObject.SetActive(false);
             activeButtonCount--;
        }

    }

    public void buyJets()
    {
        if (gameGod.GetComponent<GameGod>().buySomething(0))
        {
            player.GetComponent<DriftController>().ActivateTurbine();
            Debug.Log("Buying Jumpers");
            canvasGroup.transform.Find("Jets").transform.gameObject.SetActive(false);
            activeButtonCount--;
        }
    }
}
