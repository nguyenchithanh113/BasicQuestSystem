using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestDetail : ScriptableObject
{
    public string questID;
    public string description;

    private void OnValidate()
    {
        if(questID == string.Empty)
        {
            questID = System.Guid.NewGuid().ToString();
        }
    }
}
