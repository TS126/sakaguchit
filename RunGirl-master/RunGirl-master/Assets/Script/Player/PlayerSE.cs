using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSE : MonoBehaviour {
	private AudioSource audioSource;
	public AudioClip Explosion;
	public AudioClip Death;
	public AudioClip Poka;
    public AudioClip CollectItem;
    public AudioClip Boost;
	// Use this for initialization
	private void Start () {
		audioSource = GetComponent<AudioSource> ();
	}

	public void PlayExplosion(){
		if (isPlayingDeathSound ()) {
			return;
		}
		audioSource.clip = Explosion;
		audioSource.Play ();
	}

	public void PlayDeath(){
		audioSource.clip = Death;
		audioSource.Play ();
	}

	public void PlayPoka(){
		audioSource.clip = Poka;
		audioSource.Play ();
	}

    public void PlayCollectItem(){
        audioSource.clip = CollectItem;
        audioSource.Play();
    }

    public void PlayBoost(){
        audioSource.clip = Boost;
        audioSource.Play();
    }

	public bool isPlayingDeathSound(){
		return audioSource.clip == Death && audioSource.isPlaying;
	}

}
