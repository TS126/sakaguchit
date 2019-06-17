using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSE : MonoBehaviour {
	private static AudioSource audioSource;
	private void Start(){
		audioSource = GetComponent<AudioSource> ();
	}
	public static void PlayButtonSE(){
		if (audioSource != null) {
			audioSource.Play ();
		}
	}
}
