using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject[] prefabs;
    public int maxObjects = 200;
    public GameObject target;
    public float spawnInterval = 0.5f;

    private float lastSpawnTime;

    private void Start()
    {
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        
        if (GameObject.FindObjectsOfType<CarController>().Length < maxObjects && Time.time - lastSpawnTime > spawnInterval)
        {
            lastSpawnTime = Time.time;
            SpawnObject();
        }
    }

    public void SpawnObject()
    {
        Debug.Log("Spawning");
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject newObject = Instantiate(prefab, transform.position, Quaternion.identity);
        newObject.GetComponent<CarController>().target = target;
    }
}