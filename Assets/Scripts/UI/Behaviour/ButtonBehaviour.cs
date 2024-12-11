using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Behaviour
{
    public abstract class ButtonBehaviour : MonoBehaviour
    {
        public abstract void OnClick(PointerEventData eventData);
    }
}