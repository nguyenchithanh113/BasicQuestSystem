using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldIncreaseQuestEvent : QuestEvent
{
    public int goldIncrease = 0;
    public override void Construct()
    {
        goldIncrease = 0;
    }
}
