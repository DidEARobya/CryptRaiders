using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSceneScript : MonoBehaviour
{
    public CombatScript combatScript;
    public TeamScript playerTeam, enemyTeam;

    public GameObject continueButton, retryButton;
    public GameObject victoryScreen, vicCharacter1Model, vicCharacter2Model, vicCharacter3Model, vicCharacter4Model, vicCharacter1Name, vicCharacter2Name, vicCharacter3Name, vicCharacter4Name;
    public GameObject defeatScreen, defCharacter1Model, defCharacter2Model, defCharacter3Model, defCharacter4Model, defCharacter1Name, defCharacter2Name, defCharacter3Name, defCharacter4Name;

    private GameObject deadObject;
    private bool displayed;
    // Start is called before the first frame update
    void Start()
    {

        playerTeam.InitPlayer(StoreTeamsScript.instance.GetPlayerTeam());
        enemyTeam.InitEnemy(StoreTeamsScript.instance.GetEnemyTeam(), StoreStageScript.instance.GetEnemyLevel());

        combatScript.Init(playerTeam, enemyTeam);

        Object temp;
        temp = Resources.Load("Models/DeadObject");
        deadObject = temp as GameObject;

        victoryScreen.SetActive(false);
        defeatScreen.SetActive(false);

        continueButton.SetActive(false);
        retryButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyTeam.CheckIfDead() == true)
        {
            if (displayed == false)
            {
                DisplayVictoryScreen();
            }
        }
        else if (playerTeam.CheckIfDead() == true)
        {
            if (displayed == false)
            {
                DisplayDefeatScreen();
            }
        }

        if(displayed == true)
        {
            SetAction();
        }
    }

    public void DisplayVictoryScreen()
    {
        combatScript.TogglePause();
        StoreStageScript.instance.GetStageData().SetIsComplete(true);
        ChapterPersistenceObject.instance.SaveChapter();

        GameObject temp;
        List<CharacterControl> team = playerTeam.GetTeam();

        if (team.Count >= 1 && team[0] != null)
        {
            temp = Instantiate(team[0].character.characterModel);
            temp.transform.position = vicCharacter1Model.transform.position;
            temp.transform.SetParent(vicCharacter1Model.transform);

            vicCharacter1Name.GetComponent<TextMeshPro>().text = team[0].character.characterData.characterName;

            if (team[0].GetState() == characterStates.DEAD)
            {
                GameObject dead = Instantiate(deadObject);

                dead.transform.position = vicCharacter1Model.transform.position + new Vector3(-0.85f, 0.35f);
            }
        }
        if (team.Count >= 2 && team[1] != null)
        {
            temp = Instantiate(team[1].character.characterModel);
            temp.transform.position = vicCharacter2Model.transform.position;
            temp.transform.SetParent(vicCharacter2Model.transform);

            vicCharacter2Name.GetComponent<TextMeshPro>().text = team[1].character.characterData.characterName;

            if (team[1].GetState() == characterStates.DEAD)
            {
                GameObject dead = Instantiate(deadObject);

                dead.transform.position = vicCharacter2Model.transform.position + new Vector3(-0.85f, 0.35f);
            }
        }
        if (team.Count >= 3 && team[2] != null)
        {
            temp = Instantiate(team[2].character.characterModel);
            temp.transform.position = vicCharacter3Model.transform.position;
            temp.transform.SetParent(vicCharacter3Model.transform);

            vicCharacter3Name.GetComponent<TextMeshPro>().text = team[2].character.characterData.characterName;

            if (team[2].GetState() == characterStates.DEAD)
            {
                GameObject dead = Instantiate(deadObject);

                dead.transform.position = vicCharacter3Model.transform.position + new Vector3(-0.85f, 0.35f);
            }
        }
        if (team.Count >= 4 && team[3] != null)
        {
            temp = Instantiate(team[3].character.characterModel);
            temp.transform.position = vicCharacter4Model.transform.position;
            temp.transform.SetParent(vicCharacter4Model.transform);

            vicCharacter4Name.GetComponent<TextMeshPro>().text = team[3].character.characterData.characterName;

            if (team[3].GetState() == characterStates.DEAD)
            {
                GameObject dead = Instantiate(deadObject);

                dead.transform.position = vicCharacter4Model.transform.position + new Vector3(-0.85f, 0.35f);
            }
        }

        victoryScreen.SetActive(true);
        continueButton.SetActive(true);
        retryButton.SetActive(true);

        displayed = true;
    }
    public void DisplayDefeatScreen()
    {
        combatScript.TogglePause();

        GameObject temp;
        List<CharacterControl> team = playerTeam.GetTeam();

        if (team.Count >= 1 && team[0] != null)
        {
            temp = Instantiate(team[0].character.characterModel);
            temp.transform.position = defCharacter1Model.transform.position;
            temp.transform.SetParent(defCharacter1Model.transform);

            defCharacter1Name.GetComponent<TextMeshPro>().text = team[0].character.characterData.characterName;

            if (team[0].GetState() == characterStates.DEAD)
            {
                GameObject dead = Instantiate(deadObject);

                dead.transform.position = defCharacter1Model.transform.position + new Vector3(-0.85f, 0.35f);
            }
        }
        if (team.Count >= 2 && team[1] != null)
        {
            temp = Instantiate(team[1].character.characterModel);
            temp.transform.position = defCharacter2Model.transform.position;
            temp.transform.SetParent(defCharacter2Model.transform);

            defCharacter2Name.GetComponent<TextMeshPro>().text = team[1].character.characterData.characterName;

            if (team[1].GetState() == characterStates.DEAD)
            {
                GameObject dead = Instantiate(deadObject);

                dead.transform.position = defCharacter2Model.transform.position + new Vector3(-0.85f, 0.35f);
            }
        }
        if (team.Count >= 3 && team[2] != null)
        {
            temp = Instantiate(team[2].character.characterModel);
            temp.transform.position = defCharacter3Model.transform.position;
            temp.transform.SetParent(defCharacter3Model.transform);

            defCharacter3Name.GetComponent<TextMeshPro>().text = team[2].character.characterData.characterName;

            if (team[2].GetState() == characterStates.DEAD)
            {
                GameObject dead = Instantiate(deadObject);

                dead.transform.position = defCharacter3Model.transform.position + new Vector3(-0.85f, 0.35f);
            }
        }
        if (team.Count >= 4 && team[3] != null)
        {
            temp = Instantiate(team[3].character.characterModel);
            temp.transform.position = defCharacter4Model.transform.position;
            temp.transform.SetParent(defCharacter4Model.transform);

            defCharacter4Name.GetComponent<TextMeshPro>().text = team[3].character.characterData.characterName;

            if (team[3].GetState() == characterStates.DEAD)
            {
                GameObject dead = Instantiate(deadObject);

                dead.transform.position = defCharacter4Model.transform.position + new Vector3(-0.85f, 0.35f);
            }
        }

        defeatScreen.SetActive(true);
        continueButton.SetActive(true);
        retryButton.SetActive(true);

        displayed = true;
    }
    private void SetAction()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 rayStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

            if (hitData.collider && hitData.collider.gameObject == continueButton)
            {
                SceneManager.LoadScene(3);
            }
            else if (hitData.collider && hitData.collider.gameObject == retryButton)
            {
                RestartScene();
            }
        }
    }

    private void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
