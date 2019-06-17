using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Animations;
public class GameManager : MonoBehaviour {
    public Text ClearTimeText;
    public GameObject StageNameText;
    public GameObject StageSelect;
    public GameObject Canvas;
    public GameObject ClickMask;
    public GameObject[] StageButton;
	public List<int> ConquerElements;
    public Image RightArrow;
    public static float ClearTime;
	public static int CollectNum;
	private int collectSum;
	public int CollectSum {
		get {
			return collectSum;
		}
		set {
			collectSum = value;
			OnChangedCollectSum (value);
		}
	}
	[SerializeField]
	private Text StarNumText;
	private void OnChangedCollectSum(int value){
		StarNumText.text = value.ToString ();
	}
    public RuntimeAnimatorController LockOpen;
	private const string CollectSumKey = "CollectSum";
    public int ThisStage;
    public bool IsClear;
	public int WasCleared;
    public string MissionText;
    // Use this for initialization
    static GameManager _First;
    public static GameManager First
    {
        get { return _First ?? (_First = FindObjectOfType<GameManager>()); }
    }
    void Start(){
		CollectSum = PlayerPrefs.GetInt (CollectSumKey, 0);
		for (int i = 0; i < StageButton.Length; ++i) {
			ConquerElements.Add (i);
		}
    }

	private int GetConquerElements(int num){
		string str = "ConquerElements" + num.ToString ();
		return PlayerPrefs.GetInt (str, 0);
	}
	private void SetConquerElements(int num,int amount){
		string str = "ConquerElements" + num.ToString ();
		PlayerPrefs.SetInt (str, amount);
	}
    void Awake(){
        if(First != this){
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }
	private void SetClearTime(){
		for (int i = 0; i < StageButton.Length; ++i) {
			SceneChange sceneChange = StageButton [i].GetComponent<SceneChange> ();
			if (sceneChange == null) {
				continue;
			}
			String str = sceneChange.StageName+"ClearTime";
			float clearTime = PlayerPrefs.GetFloat (str, 5999.999f);
			Text text = StageButton [i].transform.Find ("TopRecordText").GetComponent<Text> ();
            text.text = String.Format("{0:00}:{1:00.000}", (int)(clearTime / 60f), clearTime % 60f);
		}
		for (int i = 0; i < StageButton.Length; ++i) {
			DrowStageButton (i);
		}
	}
	public Sprite StarTexture;
    public Material StarFishMaterial;
    private void DrowStageButton(int place){
		int num = GetConquerElements (StageButton [place].GetComponent<SceneChange> ().StageID);
		GameObject parent = StageButton [place].transform.Find ("ConquersParent").gameObject;
		for (int i = 0; i < 3; ++i) {
            if ((num & (1 << i)) != 0) {
				parent.GetComponentsInChildren<Image> () [i].sprite = StarTexture;
			}
		}
        if(num!=0){
            StageButton[place].GetComponent<SceneChange>(). SetMissionStr();
        }
	}
	public int GetConquersNum(GameObject StageButton){
		int num = GetConquerElements (StageButton.GetComponent<SceneChange> ().StageID);
		return num;
	}
    public void SelectedWorld()
    {
		for (int i = 0; i < StageButton.Length; ++i)
        {
			if (StageButton [i].GetComponent<SceneChange> ().NeedSumToRelease <= CollectSum) {
				if (!StageButton [i].GetComponent<Button> ().interactable) {
					StageButton [i].GetComponent<Button> ().interactable = true;
					PlayLockOpenAnimation (StageButton [i]);
					fadeOutStarAndKey (StageButton [i]);
				}
			}
			Text text = StageButton [i].transform.Find ("PlayAndStar").Find("StarNum").GetComponent<Text> ();
			text.text = StageButton [i].GetComponent<SceneChange> ().NeedSumToRelease.ToString ();
        }
		SetClearTime ();
    }

    private void OnLevelWasLoaded(int level)
    {
        Time.timeScale = 1f;
        if(SceneManager.GetActiveScene().name == "Title"){
            //ClickMask.GetComponent<Animator>().SetBool("Whiteout", false);
            ClickMask.GetComponent<Image>().color = new Color(1,1,1,0);
            ClickMask.SetActive(false);
            Canvas.GetComponent<Canvas>().enabled = true;
            if (IsClear)
            {
				for (int i = 0; i <StageButton.Length; i++)
                {
					if (StageButton [i].GetComponent<SceneChange> ().NeedSumToRelease <= CollectSum) {
						if (!StageButton [i].GetComponent<Button> ().interactable) {
							StageButton [i].GetComponent<Button> ().interactable = true;
							PlayLockOpenAnimation (StageButton [i]);
							fadeOutStarAndKey (StageButton [i]);
						}
					}
                }
                StageSelect.GetComponent<ScrollRectSnap>().UpdateIndex();
                StageSelect.GetComponent<ScrollRectSnap>().targetPosition = StageSelect.GetComponent<ScrollRectSnap>().GetSnapPosition();
            }
			SetClearTime ();
        }else{
            if (SceneManager.GetActiveScene().name == "Result")
            {
				PlayerPrefs.Save();
                ClickMask.SetActive(false);
            }
            else
            {
                ClickMask.SetActive(true);
                Canvas.GetComponent<Canvas>().enabled = false;
            }
        }
        if(SceneManager.GetActiveScene().name == "Result"){
			StartCoroutine (DrowStarInResult ());
			setStageName ();
			bool updatedRecord = setClearTime ();
			changeFromRecord (updatedRecord);
			updateConquerElements();
			PlayerPrefs.Save ();
        } 
    }

	private IEnumerator DrowStarInResult(){
        int num = CollectNum;
        num |= WasCleared;
		List<GameObject> Line = new List<GameObject> ();
		for (int i = 1; i <= 3; ++i) {
			Line.Add (GameObject.Find ("Star" + i.ToString ()));
		}
        for (int i = 0; i < 3;++i){
            Line[i].GetComponentInParent<Image>().enabled = false;
            if(Line[i] != null && (WasCleared & (1<<i)) != 0){
                Destroy(Line[i].GetComponentInChildren<Animator>());
                Line[i].GetComponentInChildren<Star>().DrowStar();
            }
        }
        yield return new WaitForSeconds (0.5f);
		for (int i = 0; i < 3; ++i) {
            if (Line [i] != null && Line [i].activeInHierarchy && ((num ^ WasCleared)&(1<<i)) != 0) {
				Line [i].GetComponentInChildren<Animator> ().SetTrigger ("Turn");
				yield return new WaitForSeconds (0.5f);
			}
		}
	}

	private void updateConquerElements(){
		int bCollectNum = GetConquerElements (ThisStage);
		CollectNum |= bCollectNum;
		if (bCollectNum < CollectNum) {
			for (int i = 0; i < 3; ++i) {
				if (((CollectNum - bCollectNum) & (1 << i)) != 0) {
					++CollectSum;
				}
			}
			PlayerPrefs.SetInt (CollectSumKey, CollectSum);
			SetConquerElements (ThisStage, CollectNum);
		}
	}

	private string getConquerElementsStr(int num){
		return "Stage " + num.ToString () + "Collect";
	}
	private void changeFromRecord(bool updatedRecord){
		if (!updatedRecord) {
			ParticleSystem[] paperBlizzardz = GameObject.FindObjectsOfType<ParticleSystem> ();
			foreach (var i in paperBlizzardz) {
				if (i.name == "PaperBlizzard") {
					i.gameObject.SetActive (false);
				}
			}
			GameObject newRecordText = GameObject.Find ("New Record");
			if (newRecordText != null) {
				newRecordText.SetActive (false);
			}
		}
	}
	private void displayNewRecord(bool updatedRecord){
		if (!updatedRecord) {
			return;
		}

	}
	private void setStageName(){
		StageNameText = GameObject.Find("Stage Name");
		if (StageNameText == null) {
			return;
		}
		Text text = StageNameText.GetComponent<Text> ();
		if (text == null) {
			return;
		}
		text.text = "Stage " + ThisStage.ToString ();
	}

	private bool setClearTime(){
		GameObject Text = GameObject.Find ("Time");
		if (Text == null) {
			return false;
		}
		ClearTimeText = Text.GetComponent<Text>();
		if (ClearTimeText == null) {
			return false;
		}
		string clearTimeStr = "";
        clearTimeStr = String.Format ("{0:00}:{1:00.000}", (int)(ClearTime / 60f), ClearTime % 60f);
		ClearTimeText.text = clearTimeStr;
		float BeforeTime = PlayerPrefs.GetFloat (StageButton[ThisStage - 1].GetComponent<SceneChange>().StageName + "ClearTime", 5999.999f);
		ClearTime = Mathf.Min(PlayerPrefs.GetFloat (StageButton[ThisStage - 1].GetComponent<SceneChange>().StageName + "ClearTime", ClearTime),ClearTime);
		PlayerPrefs.SetFloat (StageButton[ThisStage - 1].GetComponent<SceneChange>().StageName + "ClearTime", ClearTime);
		return ClearTime < BeforeTime;
	}

    private void PlayLockOpenAnimation(GameObject button){
        foreach(var i in button.GetComponentsInChildren<Animator>()){
            if(i.runtimeAnimatorController == LockOpen){
                i.SetTrigger("Open");
            }
        }
    }
	private void fadeOutStarAndKey(GameObject target){
		GameObject child = target.transform.Find ("PlayAndStar").gameObject;
		if (child == null) {
			return;
		}
		Animator animator = child.GetComponent<Animator> ();
		if (animator == null) {
			return;
		}
		animator.SetTrigger ("FadeOut");
	}
}
