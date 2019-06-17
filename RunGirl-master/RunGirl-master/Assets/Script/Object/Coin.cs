using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    public GameObject Player;
    private bool stop;
	// Use this for initialization
    void Start(){
        Player = GameObject.FindGameObjectWithTag("Player");
        stop = false;
    }
	// Update is called once per frame
	void Update () {
        if(stop){
            return;
        }
        float movescaler = Mathf.Max(0f, (this.transform.position - Player.transform.position).magnitude -100f  * Time.deltaTime);
        this.transform.position = Player.transform.position + (this.transform.position - Player.transform.position).normalized * movescaler;
        float rotatevalue = 360f * Time.deltaTime;
        this.transform.Rotate(new Vector3(rotatevalue, rotatevalue, rotatevalue), Space.World);
        if (movescaler < 0.5f)
        {
			Player.GetComponent<PlayerMovement>().GetBoostItem();
            StartCoroutine(playGetSound());
            stop = true;
        }
	}

    private IEnumerator playGetSound(){
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
        AudioSource audioSource = this.GetComponent<AudioSource>();
        if(audioSource!=null){
            audioSource.Play();
            while(audioSource.isPlaying){
                yield return null;
            }
        }
        Destroy(this.gameObject);
    }
}
