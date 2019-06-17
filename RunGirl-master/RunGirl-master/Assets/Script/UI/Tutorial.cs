using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public float TimeScale;
    public Sprite texture;
	public string TutorialText;
    public float DisplayTime;
    private bool used;
    void Start(){
        used = false;
    }
    void OnTriggerEnter(){
        if(used){
            return;
        }
        used = true;
        StartCoroutine(DisplayTutorial());
    }

    IEnumerator DisplayTutorial(){
        Time.timeScale = TimeScale;
		GameObject tutorialImage = GameObject.Find("TutorialImage");
		GameObject tutorialText = GameObject.Find ("TutorialText");
        Image image = GetImage(tutorialImage);
		Text text = GetText (tutorialText);
        image.enabled = true;
		text.enabled = true;
		text.text = TutorialText;
		SetBand (true);
        if(texture == null){
            image.enabled = false;
        }else{
			image.sprite = texture;
        }
        yield return new WaitForSeconds(DisplayTime * TimeScale);
		if (text.text == TutorialText) {
			Time.timeScale = 1f;
			SetBand (false);
		}
		SetBand (false);
        if (texture == image.sprite)
        {
            Time.timeScale = 1f;
            image.enabled = false;
        }
    }

    Image GetImage(GameObject gameObject){
        if(gameObject == null){
            return null;
        }
        Image res = gameObject.GetComponent<Image>();
        return res;
    }
	void SetBand(bool flag){
		GameObject Bands = GameObject.Find ("Bands");
		Bands.GetComponent<Animator> ().SetBool ("Spawn", flag);
		Bands.GetComponent<Animator> ().SetFloat ("Speed", flag ? 1 / TimeScale : -1f);
	}
	Text GetText(GameObject gameObject){
		if (gameObject == null) {
			return null;
		}
		Text res = gameObject.GetComponent<Text> ();
		return res;
	}
}
