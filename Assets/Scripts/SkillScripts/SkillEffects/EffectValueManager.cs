using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectValueManager : MonoBehaviour
{
    public static EffectValueManager instance;

    private List<int> values;

    void Awake()
    {
        if (instance == null)
        {
            values = new List<int>();
            instance = this;
        }
    }

    public int GenerateValue()
    {
        int temp = 0;

        if (values.Count == 0)
        {
            temp = Random.Range(1, 1001);
            values.Add(temp);
        }
        else
        {
            bool check = false;

            while (check == false)
            {
                temp = Random.Range(1, 1001);

                if (values.Contains(temp))
                {

                }
                else
                {
                    values.Add(temp);
                    check = true;
                }
            }
        }

        return temp;
    }

    public void RemoveValue(int val)
    {
        values.Remove(val);
    }
}
