using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShokWabe : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<SphereCollider>().radius = 0f;
	}
    public float expandspeed = 60f;
    public float maxradius = 50f;
    public float wabepower = 50;
	// Update is called once per frame
	void Update () {
        this.GetComponent<SphereCollider>().radius += Time.deltaTime * expandspeed;
        if (this.GetComponent<SphereCollider>().radius > maxradius){
            Destroy(this.gameObject);
        }
	}
}
