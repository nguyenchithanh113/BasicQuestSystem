using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalQuestView : QuestView
{
    [SerializeField] RectTransform _rectCompleteGroup;
    [SerializeField] Text _description;
    [SerializeField] Button _btnInteract;
    [SerializeField] Image _rewardParticle;
    [SerializeField] RectTransform _rectMoneyGroup;
    [SerializeField] RectTransform _rectCrystalGroup;

    bool _isRewarding;

    public override void Construct(Quest quest)
    {
        base.Construct(quest);
        _btnInteract.onClick.AddListener(OnInteractClick);
        UpdateGUI();
    }

    protected override void Display(QuestState questState)
    {
        if(questState == QuestState.Complete)
        {
            _rectCompleteGroup.gameObject.SetActive(true);
        }
    }

    protected override void UpdateGUI()
    {
        _description.text = _quest.GetQuestDesc();
    }

    void OnInteractClick()
    {
        if (_isRewarding) return;
        if (_quest.State == QuestState.Complete)
        {
            StartCoroutine(RewardingSequence());
        }
    }

    IEnumerator RewardingSequence()
    {
        _isRewarding = true;
        List<Image> rewardingParticles = new List<Image>();

        foreach (QuestReward questReward in _quest.Rewards)
        {
            Image rewardingParticle = Instantiate(_rewardParticle, transform);
            rewardingParticle.gameObject.SetActive(true);

            rewardingParticle.sprite = questReward.rewardIcon;
            rewardingParticles.Add(rewardingParticle);
        }



        Vector3 original = transform.position;

        float t = 0;
        while (t < 2)
        {
            t += Time.deltaTime;
            for(int i = 0; i < _quest.Rewards.Count; i++)
            {
                Vector3 pos = Vector3.zero;
                if(_quest.Rewards[i].rewardItem == RewardItem.Gold)
                {
                    pos = Vector3.Lerp(original, _rectMoneyGroup.transform.position, t / 3f);                   
                }
                else if (_quest.Rewards[i].rewardItem == RewardItem.Crystal)
                {
                    pos = Vector3.Lerp(original, _rectCrystalGroup.transform.position, t / 3f);
                }
                rewardingParticles[i].transform.position = pos;
            }
            yield return new WaitForEndOfFrame();
        }

        _quest.Rewards.ForEach((x) => QuestDemoScene.Instance.OnReceiveQuestReward(x));
        _quest.FinishQuest();
    }
}
