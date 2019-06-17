using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {

    public float spinSpeed;
    public float MoveRadius;
    public float Helz;
    private Vector3 startPosition;
    public enum MoveType
    {
        Spin,
        RLmove
    }

    public MoveType moveType;
    private bool Rmove = true;
    private float time;
    private void Start()
    {
        startPosition = this.transform.position;
    }
    // Update is called once per frame
    void Update () {
        
        switch (moveType)
        {
            case MoveType.Spin:
                this.transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime, Space.Self);
                break;

            case MoveType.RLmove:
                this.transform.position = startPosition + Mathf.Cos(Time.time * Helz * Mathf.PI * 2) * this.transform.up * MoveRadius;
                break;

        }
        
        

	}
}
