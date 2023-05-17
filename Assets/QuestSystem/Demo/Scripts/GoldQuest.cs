using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GoldQuest : Quest
{
    [System.Serializable]
    public class SaveableData
    {
        public int currentIncreaseGold = 0;
    }
    public int goldRequire = 0;
    private SaveableData _data;
    public override void Dispose()
    {
        
    }

    protected override void ConstructQuestEvent()
    {
        _manager.Subscribe<GoldIncreaseQuestEvent>(OnGoldEventUpdate);
    }

    protected override void DeconstructQuestEvent()
    {
        _manager.Unsubscribe<GoldIncreaseQuestEvent>(OnGoldEventUpdate);
    }

    void OnGoldEventUpdate(GoldIncreaseQuestEvent goldIncrease)
    {       
        _data.currentIncreaseGold += goldIncrease.goldIncrease;
        onUpdateEvent?.Invoke();
        Debug.Log(GetQuestDesc());
        if (_data.currentIncreaseGold >= goldRequire)
        {
            OnQuestComplete();
        }
    }

    public override string GetQuestDesc()
    {
        return System.Text.RegularExpressions.Regex.Replace(_description, @"{gold}", _data.currentIncreaseGold + "/" + goldRequire.ToString());
    }

    protected override QuestState GetVerifyState()
    {
        _data = ES3.Load(DataKey(), defaultValue: new SaveableData());


        if(_data.currentIncreaseGold >= goldRequire)
        {
            return QuestState.Complete;
        }
        else
        {
            Debug.Log(GetQuestDesc());
            return QuestState.Active;
        }        
    }

    public override void SaveData()
    {
        ES3.Save(DataKey(), _data);
    }
}
