using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButton : MonoBehaviour {

    public GameObject Title;
    public GameObject Select;

    public void StartButton (){
		GameManager.First.SelectedWorld ();
        Title.GetComponent<Animator>().SetBool("Menu",true);
		Select.GetComponent<Animator>().SetBool("Select", true);
		TitleSE.PlayButtonSE ();
    }

    public void BackButton (){
        Title.GetComponent<Animator>().SetBool("Menu", false);
		Select.GetComponent<Animator>().SetBool("Select", false);
		TitleSE.PlayButtonSE ();
    }
}
