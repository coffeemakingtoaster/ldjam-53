using UnityEngine;

public class DropOffPoint : MonoBehaviour
{
    public string addressName;

    public int dropOffLifeSpanSeconds = 600;

    bool isActive = false;

    GameGod gameGod;

    System.DateTime activateTime;

    public Canvas Minimap;
    
    public GameObject Minimapmarker;

    private GameObject ownMarker;

    public GameObject notes;

    public GameObject notePizza;
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
        setMarker();
        setNotification();
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Colliding");
        Debug.Log(collider.gameObject.tag);
        if (collider.gameObject.tag == "Player")
        {
            removeMarker();
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

    void setMarker(){
        ownMarker = Instantiate(Minimapmarker, new Vector3(transform.position.x/7.7f,Minimap.transform.position.y,transform.position.z/7.7f), Quaternion.Euler(90, 0, 0), Minimap.transform);

    }   

    void removeMarker(){
        Destroy(ownMarker);
    }

    void setNotification(){
        print(notes.transform.childCount);
        Instantiate(notePizza, new Vector3(141+(notes.transform.childCount*250),780,0),Quaternion.Euler(0, 0, 0), notes.transform);
    }

}
