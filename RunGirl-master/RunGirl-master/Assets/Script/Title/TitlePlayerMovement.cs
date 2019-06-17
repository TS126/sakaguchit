using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayerMovement : MonoBehaviour {
	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
		animator.SetBool ("run", true);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += this.transform.forward * 40f*Time.deltaTime;
	}
}
