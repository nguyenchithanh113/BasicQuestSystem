using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestViewManager : MonoBehaviour
{
    [SerializeField] QuestManager _questManager;
    [SerializeField] QuestView _viewPreb;
    [SerializeField] RectTransform _viewHolder;

    private Dictionary<Quest, QuestView> _questViewMap = new Dictionary<Quest, QuestView>();

    void Start()
    {
        Init();
    }

    void Init()
    {
        _questManager.onNewQuestAvailable += OnNewQuestAvailable;

        List<Quest> activeQuest = _questManager.GetActiveQuest();
        ConstructView(activeQuest.ToArray());
    }

    void ConstructView(params Quest[] quests)
    {
        foreach(Quest quest in quests)
        {
            quest.onQuestFinish += OnQuestFinish;

            QuestView questView = Instantiate(_viewPreb, _viewHolder);
            questView.Construct(quest);
            questView.gameObject.SetActive(true);
            _questViewMap.Add(quest,questView);
        }
    }
    void OnNewQuestAvailable(List<Quest> quests)
    {
        ConstructView(quests.ToArray());
    }

    void OnQuestFinish(Quest quest)
    {
        quest.onQuestFinish -= OnQuestFinish;
        QuestView questView = null;
        if(_questViewMap.TryGetValue(quest, out questView))
        {
            _questViewMap.Remove(quest);
            questView.Dispose();
        }
    }
}
