using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour {

	// Use this for initialization
	float WaitTime = 5f;
	GameObject loadDisplay;
	void Start () {
		Time.timeScale = 0f;
		loadDisplay = GameObject.Find ("Load");
		loadDisplay.SetActive (true);
		StartCoroutine (StayLoading ());
	}

	IEnumerator StayLoading(){
		float stime = Time.unscaledTime;
		while (true) {
			if (WaitTime < Time.unscaledTime - stime) {
				break;
			}
			yield return null;
		}
		Time.timeScale = 1f;
		loadDisplay.SetActive (false);
	}
}
