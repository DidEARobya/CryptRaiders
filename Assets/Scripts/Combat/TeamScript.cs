using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum teamType
{
    PLAYER,
    ENEMY,
    BOSS,
    PREBATTLE
}

public class TeamScript : MonoBehaviour
{
    public teamType teamType;

    private CharacterObject character1, character2, character3, character4;
    private List<CharacterObject> characterList;

    private GameObject characterModel1, characterModel2, characterModel3, characterModel4;

    private CharacterControl control1, control2, control3, control4;
    private List<CharacterControl> controlList;

    private void Awake()
    {
        characterList = new List<CharacterObject>();
        controlList = new List<CharacterControl>();
    }

    public void InitPlayer(List<PlayerCharacterData> team)
    {
        List<CharacterObject> temp = new List<CharacterObject>();

        for (int i = 0; i < team.Count; i++)
        {
            CharacterObject obj = this.AddComponent<CharacterObject>();
            obj.SetPlayerData(team[i]);

            temp.Add(obj);

        }

        if (temp.Count >= 1 && temp[0] != null)
        {
            character1 = temp[0];
        }
        if (temp.Count >= 2 && temp[1] != null)
        {
            character2 = temp[1];
        }
        if (temp.Count >= 3 && temp[2] != null)
        {
            character3 = temp[2];
        }
        if (temp.Count >= 4 && temp[3] != null)
        {
            character4 = temp[3];
        }

        if (teamType == teamType.PLAYER)
        {
            this.transform.position = new Vector3(-8f, -0.25f, 0);
        }
        else if (teamType == teamType.ENEMY)
        {
            this.transform.position = new Vector3(8f, -0.25f, 0);
        }

        if (character1 != null)
        {
            characterModel1 = new GameObject();
            characterModel1.transform.parent = this.transform;

            control1 = characterModel1.AddComponent<CharacterControl>();
            character1.SetActiveStats();
            control1.character = character1;
            control1.Init();

            AddCharacter(control1);

            characterModel1.transform.position = this.transform.position + new Vector3(0, 3.25f, 0);
            characterModel1.name = control1.character.GetCharacterName();
            character1.transform.SetParent(control1.transform);
        }

        if (character2 != null)
        {
            characterModel2 = new GameObject();
            characterModel2.transform.parent = this.transform;

            control2 = characterModel2.AddComponent<CharacterControl>();
            character2.SetActiveStats();
            control2.character = character2;
            control2.Init();

            AddCharacter(control2);

            characterModel2.transform.position = this.transform.position + new Vector3(0, 1f, 0);
            characterModel2.name = control2.character.GetCharacterName();
            character2.transform.SetParent(control2.transform);
        }

        if (character3 != null)
        {
            characterModel3 = new GameObject();
            characterModel3.transform.parent = this.transform;

            control3 = characterModel3.AddComponent<CharacterControl>();
            character3.SetActiveStats();
            control3.character = character3;
            control3.Init();

            AddCharacter(control3);

            characterModel3.transform.position = this.transform.position + new Vector3(0, -1.25f, 0);
            characterModel3.name = control3.character.GetCharacterName();
            character3.transform.SetParent(control3.transform);
        }

        if (character4 != null)
        {
            characterModel4 = new GameObject();
            characterModel4.transform.parent = this.transform;

            control4 = characterModel4.AddComponent<CharacterControl>();
            character4.SetActiveStats();
            control4.character = character4;
            control4.Init();

            AddCharacter(control4);

            characterModel4.transform.position = this.transform.position + new Vector3(0, -3.5f, 0);
            characterModel4.name = control4.character.GetCharacterName();
            character4.transform.SetParent(control4.transform);
        }
    }
    public void InitEnemy(List<CharacterData> team, int level)
    {
        List<CharacterObject> temp = new List<CharacterObject>();

        for (int i = 0; i < team.Count; i++)
        {
            CharacterObject obj = this.AddComponent<CharacterObject>();
            obj.characterData = (team[i]);

            temp.Add(obj);

        }

        if (temp.Count >= 1 && temp[0] != null)
        {
            character1 = temp[0];
        }
        if (temp.Count >= 2 && temp[1] != null)
        {
            character2 = temp[1];
        }
        if (temp.Count >= 3 && temp[2] != null)
        {
            character3 = temp[2];
        }
        if (temp.Count >= 4 && temp[3] != null)
        {
            character4 = temp[3];
        }

        if (teamType == teamType.PLAYER)
        {
            this.transform.position = new Vector3(-8f, 0, 0);
        }
        else if (teamType == teamType.ENEMY)
        {
            this.transform.position = new Vector3(8f, 0, 0);
        }

        if (character1 != null)
        {
            characterModel1 = new GameObject();
            characterModel1.transform.parent = this.transform;

            control1 = characterModel1.AddComponent<CharacterControl>();
            character1.SetActiveStats(level);
            control1.character = character1;
            control1.Init();

            AddCharacter(control1);

            characterModel1.transform.position = this.transform.position + new Vector3(0, 3.25f, 0);
            characterModel1.name = control1.character.GetCharacterName();
            character1.transform.SetParent(control1.transform);
        }

        if (character2 != null)
        {
            characterModel2 = new GameObject();
            characterModel2.transform.parent = this.transform;

            control2 = characterModel2.AddComponent<CharacterControl>();
            character2.SetActiveStats(level);
            control2.character = character2;
            control2.Init();

            AddCharacter(control2);

            characterModel2.transform.position = this.transform.position + new Vector3(0, 1f, 0);
            characterModel2.name = control2.character.GetCharacterName();
            character2.transform.SetParent(control2.transform);
        }

        if (character3 != null)
        {
            characterModel3 = new GameObject();
            characterModel3.transform.parent = this.transform;

            control3 = characterModel3.AddComponent<CharacterControl>();
            character3.SetActiveStats(level);
            control3.character = character3;
            control3.Init();

            AddCharacter(control3);

            characterModel3.transform.position = this.transform.position + new Vector3(0, -1.25f, 0);
            characterModel3.name = control3.character.GetCharacterName();
            character3.transform.SetParent(control3.transform);
        }

        if (character4 != null)
        {
            characterModel4 = new GameObject();
            characterModel4.transform.parent = this.transform;

            control4 = characterModel4.AddComponent<CharacterControl>();
            character4.SetActiveStats(level);
            control4.character = character4;
            control4.Init();

            AddCharacter(control4);

            characterModel4.transform.position = this.transform.position + new Vector3(0, -3.5f, 0);
            characterModel4.name = control4.character.GetCharacterName();
            character4.transform.SetParent(control4.transform);
        }
    }

    // Start is called before the first frame update
    void test()
    {
        if (teamType == teamType.PLAYER)
        {
            string temp = "Alvi";

            if(PlayerIndex.instance.LoadCharacterData(temp) == true)
            {
                character1 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
                character1.SetPlayerData(PlayerIndex.instance.LoadCharacterData(temp));
            }
            else
            {
                character1 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
            }

            temp = "Vuduin";

            if (PlayerIndex.instance.LoadCharacterData(temp) == true)
            {
                character2 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
                character2.SetPlayerData(PlayerIndex.instance.LoadCharacterData(temp));
            }
            else
            {
               character2 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
            }

            temp = "Hemal";

            if (PlayerIndex.instance.LoadCharacterData(temp) == true)
            {
                character3 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
                character3.SetPlayerData(PlayerIndex.instance.LoadCharacterData(temp));
            }
            else
            {
                character3 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
            }

            temp = "Vividus";

            if (PlayerIndex.instance.LoadCharacterData(temp) == true)
            {
                character4 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
                character4.SetPlayerData(PlayerIndex.instance.LoadCharacterData(temp));
            }
            else
            {
                character4 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
            }
        }
        if (teamType == teamType.ENEMY)
        { 
            string temp = "Alvi";

            //if (PlayerIndex.instance.LoadCharacterData(temp) == true)
            //{
            //    character1 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
            //    character1.SetPlayerData(PlayerIndex.instance.LoadCharacterData(temp));
            //}
            //else
           // {
                character1 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
           // }

            temp = "Kilnuak";

            if (PlayerIndex.instance.LoadCharacterData(temp) == true)
            {
                character2 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
                character2.SetPlayerData(PlayerIndex.instance.LoadCharacterData(temp));
            }
            else
            {
                character2 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
            }

            temp = "Vuduin";

            if (PlayerIndex.instance.LoadCharacterData(temp) == true)
            {
                character3 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
                character3.SetPlayerData(PlayerIndex.instance.LoadCharacterData(temp));
            }
            else
            {
                character3 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
            }

            temp = "Vividus";

            if (PlayerIndex.instance.LoadCharacterData(temp) == true)
            {
                character4 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
                character4.SetPlayerData(PlayerIndex.instance.LoadCharacterData(temp));
            }
            else
            {
                character4 = Instantiate(CharacterIndex.instance.GetCharacter(temp));
            }
        }

        if (teamType == teamType.PLAYER)
        {
            this.transform.position = new Vector3(-8f, 0, 0);
        }
        else if (teamType == teamType.ENEMY)
        {
            this.transform.position = new Vector3(8f, 0, 0);
        }

        if(character1 != null)
        {
            characterModel1 = new GameObject();
            characterModel1.transform.parent = this.transform;

            control1 = characterModel1.AddComponent<CharacterControl>();
            control1.character = character1;
            control1.Init();

            AddCharacter(control1);

            characterModel1.transform.position = this.transform.position + new Vector3(0, 3.25f, 0);
            characterModel1.name = control1.character.GetCharacterName();
            character1.transform.SetParent(control1.transform);
        }

        if (character2 != null)
        {
            characterModel2 = new GameObject();
            characterModel2.transform.parent = this.transform;

            control2 = characterModel2.AddComponent<CharacterControl>();
            control2.character = character2;
            control2.Init();

            AddCharacter(control2);

            characterModel2.transform.position = this.transform.position + new Vector3(0, 1f, 0);
            characterModel2.name = control2.character.GetCharacterName();
            character2.transform.SetParent(control2.transform);
        }

        if (character3 != null)
        {
            characterModel3 = new GameObject();
            characterModel3.transform.parent = this.transform;

            control3 = characterModel3.AddComponent<CharacterControl>();
            control3.character = character3;
            control3.Init();

            AddCharacter(control3);

            characterModel3.transform.position = this.transform.position + new Vector3(0, -1.25f, 0);
            characterModel3.name = control3.character.GetCharacterName();
            character3.transform.SetParent(control3.transform);
        }

        if (character4 != null)
        {
            characterModel4 = new GameObject();
            characterModel4.transform.parent = this.transform;

            control4 = characterModel4.AddComponent<CharacterControl>();
            control4.character = character4;
            control4.Init();

            AddCharacter(control4);

            characterModel4.transform.position = this.transform.position + new Vector3(0, -3.5f, 0);
            characterModel4.name = control4.character.GetCharacterName();
            character4.transform.SetParent(control4.transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //if (teamType == teamType.PLAYER)
        //{
        //    this.transform.position = new Vector3(ResolutionScript.instance.xMin + 2, ResolutionScript.instance.yMin + 6.5f, 0);
        //}
        //else if (teamType == teamType.ENEMY)
        //{
        //    this.transform.position = new Vector3(ResolutionScript.instance.xMax - 2, ResolutionScript.instance.yMin + 6.5f, 0);
        //}
        //else if (teamType == teamType.BOSS)
        //{

        //}
        //else if (teamType == teamType.PREBATTLE)
        //{
        //    return;
        //}
    }
    public void AddCharacter(CharacterControl character)
    {
        if (character)
        {
            if (controlList.Count < 4)
            {
                controlList.Add(character);
                characterList.Add(character.character);
            }
        }
    }

    public bool CheckIfDead()
    {
        for(int i = 0; i < controlList.Count; i++)
        {
            if (controlList[i].GetState() != characterStates.DEAD)
            {
                return false;
            }
        }

        return true;
    }
    public List<CharacterControl> GetTeam()
    {
        return controlList;
    }
    public List<CharacterObject> GetTeamData()
    {
        return characterList;
    }
}
