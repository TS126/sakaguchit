using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHelp : MonoBehaviour
{
    private int wasTouchNum;
    private bool wasMouseState;
    private int Num;
    public Image ShowUI;
    public Button DisplayButton;
    public Sprite[] Images;
    // Use this for initialization
    void Start()
    {
        wasTouchNum = Images.Length;
        wasMouseState = false;
    }

    public void OnClickHelpDisplay()
    {
        if (ShowUI.enabled == false)
        {
            return;
        }
        ++Num;
        if (Images.Length == Num)
        {
            ShowUI.enabled = false;
            DisplayButton.enabled = false;
            return;
        }
        ShowUI.sprite = Images[Num];
    }

    public void OnClickHelp()
    {
        if (Images.Length == 0)
        {
            return;
        }
        Num = 0;
        ShowUI.enabled = true;
        DisplayButton.enabled = true;
        ShowUI.sprite = Images[0];
    }
}
