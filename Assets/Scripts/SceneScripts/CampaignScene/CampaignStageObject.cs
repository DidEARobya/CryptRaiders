using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StageNumber
{
    STAGEONE, 
    STAGETWO, 
    STAGETHREE,
    BOSS
}
public class CampaignStageObject : MonoBehaviour
{
    public List<CharacterObject> enemyTeam;
    public List<CampaignStageObject> nextStage;

    private string stageName;

    public int enemyLevel;
    public GameObject stageIcon;
    private GameObject fadeObject;

    public StageNumber stageNumber;

    public CampaignStageData stageData;

    public bool isActive, isComplete;

    // Start is called before the first frame update
    void Start()
    {
        if (stageData != null)
        {
            if (stageData.GetIsComplete())
            {
                isComplete = stageData.GetIsComplete();

                if(isComplete)
                {
                    for (int i = 0; i < nextStage.Count; i++)
                    {
                        nextStage[i].isActive = true;
                    }
                }
                else
                {
                    for (int i = 0; i < nextStage.Count; i++)
                    {
                        nextStage[i].isActive = false;
                    }
                }
            }
            if(stageData.GetIsActive())
            {
                isActive = stageData.GetIsActive();
            }
        }

        if (stageIcon != null)
        {
            if(stageIcon.name.Contains("StageOne"))
            {
                isActive = true;
                stageNumber = StageNumber.STAGEONE;
            }
            else if (stageIcon.name.Contains("StageTwo"))
            {
                stageNumber = StageNumber.STAGETWO;
            }
            else if (stageIcon.name.Contains("StageThree"))
            {
                stageNumber = StageNumber.STAGETHREE;
            }
            else if (stageIcon.name.Contains("Boss"))
            {
                stageNumber = StageNumber.BOSS;
            }

            if (stageIcon.transform.Find("Fade"))
            {
                fadeObject = stageIcon.transform.Find("Fade").gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(fadeObject != null)
        {
            if(isActive == true)
            {
                fadeObject.SetActive(false);
            }
            else
            {
                fadeObject.SetActive(true);
            }
        }
    }
    public void SetIsActive(bool check)
    {
        isActive = check;
        stageData.SetIsActive(isActive);
    }
}
