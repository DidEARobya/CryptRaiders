using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterPageDisplay : MonoBehaviour
{
    private CharacterControl currentControl;
    private CharacterObject currentObject;

    private CharacterControl target;
    private SkillObject targetSkill, currentSkill;

    public GameObject characterDisplay, border, closeButton;
    public GameObject characterModel, characterName, characterRole, healthText, defText, atkText, spdText, skillOnePos, skillTwoPos, skillThreePos, skillDescription, skillCooldown, skillScaleType;
    private GameObject skillOneModel, skillTwoModel, skillThreeModel;

    private float delay, timer;
    public bool active;
    // Start is called before the first frame update
    void Start()
    {
        characterDisplay.SetActive(false);
        timer = 0;
        delay = 1f;
        active = false;
    }

    private void Init(CharacterControl control)
    {
        characterDisplay.SetActive(true);
        active = true;

        currentControl = control;
        currentObject = currentControl.character;

        CharacterData temp = currentObject.characterData;

        characterModel.GetComponent<SpriteRenderer>().material = currentObject.characterModel.GetComponent<SpriteRenderer>().sharedMaterial;
        characterName.GetComponent<TextMeshPro>().text = temp.characterName;
        characterRole.GetComponent<TextMeshPro>().text = temp.roleType.ToString();

        healthText.GetComponent<TextMeshPro>().text = "Base HP: " + temp.baseHealth.ToString();
        defText.GetComponent<TextMeshPro>().text = "Base DEF: " + temp.baseDefense.ToString();
        atkText.GetComponent<TextMeshPro>().text = "Base ATK: " + temp.baseAttack.ToString();
        spdText.GetComponent<TextMeshPro>().text = "Base SPD: " + temp.baseSpeed.ToString();

        LoadSkills(currentObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (active == false)
        {
            if (skillDescription.GetComponent<TextMeshPro>().text != " ")
            {
                ClearSkillDisplay();
            }

            if (SetTarget() != null)
            {
                Init(target);
            }
        }

        if (active == true)
        {
            SetAction();
            SetSkill();

            if (targetSkill != null)
            {
                if (targetSkill != currentSkill)
                {
                    currentSkill = targetSkill;

                    UpdateSkillDisplay(currentSkill);
                }
            }
        }
    }
    private CharacterControl SetTarget()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("test2");
            Vector3 rayStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

            if (hitData.collider && hitData.collider.GetComponentInParent<CharacterControl>())
            {
                Debug.Log("test3");
                timer += Time.deltaTime;

                if (timer >= delay)
                {
                    target = hitData.transform.gameObject.GetComponentInParent<CharacterControl>();
                    return target;
                }
            }
        }
        else
        {
            timer = 0;
        }

        return null;
    }
    private void SetAction()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 rayStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

            if (hitData.collider && hitData.collider.gameObject == closeButton)
            {
                characterDisplay.SetActive(false);
                active = false;
            }
        }
    }
    private void UpdateSkillDisplay(SkillObject skill)
    {
        skillDescription.GetComponent<TextMeshPro>().text = skill.skillData.skillDescription;
        skillCooldown.GetComponent<TextMeshPro>().text = "Cooldown: " + skill.skillData.skillCooldown.ToString();
        skillScaleType.GetComponent<TextMeshPro>().text = "Scales from: " + skill.skillData.skillScaleType.ToString();
    }
    private void ClearSkillDisplay()
    {
        skillDescription.GetComponent<TextMeshPro>().text = " ";
        skillCooldown.GetComponent<TextMeshPro>().text = " ";
        skillScaleType.GetComponent<TextMeshPro>().text = " ";
    }
    private SkillObject SetSkill()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 rayStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

        if (data.skillOne != null)
        {
            LoadSkillOne(data.skillOne);
        }
        if (data.skillTwo != null)
        {
            LoadSkillTwo(data.skillTwo);
        }
        if (data.skillThree != null)
        {
            LoadSkillThree(data.skillThree);
        }

    }
    private void LoadSkillOne(SkillObject skill)
    {
        skillOneModel = Instantiate(skill.skillData.skillIcon);
        skillOneModel.AddComponent<SkillReference>();
        skillOneModel.GetComponent<SkillReference>().SetSkillReference(skill);
        skillOneModel.transform.position = skillOnePos.transform.position;
        skillOneModel.transform.SetParent(skillOnePos.transform);
    }

    private void LoadSkillTwo(SkillObject skill)
    {
        skillTwoModel = Instantiate(skill.skillData.skillIcon);
        skillTwoModel.AddComponent<SkillReference>();
        skillTwoModel.GetComponent<SkillReference>().SetSkillReference(skill);
        skillTwoModel.transform.position = skillTwoPos.transform.position;
        skillTwoModel.transform.SetParent(skillTwoPos.transform);
    }

    private void LoadSkillThree(SkillObject skill)
    {
        skillThreeModel = Instantiate(skill.skillData.skillIcon);
        skillThreeModel.AddComponent<SkillReference>();
        skillThreeModel.GetComponent<SkillReference>().SetSkillReference(skill);
        skillThreeModel.transform.position = skillThreePos.transform.position;
        skillThreeModel.transform.SetParent(skillThreePos.transform);
    }
}
