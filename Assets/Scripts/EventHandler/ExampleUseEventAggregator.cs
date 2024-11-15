using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    public class TestEvent
    {
        public string EventName = "Test";
    }
    public class ExampleUseEventAggregator : MonoBehaviour, IGameEventListener<TestEvent>
    {
        public void Handle(TestEvent @event)
        {
            Debug.Log(@event.EventName);
        }

        // Start is called before the first frame update
        void Start() 
        {
            EventAggregator.Register<TestEvent>(this);
            StartCoroutine(RaiseTestEvent());
        }

        private IEnumerator RaiseTestEvent()
        {
            var time = Time.time;
            int loops = 5;
            while(true)
            {
                if(loops == 0) break;
                if(Time.time - time >= 2)
                {
                    EventAggregator.RaiseEvent<TestEvent>( new TestEvent() );
                    time = Time.time;
                    loops--;
                    yield return null;
                }
                yield return null;
            }
        }

        // Update is called once per frame
        void Update() { 

        }
    }
}
