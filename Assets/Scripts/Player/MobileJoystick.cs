using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;

    public Vector2 Direction { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (background == null || handle == null)
            return;

        Vector2 position;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out position))
            return;

        Vector2 halfSize = background.rect.size * 0.5f;

        Direction = new Vector2(
            position.x / halfSize.x,
            position.y / halfSize.y
        );

        Direction = Vector2.ClampMagnitude(Direction, 1f);

        handle.anchoredPosition = new Vector2(
            Direction.x * halfSize.x * 0.5f,
            Direction.y * halfSize.y * 0.5f
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Direction = Vector2.zero;

        if (handle != null)
            handle.anchoredPosition = Vector2.zero;
    }
}
