using System.Collections;
using UnityEngine;

public class RomboController : MonoBehaviour
{
    public Animator animator;
    public string animationName = "Desinteres";
    public string idleAnimationName = "Idle";
    public float proximityThreshold = 1.0f;
    public float moveDistance = 0.5f;
    public float moveSpeed = 5.0f;
    public AudioClip sonidoInteraccion;
    public AudioClip sonidoReinicio;
    public float intervaloSonido = 2.0f;
    public float tiempoEsperaReinicio = 10.0f;
    public float duracionTransicionEstrella = 2f;
    public float volumenSonidoReinicio = 1.0f; // Nuevo campo para el volumen del sonido de reinicio

    private Transform estrellaTransform;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private AudioSource audioSource;
    private bool haInteractuado = false;
    private bool sonidoReproducido = false;
    private bool reiniciando = false;
    private Vector3 posicionInicialEstrella;
    private float tiempoDesdeUltimoSonido = 0f;

    private void Start()
    {
        GameObject estrella = GameObject.FindWithTag("Estrella");
        if (estrella != null)
        {
            estrellaTransform = estrella.transform;
            posicionInicialEstrella = estrellaTransform.position;
        }

        originalPosition = transform.position;
        targetPosition = originalPosition;

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
            float distance = Vector3.Distance(transform.position, estrellaTransform.position);

            if (distance < proximityThreshold)
            {
                if (animator != null)
                {
                    animator.Play(animationName);
                }

                if (sonidoInteraccion != null)
                {
                    tiempoDesdeUltimoSonido += Time.deltaTime;
                    if (!sonidoReproducido || tiempoDesdeUltimoSonido >= intervaloSonido)
                    {
                        audioSource.PlayOneShot(sonidoInteraccion);
                        sonidoReproducido = true;
                        tiempoDesdeUltimoSonido = 0f;
                    }
                }

                Vector3 direction = (transform.position - estrellaTransform.position).normalized;
                targetPosition = originalPosition + direction * moveDistance;

                if (!haInteractuado && !reiniciando)
                {
                    haInteractuado = true;
                    StartCoroutine(ReiniciarDespuesDeInteraccion());
                }
            }
            else
            {
                if (animator != null)
                {
                    animator.Play(idleAnimationName);
                }

                targetPosition = originalPosition;
                sonidoReproducido = false;
                haInteractuado = false;
            }

            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    IEnumerator ReiniciarDespuesDeInteraccion()
    {
        reiniciando = true;

        yield return new WaitForSeconds(tiempoEsperaReinicio);

        yield return StartCoroutine(RetornarEstrellaGradualmente());

        Reiniciar();
        reiniciando = false;
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

        estrellaTransform.position = posicionInicialEstrella;
    }

    private void Reiniciar()
    {
        sonidoReproducido = false;
        targetPosition = originalPosition;
        transform.position = originalPosition;

        if (sonidoReinicio != null)
        {
            audioSource.PlayOneShot(sonidoReinicio, volumenSonidoReinicio); // Usa el volumen especificado
        }

        if (animator != null)
        {
            animator.Play(idleAnimationName);
        }
    }
}
