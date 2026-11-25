
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileCameraLook : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    [SerializeField] private float sensitivity = 0.15f;

    public Vector2 LookDelta { get; private set; }

    private Vector2 lastPosition;
    private bool dragging;

    public void OnPointerDown(PointerEventData eventData)
    {
        lastPosition = eventData.position;
        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging)
            return;

        Vector2 currentPosition = eventData.position;
        LookDelta = (currentPosition - lastPosition) * sensitivity;
        lastPosition = currentPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        LookDelta = Vector2.zero;
    }

    public void ResetDelta()
    {
        LookDelta = Vector2.zero;
    }
}
