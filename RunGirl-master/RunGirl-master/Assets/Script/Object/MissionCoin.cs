using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCoin : MonoBehaviour
{


    private ParticleSystem par;
    private AudioSource AS;

    // Use this for initialization
    void Start()
    {

        par = GameObject.FindGameObjectWithTag("Player").transform.Find("Particle").gameObject.GetComponent<ParticleSystem>();
        AS = gameObject.GetComponent<AudioSource>();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine("Coin");
        }
    }

    private IEnumerator Coin()
    {
        par.Play();
        AS.Play();

        yield return new WaitForSeconds(2.0f);

        Destroy(this.gameObject);
    }
}