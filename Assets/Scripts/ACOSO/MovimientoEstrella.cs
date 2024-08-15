using UnityEngine;

public class MovimientoEstrella : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;

    void Update()
    {
        // Detección de mouse
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Estrella"))
            {
                dragging = true;
                offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        if (dragging)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPosition.z = 0;
            transform.position = newPosition;
        }

        // Detección de touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("Estrella"))
                {
                    dragging = true;
                    offset = transform.position - touchPosition;
                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                dragging = false;
            }

            if (dragging && (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
            {
                transform.position = touchPosition + offset;
            }
        }
    }
}
