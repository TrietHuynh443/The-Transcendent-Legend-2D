using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Behaviour
{
    public class ResumeBehaviour : ButtonBehaviour
    {
        public override void OnClick(PointerEventData eventData)
        {
            EventAggregator.RaiseEvent<ResumeEvent>(
                new ResumeEvent()
                {
                    
                }
            );
        }
    }
}