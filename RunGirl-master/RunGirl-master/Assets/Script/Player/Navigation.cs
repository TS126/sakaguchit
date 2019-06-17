using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation:MonoBehaviour {

    public List<GameObject> GetNavigationPoints()
    {
        List<GameObject> Partslist = new List<GameObject>(GameObject.FindGameObjectsWithTag("RoadParts"));
        Partslist.Sort((a, b) => string.Compare(a.name, b.name));
        List<GameObject> res = new List<GameObject>();
        foreach (var i in Partslist)
        {
            List<GameObject> Points = new List<GameObject>();
            foreach (Transform j in i.transform)
            {
                if (j.tag == "MovePoint")
                {
                    Points.Add(j.gameObject);
                }
            }
            Points.Sort((a, b) => string.Compare(a.name, b.name));
            res.AddRange(Points);
        }
        foreach(var i in res){
            if(i.GetComponent<MeshRenderer>()!=null){
                i.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        return res;
    }
}
