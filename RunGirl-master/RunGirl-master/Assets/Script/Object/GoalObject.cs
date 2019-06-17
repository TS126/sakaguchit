using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoalObject : MonoBehaviour
{
	private Animator animation;
	bool flag = false;
	private Camera mainCamera;
	void Start ()
	{
		animation = this.GetComponent<Animator> ();
		mainCamera = Camera.main;
	}

	public void OnTriggerEnter (Collider other)
	{
        if (flag){
            return;
        }
		if (other.tag == "Player") {
			flag = true;
			animation.SetTrigger ("Open");
			Destroy (mainCamera.GetComponent<CameraMovement> ());
			StartCoroutine (LookCamera ());
		}
	}

	private void Update ()
	{
		if (!flag) {
			this.transform.rotation = Quaternion.LookRotation ((mainCamera.transform.position - this.transform.position).normalized, mainCamera.transform.up);
		}
	}

	private IEnumerator LookCamera ()
	{
		float st = Time.time;
		float startDistance = (this.transform.position - mainCamera.transform.position).magnitude;
		GameManager gameManager = GameManager.First;
		Image whiteOutImage = gameManager.ClickMask.GetComponent<Image> ();
		while (true) {
			Vector3 vec = this.transform.position - mainCamera.transform.position;
			Vector4 newColor = whiteOutImage.color;
			newColor.w = Mathf.Pow ((startDistance - vec.magnitude) / startDistance, 2f);
			whiteOutImage.color = newColor;
			Quaternion forward = Quaternion.LookRotation (vec, Vector3.Cross (vec, mainCamera.transform.right));
			mainCamera.transform.rotation = Quaternion.Lerp (mainCamera.transform.rotation, forward, 0.01f);
			forward = forward * Quaternion.Euler(90f,180f,0f);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, forward, 0.02f);
			if (Time.time - st > 1f) {
				float d = Mathf.Min(Time.deltaTime * 15f,vec.magnitude - 0.3f);
				mainCamera.transform.position += vec.normalized * d;
			}
			yield return null;
		}
	}
}
