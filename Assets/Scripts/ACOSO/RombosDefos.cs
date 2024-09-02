using System.Collections;
using UnityEngine;

public class RombosDefos : MonoBehaviour
{
    public GameObject estrella;
    public float distanciaReferencia = 5f;
    public Vector3 escalaBase = new Vector3(1f, 1f, 1f);
    public float factorEscalado = 2f;
    public AudioClip sonidoInteraccion;
    public AudioClip sonidoReinicio;  // AudioClip para el sonido de reinicio
    public float volumenInteraccion = 0.5f;
    public float volumenReinicio = 0.5f;  // Volumen espec�fico para el sonido de reinicio
    public float tiempoEsperaReinicio = 3f;
    public float duracionTransicion = 2f;

    private AudioSource audioSourceInteraccion;
    private AudioSource audioSourceReinicio;
    private bool sonidoReproducido = false;
    private Vector3 escalaOriginal;
    private Vector3 posicionOriginal;  // Variable para almacenar la posici�n original
    private bool enTransicion = false;
    private bool haInteractuado = false;

    void Start()
    {
        // Inicializa el AudioSource para la interacci�n
        audioSourceInteraccion = gameObject.AddComponent<AudioSource>();
        audioSourceInteraccion.clip = sonidoInteraccion;
        audioSourceInteraccion.volume = volumenInteraccion;

        // Inicializa el AudioSource para el sonido de reinicio
        audioSourceReinicio = gameObject.AddComponent<AudioSource>();
        audioSourceReinicio.clip = sonidoReinicio;
        audioSourceReinicio.volume = volumenReinicio;

        escalaOriginal = transform.localScale;
        posicionOriginal = estrella.transform.position;  // Guardamos la posici�n original de la estrella

        Debug.Log("Posici�n Original de la Estrella: " + posicionOriginal);
    }

    void Update()
    {
        if (estrella == null || enTransicion) return;

        float distancia = Vector3.Distance(transform.position, estrella.transform.position);
        float escala = Mathf.Clamp(distancia / distanciaReferencia, 0f, factorEscalado);
        transform.localScale = escalaBase * escala;

        if (distancia < distanciaReferencia && !sonidoReproducido)
        {
            audioSourceInteraccion.Play();
            sonidoReproducido = true;

            if (!haInteractuado)
            {
                haInteractuado = true;
                StartCoroutine(ReiniciarDespuesDeInteraccion());
            }
        }

        if (distancia >= distanciaReferencia)
        {
            sonidoReproducido = false;
        }
    }

    IEnumerator ReiniciarDespuesDeInteraccion()
    {
        yield return new WaitForSeconds(tiempoEsperaReinicio);
        StartCoroutine(RetornarGradualmente());
    }

    IEnumerator RetornarGradualmente()
    {
        enTransicion = true;
        Vector3 escalaActual = transform.localScale;
        Vector3 posicionActual = estrella.transform.position;  // Tomamos la posici�n actual de la estrella
        float tiempoTranscurrido = 0f;

        // Reproduce el sonido de reinicio usando el AudioSource espec�fico
        if (audioSourceReinicio.clip != null)
        {
            audioSourceReinicio.Play();
            Debug.Log("Sonido de reinicio reproducido.");
        }
        else
        {
            Debug.LogWarning("AudioClip de reinicio no est� asignado.");
        }

        Debug.Log("Posici�n Actual de la Estrella al Iniciar Transici�n: " + posicionActual);

        // Transici�n de escala y posici�n
        while (tiempoTranscurrido < duracionTransicion)
        {
            transform.localScale = Vector3.Lerp(escalaActual, escalaOriginal, tiempoTranscurrido / duracionTransicion);
            estrella.transform.position = Vector3.Lerp(posicionActual, posicionOriginal, tiempoTranscurrido / duracionTransicion);  // Transici�n de posici�n
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        transform.localScale = escalaOriginal;
        estrella.transform.position = posicionOriginal;  // Asegura que la posici�n final de la estrella sea exacta

        Debug.Log("Posici�n Final de la Estrella despu�s de Transici�n: " + estrella.transform.position);

        enTransicion = false;
        haInteractuado = false;
    }
}
