using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionClear : MonoBehaviour {

    private GameObject player;
    private float time = 0;
    private float startTime;
    private float clearTime = 20f;
    private int objectsLength;
    private int boostLength;
    private bool flag;

    public enum StageSelect
    {
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
        Stage6,
        Stage7,
        Stage8,
        Stage9,
        Stage10,
        Stage11,
        Stage12
    }
    public StageSelect stageSelect;

    private void Start()
    {
        objectsLength = GameObject.FindGameObjectsWithTag("Obstacle").Length;
        boostLength = GameObject.FindGameObjectsWithTag("BoostItem").Length;
        startTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
        flag = false;
    }

    private void Update()
    {
        if(stageSelect == StageSelect.Stage7)
        {
            player.GetComponent<PlayerMovement>().InputSum = Mathf.Min(0f, player.GetComponent<PlayerMovement>().InputSum);
            if(player.GetComponent<PlayerMovement>().InputSum < -0.1f)
            {
                flag = true;
            }
        }
        if (stageSelect == StageSelect.Stage8)
        {
            player.GetComponent<PlayerMovement>().InputSum = Mathf.Max(0f, player.GetComponent<PlayerMovement>().InputSum);
            if (player.GetComponent<PlayerMovement>().InputSum > 0.1f)
            {
                flag = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            time = Time.time - startTime;
            print(time);
            if (GameObject.FindGameObjectWithTag("Mission") == null && stageSelect == StageSelect.Stage1)
                player.GetComponent<PlayerMovement>().ClearedChallenge();
            else if (GameObject.FindObjectOfType<Obstacle>() == null && stageSelect == StageSelect.Stage2)
                player.GetComponent<PlayerMovement>().ClearedChallenge();
            else if (time <= clearTime && stageSelect == StageSelect.Stage3)
                player.GetComponent<PlayerMovement>().ClearedChallenge();
            else if (player.GetComponent<PlayerMovement>().JumpNum <= 1 && stageSelect == StageSelect.Stage4)
            {
                player.GetComponent<PlayerMovement>().ClearedChallenge();
            }
            else if (player.GetComponent<PlayerMovement>().JumpNum <= 4 && stageSelect == StageSelect.Stage5)
            {
                player.GetComponent<PlayerMovement>().ClearedChallenge();
            }
            else if (GameObject.FindGameObjectWithTag("Coin") == null && (stageSelect == StageSelect.Stage6 || stageSelect == StageSelect.Stage9))
            {
                player.GetComponent<PlayerMovement>().ClearedChallenge();
            }
            else if (flag == false && stageSelect == StageSelect.Stage7)
            {
                player.GetComponent<PlayerMovement>().ClearedChallenge();
            }
            else if (flag == false && stageSelect == StageSelect.Stage8)
            {
                player.GetComponent<PlayerMovement>().ClearedChallenge();
            }
            else if (GameObject.FindGameObjectWithTag("BoostItem") == null && stageSelect == StageSelect.Stage10)
            {
                player.GetComponent<PlayerMovement>().ClearedChallenge();
            }
            else if (time < 50f && stageSelect == StageSelect.Stage11)
            {
                player.GetComponent<PlayerMovement>().ClearedChallenge();
            }
            else if (boostLength - GameObject.FindGameObjectsWithTag("BoostItem").Length >= 3 && stageSelect == StageSelect.Stage12)
            {
                player.GetComponent<PlayerMovement>().ClearedChallenge();
            }
        }
    }
}
