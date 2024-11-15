using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventAggregator
{
    private static Dictionary<Type, List<object>> _eventDic = new();

    public static void Register<T>(object listener)
    {
        if(!typeof(IGameEventListener<T>).IsAssignableFrom(listener.GetType()))
            throw new InvalidCastException();
        if(!_eventDic.ContainsKey(typeof(T))){
            _eventDic.Add(typeof(T), new List<object>(){});
        }
        _eventDic[typeof(T)].Add(listener);
    }

    public static void Unregister<T>(object listener)
    {
        if(!_eventDic.ContainsKey(typeof(T))) return;

        _eventDic[typeof(T)].Remove(listener);
    }

    public static void UnregisterAll<T>(){
        if(!_eventDic.ContainsKey(typeof(T))) return;
        
        _eventDic[typeof(T)].Clear();
    }

    public static void RaiseEvent<T>(T @event)
    {
        if(!_eventDic.ContainsKey(typeof(T))) return;
        foreach(var listener in _eventDic[typeof(T)])
        {
            ((IGameEventListener<T>)listener).Handle(@event);
        }
    }

}
