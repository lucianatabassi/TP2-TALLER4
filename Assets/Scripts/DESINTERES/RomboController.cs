using UnityEngine;

public class RomboController : MonoBehaviour
{
    public Animator animator; // Asigna el Animator desde el Inspector
    public string animationName = "Desinteres"; // Nombre de la animación a reproducir
    public string idleAnimationName = "Idle"; // Nombre de la animación Idle (por defecto)
    public float proximityThreshold = 1.0f; // Distancia mínima para activar la animación
    public float moveDistance = 0.5f; // Distancia que se alejará el rombo cuando la estrella se acerque
    public float moveSpeed = 5.0f; // Velocidad a la que el rombo se mueve (puedes ajustar esta velocidad)

    private Transform estrellaTransform; // Referencia al transform de la estrella
    private Vector3 originalPosition; // Posición original del rombo
    private Vector3 targetPosition; // Posición objetivo a la que se moverá el rombo

    private void Start()
    {
        // Encuentra el objeto estrella en la escena
        GameObject estrella = GameObject.FindWithTag("Estrella");
        if (estrella != null)
        {
            estrellaTransform = estrella.transform;
        }

        // Guarda la posición original del rombo
        originalPosition = transform.position;
        targetPosition = originalPosition; // Inicializa la posición objetivo
    }

    private void Update()
    {
        if (estrellaTransform != null)
        {
            // Calcula la distancia entre la estrella y el rombo
            float distance = Vector3.Distance(transform.position, estrellaTransform.position);

            if (distance < proximityThreshold)
            {
                // Reproduce la animación Desinteres si la estrella está cerca
                if (animator != null)
                {
                    animator.Play(animationName);
                }

                // Calcula la dirección y establece la posición objetivo
                Vector3 direction = (transform.position - estrellaTransform.position).normalized;
                targetPosition = originalPosition + direction * moveDistance;
            }
            else
            {
                // Reproduce la animación Idle si la estrella está lejos
                if (animator != null)
                {
                    animator.Play(idleAnimationName);
                }

                // Regresa a la posición original
                targetPosition = originalPosition;
            }

            // Mueve el rombo suavemente hacia la posición objetivo
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
