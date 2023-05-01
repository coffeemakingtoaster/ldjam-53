using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMapCursor : MonoBehaviour
{
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion playerRotation = player.transform.rotation;

    // Create a new Quaternion with the z rotation set to the y rotation of the player
    Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, playerRotation.eulerAngles.y*(-1f));


    // Set the rotation of this gameobject to the new rotation
    transform.rotation = newRotation;

    
    }
}
