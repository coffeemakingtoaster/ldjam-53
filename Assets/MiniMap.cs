using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    
    private Camera minimapCamera;

    void Start()
    {
       
       
    }

    void LateUpdate()
    {
        float x = (player.position.x / 10f);
        float z = (player.position.z / 10f);
        Vector3 newPosition = new Vector3(x,transform.position.y,z);
        transform.position = newPosition;

        
    }
}
