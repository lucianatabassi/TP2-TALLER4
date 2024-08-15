using UnityEngine;

public class ConstrainToCircle : MonoBehaviour
{
    public Transform center; // El objeto que representa el centro del c�rculo
    public float radius = 5.0f; // El radio del c�rculo

    private float angle; // El �ngulo actual en radianes

    private void Start()
    {
        // Inicializar el �ngulo bas�ndose en la posici�n inicial del objeto
        Vector3 offset = transform.position - center.position;
        angle = Mathf.Atan2(offset.y, offset.x);
    }

    private void Update()
    {
        // Detectar el input horizontal y actualizar el �ngulo
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontalInput) > 0.01f) // Asegurarse de que hay input significativo
        {
            angle += horizontalInput * Time.deltaTime;

            // Actualizar la posici�n del objeto sobre el c�rculo bas�ndose en el �ngulo
            Vector3 newPosition = center.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            transform.position = newPosition;
        }
    }
}
