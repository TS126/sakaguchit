using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSelect : MonoBehaviour {

    public GameObject ThisWorld;
    public GameObject StageSelect;
    public GameObject [] Some;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickworldButton(){
        ThisWorld.GetComponent<Animator>().SetBool("Selected",true);
        Some[0].SetActive(false);
        Some[1].SetActive(false);
        Some[2].SetActive(false);
        StageSelect.GetComponent<ScrollRectSnap>().horizontalNormalizedPosition = 0f;
        StageSelect.SetActive(true);
        GameManager.First.SelectedWorld();
		TitleSE.PlayButtonSE ();
    }

    public void ClickSubBackButton(){
        ThisWorld.GetComponent<Animator>().SetBool("Selected", false);
        Some[0].SetActive(true);
        Some[1].SetActive(true);
        Some[2].SetActive(true);
		StageSelect.SetActive(false);
		TitleSE.PlayButtonSE ();
    }
}
