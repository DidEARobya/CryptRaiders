using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class TeamSelectionScript : MonoBehaviour
{
    private new Camera camera;
    public CharacterPageDisplay characterDisplay;
    public GameObject closeButton, continueButton, firstCharacterPos, playerTeam, enemyTeam;
    private GameObject pTeamOne, pTeamTwo, pTeamThree, pTeamFour, eTeamOne, eTeamTwo, eTeamThree, eTeamFour;
    private SceneState state;

    private List<CharacterObject> enemyObjects, playerObjects;
    private List<PlayerCharacterData> characterData;
    private List<GameObject> modelObjects, dataObjects;

    private GameObject[] removeObjects;

    private CharacterControl target, remove;
    private CharacterObject targetData, removeData;

    private bool teamSet;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        modelObjects = new List<GameObject>();
        dataObjects = new List<GameObject>();
        characterData = PlayerIndex.instance.GetPlayerCharacters();
        removeObjects = new GameObject[4];

        if(playerTeam != null)
        {
            pTeamOne = playerTeam.transform.Find("Pos1").gameObject;
            pTeamTwo = playerTeam.transform.Find("Pos2").gameObject;
            pTeamThree = playerTeam.transform.Find("Pos3").gameObject;
            pTeamFour = playerTeam.transform.Find("Pos4").gameObject;
        }
        if(enemyTeam != null)
        {
            eTeamOne = enemyTeam.transform.Find("Pos1").gameObject;
            eTeamTwo = enemyTeam.transform.Find("Pos2").gameObject;
            eTeamThree = enemyTeam.transform.Find("Pos3").gameObject;
            eTeamFour = enemyTeam.transform.Find("Pos4").gameObject;

            //if (StoreStageScript.instance.GetStageData() != null)
            //{
            if (StoreStageScript.instance.GetEnemyTeam() != null)
            {
                enemyObjects = StoreStageScript.instance.GetEnemyTeam();

                if (enemyObjects != null)
                {
                    if(eTeamOne != null)
                    {
                        CharacterObject temp;

                        if (enemyObjects.Count >= 1 && enemyObjects[0] != null)
                        {   
                            temp = enemyObjects[0];
                            eTeamOne.GetComponent<CharacterControl>().character = temp;
                            eTeamOne.GetComponent<CharacterControl>().IndexInit();

                            eTeamOne.transform.Find(temp.characterData.characterName + "(Clone)").gameObject.transform.localScale += new Vector3(0.5f,0.5f);
                        }
                        if (enemyObjects.Count >= 2 && enemyObjects[1] != null)
                        {
                            temp = enemyObjects[1];
                            eTeamTwo.GetComponent<CharacterControl>().character = temp;
                            eTeamTwo.GetComponent<CharacterControl>().IndexInit();

                            eTeamTwo.transform.Find(temp.characterData.characterName + "(Clone)").gameObject.transform.localScale += new Vector3(0.5f, 0.5f);
                        }
                        if (enemyObjects.Count >= 3 && enemyObjects[2] != null)
                        {
                            temp = enemyObjects[2];
                            eTeamThree.GetComponent<CharacterControl>().character = temp;
                            eTeamThree.GetComponent<CharacterControl>().IndexInit();

                            eTeamThree.transform.Find(temp.characterData.characterName + "(Clone)").gameObject.transform.localScale += new Vector3(0.5f, 0.5f);
                        }
                        if (enemyObjects.Count >= 4 && enemyObjects[3] != null)
                        {
                            temp = enemyObjects[3];
                            eTeamFour.GetComponent<CharacterControl>().character = temp;
                            eTeamFour.GetComponent<CharacterControl>().IndexInit();

                            eTeamFour.transform.Find(temp.characterData.characterName + "(Clone)").gameObject.transform.localScale += new Vector3(0.5f, 0.5f);
                        }
                    }
                }
            }
            //}
        }

        Index();
        IndexDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterDisplay.active == false)
        {
            if (SetTarget() != null)
            {
                SelectCharacter();
            }
            if (SetRemove() != null)
            {
                RemoveCharacter();
            }

            if (state == SceneState.BATTLE)
            {
                state = SceneState.TEAMSELECT;
                ConfirmTeams();
            }
        }
    }
    public void Index()
    {
        for (int i = 0; i < characterData.Count; i++)
        {
            GameObject dataObject = new GameObject();
            dataObject.transform.parent = this.transform;
            dataObject.transform.name = characterData[i].characterData.characterName + " DataObject";

            CharacterObject character = dataObject.AddComponent<CharacterObject>(); ;
            character.PlayerIndexInit(characterData[i]);

            dataObjects.Add(dataObject);

        }
    }
    public void IndexDisplay()
    {
        if (modelObjects.Count != 0)
        {
            for (int i = 0; i < modelObjects.Count; i++)
            {
                Destroy(modelObjects[i]);
            }

            modelObjects.Clear();
        }

        //resX = ResolutionScript.instance.xMin;
        //resY = ResolutionScript.instance.yMin;

        float xPos = firstCharacterPos.transform.position.x;
        float yPos = firstCharacterPos.transform.position.y;

        bool temp = false;
        bool first = false;

        for (int i = 0; i < dataObjects.Count; i++)
        {
            GameObject modelObject = new GameObject();
            CharacterControl character;

            character = modelObject.AddComponent<CharacterControl>();
            character.character = dataObjects[i].GetComponent<CharacterObject>();
            modelObject.name = character.character.characterData.characterName + " Model";
            modelObject.transform.parent = this.transform;

            if (first == false)
            {
                modelObject.transform.position = firstCharacterPos.transform.position;
                first = true;
            }
            else
            {
                if (temp == false)
                {
                    yPos += -1.3f;

                    modelObject.transform.position = new Vector3(xPos, yPos, 0);
                    temp = false;
                }
                else
                {
                    yPos = firstCharacterPos.transform.position.x;
                    xPos = xPos + 1.5f;


                    modelObject.transform.position = new Vector3(xPos, yPos, 0);
                    temp = true;
                }
            }

            modelObjects.Add(modelObject);
            character.IndexInit();
        }
    }
    public void SelectCharacter()
    {
        if (target != null)
        {
            if (targetData != target.character)
            {
                targetData = target.character;

                if (targetData != null)
                {
                    if (pTeamOne.GetComponent<CharacterControl>().character == null)
                    {
                        pTeamOne.GetComponent<CharacterControl>().character = targetData;
                        pTeamOne.GetComponent<CharacterControl>().IndexInit();

                        pTeamOne.transform.Find(targetData.characterData.characterName + "(Clone)").gameObject.transform.localScale += new Vector3(0.5f,0.5f);
                        removeObjects[0] = target.gameObject;

                        target.gameObject.SetActive(false);

                        target = null;
                        targetData = null;
                        return;
                    }
                    else if (pTeamTwo.GetComponent<CharacterControl>().character == null)
                    {
                        pTeamTwo.GetComponent<CharacterControl>().character = targetData;
                        pTeamTwo.GetComponent<CharacterControl>().IndexInit();

                        pTeamTwo.transform.Find(targetData.characterData.characterName + "(Clone)").gameObject.transform.localScale += new Vector3(0.5f, 0.5f);
                        removeObjects[1] = target.gameObject;

                        target.gameObject.SetActive(false);

                        target = null;
                        targetData = null;
                        return;
                    }
                    else if (pTeamThree.GetComponent<CharacterControl>().character == null)
                    {
                        pTeamThree.GetComponent<CharacterControl>().character = targetData;
                        pTeamThree.GetComponent<CharacterControl>().IndexInit();

                        pTeamThree.transform.Find(targetData.characterData.characterName + "(Clone)").gameObject.transform.localScale += new Vector3(0.5f, 0.5f);
                        removeObjects[2] = target.gameObject;

                        target.gameObject.SetActive(false);

                        target = null;
                        targetData = null;
                        return;
                    }
                    else if (pTeamFour.GetComponent<CharacterControl>().character == null)
                    {
                        pTeamFour.GetComponent<CharacterControl>().character = targetData;
                        pTeamFour.GetComponent<CharacterControl>().IndexInit();

                        pTeamFour.transform.Find(targetData.characterData.characterName + "(Clone)").gameObject.transform.localScale += new Vector3(0.5f, 0.5f);
                        removeObjects[3] = target.gameObject;

                        target.gameObject.SetActive(false);

                        target = null;
                        targetData = null;
                        return;
                    }
                }
            }
        }
    }
    public void RemoveCharacter()
    {
        if (removeData != remove.character)
        {
            if (remove.character != null)
            {
                removeData = remove.character;
            }
            else
            {
                remove = null;
                removeData = null;
            }
        }

        if (removeData == pTeamOne.GetComponent<CharacterControl>().character)
        {
            pTeamOne.GetComponent<CharacterControl>().character = null;
            pTeamOne.GetComponent<CharacterControl>().DestroyCharacter();

            removeObjects[0].SetActive(true);
            removeObjects[0] = null;

            remove = null;
            removeData = null;
            return;
        }
        else if (removeData == pTeamTwo.GetComponent<CharacterControl>().character)
        {
            pTeamTwo.GetComponent<CharacterControl>().character = null;
            pTeamTwo.GetComponent<CharacterControl>().DestroyCharacter();

            removeObjects[1].SetActive(true);
            removeObjects[1] = null;

            remove = null;
            removeData = null;
            return;
        }
        else if (removeData == pTeamThree.GetComponent<CharacterControl>().character)
        {
            pTeamThree.GetComponent<CharacterControl>().character = null;
            pTeamThree.GetComponent<CharacterControl>().DestroyCharacter();

            removeObjects[2].SetActive(true);
            removeObjects[2] = null;

            remove = null;
            removeData = null;
            return;
        }
        else if (removeData == pTeamFour.GetComponent<CharacterControl>().character)
        {
            pTeamFour.GetComponent<CharacterControl>().character = null;
            pTeamFour.GetComponent<CharacterControl>().DestroyCharacter();

            removeObjects[3].SetActive(true);
            removeObjects[3] = null;

            remove = null;
            removeData = null;
            return;
        }
    }

    private void ConfirmTeams()
    {
        List<PlayerCharacterData> pTemp = new List<PlayerCharacterData>();
        List<CharacterData> eTemp = new List<CharacterData>();

        if(pTeamOne.GetComponent<CharacterControl>().character != null)
        {
            pTemp.Add(pTeamOne.GetComponent<CharacterControl>().character.GetPlayerData());
        }
        if (pTeamTwo.GetComponent<CharacterControl>().character != null)
        {
            pTemp.Add(pTeamTwo.GetComponent<CharacterControl>().character.GetPlayerData());
        }
        if (pTeamThree.GetComponent<CharacterControl>().character != null)
        {
            pTemp.Add(pTeamThree.GetComponent<CharacterControl>().character.GetPlayerData());
        }
        if (pTeamFour.GetComponent<CharacterControl>().character != null)
        {
            pTemp.Add(pTeamFour.GetComponent<CharacterControl>().character.GetPlayerData());
        }

        if(pTemp.Count != 0)
        {
            StoreTeamsScript.instance.SetPlayerTeam(pTemp);
            teamSet = true;
        }

        if (eTeamOne.GetComponent<CharacterControl>().character)
        {
            eTemp.Add(Instantiate(eTeamOne.GetComponent<CharacterControl>().character.characterData));
        }
        if (eTeamTwo.GetComponent<CharacterControl>().character)
        {
            eTemp.Add(Instantiate(eTeamTwo.GetComponent<CharacterControl>().character.characterData));
        }
        if (eTeamThree.GetComponent<CharacterControl>().character)
        {
            eTemp.Add(Instantiate(eTeamThree.GetComponent<CharacterControl>().character.characterData));
        }
        if (eTeamFour.GetComponent<CharacterControl>().character)
        {
            eTemp.Add(Instantiate(eTeamFour.GetComponent<CharacterControl>().character.characterData));
        }

        if(eTemp.Count != 0)
        {
            StoreTeamsScript.instance.SetEnemyTeam(eTemp);
        }

        if(teamSet == true)
        {
            SceneManager.LoadScene(6);
        }
    }
    private CharacterControl SetTarget()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);


            if (hitData.collider && hitData.collider.GetComponentInParent<CharacterControl>() && hitData.collider.GetComponentInParent<CharacterControl>().character != null)
            {
                GameObject temp = hitData.collider.transform.parent.gameObject;

                if (temp.name.Contains("Pos") || temp.name.Contains("enemy"))
                {
                    return null;
                }
                else
                {
                    if (hitData.collider.name.Contains("Pos") || hitData.collider.name.Contains("enemy"))
                    {
                        return null;
                    }

                    target = hitData.transform.gameObject.GetComponentInParent<CharacterControl>();
                    return target;
                }
            }
            else if (hitData.collider && hitData.collider.gameObject == continueButton)
            {
                state = SceneState.BATTLE;
            }
            else if (hitData.collider && hitData.collider.gameObject == closeButton)
            {
                if (GameObject.Find("ChapterPersistenceObject"))
                {
                    Destroy(GameObject.Find("ChapterPersistenceObject").gameObject);
                }
                SceneManager.LoadScene(4);
            }
        }

        return null;
    }
    public CharacterControl SetRemove()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

            if (hitData.collider && hitData.collider.GetComponentInParent<CharacterControl>() && hitData.collider.GetComponentInParent<CharacterControl>().character != null)
            {
                GameObject temp = hitData.collider.transform.parent.gameObject;

                if (temp.name.Contains("enemy"))
                {
                    return null;
                }
                else
                {
                    if (hitData.collider.name.Contains("enemy"))
                    {
                        return null;
                    }

                    remove = hitData.transform.gameObject.GetComponentInParent<CharacterControl>();

                    return remove;
                }
            }
        }

        return null;
    }
}
