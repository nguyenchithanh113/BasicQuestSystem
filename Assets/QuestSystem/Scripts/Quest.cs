using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : ScriptableObject
{
    [SerializeField] protected string _questID;
    [SerializeField] protected string _description;
    [SerializeField] protected QuestDisposeType _disposeType = QuestDisposeType.Destroy;
    [SerializeField] List<QuestReward> _rewards;

    protected QuestState _state;

    public System.Action<Quest> onQuestFinish;
    public System.Action onUpdateEvent;
    public System.Action<QuestState> onStateChange;

    public QuestState State => _state;
    public QuestDisposeType DisposeType => _disposeType;
    public List<QuestReward> Rewards => _rewards;
    public string ID => _questID;

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

        onStateChange.Invoke(_state);
    }

    protected virtual void OnQuestComplete()
    {
        SwitchState(QuestState.Complete);       
    }

    public virtual bool FinishQuest()
    {
        if(_state == QuestState.Complete)
        {
            DeconstructQuestEvent();
            onQuestFinish?.Invoke(this);
            return true;
        }

        return false;
    }

    protected abstract QuestState GetVerifyState();
    protected abstract void ConstructQuestEvent();
    protected abstract void DeconstructQuestEvent();
    public abstract string GetQuestDesc();

    public abstract void Dispose();
    public abstract void SaveData();

    public Quest Clone()
    {
        return Instantiate(this);
    }
    private void OnValidate()
    {
        if (_questID == string.Empty)
        {
            _questID = System.Guid.NewGuid().ToString();
        }
    }

    public virtual void OnDestroy()
    {
        SaveData();
    }

    public string DataKey()
    {
        return "QuestData_" + _questID;
    }

}

