using System.Collections;
using System.Collections.Generic;
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

    private Transform estrellaTransform; // Referencia al transform de la estrella
    private Vector3 originalPosition; // Posición original del rombo
    private Vector3 targetPosition; // Posición objetivo a la que se moverá el rombo
    private bool isReturningToOriginalPosition = false;

    void Start()
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

                // Calcula la dirección y establece la posición objetivo
                Vector3 direction = (transform.position - estrellaTransform.position).normalized;
                targetPosition = originalPosition + direction * moveDistance;

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

                // Establecer la posición objetivo para volver a la posición original después de un segundo
                if (!isReturningToOriginalPosition)
                {
                    StartCoroutine(ReturnToOriginalPositionAfterDelay());
                }
            }

            // Mueve el rombo suavemente hacia la posición objetivo
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
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
        }

        isReturningToOriginalPosition = false;
    }

    public void ReiniciarPosicionYAnimacion()
    {
        // Reiniciar la posición del rombo a la original
        transform.position = originalPosition;

        // Reiniciar la animación a Idle
        if (animator != null)
        {
            animator.Play(idleAnimationName);
        }

        // Resetear el estado de proximidad
        StopAllCoroutines();
        isReturningToOriginalPosition = false;
    }
}
