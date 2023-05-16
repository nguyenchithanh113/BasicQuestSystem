using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class QuestGroup : ScriptableObject
{
    [SerializeField] protected string _id;
    [SerializeField] readonly protected List<Quest> _listQuestRead;
    [SerializeField] QuestGroup _nextGroup;

    public List<Quest> allQuest = new List<Quest>();

    protected int _totalComplete = 0;

    public string ID => _id;
    public QuestGroup NextGroup => _nextGroup;

    public System.Action<QuestGroup> onProgressToNextGroupQuest;
    public System.Action<QuestGroup> onDispose;

    public void Construct(QuestManager questManager)
    {
        foreach (Quest quest in _listQuestRead)
        {
            Quest clone = quest.Clone();
            clone.Construct(questManager);

            if (clone.State == QuestState.Complete)
            {
                _totalComplete++;
            }

            allQuest.Add(clone);
        }
    }

    public List<Quest> GetActiveQuest()
    {
        List<Quest> temp = new List<Quest>();
        foreach(Quest quest in allQuest)
        {
            if(quest.State == QuestState.Active)
            {
                temp.Add(quest);
            }
        }
        return temp;
    }

    protected void OnQuestFinish(Quest quest)
    {
        quest.Dispose();
        allQuest.Remove(quest);
        _totalComplete++;

        if (_totalComplete == allQuest.Count - 1)
        {
            if(_nextGroup != null)
            {
                onProgressToNextGroupQuest?.Invoke(_nextGroup.Clone());
            }
            Dispose();
        }
    }

    public void Dispose()
    {
        onDispose?.Invoke(this);
    }

    public QuestGroup Clone()
    {
        return Instantiate(this);
    }

    private void OnValidate()
    {
        if (_id == string.Empty)
        {
            _id = System.Guid.NewGuid().ToString();
        }
    }
}
