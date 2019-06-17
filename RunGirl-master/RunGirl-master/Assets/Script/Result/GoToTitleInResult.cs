using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToTitleInResult : MonoBehaviour {
	private bool isFirstClear;
    private int wasClear;
	public const string TitleStr = "Title";
	public string MissionText;
	private bool wasMouseState;
	private int wasTouchNum;
    public Button MissionButton;
	void Start(){
		wasMouseState = false;
		wasTouchNum = 0;
        isFirstClear = GameManager.First.WasCleared == 0;
        MissionText = GameManager.First.MissionText;
	}

    public void OnTouchResultScreen()
    {
        if (!isFirstClear)
        {
            SceneManager.LoadScene(TitleStr);
        }
        GameObject Mission = GameObject.Find("MissionBackScreen");
        Mission.transform.Find("MissionText").GetComponent<Text>().text = MissionText;
        Animator animator = Mission.GetComponent<Animator>();
        animator.SetTrigger("Open");
    }

    public void OnTouchMissionScreen()
    {
        SceneManager.LoadScene(TitleStr);
    }
}
