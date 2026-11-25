using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler
{
    private static bool jumpRequested;

    public void OnPointerDown(PointerEventData eventData)
    {
        jumpRequested = true;
    }

    public static bool ConsumeJumpRequest()
    {
        bool requested = jumpRequested;
        jumpRequested = false;
        return requested;
    }

    public static void ResetJump()
    {
        jumpRequested = false;
    }
}