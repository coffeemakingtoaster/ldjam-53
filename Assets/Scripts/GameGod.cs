using System.Collections.Generic;
using TMPro;
using UnityEngine;

class GameState
{
    public int money;
}




public class GameGod : MonoBehaviour
{


    public AudioSource newjobSound;

    public AudioSource successAudio;

    public AudioSource failure;
    public int MinimumSecondsBetweenPizzaJobs = 30;
    public int MinimumSecondsBetweenHitManJobs = 60;

    public int HitmanRange = 20;

    public int PizzaRange = 20;

    public int PizzaDeliveryRewardMin = 15;
    public int PizzaDeliveryRewardMax = 50;

    public int PizzaTimer = 40;

    public int HitmanTimer = 60;

    public int HitManJobRewardMin = 150;
    public int HitManJobRewardMax = 300;

    public int CurrentPizzaSpawnOffset = 10;

    public int CurrentHitmanSpawnOffset = 30;

    public int maximumPizzaJobs = 4;

    public int minimumPizzaJobs = 1;

    private int currentPizzaJobCount = 0;

    public int maximumHitmanJobs = 1;

    public int minimumHitmanJobs = 0;

    private int currentHitmanJobCount = 0;

    public GameObject moneydisplay;

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
        if (((System.DateTime.Now - lastSpawnedPizzaJob).TotalSeconds > (CurrentPizzaSpawnOffset) && currentPizzaJobCount < maximumPizzaJobs) || currentPizzaJobCount < minimumPizzaJobs)
        {
            //Debug.Log("Now is the chance to spawn new Pizza Jobs");

            List<GameObject> PizzaLocations = new List<GameObject>();
            foreach (Transform t in GameObject.Find("DropOffPoints").transform)
            {
                if (!newjobSound.isPlaying)
                {
                newjobSound.Play();
                }
                // Ignore already active
                if (!t.gameObject.GetComponent<DropOffPoint>().isPointActive())
                {
                    PizzaLocations.Add(t.gameObject);
                }
            }
            GameObject ActivePizzaLocation = PizzaLocations[Random.Range(0, PizzaLocations.Count)];
            //TODO: distance to player in pizzatimer calculaten
            ActivePizzaLocation.GetComponent<DropOffPoint>().ActivateDropOff(Random.Range(PizzaDeliveryRewardMin, PizzaDeliveryRewardMax + 1), PizzaTimer + Random.Range(-10, 20));
            lastSpawnedPizzaJob = System.DateTime.Now;

            CurrentPizzaSpawnOffset = Random.Range(MinimumSecondsBetweenPizzaJobs, MinimumSecondsBetweenPizzaJobs + PizzaRange);

            currentPizzaJobCount++;

        }

        if (((System.DateTime.Now - lastSpawnedHitmanJob).TotalSeconds > (CurrentHitmanSpawnOffset) && currentHitmanJobCount < maximumHitmanJobs) || currentHitmanJobCount < minimumHitmanJobs)
        {
            //Debug.Log("Now is the chance to spawn new Hitman Jobs");

            Debug.Log("Creating Opfer");
             if (!newjobSound.isPlaying && !failure.isPlaying && !successAudio.isPlaying)
            {
            newjobSound.Play();
            }
            List<GameObject> HitManSpawners = new List<GameObject>();
            foreach (Transform t in GameObject.Find("OpferMutters").transform)
            {
                HitManSpawners.Add(t.gameObject);
            }
            HitManSpawners[Random.Range(0, HitManSpawners.Count)].GetComponent<SpawnOpferController>().SpawnOpfer(Random.Range(HitManJobRewardMin, HitManJobRewardMax + 1), HitmanTimer + Random.Range(-10, 20));
            lastSpawnedHitmanJob = System.DateTime.Now;
            CurrentHitmanSpawnOffset = Random.Range(MinimumSecondsBetweenHitManJobs, MinimumSecondsBetweenHitManJobs + HitmanRange);
            currentHitmanJobCount++;
        }
    }

    public void finishPizzaJob(int reward, bool success = true)
    {
        if (!success)
        {   
            failure.Play();
            currentPizzaJobCount--;
            return;
        }
        successAudio.Play();
        gameState.money += reward;
        moneydisplay.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = (gameState.money.ToString() + "$");
        Debug.Log("We rich now");
        currentPizzaJobCount--;
    }

    public void finishHitmanJob(int reward, bool success = true)
    {
        if (!success)
        {
            failure.Play();
            currentHitmanJobCount--;
            return;
        }
        successAudio.Play();
        gameState.money += reward;
        moneydisplay.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = (gameState.money.ToString() + "$");
        Debug.Log("We even richer now");
        currentHitmanJobCount--;
    }

    public bool buySomething(int cost)
    {
        if (cost <= gameState.money)
        {
            Debug.Log("Hey");
            gameState.money -= cost;
            moneydisplay.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = (gameState.money.ToString() + "$");
            return true;
        }
        return false;
    }

}
