using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneState
{
    HOME,
    CHARACTERS,
    CAMPAIGN,
    TEAMSELECT,
    BATTLE
}

public class HomeSceneScript : MonoBehaviour
{
    private new Camera camera;
    private SceneState state;

    public GameObject charactersButton, campaignButton;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        state = SceneState.HOME;
    }

    // Update is called once per frame
    void Update()
    {
        SetPage();

        if(state == SceneState.CHARACTERS)
        {
            SceneManager.LoadScene(2);
        }
        else if(state == SceneState.CAMPAIGN)
        {
            SceneManager.LoadScene(3);
        }
    }

    private void SetPage()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

            if (hitData.collider && hitData.collider.gameObject == charactersButton)
            {
                state = SceneState.CHARACTERS;
            }
            else if (hitData.collider && hitData.collider.gameObject == campaignButton)
            {
                state = SceneState.CAMPAIGN;
            }
        }
    }
}
