using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool Pressed { get; private set; }
    public UnityEvent OnClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        OnClick.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
}
