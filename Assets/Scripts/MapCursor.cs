using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCursor : MonoBehaviour
{

    public GameObject player;

    public GameObject Map;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    
        Quaternion playerRotation = player.transform.rotation;

    // Create a new Quaternion with the z rotation set to the y rotation of the player
    Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, playerRotation.eulerAngles.y, transform.rotation.eulerAngles.z);


    // Set the rotation of this gameobject to the new rotation
    transform.rotation = newRotation;

    transform.position = new Vector3(player.transform.position.x/7.7f+300,Map.transform.position.y,player.transform.position.z/7.7f);
    
    }
}
