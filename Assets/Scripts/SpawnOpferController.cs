using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOpferController : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject target;

    public Canvas Minimap;

    public Canvas Map;
    public GameObject Minimapmarker;
    
   

    public void SpawnOpfer(int reward, int timer)
    {
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject newObject = Instantiate(prefab, transform.position, Quaternion.identity);
        newObject.GetComponent<CarController>().target = target;
        newObject.GetComponent<OpferCar>().setValues(reward,timer,Minimap,Map,Minimapmarker);
        
    
        
    
    


    }
}
