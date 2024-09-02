using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    public PokeBallManager pokeBallManager;
    public float doubleTapThreshold = 0.3f;
    public float swipeThreshold = 400f; 

    private float lastTapTime = 0;
    private GameObject draggedObject = null;
    private Vector2 swipeStartPos;
    private bool isSwiping = false;

    public void OnTouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector2 touchPosition = context.ReadValue<Vector2>();
            swipeStartPos = touchPosition;
            isSwiping = false;

            if (Time.time - lastTapTime < doubleTapThreshold)
            {
                HandleDoubleTap(touchPosition);
            }
            else
            {
                HandleSimpleTap(touchPosition);
                CheckForDrag(touchPosition);
            }

            lastTapTime = Time.time;
        }
        else if (context.performed)
        {
            Vector2 touchPosition = context.ReadValue<Vector2>();

            if (draggedObject != null)
            {
                DragObject(touchPosition);
            }

            if ((touchPosition - swipeStartPos).sqrMagnitude > swipeThreshold) 
            {
                isSwiping = true;
            }
        }
        else if (context.canceled)
        {
            if (isSwiping)
            {
                HandleSwipe();
            }
            draggedObject = null;
        }
    }

    private void HandleSimpleTap(Vector2 touchPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;
        pokeBallManager.SpawnPokeBall(worldPosition);
    }

    private void HandleDoubleTap(Vector2 touchPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider != null)
        {
            Destroy(hit.collider.gameObject);
        }
    }

    private void CheckForDrag(Vector2 touchPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider != null)
        {
            draggedObject = hit.collider.gameObject;
        }
    }

    private void DragObject(Vector2 touchPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;
        draggedObject.transform.position = worldPosition;
    }

    private void HandleSwipe()
    {
        pokeBallManager.DeleteAllPokeBalls();
    }
}
