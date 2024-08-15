using UnityEngine;

public class CenterMoveWithinCircle : MonoBehaviour
{
    public Transform center; // Objeto que representa el centro del círculo
    public float radius = 5.0f; // Radio del círculo

    private bool isDragging = false;
    private Vector3 offset;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        Vector3 newPosition = transform.position; // Inicializar con la posición actual

        // Manejo del mouse
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Asegurarse de que la posición Z es 0

            if (IsTouchingObject(mousePosition))
            {
                isDragging = true;
                offset = transform.position - mousePosition;
            }
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Asegurarse de que la posición Z es 0

            newPosition = mousePosition + offset;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Manejo de toques
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0f;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (IsTouchingObject(touchPosition))
                    {
                        isDragging = true;
                        offset = transform.position - touchPosition;
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        newPosition = touchPosition + offset;
                    }
                    break;

                case TouchPhase.Ended:
                    isDragging = false;
                    break;
            }
        }

        Vector3 direction = newPosition - center.position;
        if (direction.magnitude > radius)
        {
            direction = direction.normalized * radius;
        }

        transform.position = center.position + direction;
    }

    private bool IsTouchingObject(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        return hit.collider != null && hit.collider.gameObject == gameObject;
    }
}
