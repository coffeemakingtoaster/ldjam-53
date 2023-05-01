using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpferCar : MonoBehaviour
{

    public int hitPoints = 3;
    public GameObject notes;
    public GameObject noteKill;
    private GameObject ownnote;
    private int Reward;
    private int Timer;
    System.DateTime activateTime;
    public GameObject explosionParticles;
    System.DateTime lastHitTime = System.DateTime.Now;
    bool hasBeenHit = false;
    GameGod gameGod;
    public GameObject Minimapmarker;

    private GameObject ownMiniMarker;

    private GameObject ownMarker;
    private GameObject ownMarkerDot;
    public Canvas Minimap;

    public Canvas Map;

    private int maxTime;

    private bool isDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        gameGod = GameObject.Find("GameGod").GetComponent<GameGod>();
        activateTime = System.DateTime.Now;

    }

    // Update is called once per frame
    void Update()
    {
        if ((hitPoints == 0 || transform.position.y < 14.7) && !isDestroyed)
        {
            GameObject exp = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            gameGod.finishHitmanJob(Reward);
            removeNoteGood();
            removeMarker();
            Destroy(exp, 2);
            Destroy(transform.gameObject, 2);
            isDestroyed = true;

        }

        if ((System.DateTime.Now - activateTime).TotalSeconds > maxTime && !isDestroyed)
        {

            removeMarker();
            removeNoteBad();
            Destroy(transform.gameObject);
            isDestroyed = true;

        }
        if (isDestroyed)
        {
            return;
        }
        ownMarker.transform.position = new Vector3(transform.position.x / 7.7f + 310, Minimap.transform.position.y, transform.position.z / 7.7f + 5);
        ownMiniMarker.transform.position = new Vector3(transform.position.x / 7.7f, Minimap.transform.position.y, transform.position.z / 7.7f);
        ownMarkerDot.transform.position = new Vector3(transform.position.x / 7.7f + 300, Minimap.transform.position.y, transform.position.z / 7.7f);


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && (System.DateTime.Now - lastHitTime).TotalSeconds > 1)
        {
            Debug.Log("I am hit");
            hitPoints--;
            lastHitTime = System.DateTime.Now;
            // Speed up on first hit
            if (!hasBeenHit)
            {
                GetComponent<CarController>().speed = GetComponent<CarController>().speed * 1.3f;
                hasBeenHit = true;
            }
        }
    }

    public void setValues(int reward, int timer, Canvas MM, Canvas M, GameObject MMM)
    {
        notes = GameObject.Find("Notifications");
        Reward = reward;
        Timer = timer;
        maxTime = timer;
        Minimap = MM;
        Map = M;
        Minimapmarker = MMM;
        setNotification();
        setMarker();

    }

    void setNotification()
    {

        ownnote = Instantiate(noteKill, new Vector3(141 + (notes.transform.childCount * 250), 811, 0), Quaternion.Euler(0, 0, 0), notes.transform);
        ownnote.GetComponent<Notification>().setValues(Reward, Timer, "");

    }

    void removeNoteGood()
    {
        Destroy(ownnote);
    }

    void removeNoteBad()
    {
        Destroy(ownnote);
    }

    void setMarker()
    {
        ownMiniMarker = Instantiate(Minimapmarker, new Vector3(transform.position.x / 7.7f, Minimap.transform.position.y, transform.position.z / 7.7f), Quaternion.Euler(90, 0, 0), Minimap.transform);
        ownMarkerDot = Instantiate(Minimapmarker, new Vector3(transform.position.x / 7.7f + 300, Minimap.transform.position.y, transform.position.z / 7.7f), Quaternion.Euler(90, 0, 0), Map.transform);
        ownMarker = Instantiate(noteKill, new Vector3(transform.position.x / 7.7f + 310, Minimap.transform.position.y, transform.position.z / 7.7f + 5), Quaternion.Euler(90, 0, 0), Map.transform);
        ownMarker.GetComponent<Notification>().setValues(Reward, Timer, "");
        ownMarker.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
    void removeMarker()
    {
        Destroy(ownMiniMarker);
        Destroy(ownMarker);
        Destroy(ownMarkerDot);
    }
}
