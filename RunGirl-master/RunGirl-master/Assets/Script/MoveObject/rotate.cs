using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {

    [SerializeField]
    private float RotateSpeed = 270f;
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(RotateSpeed * Time.deltaTime, 0f, 0f, Space.Self);
	}
}
