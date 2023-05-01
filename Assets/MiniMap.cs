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
        float x = (player.position.x / 7.7f);
        float z = (player.position.z / 7.7f);
        Vector3 newPosition = new Vector3(x,transform.position.y,z);
        transform.position = newPosition;

        
    }
}
