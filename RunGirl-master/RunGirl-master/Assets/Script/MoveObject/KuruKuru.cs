using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuruKuru : MonoBehaviour {
	public float kurukurux;
	public float kurukuruy;
	public float kurukuruz;
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (kurukurux*Time.deltaTime, kurukuruy*Time.deltaTime, kurukuruz*Time.deltaTime));
	}
}
