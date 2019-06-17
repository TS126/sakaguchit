using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour {

    // Use this for initialization
    void Start () {
        
    }
	

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
