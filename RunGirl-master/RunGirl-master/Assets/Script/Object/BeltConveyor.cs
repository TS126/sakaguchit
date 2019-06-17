using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltConveyor : MonoBehaviour {
    [SerializeField]
    private float RotatePerSecond;
    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Camera.main.transform.Rotate(new Vector3(0f, 0f, RotatePerSecond * Time.deltaTime), Space.Self);
        }
    }
}
