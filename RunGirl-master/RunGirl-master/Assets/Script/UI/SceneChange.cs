using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{

    public int StageID;
    //public Object NextScene;
    GameManager gameManager;
    public string StageName;
    public int NeedSumToRelease;
    public string MissionStr;
    public string NormalStr = "";
    public static bool Selected;

    // Use this for initialization
    void Start()
    {
        //text.text = "ugoitayo-";
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        this.transform.Find("MissionText").gameObject.GetComponent<Text>().text = NormalStr;
        Selected = false;
    }
    // Update is called once per frame

    public void ChangeNextScene()
    {
        if(Selected){
            return;
        }
        checkIsStageCleared();
        AddMissionText();
        TitleSE.PlayButtonSE();
        gameManager.ThisStage = StageID;
        if (StageName != "")
        {
            StartCoroutine(LoadScene());
            Selected = true;
        }
    }
    public void SetMissionStr()
    {
        this.transform.Find("MissionText").gameObject.GetComponent<Text>().text = MissionStr;
    }
    public void AddMissionText()
    {
        GameManager.First.MissionText = MissionStr;
    }
    private void checkIsStageCleared()
    {
        int num = GameManager.First.GetConquersNum(this.gameObject);
        GameManager.First.WasCleared = num;
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(StageName);
        async.allowSceneActivation = false;
        while (async.progress < 0.89f){
            print("HOGE");
            yield return null;
        }
		yield return new WaitForSeconds(0.5f);
        async.allowSceneActivation = true;
        yield return async;
    }
}
