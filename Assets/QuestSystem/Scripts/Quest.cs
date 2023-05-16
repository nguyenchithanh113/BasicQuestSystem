using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : ScriptableObject
{
    [SerializeField] protected string questID;
    [SerializeField] protected string description;
    [SerializeField] protected QuestDisposeType disposeType = QuestDisposeType.Destroy;

    protected QuestState _state;

    public System.Action<Quest> onQuestFinish;
    public System.Action onUpdateEvent;
    public System.Action<QuestState> onStateChange;

    public QuestState State => _state;

    protected QuestManager _manager;

    public virtual void Construct(QuestManager questManager)
    {
        _manager = questManager;

        _state = GetVerifyState();

        if(_state == QuestState.Active)
        {
            ConstructQuestEvent();
        }
    }

    protected void SwitchState(QuestState state)
    {
        _state = state;

        OnStateChange(_state);
    }

    protected virtual void OnQuestComplete()
    {
        SwitchState(QuestState.Complete);
        DeconstructQuestEvent();
    }

    protected virtual bool FinishQuest()
    {
        if(_state == QuestState.Complete)
        {
            OnQuestFinish();
            onQuestFinish?.Invoke(this);
            return true;
        }

        return false;
    }

    protected abstract QuestState GetVerifyState();
    protected abstract void OnStateChange(QuestState state);
    protected abstract void OnQuestFinish();
    protected abstract void ConstructQuestEvent();
    protected abstract void DeconstructQuestEvent();
    protected abstract string GetQuestDesc();

    public abstract void Dispose();

    public Quest Clone()
    {
        return Instantiate(this);
    }
    private void OnValidate()
    {
        if (questID == string.Empty)
        {
            questID = System.Guid.NewGuid().ToString();
        }
    }

}

