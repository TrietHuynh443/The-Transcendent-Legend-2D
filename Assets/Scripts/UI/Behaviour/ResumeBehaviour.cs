using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Behaviour
{
    public class ResumeBehaviour : ButtonBehaviour
    {
        public override void OnClick(PointerEventData eventData)
        {
            PauseEvent pauseEvent = new PauseEvent();
            
            bool isPause = Time.timeScale == 0;

            if (isPause)
            {
                pauseEvent.Pause();
            }
            
            EventAggregator.RaiseEvent<PauseEvent>(pauseEvent);
        }
    }
}