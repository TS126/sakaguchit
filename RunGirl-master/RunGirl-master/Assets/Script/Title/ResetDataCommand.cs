using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDataCommand : MonoBehaviour {
    private Coroutine coroutine;
    // Update is called once per frame
    void Update()
    {
        if (Input.touches.Length == 4)
        {
            StartCoroutine(func());
        }
    }

    private IEnumerator func(){
        while (Input.touches.Length == 4)
        {
            yield return null;
        }
        if (Input.touches.Length != 3)
        {
            yield break;
        }
        while (Input.touches.Length == 3)
        {
            yield return null;
        }
        if (Input.touches.Length != 4)
        {
            yield break;
        }
        while (Input.touches.Length == 4)
        {
            yield return null;
        }
        if (Input.touches.Length != 3)
        {
            yield break;
        }
        while (Input.touches.Length == 3)
        {
            yield return null;
        }
        if (Input.touches.Length == 2)
        {
            PlayerPrefs.DeleteAll();
        }
        if(Input.touches.Length == 4)
        {
            PlayerPrefs.SetInt("CollectSum", 9999);
            PlayerPrefs.Save();
        }
    }
}
