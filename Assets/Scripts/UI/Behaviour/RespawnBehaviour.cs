using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Behaviour
{
    public class RespawnBehaviour : ButtonBehaviour
    {
        public override void OnClick(PointerEventData eventData)
        {
            EventAggregator.RaiseEvent<RespawnEvent>(
                new RespawnEvent()
                {
                    
                }
            );
        }
    }
}