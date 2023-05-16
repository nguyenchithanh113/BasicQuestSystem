using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestSystem
{
    public static bool IsQuestFinish(string id)
    {
        return PlayerPrefs.GetInt("Quest_" + id, 0) == 1;
    }
    public static void SetQuestFinish(string id)
    {
        PlayerPrefs.SetInt("Quest_" + id, 1);
    }
    public static bool IsQuestGroupFinish(string id)
    {
        return PlayerPrefs.GetInt("QuestGroup_" + id, 0) == 1;
    }
    public static void SetQuestGroupFinish(string id)
    {
        PlayerPrefs.SetInt("QuestGroup_" + id, 1);
    }
}
