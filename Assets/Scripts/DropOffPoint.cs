using UnityEngine;
using UnityEngine.UI;

public class DropOffPoint : MonoBehaviour
{
    public string addressName;

    public int Reward;

    public int dropOffLifeSpanSeconds = 600;

    bool isActive = false;

    GameGod gameGod;

    System.DateTime activateTime;

    public Canvas Minimap;

    public Canvas Map;
    
    public GameObject Minimapmarker;

    private GameObject ownMiniMarker;

    private GameObject ownMarker;

    public GameObject notes;

    public GameObject notePizza;

    private GameObject ownnote;

    private GameObject ownMarkerDot;
    void Awake()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponentInChildren<ParticleSystem>().Clear();
        gameGod = GameObject.Find("GameGod").GetComponent<GameGod>();
    }

    void Update()
    {
        // Make sure that points dont exceed their ttl
        if (isActive)
        {
            if ((System.DateTime.Now - activateTime).TotalSeconds > dropOffLifeSpanSeconds)
            {
                //Debug.Log("Timed out");
                isActive = false;
                removeNotificationBad();
                removeMarker();
                GetComponentInChildren<MeshRenderer>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
                GetComponentInChildren<ParticleSystem>().Stop();
                GetComponentInChildren<ParticleSystem>().Clear();

            }
        }
    }

    public void ActivateDropOff(int reward, int time)
    {
        Reward = reward;
        dropOffLifeSpanSeconds = time;
        Debug.Log("Activated");
        GetComponentInChildren<MeshRenderer>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponentInChildren<ParticleSystem>().Play();
        isActive = true;
        activateTime = System.DateTime.Now;
        
        setNotification();
        setMarker();
    }

    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Colliding");
        //Debug.Log(collider.gameObject.tag);
        if (collider.gameObject.tag == "Player")
        {
            removeMarker();
            removeNotificationGood();
            isActive = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
            gameGod.finishPizzaJob(Reward);
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponentInChildren<ParticleSystem>().Stop();
            GetComponentInChildren<ParticleSystem>().Clear();

        }
    }

    public bool isPointActive()
    {
        return isActive;
    }

    void setMarker(){
        ownMiniMarker = Instantiate(Minimapmarker, new Vector3(transform.position.x/7.7f,Minimap.transform.position.y,transform.position.z/7.7f), Quaternion.Euler(90, 0, 0), Minimap.transform);
        ownMarkerDot = Instantiate(Minimapmarker, new Vector3(transform.position.x/7.7f+300,Minimap.transform.position.y,transform.position.z/7.7f), Quaternion.Euler(90, 0, 0), Map.transform);
        ownMarker = Instantiate(notePizza, new Vector3(transform.position.x/7.7f+310,Minimap.transform.position.y,transform.position.z/7.7f+5), Quaternion.Euler(90, 0, 0), Map.transform);
        ownMarker.GetComponent<Notification>().setValues(Reward,dropOffLifeSpanSeconds,addressName);
        ownMarker.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
    }   

    void removeMarker(){
        Destroy(ownMiniMarker);
        Destroy(ownMarker);
        Destroy(ownMarkerDot);
    }

    void setNotification() {
    
    ownnote = Instantiate(notePizza, new Vector3(141 + (notes.transform.childCount * 250), 811, 0), Quaternion.Euler(0, 0, 0), notes.transform);
    ownnote.GetComponent<Notification>().setValues(Reward,dropOffLifeSpanSeconds,addressName);
    
    }

    void removeNotificationGood(){
        Destroy(ownnote);
    }

    void removeNotificationBad(){
        Destroy(ownnote);
    }

}
