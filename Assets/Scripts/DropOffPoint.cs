using UnityEngine;

public class DropOffPoint : MonoBehaviour
{
    public string addressName;

    public int dropOffLifeSpanSeconds = 600;

    bool isActive = false;

    GameGod gameGod;

    System.DateTime activateTime;

    void Awake()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        gameGod = GameObject.Find("GameGod").GetComponent<GameGod>();
    }

    void Update()
    {
        // Make sure that points dont exceed their ttl
        if (isActive)
        {
            if ((System.DateTime.Now - activateTime).Seconds > dropOffLifeSpanSeconds)
            {
                Debug.Log("Timed out");
                isActive = false;
                GetComponentInChildren<MeshRenderer>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
            }
        }
    }

    public void ActivateDropOff()
    {
        Debug.Log("Activated");
        GetComponentInChildren<MeshRenderer>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        isActive = true;
        activateTime = System.DateTime.Now;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            isActive = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
            gameGod.finishPizzaJob();
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    public bool isPointActive()
    {
        return isActive;
    }

}
