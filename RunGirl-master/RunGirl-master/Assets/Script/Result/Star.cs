using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Star : MonoBehaviour {
	public Material material;
	public void DrowStar(){
        this.GetComponent<MeshRenderer>().material = material;
	}
}
