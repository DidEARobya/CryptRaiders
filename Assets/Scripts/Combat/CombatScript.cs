using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public TeamScript pTest, eTest;

    private TeamScript playerTeam, enemyTeam;
    private List<CharacterControl> playerList, enemyList, combatList;

    private CharacterControl activeCharacter;
    private List<CharacterControl> readyCharacters;

    private int maxTM;

    private float startdelay;
    private bool started, inited, paused;

    // Start is called before the first frame update
    private void Start()
    {
        //if(pTest != null && eTest != null) 
        //{
        //    startdelay = 0.0f;
        //    started = false;

        //    inited = false;
        //}
    }

    public void Init(TeamScript player, TeamScript enemy)
    {
        combatList = new List<CharacterControl>();
        readyCharacters = new List<CharacterControl>();

        maxTM = CharacterObject.maxTM;

        playerTeam = player;
        enemyTeam = enemy;

        if(playerTeam != null )
        {
            playerList = playerTeam.GetTeam();

            for(int i = 0; i < playerList.Count; i++)
            {
                playerList[i].TogglePlayable();
                playerList[i].ToggleCombat();
                playerList[i].character.characterAI.SetCombatScript(this);
                combatList.Add(playerList[i]);
            }
        }

        if(enemyTeam != null )
        {
            enemyList = enemyTeam.GetTeam();

            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].ToggleCombat();
                enemyList[i].character.characterAI.SetCombatScript(this);
                combatList.Add(enemyList[i]);
            }
        }

        this.AddComponent<SkillDisplay>();
        this.GetComponent<SkillDisplay>().Init(this);
        this.GetComponent<SkillDisplay>().transform.position = new Vector3(0, -1.75f, 0);

        paused = false;
        inited = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (started == false)
        //{
        //    startdelay += Time.deltaTime;

        //    if (startdelay >= 0.5f)
        //    {
        //        Init(pTest, eTest);
        //        started = true;
        //    }
        //}

        if(paused == false)
        {
            if (inited == true && combatList.Count != 0)
            {
                if (activeCharacter == null)
                {
                    TickCharacters();

                    if (readyCharacters.Count != 0)
                    {
                        int temp = readyCharacters.Count - 1;

                        readyCharacters.Sort(SortByTM);
                        SetActiveCharacter(readyCharacters[temp]);
                        readyCharacters.Remove(activeCharacter);
                    }
                }
                else
                {
                    if (activeCharacter.GetState() == characterStates.INACTIVE || activeCharacter.GetState() == characterStates.DEAD)
                    {
                        activeCharacter = null;
                    }
                }
            }
        }
    }

    public CharacterControl GetActiveCharacter()
    {
        return activeCharacter;
    }
    public void SetActiveCharacter(CharacterControl active)
    {
        if (activeCharacter != active)
        {
            if (activeCharacter != null)
            {
                activeCharacter.SetState(characterStates.INACTIVE);
            }

            activeCharacter = active;

            activeCharacter.character.ResetTM();

            if (active.PlayerCheck() == true)
            {
                activeCharacter.SetState(characterStates.ACTIVE);
            }
            else
            {
                activeCharacter.SetState(characterStates.AUTO);
            }
        }
    }
    static int SortByTM(CharacterControl p1, CharacterControl p2)
    {
        return p1.character.GetTM().CompareTo(p2.character.GetTM());
    }

    public void TickCharacters()
    {
        for (int i = 0; i < combatList.Count; i++)
        {
            if (combatList[i].GetState() != characterStates.DEAD)
            {
                combatList[i].character.TickTM();

                if (combatList[i].character.GetTM() > maxTM)
                {
                    if (readyCharacters.Contains(combatList[i]) == false)
                    {
                        readyCharacters.Add(combatList[i]);
                    }
                }
            }
        }
    }

    public void TogglePause()
    {
        paused = !paused;
    }
    public List<CharacterControl> GetPlayerTeam()
    {
        return playerList;
    }
    public List<CharacterControl> GetEnemyTeam()
    {
        return enemyList;
    }
}
