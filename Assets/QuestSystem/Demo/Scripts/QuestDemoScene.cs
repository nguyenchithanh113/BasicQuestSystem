using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDemoScene : MonoBehaviour
{
    [SerializeField] QuestManager _questManager;

    int goldIncrease;
    int totalGold = 0;

    public static QuestDemoScene Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        totalGold = PlayerPrefs.GetInt("TotalGold", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            totalGold += 10;
            PlayerPrefs.SetInt("TotalGold", totalGold);

            _questManager.Fire(new GoldIncreaseQuestEvent(10));
            _questManager.Fire(new TotalGoldEarnEvent(totalGold));
        }   
    }

    public void OnReceiveQuestReward(QuestReward questReward)
    {

    }
}
