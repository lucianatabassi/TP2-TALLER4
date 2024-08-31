using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RombosDefos : MonoBehaviour
{
    public GameObject estrella;  // Referencia al objeto estrella
    public float distanciaReferencia = 5f;  // Distancia en la que el rombo se ajusta para tocar la estrella
    public Vector3 escalaBase = new Vector3(1f, 1f, 1f);  // Tamaño base del rombo
    public float factorEscalado = 2f;  // Factor de escala para ajustar la forma del rombo
    public AudioClip sonidoInteraccion;  // Sonido a reproducir cuando se produce la interacción
    public float volumen = 0.5f;  // Volumen del sonido (0.0f a 1.0f)
    public float tiempoEsperaReinicio = 3f; // Tiempo de espera después de la interacción antes de reiniciar
    public float duracionTransicion = 2f; // Duración en segundos para la transición gradual

    private AudioSource audioSource;  // Componente de AudioSource
    private bool sonidoReproducido = false;  // Para evitar reproducir el sonido varias veces
    private Vector3 escalaOriginal; // Para guardar la escala original del rombo
    private bool enTransicion = false;  // Indica si el rombo está en transición
    private bool haInteractuado = false; // Indica si ha habido interacción

    void Start()
    {
        // Inicializa el componente AudioSource y guarda las propiedades originales
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sonidoInteraccion;
        audioSource.volume = volumen;  // Ajusta el volumen
        escalaOriginal = transform.localScale;  // Guarda la escala original
    }

    void Update()
    {
        if (estrella == null || enTransicion) return;

        // Calcula la distancia entre el rombo y la estrella
        float distancia = Vector3.Distance(transform.position, estrella.transform.position);

        // Calcula la escala en función de la distancia
        float escala = Mathf.Clamp(distancia / distanciaReferencia, 0f, factorEscalado);

        // Ajusta la escala del rombo
        transform.localScale = escalaBase * escala;

        // Reproduce el sonido cuando la distancia es menor que la distancia de referencia y el sonido no se ha reproducido
        if (distancia < distanciaReferencia && !sonidoReproducido)
        {
            audioSource.Play();
            sonidoReproducido = true;  // Evita que el sonido se reproduzca varias veces

            if (!haInteractuado)
            {
                haInteractuado = true;
                StartCoroutine(ReiniciarDespuesDeInteraccion());
            }
        }

        // Restablece la posibilidad de reproducir el sonido si la distancia aumenta nuevamente
        if (distancia >= distanciaReferencia)
        {
            sonidoReproducido = false;
        }
    }

    IEnumerator ReiniciarDespuesDeInteraccion()
    {
        // Espera el tiempo especificado antes de iniciar el retorno gradual
        yield return new WaitForSeconds(tiempoEsperaReinicio);

        // Inicia la transición de escala gradual
        StartCoroutine(RetornarGradualmente());
    }

    IEnumerator RetornarGradualmente()
    {
        enTransicion = true;
        Vector3 escalaActual = transform.localScale;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionTransicion)
        {
            // Interpolación lineal para cambiar la escala del rombo gradualmente a su escala original
            transform.localScale = Vector3.Lerp(escalaActual, escalaOriginal, tiempoTranscurrido / duracionTransicion);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que el rombo llegue exactamente a la escala original
        transform.localScale = escalaOriginal;
        enTransicion = false;
        haInteractuado = false; // Reinicia la variable para futuras interacciones
    }
}
