using UnityEngine.EventSystems;

namespace UI.Behaviour
{
    public class QuitBehaviour : ButtonBehaviour
    {
        public override void OnClick(PointerEventData eventData)
        {
            EventAggregator.RaiseEvent<QuitToMenuEvent>(
                new QuitToMenuEvent()
                {
                    
                }
            );
        }
    }
}