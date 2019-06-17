using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayGravity : MonoBehaviour {
    // Update is called once per frame
    public GameObject GravityObject;
    private Vector3 bacc;
    public float ViewportRectX;
    public float ViewportRectY;
    public float ViewportRectW;
    public float ViewportRectH;
    public bool IsWantChangeCameraRect = false;
    private bool Isturning;
    void Start(){
        Isturning = false;
        if (IsWantChangeCameraRect)
        {
            Camera.main.rect = new Rect(ViewportRectX, ViewportRectY, ViewportRectW, ViewportRectH);
        }
    }
	void Update () {
        Vector3 acc;
        acc = new Vector3(-Input.acceleration.x, Input.acceleration.y, 0f).normalized;
        acc = Vector3.Lerp(bacc, acc, 0.1f);
        bacc = acc;
        if (Isturning)
        {
            GravityObject.transform.up = acc;
        }else{
            GravityObject.transform.up = -acc;
        }
	}

    public void TurnGravity(){
        Isturning = !Isturning;
    }
}
