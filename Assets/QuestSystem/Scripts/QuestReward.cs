using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestReward
{
    public RewardItem rewardItem;
    public int rewardValue;
    public Sprite rewardIcon;
}
public enum RewardItem
{
    Gold = 0,
    Crystal = 1,
}