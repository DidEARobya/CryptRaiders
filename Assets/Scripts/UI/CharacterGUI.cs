using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGUI : MonoBehaviour
{
    private GameObject baseBar, baseTMBar;
    private GameObject healthBar, tmBar;
    private GameObject healthObject, healthTextObject, tmObject, deadObject;

    private Object healthTextPrefab;

    private Renderer healthRenderer, tmRenderer;
    private TextMeshPro healthText;

    private CharacterControl characterControl;
    private CharacterObject character;

    private List<EffectBase> effectList;
    private List<GameObject> iconList;

    private float healthBarSize;
    private float baseHealth;
    private float healthBarBounds;
    private float fadeDuration;

    private float tmMax;
    private float tmBarSize;
    private float tmBarBounds;

    private bool effectsInited;
    private bool effectsReady;
    private bool firstEffect;
    private bool isDead;
    private bool hpText;

    // Start is called before the first frame update
    void Start()
    {
        characterControl = this.GetComponentInParent<CharacterControl>();
        effectsReady = false;

        if (characterControl != null)
        {
            character = characterControl.character;

            if(character != null)
            {
                isDead = false;

                SetupHealthBar();
                SetUpDeadObject();
                SetupTMBar();
                SetUpEffects();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(characterControl.CombatCheck() == true)
        {
            if (characterControl.GetState() == characterStates.DEAD)
            {
                deadObject.SetActive(true);
                healthObject.SetActive(false);
                tmObject.SetActive(false);

                isDead = true;
            }
            else
            {
                if (isDead == true)
                {
                    deadObject.SetActive(false);
                    healthObject.SetActive(true);
                    tmObject.SetActive(true);

                    isDead = false;
                }

                UpdateHealthBar();
                UpdateTMBar();
            }
        }
    }

    private void SetupHealthBar()
    {
        Object temp;
        healthObject = new GameObject();

        temp = Resources.Load("Models/BaseHealthBar");
        baseBar = Instantiate(temp) as GameObject;
        baseBar.transform.position += new Vector3(0, 0, 0.5f);
        baseBar.transform.SetParent(healthObject.transform);

        temp = Resources.Load("Models/HealthBar");
        healthBar = Instantiate(temp) as GameObject;
        healthBar.transform.SetParent(healthObject.transform);
        healthBarSize = healthBar.transform.localScale.x;

        healthTextPrefab = Resources.Load("Models/HealthText");
        fadeDuration = 2.0f;

        healthObject.transform.SetParent(this.transform);
        healthObject.name = "HealthObject";

        baseHealth = character.GetCombatHealth();

        healthRenderer = healthBar.GetComponent<Renderer>();
    }
    private void UpdateHealthBar()
    {
        healthObject.transform.localPosition = new Vector3(0f, 0.85f);

        healthBarBounds = healthRenderer.bounds.min.x;
        float percent = (character.GetHealth() / baseHealth);

        Vector3 tempScale = healthBar.transform.localScale;
        Vector3 tempPos = healthBar.transform.position;

        tempScale.x = healthBarSize * percent;

        healthBar.transform.localScale = tempScale;

        float tempBounds = healthRenderer.bounds.min.x;

        float boundsDif = tempBounds - healthBarBounds;
        healthBar.transform.Translate(new Vector3(-boundsDif, 0f, 0f));

        if (percent > 0.7f)
        {
            healthRenderer.material.color = Color.green;
        }
        else if (percent > 0.3f)
        {
            healthRenderer.material.color = Color.yellow;
        }
        else
        {
            healthRenderer.material.color = Color.red;
        }
    }

    public void StartHealthCoroutine(float value, bool dmg)
    {
        StartCoroutine(HPChangeDisplay(value, dmg));
    }
    IEnumerator HPChangeDisplay(float value, bool dmg)
    {
        GameObject tempObject;
        TextMeshPro tempText;

        tempObject = Instantiate(healthTextPrefab) as GameObject;
        tempText = tempObject.GetComponent<TextMeshPro>();
        tempObject.transform.SetParent(healthObject.transform);

        if(hpText == false)
        {
            tempObject.transform.localPosition = new Vector3(-0.175f, -1.6f, 0);
        }
        else
        {
            tempObject.transform.localPosition = new Vector3(-0.175f, -1.85f, 0);
        }

        if (dmg == true)
        {
            tempText.text = "- " + value.ToString();
            tempText.color = Color.red;
        }
        else
        {
            tempText.text = "+ " + value.ToString();
            tempText.color = Color.green;
        }

        tempText.alpha = 1;
        StartCoroutine(HPFadeOut(tempObject));

        hpText = true;

        yield return new WaitForSeconds(2.0f);

        Destroy(tempObject);
        hpText = false;
    }

    IEnumerator HPFadeOut(GameObject obj)
    {
        Vector2 initialPos = obj.transform.position;
        Vector2 targetPos = initialPos + new Vector2(0, 2.0f);

        TextMeshPro text = obj.GetComponent<TextMeshPro>();
        Color initialColor = text.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            obj.transform.position = Vector2.Lerp(initialPos, targetPos, elapsedTime / fadeDuration);
            text.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
           
            yield return null;
        }
    }
    private void SetupTMBar()
    {
        Object temp;
        tmObject = new GameObject();

        temp = Resources.Load("Models/BaseTMBar");
        baseTMBar = Instantiate(temp) as GameObject;
        baseTMBar.transform.position += new Vector3(0, 0, 0.5f);
        baseTMBar.transform.SetParent(tmObject.transform);

        temp = Resources.Load("Models/TMBar");
        tmBar = Instantiate(temp) as GameObject;
        tmBar.transform.SetParent(tmObject.transform);
        tmBarSize = tmBar.transform.localScale.x;

        tmObject.transform.SetParent(this.transform);
        tmObject.name = "TMObject";

        tmMax = CharacterObject.maxTM;

        tmRenderer = tmBar.GetComponent<Renderer>();
    }
    private void UpdateTMBar()
    {
        tmObject.transform.localPosition = new Vector3(0f, 0.65f);

        tmBarBounds = tmRenderer.bounds.min.x;
        float percent = (character.GetTM() / tmMax);

        Vector3 tempScale = tmBar.transform.localScale;
        Vector3 tempPos = tmBar.transform.position;

        tempScale.x = tmBarSize * percent;

        if (tempScale.x > tmBarSize)
        {
            tempScale.x = tmBarSize;
        }

        tmBar.transform.localScale = tempScale;

        float tempBounds = tmRenderer.bounds.min.x;

        float boundsDif = tempBounds - tmBarBounds;
        tmBar.transform.Translate(new Vector3(-boundsDif, 0f, 0f));
    }

    private void SetUpDeadObject()
    {
        Object temp;
        temp = Resources.Load("Models/DeadObject");
        deadObject = Instantiate(temp) as GameObject;
        deadObject.transform.position = characterControl.transform.position + new Vector3(-0.85f, 0.4f, 0); 
        deadObject.transform.SetParent(this.transform);
        deadObject.SetActive(false);
    }
    private void SetUpEffects()
    {
        effectList = new List<EffectBase>();
        iconList = new List<GameObject>();

        effectsInited = true;
        firstEffect = false;
    }

    public void UpdateEffects(List<EffectBase> effects)
    {
        if(effectsInited == true)
        {
            if (effects.Count != iconList.Count)
            {
                SetEffects(effects);
            }
            else
            {
                UpdateEffects();
            }
        }
    }

    private void SetEffects(List<EffectBase> effects)
    {
        firstEffect = false;

        GameObject icon;
        effectList = effects;

        for (int i = 0; i < iconList.Count; i++)
        {
            Destroy(iconList[i].gameObject);
        }

        iconList.Clear();
        

        if (effects.Count != 0)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                icon = Instantiate(effects[i].effectIcon);
                icon.name = effects[i].objectVal.ToString();
                icon.transform.SetParent(this.transform);

                iconList.Add(icon);
            }
        }
    }
    public void UpdateEffects()
    {
        firstEffect = false;

        for (int i = 0; i < iconList.Count; i++)
        {
            if (firstEffect == false)
            {
                if (character.GetControl().PlayerCheck() == true)
                {
                    iconList[i].transform.position = healthObject.transform.position + new Vector3(-0.8f, 0.4f, 0);
                }
                else
                {
                    iconList[i].transform.position = healthObject.transform.position + new Vector3(0.8f, 0.4f, 0);
                }

                firstEffect = true;
            }
            else
            {
                if (character.GetControl().PlayerCheck() == true)
                {
                    iconList[i].transform.position = iconList[i - 1].transform.position + new Vector3(0.5f, 0, 0);
                }
                else
                {
                    iconList[i].transform.position = iconList[i - 1].transform.position + new Vector3(-0.5f, 0, 0);
                }
            }
        }
    }
}
