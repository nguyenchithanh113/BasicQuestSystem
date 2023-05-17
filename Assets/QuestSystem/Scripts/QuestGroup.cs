using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu()]
public class QuestGroup : ScriptableObject
{
    [SerializeField] protected string _id;
    [SerializeField] protected List<Quest> _listQuestRead;
    [SerializeField] QuestGroup _nextGroup;

    [HideInInspector] public List<Quest> allQuest = new List<Quest>();

    //protected int _totalFinish = 0;

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

            if (QuestSystem.IsQuestFinish(clone.ID))
            {
                //_totalFinish++;
            }
            else
            {
                clone.onQuestFinish += OnQuestFinish;
                allQuest.Add(clone);
            }

            
        }
    }

    public List<Quest> GetActiveQuest()
    {
        //List<Quest> temp = new List<Quest>();
        //foreach(Quest quest in allQuest)
        //{
        //    if(quest.State == QuestState.Active)
        //    {
        //        temp.Add(quest);
        //    }
        //}
        return allQuest;
    }

    protected void OnQuestFinish(Quest quest)
    {
        QuestSystem.SetQuestFinish(quest.ID);
        quest.Dispose();
        quest.onQuestFinish -= OnQuestFinish;
        allQuest.Remove(quest);
        //_totalFinish++;

        if (allQuest.Count == 0)
        {
            QuestSystem.SetQuestGroupFinish(_id);
            if (_nextGroup != null)
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
#if UNITY_EDITOR
    [ContextMenu("Add child quest to nest")]
    private void AddChildQuestToNest()
    {
        foreach(Quest quest in _listQuestRead)
        {           
            AssetDatabase.AddObjectToAsset(ScriptableObject.Instantiate(quest), this);
        }

        
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
        foreach (Quest quest in _listQuestRead)
        {
            EditorUtility.SetDirty(quest);
        }
    }

#endif
}
