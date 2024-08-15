using UnityEngine;

public class ConstrainToCircle : MonoBehaviour
{
    public Transform center; // El objeto que representa el centro del círculo
    public float radius = 5.0f; // El radio del círculo

    private float angle; // El ángulo actual en radianes

    private void Start()
    {
        // Inicializar el ángulo basándose en la posición inicial del objeto
        Vector3 offset = transform.position - center.position;
        angle = Mathf.Atan2(offset.y, offset.x);
    }

    private void Update()
    {
        // Detectar el input horizontal y actualizar el ángulo
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontalInput) > 0.01f) // Asegurarse de que hay input significativo
        {
            angle += horizontalInput * Time.deltaTime;

            // Actualizar la posición del objeto sobre el círculo basándose en el ángulo
            Vector3 newPosition = center.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            transform.position = newPosition;
        }
    }
}
