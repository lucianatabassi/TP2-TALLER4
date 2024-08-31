using System.Collections;
using UnityEngine;

public class RomboController : MonoBehaviour
{
    public Animator animator; // Asigna el Animator desde el Inspector
    public string animationName = "Desinteres"; // Nombre de la animación a reproducir
    public string idleAnimationName = "Idle"; // Nombre de la animación Idle (por defecto)
    public float proximityThreshold = 1.0f; // Distancia mínima para activar la animación
    public float moveDistance = 0.5f; // Distancia que se alejará el rombo cuando la estrella se acerque
    public float moveSpeed = 5.0f; // Velocidad a la que el rombo se mueve (puedes ajustar esta velocidad)
    public AudioClip sonidoInteraccion; // AudioClip para el sonido de interacción
    public float intervaloSonido = 2.0f; // Intervalo de tiempo para repetir el sonido
    public float tiempoEsperaReinicio = 10.0f; // Tiempo de espera antes de reiniciar

    private Transform estrellaTransform; // Referencia al transform de la estrella
    private Vector3 originalPosition; // Posición original del rombo
    private Vector3 targetPosition; // Posición objetivo a la que se moverá el rombo
    private AudioSource audioSource; // Referencia al AudioSource
    private float tiempoUltimoSonido = 0f; // Tiempo en el que se reprodujo el sonido por última vez
    private bool haInteractuado = false; // Bandera para controlar la interacción
    private Vector3 posicionInicialEstrella; // Posición inicial de la estrella
    private float duracionTransicionEstrella = 2f; // Duración de la transición de la estrella

    private void Start()
    {
        // Encuentra el objeto estrella en la escena
        GameObject estrella = GameObject.FindWithTag("Estrella");
        if (estrella != null)
        {
            estrellaTransform = estrella.transform;
            posicionInicialEstrella = estrellaTransform.position; // Guardar la posición inicial de la estrella
        }

        // Guarda la posición original del rombo
        originalPosition = transform.position;
        targetPosition = originalPosition; // Inicializa la posición objetivo

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
                // Reproduce la animación Desinteres si la estrella está cerca
                if (animator != null)
                {
                    animator.Play(animationName);
                }

                // Reproduce el sonido de interacción a intervalos
                if (sonidoInteraccion != null && Time.time - tiempoUltimoSonido >= intervaloSonido)
                {
                    audioSource.PlayOneShot(sonidoInteraccion);
                    tiempoUltimoSonido = Time.time; // Actualiza el tiempo del último sonido
                }

                // Calcula la dirección y establece la posición objetivo
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

    IEnumerator ReiniciarDespuesDeInteraccion()
    {
        // Espera el tiempo especificado antes de iniciar el retorno de la estrella
        yield return new WaitForSeconds(tiempoEsperaReinicio);

        // Retornar la estrella a su posición inicial gradualmente
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

        // Asegura que la estrella vuelva exactamente a su posición inicial
        estrellaTransform.position = posicionInicialEstrella;
    }

    private void Reiniciar()
    {
        haInteractuado = false; // Reinicia la bandera de interacción
        targetPosition = originalPosition; // Restablece la posición objetivo
        transform.position = originalPosition; // Restablece la posición del rombo

        // Reinicia la animación al estado Idle
        if (animator != null)
        {
            animator.Play(idleAnimationName);
        }
    }
}

