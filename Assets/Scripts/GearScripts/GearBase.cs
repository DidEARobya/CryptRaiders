using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GearType
{
    WEAPON,
    HELMET,
    SHIELD,
    GLOVES,
    CHESTPIECE,
    BOOTS
}
public class GearBase : MonoBehaviour
{
    public GearType type;

    private int mainStat;
    private List<int> subStats;

    private List<string> statList;

    private void Awake()
    {
        statList = new List<string>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
