using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bigger : MonoBehaviour {
    public float ExpandSpeed;
    public float WavePower;
    float time;
    public float EndTime;
    private void Update()
    {
        Vector3 scale = this.transform.localScale;
        float num = Mathf.Min(scale.x + ExpandSpeed * Time.deltaTime, EndTime * ExpandSpeed);

        scale.x = num;
		scale.y = num;
		scale.z = num;
        this.transform.localScale = scale;
        if (time >= EndTime) Destroy(this.gameObject, 0.5f);
        time += Time.deltaTime;
    }
}
