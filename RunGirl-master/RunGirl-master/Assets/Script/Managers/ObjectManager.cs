using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour {
	public static GameObject BoostCoin;
	public GameObject BoostCoinSet;
    public static AudioSource NormalBGM;
    public AudioSource NormalBGMSet;
    public static AudioSource SurfBGM;
    public AudioSource SurfBGMSet;
	// Use this for initialization
	void Start () {
		BoostCoin = BoostCoinSet;
        NormalBGM = NormalBGMSet;
        SurfBGM = SurfBGMSet;
        SceneManager.sceneLoaded += OnLoadScene;
	}

    void OnLoadScene(Scene scene,LoadSceneMode mode){
        NormalBGM.Stop();
        SurfBGM.Stop();
    }
}
