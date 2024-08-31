using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RombosDesaparecen : MonoBehaviour
{
    public GameObject estrella;  // Referencia al objeto estrella
    public float distanciaDesaparicion = 1f;  // Distancia para que comience la desaparici�n
    public float velocidadDesaparicion = 2f;  // Velocidad de desaparici�n
    public AudioClip sonidoInteraccion;  // Clip de sonido a reproducir
    public float volumenSonido = 1f;  // Volumen del sonido
    public float tiempoEsperaReinicio = 3f;  // Tiempo de espera antes de reiniciar
    public float duracionTransicion = 2f;  // Duraci�n en segundos para que la estrella vuelva a su posici�n inicial
    public float velocidadReaparicion = 2f;  // Velocidad de reaparici�n de los rombos

    private SpriteRenderer spriteRenderer;
    private bool debeDesaparecer = false;  // Bandera para activar la desaparici�n
    private Color colorInicial;
    private AudioSource audioSource;  // Fuente de audio para reproducir el sonido
    private Vector3 posicionInicialEstrella;  // Posici�n inicial de la estrella
    private bool haInteractuado = false;  // Para controlar la interacci�n

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  // Obtiene el SpriteRenderer del rombo
        colorInicial = spriteRenderer.color;  // Guarda el color inicial del rombo
        audioSource = GetComponent<AudioSource>();  // Obtiene el AudioSource del rombo

        if (audioSource != null && sonidoInteraccion != null)
        {
            audioSource.clip = sonidoInteraccion;
            audioSource.volume = volumenSonido;
        }

        // Guardar la posici�n inicial de la estrella
        if (estrella != null)
        {
            posicionInicialEstrella = estrella.transform.position;
        }
    }

    void Update()
    {
        // Si el rombo debe desaparecer, reducimos su opacidad
        if (debeDesaparecer)
        {
            float nuevoAlpha = Mathf.Lerp(spriteRenderer.color.a, 0f, Time.deltaTime * velocidadDesaparicion);
            Color nuevoColor = new Color(colorInicial.r, colorInicial.g, colorInicial.b, nuevoAlpha);
            spriteRenderer.color = nuevoColor;

            // No destruimos el objeto, solo lo hacemos invisible
            if (nuevoAlpha <= 0.01f)
            {
                spriteRenderer.enabled = false;  // Desactivar el SpriteRenderer para hacerlo invisible
            }

            // Iniciar la corrutina de reinicio despu�s de la interacci�n
            if (!haInteractuado)
            {
                haInteractuado = true;
                StartCoroutine(ReiniciarDespuesDeInteraccion());
            }
        }
        else
        {
            // Si la estrella se aproxima al rombo, activa la desaparici�n
            float distancia = Vector3.Distance(transform.position, estrella.transform.position);
            if (distancia <= distanciaDesaparicion)
            {
                debeDesaparecer = true;

                // Reproducir el sonido de interacci�n
                if (audioSource != null && !audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
    }

    IEnumerator ReiniciarDespuesDeInteraccion()
    {
        // Esperar el tiempo especificado antes de iniciar el retorno de la estrella y el reinicio
        yield return new WaitForSeconds(tiempoEsperaReinicio);

        // Iniciar el retorno de la estrella a su posici�n inicial
        yield return StartCoroutine(RetornarEstrellaGradualmente());

        // Iniciar el proceso de reaparici�n del rombo
        yield return StartCoroutine(ReaparecerRomboGradualmente());

        // Reiniciar el estado del rombo
        Reiniciar();
        haInteractuado = false;  // Reiniciar la variable para permitir futuras interacciones
    }

    IEnumerator RetornarEstrellaGradualmente()
    {
        Vector3 posicionActual = estrella.transform.position;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionTransicion)
        {
            // Interpolaci�n lineal para mover la estrella gradualmente a su posici�n inicial
            estrella.transform.position = Vector3.Lerp(posicionActual, posicionInicialEstrella, tiempoTranscurrido / duracionTransicion);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la estrella llegue exactamente a la posici�n inicial
        estrella.transform.position = posicionInicialEstrella;
    }

    IEnumerator ReaparecerRomboGradualmente()
    {
        // Restablecer la opacidad del rombo desde 0 a 1
        float tiempoTranscurrido = 0f;
        spriteRenderer.enabled = true;  // Asegurarse de que el SpriteRenderer est� activado

        while (tiempoTranscurrido < 1f / velocidadReaparicion)
        {
            float nuevoAlpha = Mathf.Lerp(0f, 1f, tiempoTranscurrido * velocidadReaparicion);
            Color nuevoColor = new Color(colorInicial.r, colorInicial.g, colorInicial.b, nuevoAlpha);
            spriteRenderer.color = nuevoColor;

            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que el rombo est� completamente visible
        spriteRenderer.color = colorInicial;
    }

    // M�todo para reiniciar el estado del rombo
    public void Reiniciar()
    {
        debeDesaparecer = false;
        spriteRenderer.enabled = true;  // Volver a activar el SpriteRenderer
        Color colorRestaurado = new Color(colorInicial.r, colorInicial.g, colorInicial.b, 1f);
        spriteRenderer.color = colorRestaurado;

        // Detener el sonido si est� reproduci�ndose
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
