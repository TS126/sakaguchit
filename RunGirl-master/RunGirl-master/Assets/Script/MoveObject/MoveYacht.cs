using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveYacht : MonoBehaviour {
	public List<Vector3> Positions = new List<Vector3>();
	public List<Quaternion> Rotations = new List<Quaternion>();
	public LayerMask hitroad;
    public float MoveSpeed;
	int place;
	// Use this for initialization
	void Start () {
		place = 0;
        this.transform.position = Positions[0];
	}
	
	// Update is called once per frame
	void Update () {
        float moveDistance = 0f;
        while (MoveSpeed * Time.deltaTime - moveDistance > (Positions[place] - this.transform.position).magnitude){
            moveDistance += (Positions[place] - this.transform.position).magnitude;
            this.transform.position = Positions[place];
			++place;
			if (place == Positions.Count) {
				place = 0;
			}
        }
        this.transform.position += (MoveSpeed * Time.deltaTime - moveDistance) * (Positions[place] - this.transform.position).normalized;
        this.transform.rotation = Rotations [place];
	}
}
