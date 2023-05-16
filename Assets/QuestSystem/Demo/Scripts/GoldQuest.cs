using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GoldQuest : Quest
{
    public int goldRequire = 0;
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
        if(goldIncrease.goldIncrease >= goldRequire)
        {
            SwitchState(QuestState.Complete);
        }
    }

    protected override string GetQuestDesc()
    {
        throw new System.NotImplementedException();
    }

    protected override QuestState GetVerifyState()
    {
        if(_manager.GetEventData<GoldIncreaseQuestEvent>().goldIncrease >= goldRequire)
        {
            return QuestState.Complete;
        }
        else
        {
            return QuestState.Active;
        }
    }


    protected override void OnQuestFinish()
    {
        
    }

    protected override void OnStateChange(QuestState state)
    {
        
    }
}
