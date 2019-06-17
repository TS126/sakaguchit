using UnityEngine;
using UnityEditor;
using System.Collections;

public class MakeMovePoint: EditorWindow
{
	static public LayerMask Layer;

	[MenuItem ("Window/GetStar")]
	static void Open ()
	{
		GameObject other = Selection.activeGameObject;
        SetFind(other, other);
	}
    static public void SetFind(GameObject target,GameObject parent){
        foreach (Transform i in parent.transform){
            if(i.gameObject.name == "StarFish"){
                target.GetComponent<TitleCanvas>().Stars.Add(i.gameObject);
            }
            SetFind(target, i.gameObject);
        }
    }
	[MenuItem ("Tools/Make Position for Yacht")]
	static void Make ()
	{
		GameObject target = Selection.activeGameObject;
		target.GetComponent<MoveYacht> ().Positions.Clear ();
		target.GetComponent<MoveYacht> ().Rotations.Clear ();
		LayerMask layer = target.GetComponent<MoveYacht> ().hitroad;
		for (int i = 0; i < 360; ++i) {
			RaycastHit hit;
			Vector3 vec = target.transform.up * Mathf.Sin (i * Mathf.Deg2Rad) + target.transform.right * Mathf.Cos (i * Mathf.Deg2Rad);
			if (Physics.Raycast (target.transform.position, vec, out hit,layer)) {
				Debug.Log ("HOGE");
				target.GetComponent<MoveYacht> ().Positions.Add (hit.point);
				target.GetComponent<MoveYacht> ().Rotations.Add (Quaternion.LookRotation (target.transform.forward, -vec));
			}
		}
		target.transform.position = target.GetComponent<MoveYacht> ().Positions [0];
		target.transform.rotation = target.GetComponent<MoveYacht> ().Rotations [0];
	}
}