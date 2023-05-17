using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalGoldEarnEvent : QuestEvent
{
    public int goldEarn;
    public TotalGoldEarnEvent(int goldEarn)
    {
        this.goldEarn = goldEarn;
    }
}
