using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tyui : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Player")
        {
            this.GetComponent<Canvas>().enabled = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Collider>().tag == "Player")
        {
            this.GetComponent<Canvas>().enabled = false;
        }
    }
}
