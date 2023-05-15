using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestManager : MonoBehaviour
{
    public class QuestSubscription
    {
        public object identifier;
        public Action<QuestDetail> callback;

        public QuestSubscription(object identifier, Action<QuestDetail> callback)
        {
            this.identifier = identifier;
            this.callback = callback;
        }
    }
    protected Dictionary<object, List<QuestSubscription>> _signalMap;
    void Start()
    {
        
    }

    public void Subscribe<T>(Action<T> callback) where T : QuestDetail
    {
        Action<QuestDetail> wrapperCallback = (x) =>
        {
            callback((T)x);
        };

        QuestSubscription questSubscription = new QuestSubscription(callback, wrapperCallback);

        if (_signalMap.ContainsKey(typeof(T)))
        {
            bool subscriptionExist = false;
            for(int i = 0; i < _signalMap[typeof(T)].Count; i++)
            {
                if(_signalMap[typeof(T)][i].identifier.Equals(callback))
                {
                    subscriptionExist = true;
                    break;
                }
            }

            if (subscriptionExist)
            {
                Debug.LogError("Subscription Already Exist");
            }
            else
            {
                _signalMap[typeof(T)].Add(questSubscription);
            }           
        }
        else
        {
            _signalMap.Add(typeof(T), new List<QuestSubscription>() { questSubscription });
        }
    }

    public void Fire<T>(T data) where T : QuestDetail
    {
        if (_signalMap.ContainsKey(typeof(T)))
        {
            foreach(QuestSubscription questSubscription in _signalMap[typeof(T)])
            {
                questSubscription.callback.Invoke(data);
            }
        }
        else
        {
            Debug.LogError("Quest contain no data event for: " + typeof(T).ToString());
        }
    }
}
