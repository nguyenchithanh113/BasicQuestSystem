using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestView : MonoBehaviour
{
    protected Quest _quest;

    public virtual void Construct(Quest quest)
    {
        _quest = quest;
        _quest.onStateChange += Display;
        _quest.onUpdateEvent += UpdateGUI;
    }
    protected abstract void Display(QuestState questState);
    protected abstract void UpdateGUI();

    public virtual void Dispose()
    {
        if(_quest.DisposeType == QuestDisposeType.Destroy)
        {
            Destroy(gameObject);
        }else if(_quest.DisposeType == QuestDisposeType.Disable)
        {
            gameObject.SetActive(false);
        }
    }
}
