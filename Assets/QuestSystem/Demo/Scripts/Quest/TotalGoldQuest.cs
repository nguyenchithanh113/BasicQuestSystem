using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TotalGoldQuest : Quest
{
    public int goldRequire = 0;
    private int _totalGold;
    public override void Dispose()
    {
        
    }

    public override string GetQuestDesc()
    {
        return System.Text.RegularExpressions.Regex.Replace(_description, @"{gold}", _totalGold + "/" + goldRequire.ToString());
    }

    public override void SaveData()
    {
        
    }

    protected override void ConstructQuestEvent()
    {
        _manager.Subscribe<TotalGoldEarnEvent>(OnGoldEventUpdate);
    }

    protected override void DeconstructQuestEvent()
    {
        _manager.Unsubscribe<TotalGoldEarnEvent>(OnGoldEventUpdate);
    }

    private void OnGoldEventUpdate(TotalGoldEarnEvent obj)
    {
        _totalGold = obj.goldEarn;
        onUpdateEvent?.Invoke();
        if(_totalGold >= goldRequire)
        {
            OnQuestComplete();
        }
    }

    protected override QuestState GetVerifyState()
    {
        _totalGold = PlayerPrefs.GetInt("TotalGold", 0);


        if (_totalGold >= goldRequire)
        {
            return QuestState.Complete;
        }
        else
        {
            Debug.Log(GetQuestDesc());
            return QuestState.Active;
        }
    }
}
