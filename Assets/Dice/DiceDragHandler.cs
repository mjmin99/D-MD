using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace DiceSystem
{
    public class DiceDragHandler : MonoBehaviour
    {
        private bool isDragging;
        private Vector3 offset;
        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current == null)
                return;
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            if (!isDragging && Mouse.current.leftButton.wasPressedThisFrame)
            {
                TryBeginDrag();
            }

            if (isDragging && Mouse.current.leftButton.isPressed)
            {
                Drag();
            }

            if (isDragging && Mouse.current.leftButton.wasReleasedThisFrame)
            {
                EndDrag();
            }
        }

        private void TryBeginDrag()
        {
            Vector2 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                offset = transform.position - (Vector3)mouseWorld;
            }
        }

        private void Drag()
        {
            Vector2 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            transform.position = (Vector3)mouseWorld + offset;
        }

        private void EndDrag()
        {
            isDragging = false;
        }
    }
}
