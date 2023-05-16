using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestMediator
{
    public abstract void Display(QuestState questState);
    public abstract void UpdateGUI(QuestState questState);
}
