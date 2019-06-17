using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowFPS : MonoBehaviour {
	private float usetime;
	private Text text;
	// Use this for initialization
	void Start () {
		usetime = 0f;
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		usetime = Mathf.Lerp (usetime, Time.deltaTime, 0.1f);
		text.text = (1f / usetime).ToString ("F2");
	}
}
