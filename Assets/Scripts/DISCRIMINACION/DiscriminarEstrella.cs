using System.Collections;
using UnityEngine;

public class DiscriminarEstrella : MonoBehaviour
{
    public Animator animator; // Asigna el Animator desde el Inspector
    public string animationName = "Discriminacion"; // Nombre de la animaci�n a reproducir
    public string idleAnimationName = "Idle"; // Nombre de la animaci�n Idle (por defecto)
    public float proximityThreshold = 2.0f; // Distancia m�nima para activar la animaci�n
    public float moveDistance = 0.5f; // Distancia que se alejar� el rombo cuando la estrella se acerque
    public float moveSpeed = 5.0f; // Velocidad a la que el rombo se mueve (puedes ajustar esta velocidad)
    public float delayBeforeReturning = 1.0f; // Tiempo de espera antes de volver a la posici�n original
    public float starReturnDelay = 10.0f; // Tiempo antes de que la estrella comience a regresar a su posici�n original
    public float starReturnSpeed = 2.0f; // Velocidad a la que la estrella regresa a su posici�n original

    public AudioSource audioSource; // Referencia al componente AudioSource
    public AudioClip sonidoInteraccion; // Referencia al AudioClip para reproducir
    public AudioClip sonidoReinicio; // Referencia al AudioClip para el sonido de reinicio
    public float volumenSonidoReinicio = 1.0f; // Volumen del sonido de reinicio
    public Transform estrellaTransform; // Asigna el transform de la estrella desde el Inspector

    private Vector3 originalPosition; // Posici�n original del rombo
    private Vector3 targetPosition; // Posici�n objetivo a la que se mover� el rombo
    private Vector3 originalStarPosition; // Posici�n original de la estrella
    private bool isReturningToOriginalPosition = false;
    private bool isReturningStar = false; // Indica si la estrella est� regresando a su posici�n original
    private bool soundPlayed = false; // Para verificar si el sonido ya se ha reproducido

    void Start()
    {
        // Verifica que la estrella est� asignada desde el Inspector
        if (estrellaTransform != null)
        {
            originalStarPosition = estrellaTransform.position; // Guarda la posici�n original de la estrella
        }
        else
        {
            Debug.LogError("Estrella no asignada en el Inspector");
        }

        // Guarda la posici�n original del rombo
        originalPosition = transform.position;
        targetPosition = originalPosition; // Inicializa la posici�n objetivo
    }

    void Update()
    {
        if (estrellaTransform != null)
        {
            // Calcula la distancia entre la estrella y el rombo
            float distance = Vector3.Distance(transform.position, estrellaTransform.position);

            if (distance < proximityThreshold)
            {
                // Reproduce la animaci�n Discriminacion si la estrella est� cerca
                if (animator != null)
                {
                    animator.Play(animationName);
                }

                // Reproduce el sonido de interacci�n inmediatamente sin delay
                if (!soundPlayed && audioSource != null && sonidoInteraccion != null)
                {
                    audioSource.PlayOneShot(sonidoInteraccion);
                    soundPlayed = true; // Asegura que el sonido no se repita hasta que sea necesario
                }

                // Calcula la direcci�n y establece la posici�n objetivo
                Vector3 direction = (transform.position - estrellaTransform.position).normalized;
                targetPosition = originalPosition + direction * moveDistance;

                // Inicia la corrutina para que la estrella vuelva despu�s de 10 segundos
                if (!isReturningStar)
                {
                    StartCoroutine(ReturnStarAfterDelay());
                }

                // Detenemos el proceso de retorno a la posici�n original si estaba en curso
                if (isReturningToOriginalPosition)
                {
                    StopAllCoroutines();
                    isReturningToOriginalPosition = false;
                }
            }
            else
            {
                // Reproduce la animaci�n Idle si la estrella est� lejos
                if (animator != null)
                {
                    animator.Play(idleAnimationName);
                }

                // Establecer la posici�n objetivo para volver a la posici�n original
                if (!isReturningToOriginalPosition)
                {
                    StartCoroutine(ReturnToOriginalPositionAfterDelay());
                }
            }

            // Mueve el rombo suavemente hacia la posici�n objetivo
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Si la estrella est� regresando, mu�vela de regreso gradualmente a su posici�n original
            if (isReturningStar)
            {
                estrellaTransform.position = Vector3.Lerp(estrellaTransform.position, originalStarPosition, starReturnSpeed * Time.deltaTime);

                // Detenemos el retorno cuando la estrella alcanza su posici�n original
                if (Vector3.Distance(estrellaTransform.position, originalStarPosition) < 0.01f)
                {
                    estrellaTransform.position = originalStarPosition;
                    isReturningStar = false;

                    // Reproduce el sonido de reinicio solo despu�s de que la estrella ha vuelto a su posici�n original
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

        // Verificar nuevamente la distancia antes de regresar a la posici�n original
        float distance = Vector3.Distance(transform.position, estrellaTransform.position);
        if (distance >= proximityThreshold)
        {
            targetPosition = originalPosition;
            soundPlayed = false; // Resetear el estado de reproducci�n del sonido
        }

        isReturningToOriginalPosition = false;
    }

    IEnumerator ReturnStarAfterDelay()
    {
        yield return new WaitForSeconds(starReturnDelay);

        // Inicia el movimiento gradual de regreso de la estrella a su posici�n original
        isReturningStar = true;
    }
}
