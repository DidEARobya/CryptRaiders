using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CampaignChapterScript : MonoBehaviour
{
    private new Camera camera;
    public ChapterPersistenceObject chapterPersistence;

    public List<GameObject> stageIcons;
    public GameObject borderTop, borderBottom, closeButton, continueButton;

    private SceneState state;
    private CampaignStageObject stageObject;
    private List<CampaignStageData> stageData;

    private Vector3 mousePosition, cameraDif;
    private float cameraHalfHeight, borderWidth, minY, maxY;
    private bool drag;
    private int dragCheck;

    // Start is called before the first frame update
    void Awake()
    {
        if(GameObject.Find("StoreTeamsScript"))
        {
            Destroy(GameObject.Find("StoreTeamsScript").gameObject);
        }

        camera = Camera.main;
        cameraHalfHeight = camera.orthographicSize;

        borderWidth = borderTop.transform.localScale.x;

        float temp = borderWidth / 2;

        minY = (borderBottom.transform.position.y + cameraHalfHeight) - temp;
        maxY = (borderTop.transform.position.y - cameraHalfHeight) + temp;

        stageData = chapterPersistence.stageData;

        if (stageData != null)
        {
            for (int i = 0; i < stageIcons.Count; i++)
            {
                if (stageData.Count >= (i + 1) && stageData[i] != null)
                {
                    stageIcons[i].GetComponent<CampaignStageObject>().stageData = stageData[i];
                }
                else
                {
                    stageIcons[i].GetComponent<CampaignStageObject>().stageData = ScriptableObject.CreateInstance<CampaignStageData>();
                    chapterPersistence.AddStage(stageIcons[i].GetComponent<CampaignStageObject>().stageData);
                }
            }
        }
        else
        {
            stageData = new List<CampaignStageData>();

            for (int i = 0; i < stageIcons.Count; i++)
            {
                stageIcons[i].GetComponent<CampaignStageObject>().stageData = ScriptableObject.CreateInstance<CampaignStageData>();

                stageData.Add(stageIcons[i].GetComponent<CampaignStageObject>().stageData);
                stageData[i].SetStageName(stageIcons[i].name);

                chapterPersistence.AddStage(stageIcons[i].GetComponent<CampaignStageObject>().stageData);
            }

            chapterPersistence.Init(SceneManager.GetActiveScene().name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetStage();
        DragCheck();

        if(dragCheck == 2)
        {
            CameraDrag();
        }

        if(state == SceneState.CAMPAIGN)
        {
            chapterPersistence.SaveChapter();
            SceneManager.LoadScene(3);
        }
        else if (state == SceneState.TEAMSELECT)
        {
            chapterPersistence.SaveChapter();
            SceneManager.LoadScene(5);
        }
    }

    private void SetStage()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

            if (hitData.collider && hitData.collider.gameObject.GetComponent<CampaignStageObject>())
            {
                for(int i = 0; i < stageIcons.Count; i++)
                {
                    if(hitData.collider.gameObject == stageIcons[i])
                    {
                        if(hitData.collider.gameObject.GetComponent<CampaignStageObject>().isActive == true)
                        {
                            CampaignStageObject temp = hitData.collider.gameObject.GetComponent<CampaignStageObject>();
                            stageObject = temp;
                            StoreStageScript.instance.StoreStage(temp.stageData, temp.enemyTeam, temp.enemyLevel);
                        }
                    }
                }
            }
            else if (hitData.collider && hitData.collider.gameObject == closeButton)
            {
                state = SceneState.CAMPAIGN;
            }
            else if (hitData.collider && hitData.collider.gameObject == continueButton)
            {
                if(StoreStageScript.instance.GetEnemyTeam() != null)
                {
                    if(stageObject != null)
                    {
                        for(int i = 0; i < stageIcons.Count; i++)
                        {
                            if (stageIcons[i].GetComponent<CampaignStageObject>() != stageObject && stageIcons[i].GetComponent<CampaignStageObject>().stageNumber == stageObject.stageNumber)
                            {
                                CampaignStageObject temp = stageIcons[i].GetComponent<CampaignStageObject>();
                                temp.SetIsActive(false);
                            }
                        }
                    }

                    state = SceneState.TEAMSELECT;
                }
            }
        }
    }

    private void DragCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

            if (hitData.collider && hitData.collider.gameObject.GetComponent<CampaignStageObject>())
            {
                dragCheck = 1;
            }
            else
            {
                dragCheck = 2;
            }
        }
    }
    private void CameraDrag()
    {
        if (Input.GetMouseButton(0))
        {
            cameraDif = camera.ScreenToWorldPoint(Input.mousePosition) - camera.transform.position;

            if (drag == false)
            {
                drag = true;
                mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }

        if (drag == true)
        {
            Vector3 temp = mousePosition - cameraDif;
            temp.x = 0;

            temp.y = Mathf.Clamp(temp.y, minY, maxY);


            camera.transform.position = temp;
        }
    }
}
