using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleCanvas : MonoBehaviour {

    private string titleName;
    public List<GameObject> Stars = new List<GameObject>();
	// Use this for initialization
    static TitleCanvas _First;
    static TitleCanvas First
    {
        get { return _First ?? (_First = FindObjectOfType<TitleCanvas>()); }
    }

    void Awake()
    {
        if (First != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        titleName = SceneManager.GetActiveScene().name;
    }
}
