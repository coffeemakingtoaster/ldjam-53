using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMarkers : MonoBehaviour
{

    private List<GameObject> InactiveLocations = new List<GameObject>();

    private List<GameObject> ActiveLocations = new List<GameObject>();

    private List<GameObject> ToRemove = new List<GameObject>();

    public GameObject marker;
    // Start is called before the first frame update
    void Start()
    {
        
        foreach (Transform t in GameObject.Find("DropOffPoints").transform)
                {
                
                InactiveLocations.Add(t.gameObject);

                }

        
    }

    // Update is called once per frame
    void Update()
    {

        foreach (GameObject location in InactiveLocations){
            if(location.GetComponent<DropOffPoint>().isPointActive()){
                Debug.Log("Active");
                Instantiate(marker, new Vector3(location.transform.position.x/7.7f, transform.position.y, location.transform.position.z/7.7f), Quaternion.identity);
                ActiveLocations.Add(location);
                ToRemove.Add(location);
            }   
        }
        removeThem(InactiveLocations);



        foreach (GameObject location in ActiveLocations){
            if(!location.GetComponent<DropOffPoint>().isPointActive()){
                InactiveLocations.Add(location);
                ToRemove.Add(location);
            }

        }
        removeThem(ActiveLocations);
    }

    void removeThem(List<GameObject> locations){
        foreach (GameObject location in ToRemove){
            locations.Remove(location);
        }
        ToRemove.Clear();
    }
}
