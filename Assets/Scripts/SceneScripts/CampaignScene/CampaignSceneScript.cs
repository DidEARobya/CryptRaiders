using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignSceneScript : MonoBehaviour
{
    private new Camera camera;
    private SceneState state;

    public GameObject closeButton;
    public GameObject chapterOne, chapterTwo, chapterThree, chapterFour, chapterFive, chapterSix;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("StoreStageScript"))
        {
            Destroy(GameObject.Find("StoreStageScript").gameObject);
        }
        if (GameObject.Find("StoreTeamsScript"))
        {
            Destroy(GameObject.Find("StoreTeamsScript").gameObject);
        }
        if (GameObject.Find("ChapterPersistenceObject"))
        {
            Destroy(GameObject.Find("ChapterPersistenceObject").gameObject);
        }

        camera = Camera.main;
        state = SceneState.CAMPAIGN;
    }

    // Update is called once per frame
    void Update()
    {
        SetPage();

        if(state == SceneState.HOME)
        {
            SceneManager.LoadScene(1);
        }
            
    }
    private void SetPage()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

            if(hitData.collider && hitData.collider.gameObject == closeButton)
            {
                state = SceneState.HOME;
            }
            else if (hitData.collider && hitData.collider.gameObject == chapterOne)
            {
                SceneManager.LoadScene(4);
            }
            else if (hitData.collider && hitData.collider.gameObject == chapterTwo)
            {
                Debug.Log("chapterTwo");
            }
            else if (hitData.collider && hitData.collider.gameObject == chapterThree)
            {
                Debug.Log("chapterThree");
            }
            else if (hitData.collider && hitData.collider.gameObject == chapterFour)
            {
                Debug.Log("chapterFour");
            }
            else if (hitData.collider && hitData.collider.gameObject == chapterFive)
            {
                Debug.Log("chapterFive");
            }
            else if (hitData.collider && hitData.collider.gameObject == chapterSix)
            {
                Debug.Log("chapterSix");
            }
        }
    }
}
