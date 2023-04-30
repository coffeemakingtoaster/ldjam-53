using System.Collections.Generic;
using UnityEngine;

class GameState
{
    public int money;
}




public class GameGod : MonoBehaviour
{

    public int MinimumSecondsBetweenPizzaJobs = 30;
    public int MinimumSecondsBetweenHitManJobs = 60;

    public int PizzaDeliveryReward = 100;

    public int HitManJobReward = 200;

    int CurrentPizzaSpawnOffset = 0;

    int CurrentHitmanSpawnOffset = 0;

    GameState gameState = new GameState();
    System.DateTime lastSpawnedPizzaJob = System.DateTime.Now;
    System.DateTime lastSpawnedHitmanJob = System.DateTime.Now;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if ((System.DateTime.Now - lastSpawnedPizzaJob).Seconds > (MinimumSecondsBetweenPizzaJobs + CurrentPizzaSpawnOffset))
        {
            //Debug.Log("Now is the chance to spawn new Pizza Jobs");
            int rand = Random.Range(0, 1000);
            if (rand == 69)
            {
                List<GameObject> PizzaLocations = new List<GameObject>();
                foreach (Transform t in GameObject.Find("DropOffPoints").transform)
                {
                    // Ignore already active
                    if (!t.gameObject.GetComponent<DropOffPoint>().isPointActive())
                    {
                        PizzaLocations.Add(t.gameObject);
                    }
                }
                GameObject ActivePizzaLocation = PizzaLocations[Random.Range(0, PizzaLocations.Count)];
                ActivePizzaLocation.GetComponent<DropOffPoint>().ActivateDropOff();
                lastSpawnedPizzaJob = System.DateTime.Now;
                CurrentPizzaSpawnOffset = Random.Range(0, (MinimumSecondsBetweenPizzaJobs / 10));
            }
        }

        if ((System.DateTime.Now - lastSpawnedHitmanJob).Seconds > (MinimumSecondsBetweenHitManJobs + CurrentHitmanSpawnOffset))
        {
            //Debug.Log("Now is the chance to spawn new Hitman Jobs");
            int rand = Random.Range(0, 1000);
            if (rand == 69)
            {
                List<GameObject> HitManSpawners = new List<GameObject>();
                foreach (Transform t in GameObject.Find("OpferMutters").transform)
                {
                    HitManSpawners.Add(t.gameObject);
                }
                HitManSpawners[Random.Range(0, HitManSpawners.Count)].GetComponent<SpawnController>().SpawnObject();
                lastSpawnedHitmanJob = System.DateTime.Now;
                CurrentHitmanSpawnOffset = Random.Range(0, (MinimumSecondsBetweenPizzaJobs / 10));
            }
        }
    }

    public void finishPizzaJob()
    {
        gameState.money += PizzaDeliveryReward;
        Debug.Log("We rich now");
    }

    public void finishHitmanJob()
    {
        gameState.money += HitManJobReward;
        Debug.Log("We even richer now");
    }
}
