using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class QuestManager : MonoBehaviour
{
    public class QuestSubscription
    {
        public object identifier;
        public Action<object> callback;

        public QuestSubscription(object identifier, Action<object> callback)
        {
            this.identifier = identifier;
            this.callback = callback;
        }
    }
    [SerializeField] protected List<QuestGroup> _listQuestGroupRead;
    protected List<QuestGroup> _listQuestGroup = new List<QuestGroup>();
    protected Dictionary<object, List<QuestSubscription>> _signalMap = new Dictionary<object, List<QuestSubscription>>();
    //protected Dictionary<Type, QuestEvent> _eventMap = new Dictionary<Type, QuestEvent>();

    public static QuestManager Instance;

    public Action<List<Quest>> onNewQuestAvailable;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            InitQuest();
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    public void InitQuest()
    {
        foreach(QuestGroup questGroup in _listQuestGroupRead)
        {
            QuestGroup group = questGroup;

            bool groupIsValid = false;

            if(QuestSystem.IsQuestGroupFinish(group.ID) == true)
            {
                group = group.NextGroup;
                while (group != null)
                {
                    if (QuestSystem.IsQuestGroupFinish(group.ID) == false)
                    {                       
                        groupIsValid = true;
                        break;
                    }
                    else
                    {
                        group = group.NextGroup;
                    }
                }
            }
            else
            {
                groupIsValid = true;
            }

            if (groupIsValid)
            {
                group = group.Clone();
                group.onProgressToNextGroupQuest += OnNewQuestGroupProgress;
                group.onDispose += OnQuestGroupDispose;
                group.Construct(this);
                _listQuestGroup.Add(group);
            }
            
        }
    }

    public List<Quest> GetActiveQuest()
    {
        List<Quest> activeQuests = new List<Quest>();
        foreach(QuestGroup questGroup in _listQuestGroup)
        {
            activeQuests.AddRange(questGroup.GetActiveQuest());
        }
        return activeQuests;
    }

    void OnQuestGroupDispose(QuestGroup questGroup)
    {
        _listQuestGroup.Remove(questGroup);
    }
    void OnNewQuestGroupProgress(QuestGroup questGroup)
    {
        questGroup.Construct(this);
        _listQuestGroup.Add(questGroup);
        onNewQuestAvailable.Invoke(questGroup.GetActiveQuest());
    }

    public void Subscribe<T>(Action<T> callback) where T : QuestEvent
    {
        Action<object> wrapperCallback = (x) =>
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
                throw new Exception("Subscription already exist");
            }
            else
            {
                _signalMap[typeof(T)].Add(questSubscription);
            }           
        }
        else
        {
            //DeclareEvent<T>();
            _signalMap.Add(typeof(T), new List<QuestSubscription>() { questSubscription });
        }
    }

    public void Unsubscribe<T>(Action<T> callback) where T : QuestEvent
    {
        if (_signalMap.ContainsKey(typeof(T)))
        {
            for (int i = 0; i < _signalMap[typeof(T)].Count; i++)
            {
                if (_signalMap[typeof(T)][i].identifier.Equals(callback))
                {
                    Debug.Log("Remove Quest subscription: " + _signalMap[typeof(T)][i].identifier.ToString());
                    _signalMap[typeof(T)].Remove(_signalMap[typeof(T)][i]);
                    return;
                }
            }

            Debug.LogError("Remove Quest subscription fail");
        }
        else
        {
            throw new Exception("Unsubscribe non existen subscription");
        }
    }

    public void Fire<T>(T data) where T : QuestEvent
    {
        if (_signalMap.ContainsKey(typeof(T)))
        {
            //_eventMap[typeof(T)] = data;

            for (int i = _signalMap[typeof(T)].Count - 1; i >= 0; i--)
            {
                QuestSubscription questSubscription = _signalMap[typeof(T)][i];
                questSubscription.callback.Invoke(data);
            }
        }
        else
        {
            Debug.Log("Quest contain no data event for: " + typeof(T).ToString());
        }
    }

    //public T GetEventData<T>() where T : QuestEvent
    //{
    //    if (_eventMap.ContainsKey(typeof(T)))
    //    {
    //        return (T)_eventMap[typeof(T)];
    //    }
    //    else
    //    {
    //        Debug.Log("Quest: Event is not register");
    //        return null;
    //    }
    //}

    //public void DeclareEvent<T>() where T : QuestEvent
    //{
    //    if (_eventMap.ContainsKey(typeof(T)))
    //    {
            
    //    }
    //    else
    //    {
    //        var ctors = typeof(T).GetConstructors();
    //        var ctor = ctors[0];

    //        object[] types = new object[ctor.GetParameters().Length];

    //        int count = 0;
    //        foreach(ParameterInfo param in ctor.GetParameters())
    //        {
    //            Type type = param.ParameterType;
    //            types[count] = (type.IsValueType ? Activator.CreateInstance(type) : null);
    //            count++;
    //        }

    //        T e = Activator.CreateInstance(typeof(T),types) as T;
    //        _eventMap.Add(typeof(T), e);
    //    }
    //}
}
