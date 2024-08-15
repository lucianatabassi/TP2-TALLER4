using UnityEngine;

public class RomboController : MonoBehaviour
{
    public Animator animator; // Asigna el Animator desde el Inspector
    public string animationName = "Desinteres"; // Nombre de la animaci�n a reproducir
    public string idleAnimationName = "Idle"; // Nombre de la animaci�n Idle (por defecto)
    public float proximityThreshold = 1.0f; // Distancia m�nima para activar la animaci�n
    public float moveDistance = 0.5f; // Distancia que se alejar� el rombo cuando la estrella se acerque
    public float moveSpeed = 5.0f; // Velocidad a la que el rombo se mueve (puedes ajustar esta velocidad)

    private Transform estrellaTransform; // Referencia al transform de la estrella
    private Vector3 originalPosition; // Posici�n original del rombo
    private Vector3 targetPosition; // Posici�n objetivo a la que se mover� el rombo

    private void Start()
    {
        // Encuentra el objeto estrella en la escena
        GameObject estrella = GameObject.FindWithTag("Estrella");
        if (estrella != null)
        {
            estrellaTransform = estrella.transform;
        }

        // Guarda la posici�n original del rombo
        originalPosition = transform.position;
        targetPosition = originalPosition; // Inicializa la posici�n objetivo
    }

    private void Update()
    {
        if (estrellaTransform != null)
        {
            // Calcula la distancia entre la estrella y el rombo
            float distance = Vector3.Distance(transform.position, estrellaTransform.position);

            if (distance < proximityThreshold)
            {
                // Reproduce la animaci�n Desinteres si la estrella est� cerca
                if (animator != null)
                {
                    animator.Play(animationName);
                }

                // Calcula la direcci�n y establece la posici�n objetivo
                Vector3 direction = (transform.position - estrellaTransform.position).normalized;
                targetPosition = originalPosition + direction * moveDistance;
            }
            else
            {
                // Reproduce la animaci�n Idle si la estrella est� lejos
                if (animator != null)
                {
                    animator.Play(idleAnimationName);
                }

                // Regresa a la posici�n original
                targetPosition = originalPosition;
            }

            // Mueve el rombo suavemente hacia la posici�n objetivo
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
