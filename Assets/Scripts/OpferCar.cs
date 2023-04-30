using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpferCar : MonoBehaviour
{

    public int hitPoints = 3;

    public GameObject explosionParticles;

    System.DateTime lastHitTime = System.DateTime.Now;

    GameGod gameGod;

    // Start is called before the first frame update
    void Start()
    {
        gameGod = GameObject.Find("GameGod").GetComponent<GameGod>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hitPoints == 0 || transform.position.y < 14.7)
        {
            GameObject exp = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            gameGod.finishHitmanJob();
            Destroy(exp, 2);
            Destroy(transform.gameObject, 2);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && (System.DateTime.Now - lastHitTime).Seconds > 1)
        {
            Debug.Log("I am hit");
            hitPoints--;
            lastHitTime = System.DateTime.Now;
        }
    }
}
