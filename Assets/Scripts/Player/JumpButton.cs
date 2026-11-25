using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler
{
    public static bool JumpPressed { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        JumpPressed = true;
    }

    public static void ResetJump()
    {
        JumpPressed = false;
    }
}
