using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public enum IndexPage
{
    STATS,
    SKILLS
}
public class CharactersSceneMain : MonoBehaviour
{
    private new Camera camera;
    public GameObject homeButton;
    private SceneState state;

    private List<PlayerCharacterData> characterData;
    private List<GameObject> modelObjects, dataObjects;

    public GameObject characterModel;
    public TextMeshPro characterNameText, characterLevelText, characterRoleText;

    public GameObject statsButton, skillsButton;
    private IndexPage indexPage;

    public GameObject statsDisplay;
    public TextMeshPro hpText, hpGearText, defText, defGearText, atkText, atkGearText, spdText, spdGearText;

    public GameObject skillsDisplay, skillOnePos, skillTwoPos, skillThreePos;
    private GameObject skillOneModel, skillTwoModel, skillThreeModel;
    public TextMeshPro skillNameText, skillDescriptionText, skillCooldownText, skillScalingText;

    private CharacterControl target, currentTarget;
    private SkillObject targetSkill, currentSkill;
    private CharacterObject currentObj;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        modelObjects = new List<GameObject>();
        dataObjects = new List<GameObject>();
        characterData =  PlayerIndex.instance.GetPlayerCharacters();

        Index();
        IndexDisplay();

        state = SceneState.CHARACTERS;
        indexPage = IndexPage.STATS;

        if(modelObjects.Count > 0)
        {
            target = modelObjects[0].GetComponent<CharacterControl>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetPage();

        if(state == SceneState.HOME)
        {
            SceneManager.LoadScene(1);
        }
        if(indexPage == IndexPage.STATS)
        {
            statsDisplay.SetActive(true);
            skillsDisplay.SetActive(false);
        }
        else if(indexPage == IndexPage.SKILLS)
        {
            statsDisplay.SetActive(false);
            skillsDisplay.SetActive(true);
        }

        SetTarget();

        if(target != null)
        {
            if(target != currentTarget)
            {
                UpdateTarget();
            }

            SetSkill();

            if (targetSkill != null)
            {
                if (targetSkill != currentSkill)
                {
                    UpdateSkillTarget();
                }
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

        float xPos = -9.15f;
        float yPos = 3.5f;

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
                modelObject.transform.position = new Vector3(xPos, yPos, 0);
                first = true;
            }
            else
            {
                if (temp == false)
                {
                    xPos += 2;

                    modelObject.transform.position = new Vector3(xPos, yPos, 0);
                    temp = true;
                }
                else
                {
                    xPos = -9.15f;
                    yPos = yPos - 1.5f;

                    modelObject.transform.position = new Vector3(xPos, yPos, 0);
                    temp = false;
                }
            }

            modelObjects.Add(modelObject);
            character.IndexInit();
        }
    }

    private void UpdateTarget()
    {
        currentTarget = target;
        currentObj = currentTarget.character;

        characterModel.GetComponent<SpriteRenderer>().material = currentObj.characterModel.GetComponent<SpriteRenderer>().sharedMaterial;

        characterNameText.text = currentObj.characterData.characterName;
        characterLevelText.text = "Lvl: " + currentObj.GetLevel().ToString();
        characterRoleText.text = currentObj.characterData.roleType.ToString();

        hpText.text = "HP: " + currentObj.GetCombatHealth().ToString();
        hpGearText.text = "0";
        defText.text = "DEF: " + currentObj.GetCombatDefense().ToString();
        defGearText.text = "0";
        atkText.text = "ATK: " + currentObj.GetCombatAttack().ToString();
        atkGearText.text = "0";
        spdText.text = "SPD: " + currentObj.GetCombatSpeed().ToString();
        spdGearText.text = "0";

        if (currentSkill != null)
        {
            skillNameText.text = " ";
            skillDescriptionText.text = " ";
            skillCooldownText.text = " ";
            skillScalingText.text = " ";
        }

        LoadSkills(currentObj);
    }

    private void UpdateSkillTarget()
    {
        currentSkill = targetSkill;

        skillNameText.text = currentSkill.skillData.skillName;
        skillDescriptionText.text = currentSkill.skillData.skillDescription;
        skillCooldownText.text = "Skill Cooldown: " + currentSkill.skillData.skillCooldown.ToString();
        skillScalingText.text = "Skill Scaling: " + currentSkill.skillData.skillScaleType.ToString();
    }
    private void SetPage()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

            if (hitData.collider && hitData.collider.gameObject == statsButton)
            {
                indexPage = IndexPage.STATS;
            }
            else if (hitData.collider && hitData.collider.gameObject == skillsButton)
            {
                indexPage = IndexPage.SKILLS;
            }
            else if (hitData.collider && hitData.collider.gameObject == homeButton)
            {
                state = SceneState.HOME;
            }
        }
    }
    private CharacterControl SetTarget()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

            if (hitData.collider && hitData.collider.GetComponentInParent<CharacterControl>())
            {
                target = hitData.transform.gameObject.GetComponentInParent<CharacterControl>();
                return target;
            }
        }

        return null;
    }
    private SkillObject SetSkill()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

            if (hitData.collider && hitData.collider.GetComponent<SkillReference>())
            {
                targetSkill = hitData.collider.GetComponent<SkillReference>().skill;
                return targetSkill;
            }
        }

        return null;
    }
    private void LoadSkills(CharacterObject data)
    {
        if (skillOneModel != null)
        {
            Destroy(skillOneModel);
        }
        if (skillTwoModel != null)
        {
            Destroy(skillTwoModel);
        }
        if (skillThreeModel != null)
        {
            Destroy(skillThreeModel);
        }

        if(data.skillOne != null)
        {
            LoadSkillOne(data.skillOne);
        }
        if(data.skillTwo != null)
        {
            LoadSkillTwo(data.skillTwo);
        }
        if(data.skillThree != null)
        {
            LoadSkillThree(data.skillThree);
        }

    }
    private void LoadSkillOne(SkillObject skill)
    {
        skillOneModel = Instantiate(skill.activeSkill.skillIcon);
        skillOneModel.AddComponent<SkillReference>();
        skillOneModel.GetComponent<SkillReference>().SetSkillReference(skill);
        skillOneModel.transform.position = skillOnePos.transform.position;
        skillOneModel.transform.SetParent(skillOnePos.transform);
    }

    private void LoadSkillTwo(SkillObject skill)
    {
        skillTwoModel = Instantiate(skill.activeSkill.skillIcon);
        skillTwoModel.AddComponent<SkillReference>();
        skillTwoModel.GetComponent<SkillReference>().SetSkillReference(skill);
        skillTwoModel.transform.position = skillTwoPos.transform.position;
        skillTwoModel.transform.SetParent(skillTwoPos.transform);
    }

    private void LoadSkillThree(SkillObject skill)
    {
        skillThreeModel = Instantiate(skill.activeSkill.skillIcon);
        skillThreeModel.AddComponent<SkillReference>();
        skillThreeModel.GetComponent<SkillReference>().SetSkillReference(skill);
        skillThreeModel.transform.position = skillThreePos.transform.position;
        skillThreeModel.transform.SetParent(skillThreePos.transform);
    }
}
