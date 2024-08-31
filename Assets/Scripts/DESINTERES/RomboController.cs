using System.Collections;
using UnityEngine;

public class RomboController : MonoBehaviour
{
    public Animator animator; // Asigna el Animator desde el Inspector
    public string animationName = "Desinteres"; // Nombre de la animaci�n a reproducir
    public string idleAnimationName = "Idle"; // Nombre de la animaci�n Idle (por defecto)
    public float proximityThreshold = 1.0f; // Distancia m�nima para activar la animaci�n
    public float moveDistance = 0.5f; // Distancia que se alejar� el rombo cuando la estrella se acerque
    public float moveSpeed = 5.0f; // Velocidad a la que el rombo se mueve (puedes ajustar esta velocidad)
    public AudioClip sonidoInteraccion; // AudioClip para el sonido de interacci�n
    public float intervaloSonido = 2.0f; // Intervalo de tiempo para repetir el sonido
    public float tiempoEsperaReinicio = 10.0f; // Tiempo de espera antes de reiniciar

    private Transform estrellaTransform; // Referencia al transform de la estrella
    private Vector3 originalPosition; // Posici�n original del rombo
    private Vector3 targetPosition; // Posici�n objetivo a la que se mover� el rombo
    private AudioSource audioSource; // Referencia al AudioSource
    private float tiempoUltimoSonido = 0f; // Tiempo en el que se reprodujo el sonido por �ltima vez
    private bool haInteractuado = false; // Bandera para controlar la interacci�n
    private Vector3 posicionInicialEstrella; // Posici�n inicial de la estrella
    private float duracionTransicionEstrella = 2f; // Duraci�n de la transici�n de la estrella

    private void Start()
    {
        // Encuentra el objeto estrella en la escena
        GameObject estrella = GameObject.FindWithTag("Estrella");
        if (estrella != null)
        {
            estrellaTransform = estrella.transform;
            posicionInicialEstrella = estrellaTransform.position; // Guardar la posici�n inicial de la estrella
        }

        // Guarda la posici�n original del rombo
        originalPosition = transform.position;
        targetPosition = originalPosition; // Inicializa la posici�n objetivo

        // Agrega o encuentra el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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

                // Reproduce el sonido de interacci�n a intervalos
                if (sonidoInteraccion != null && Time.time - tiempoUltimoSonido >= intervaloSonido)
                {
                    audioSource.PlayOneShot(sonidoInteraccion);
                    tiempoUltimoSonido = Time.time; // Actualiza el tiempo del �ltimo sonido
                }

                // Calcula la direcci�n y establece la posici�n objetivo
                Vector3 direction = (transform.position - estrellaTransform.position).normalized;
                targetPosition = originalPosition + direction * moveDistance;

                if (!haInteractuado)
                {
                    haInteractuado = true;
                    StartCoroutine(ReiniciarDespuesDeInteraccion());
                }
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

    IEnumerator ReiniciarDespuesDeInteraccion()
    {
        // Espera el tiempo especificado antes de iniciar el retorno de la estrella
        yield return new WaitForSeconds(tiempoEsperaReinicio);

        // Retornar la estrella a su posici�n inicial gradualmente
        yield return StartCoroutine(RetornarEstrellaGradualmente());

        // Reinicia el estado del rombo
        Reiniciar();
    }

    IEnumerator RetornarEstrellaGradualmente()
    {
        Vector3 posicionActual = estrellaTransform.position;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionTransicionEstrella)
        {
            estrellaTransform.position = Vector3.Lerp(posicionActual, posicionInicialEstrella, tiempoTranscurrido / duracionTransicionEstrella);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Asegura que la estrella vuelva exactamente a su posici�n inicial
        estrellaTransform.position = posicionInicialEstrella;
    }

    private void Reiniciar()
    {
        haInteractuado = false; // Reinicia la bandera de interacci�n
        targetPosition = originalPosition; // Restablece la posici�n objetivo
        transform.position = originalPosition; // Restablece la posici�n del rombo

        // Reinicia la animaci�n al estado Idle
        if (animator != null)
        {
            animator.Play(idleAnimationName);
        }
    }
}

