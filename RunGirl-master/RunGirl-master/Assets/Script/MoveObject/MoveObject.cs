using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {
    public float MoveRadius;
    public float Helz;
    private Vector3 startPosition;
    public float Bias;
    private float sumtime;
    public enum MoveType
    {
        Vertical,
        Horizontal,
        BAndF
    }

    public MoveType moveType;
    private bool Rmove = true;
    private float time;
    private void Start()
    {
        startPosition = this.transform.position;
        sumtime = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        sumtime += Time.deltaTime;
        Vector3 direction = Vector3.up;
        if (moveType == MoveType.Vertical)
        {
            direction = this.transform.up;
        }else if(moveType == MoveType.Horizontal)
        {
            direction = this.transform.right;
        }
        else if(moveType == MoveType.BAndF)
        {
            direction = this.transform.forward;
        }

        this.transform.position = startPosition + Mathf.Sin((sumtime + Bias) * Helz * Mathf.PI * 2) * direction * MoveRadius;
    }
}
