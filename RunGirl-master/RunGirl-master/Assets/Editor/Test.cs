using UnityEngine;
using UnityEditor;
using System.Collections;

public class Test : EditorWindow{
    [MenuItem("Tools/PlayerPrefs/DeleteAll")]
    static void DeleteAll()
    {
		PlayerPrefs.DeleteAll ();
		Debug.Log ("Delete All Data");
    }
	[MenuItem("Tools/PlayerPrefs/SetAllClear")]
	static void SetAllClear(){
        PlayerPrefs.SetInt ("CollectSum", 9999);
		PlayerPrefs.Save ();
	}
}