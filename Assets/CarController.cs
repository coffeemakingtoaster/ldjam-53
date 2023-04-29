using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject target; // The current target GameObject for the car
    public float speed = 5f; // The speed at which the car moves towards the target
    public float lookheight = 1f;

    private void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;
            
            
            // Rotate the car to face the offset position
            Vector3 direction = targetPosition - transform.position;
            direction = new Vector3(direction.x,direction.y+lookheight,direction.z);
            transform.rotation = Quaternion.LookRotation(direction);
            
            // Move the car towards the offset position at the specified speed
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Check if the car has reached the target
            if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                // Set the target to a random neighboring MapMarker
                MapMarker mapMarker = target.GetComponent<MapMarker>();
                if (mapMarker != null)
                {
                    MapMarker[] neighbors = mapMarker.neighbors;
                    MapMarker randomNeighbor = neighbors[Random.Range(0, neighbors.Length)];
                    SetTarget(randomNeighbor.gameObject);
                }
            }
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}