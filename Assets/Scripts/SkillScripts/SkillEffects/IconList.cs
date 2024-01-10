using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconList : MonoBehaviour
{
    public static IconList instance;

    public List<GameObject> list;
    private Dictionary<string, GameObject> objectList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            objectList = new Dictionary<string, GameObject>();

            for (int i = 0; i < list.Count; i++)
            {
                objectList.Add(list[i].name, list[i]);
            }
        }
    }

    public GameObject FindIcon(string iconName)
    {
        return objectList[iconName];
    }
}
