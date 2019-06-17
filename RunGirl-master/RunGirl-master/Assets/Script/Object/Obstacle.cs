using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public bool isbreak;
	public GameObject Coin;
    public float SearchDistance;
	[SerializeField]
	private float DestroyTime = 1f;
	public enum ObstacleType{
		Other,
		Burrel,
		SwimRing
	}
	public ObstacleType obstacleType = ObstacleType.Other;
	private void Start ()
	{
		isbreak = false;
	}
	public void OnTriggerStay (Collider col)
	{
		if (col.GetComponent<Bigger> () != null) {

			if (isbreak == false) {
				StartCoroutine (InstantiateCoin ());
				StartCoroutine (InstantiateCoin ());
				StartCoroutine (InstantiateCoin ());
			}
			isbreak = true;
			if (obstacleType == ObstacleType.Other) {
				this.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
				Vector3 force = (this.transform.position - col.transform.position).normalized * col.GetComponent<Bigger> ().WavePower / Mathf.Pow ((this.transform.position - col.transform.position).magnitude, 2f);
				this.GetComponent<Rigidbody> ().AddForce (force);
			}
		}
	}

	public void OnTriggerEnter(Collider col){
		if (col.GetComponent<Bigger> () != null) {
			if (isbreak) {
				return;
			}
			isbreak = true;
			if (obstacleType == ObstacleType.Burrel) {
				print ("HOGE");
				foreach (Transform i in this.transform) {
                    if (i.GetComponent<Rigidbody>() != null)
                    {
                        Vector3 rand = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));
                        Vector3 arrow = (i.transform.position - this.transform.position + rand).normalized;
                        i.GetComponent<Rigidbody>().AddForceAtPosition(arrow * 1000, i.position + rand);
                    }
				}
				StartCoroutine (AddForceInChild ());
				//GameObject d = Instantiate (ObjectManager.BoostCoin)as GameObject;
				//d.transform.rotation = this.transform.rotation;
				//d.transform.position = this.transform.position;
			} else if (obstacleType == ObstacleType.SwimRing) {
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                Destroy (this.GetComponent<MeshCollider> ());
				transform.Find ("Smoke").gameObject.SetActive (true);
                StartCoroutine(shrinkSwimRing());
                //this.GetComponent<Rigidbody>().AddForce(this.transform.up * 30f + this.transform.forward * 10f, ForceMode.Impulse);
                //this.GetComponent<Rigidbody>().AddTorque(new Vector3(400f, 100f, 800f));

                this.GetComponent<Rigidbody>().AddForceAtPosition(this.transform.up * 500f + this.transform.forward * 250f,this.transform.position +  new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)));
            }
			Destroy (this.gameObject,DestroyTime);
		}
	}
    private IEnumerator shrinkSwimRing() {
        float startTime = Time.time;
        float finishTime = DestroyTime * 0.7f;
        float startSize = this.transform.localScale.z;
        float finishSize = startSize * 0.05f;
        while (true) {
            if (Time.time - startTime > finishTime) {
                break;
            }
            Vector3 newScale = this.transform.localScale;
            newScale.z = Mathf.Lerp(startSize, finishSize, (Time.time - startTime) / finishTime);
            this.transform.localScale = newScale;
            yield return null;
        }

        this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, finishSize);
        yield return new WaitForSeconds(DestroyTime * 0.1f);
        startTime = Time.time;
        finishTime = DestroyTime * 0.2f;
        startSize = this.transform.localScale.x;
        finishSize = startSize * 0.01f;
        while (true) {
            if (Time.time - startTime > finishTime) {
                break;
            }
            float size = Mathf.Lerp(startSize, finishSize, (Time.time - startTime) / finishTime);
            this.transform.localScale = new Vector3(size, size, this.transform.localScale.z);
            yield return null;
        }
        Destroy(this.gameObject);
    }

	private IEnumerator AddForceInChild(){
		float startTime = Time.time;
		while (true) {
			if (Time.time - startTime > DestroyTime) {
				break;
			}
			foreach (Transform i in this.transform) {
                if (i.GetComponent<Rigidbody>() != null)
                {
                    i.GetComponent<Rigidbody>().AddForce(-this.transform.up * Physics.gravity.magnitude * 3f);
                }
			}
			yield return null;
		}
		foreach (Transform i in this.transform) {
			Destroy (i.gameObject);
		}
		Destroy (this.gameObject);
	}

	public void GetExplosion ()
	{
		if (isbreak == false) {
			StartCoroutine (InstantiateCoin ());
			StartCoroutine (InstantiateCoin ());
			StartCoroutine (InstantiateCoin ());
		}
		isbreak = true;
	}

	private IEnumerator InstantiateCoin ()
	{
		if (Coin != null) {
			yield return new WaitForSeconds (1.0f);
			Instantiate (Coin, this.transform.position, this.transform.rotation);
			Destroy (this.gameObject);
		}
	}
}
