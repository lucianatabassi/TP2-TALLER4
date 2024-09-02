using System.Collections;
using UnityEngine;

public class DiscriminarEstrella : MonoBehaviour
{
    public Animator animator; // Asigna el Animator desde el Inspector
    public string animationName = "Discriminacion"; // Nombre de la animación a reproducir
    public string idleAnimationName = "Idle"; // Nombre de la animación Idle (por defecto)
    public float proximityThreshold = 2.0f; // Distancia mínima para activar la animación
    public float moveDistance = 0.5f; // Distancia que se alejará el rombo cuando la estrella se acerque
    public float moveSpeed = 5.0f; // Velocidad a la que el rombo se mueve (puedes ajustar esta velocidad)
    public float delayBeforeReturning = 1.0f; // Tiempo de espera antes de volver a la posición original
    public float starReturnDelay = 10.0f; // Tiempo antes de que la estrella comience a regresar a su posición original
    public float starReturnSpeed = 2.0f; // Velocidad a la que la estrella regresa a su posición original

    public AudioSource audioSource; // Referencia al componente AudioSource
    public AudioClip sonidoInteraccion; // Referencia al AudioClip para reproducir
    public AudioClip sonidoReinicio; // Referencia al AudioClip para el sonido de reinicio
    public float volumenSonidoReinicio = 1.0f; // Volumen del sonido de reinicio
    public Transform estrellaTransform; // Asigna el transform de la estrella desde el Inspector

    private Vector3 originalPosition; // Posición original del rombo
    private Vector3 targetPosition; // Posición objetivo a la que se moverá el rombo
    private Vector3 originalStarPosition; // Posición original de la estrella
    private bool isReturningToOriginalPosition = false;
    private bool isReturningStar = false; // Indica si la estrella está regresando a su posición original
    private bool soundPlayed = false; // Para verificar si el sonido ya se ha reproducido

    void Start()
    {
        // Verifica que la estrella esté asignada desde el Inspector
        if (estrellaTransform != null)
        {
            originalStarPosition = estrellaTransform.position; // Guarda la posición original de la estrella
        }
        else
        {
            Debug.LogError("Estrella no asignada en el Inspector");
        }

        // Guarda la posición original del rombo
        originalPosition = transform.position;
        targetPosition = originalPosition; // Inicializa la posición objetivo
    }

    void Update()
    {
        if (estrellaTransform != null)
        {
            // Calcula la distancia entre la estrella y el rombo
            float distance = Vector3.Distance(transform.position, estrellaTransform.position);

            if (distance < proximityThreshold)
            {
                // Reproduce la animación Discriminacion si la estrella está cerca
                if (animator != null)
                {
                    animator.Play(animationName);
                }

                // Reproduce el sonido de interacción inmediatamente sin delay
                if (!soundPlayed && audioSource != null && sonidoInteraccion != null)
                {
                    audioSource.PlayOneShot(sonidoInteraccion);
                    soundPlayed = true; // Asegura que el sonido no se repita hasta que sea necesario
                }

                // Calcula la dirección y establece la posición objetivo
                Vector3 direction = (transform.position - estrellaTransform.position).normalized;
                targetPosition = originalPosition + direction * moveDistance;

                // Inicia la corrutina para que la estrella vuelva después de 10 segundos
                if (!isReturningStar)
                {
                    StartCoroutine(ReturnStarAfterDelay());
                }

                // Detenemos el proceso de retorno a la posición original si estaba en curso
                if (isReturningToOriginalPosition)
                {
                    StopAllCoroutines();
                    isReturningToOriginalPosition = false;
                }
            }
            else
            {
                // Reproduce la animación Idle si la estrella está lejos
                if (animator != null)
                {
                    animator.Play(idleAnimationName);
                }

                // Establecer la posición objetivo para volver a la posición original
                if (!isReturningToOriginalPosition)
                {
                    StartCoroutine(ReturnToOriginalPositionAfterDelay());
                }
            }

            // Mueve el rombo suavemente hacia la posición objetivo
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Si la estrella está regresando, muévela de regreso gradualmente a su posición original
            if (isReturningStar)
            {
                estrellaTransform.position = Vector3.Lerp(estrellaTransform.position, originalStarPosition, starReturnSpeed * Time.deltaTime);

                // Detenemos el retorno cuando la estrella alcanza su posición original
                if (Vector3.Distance(estrellaTransform.position, originalStarPosition) < 0.01f)
                {
                    estrellaTransform.position = originalStarPosition;
                    isReturningStar = false;

                    // Reproduce el sonido de reinicio solo después de que la estrella ha vuelto a su posición original
                    if (audioSource != null && sonidoReinicio != null)
                    {
                        audioSource.PlayOneShot(sonidoReinicio, volumenSonidoReinicio);
                    }
                }
            }
        }
    }

    IEnumerator ReturnToOriginalPositionAfterDelay()
    {
        isReturningToOriginalPosition = true;
        yield return new WaitForSeconds(delayBeforeReturning);

        // Verificar nuevamente la distancia antes de regresar a la posición original
        float distance = Vector3.Distance(transform.position, estrellaTransform.position);
        if (distance >= proximityThreshold)
        {
            targetPosition = originalPosition;
            soundPlayed = false; // Resetear el estado de reproducción del sonido
        }

        isReturningToOriginalPosition = false;
    }

    IEnumerator ReturnStarAfterDelay()
    {
        yield return new WaitForSeconds(starReturnDelay);

        // Inicia el movimiento gradual de regreso de la estrella a su posición original
        isReturningStar = true;
    }
}
