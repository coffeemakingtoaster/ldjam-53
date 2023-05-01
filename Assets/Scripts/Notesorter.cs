using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notesorter : MonoBehaviour
{
    int notecountold = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(notecountold != transform.childCount){
            for (int i = 0; i < transform.childCount; i++){
                transform.GetChild(i).position = new Vector3(i*250+141,811, 0);
            }
            

        }

        
    }
}

