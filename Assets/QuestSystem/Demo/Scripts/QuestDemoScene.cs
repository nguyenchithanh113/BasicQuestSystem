using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDemoScene : MonoBehaviour
{
    [SerializeField] QuestManager _questManager;

    int goldIncrease;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            goldIncrease += 10;
            _questManager.Fire(new GoldIncreaseQuestEvent(10));
        }   
    }

    public void OnReceiveQuestReward(QuestReward questReward)
    {

    }
}
