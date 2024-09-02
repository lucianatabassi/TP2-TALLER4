using System.Collections;
using UnityEngine;

public class RombosDesaparecen : MonoBehaviour
{
    public GameObject estrella;  // Referencia al objeto estrella
    public float distanciaDesaparicion = 1f;  // Distancia para que comience la desaparición
    public float velocidadDesaparicion = 2f;  // Velocidad de desaparición
    public AudioClip sonidoInteraccion;  // Clip de sonido a reproducir al desaparecer
    public AudioClip sonidoReinicio;  // Clip de sonido para el reinicio
    public float volumenSonido = 1f;  // Volumen del sonido de interacción
    public float volumenReinicio = 1f;  // Volumen del sonido de reinicio
    public float tiempoEsperaReinicio = 3f;  // Tiempo de espera antes de reiniciar
    public float duracionTransicion = 2f;  // Duración en segundos para que la estrella vuelva a su posición inicial
    public float velocidadReaparicion = 2f;  // Velocidad de reaparición de los rombos

    private SpriteRenderer spriteRenderer;
    private bool debeDesaparecer = false;  // Bandera para activar la desaparición
    private Color colorInicial;
    private AudioSource audioSourceInteraccion;  // Fuente de audio para el sonido de interacción
    private AudioSource audioSourceReinicio;  // Fuente de audio para el sonido de reinicio
    private Vector3 posicionInicialEstrella;  // Posición inicial de la estrella
    private bool haInteractuado = false;  // Para controlar la interacción

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  // Obtiene el SpriteRenderer del rombo
        colorInicial = spriteRenderer.color;  // Guarda el color inicial del rombo

        // Configurar AudioSource para el sonido de interacción
        audioSourceInteraccion = gameObject.AddComponent<AudioSource>();
        audioSourceInteraccion.clip = sonidoInteraccion;
        audioSourceInteraccion.volume = volumenSonido;

        // Configurar AudioSource para el sonido de reinicio
        audioSourceReinicio = gameObject.AddComponent<AudioSource>();
        audioSourceReinicio.clip = sonidoReinicio;
        audioSourceReinicio.volume = volumenReinicio;

        // Guardar la posición inicial de la estrella
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

            // Iniciar la corrutina de reinicio después de la interacción
            if (!haInteractuado)
            {
                haInteractuado = true;
                StartCoroutine(ReiniciarDespuesDeInteraccion());
            }
        }
        else
        {
            // Si la estrella se aproxima al rombo, activa la desaparición
            float distancia = Vector3.Distance(transform.position, estrella.transform.position);
            if (distancia <= distanciaDesaparicion)
            {
                debeDesaparecer = true;

                // Reproducir el sonido de interacción
                if (audioSourceInteraccion != null && !audioSourceInteraccion.isPlaying)
                {
                    audioSourceInteraccion.Play();
                }
            }
        }
    }

    IEnumerator ReiniciarDespuesDeInteraccion()
    {
        // Esperar el tiempo especificado antes de iniciar el retorno de la estrella y el reinicio
        yield return new WaitForSeconds(tiempoEsperaReinicio);

        // Reproducir el sonido de reinicio
        if (audioSourceReinicio != null)
        {
            audioSourceReinicio.Play();
        }

        // Iniciar el retorno de la estrella a su posición inicial
        yield return StartCoroutine(RetornarEstrellaGradualmente());

        // Iniciar el proceso de reaparición del rombo
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
            // Interpolación lineal para mover la estrella gradualmente a su posición inicial
            estrella.transform.position = Vector3.Lerp(posicionActual, posicionInicialEstrella, tiempoTranscurrido / duracionTransicion);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la estrella llegue exactamente a la posición inicial
        estrella.transform.position = posicionInicialEstrella;
    }

    IEnumerator ReaparecerRomboGradualmente()
    {
        // Restablecer la opacidad del rombo desde 0 a 1
        float tiempoTranscurrido = 0f;
        spriteRenderer.enabled = true;  // Asegurarse de que el SpriteRenderer esté activado

        while (tiempoTranscurrido < 1f / velocidadReaparicion)
        {
            float nuevoAlpha = Mathf.Lerp(0f, 1f, tiempoTranscurrido * velocidadReaparicion);
            Color nuevoColor = new Color(colorInicial.r, colorInicial.g, colorInicial.b, nuevoAlpha);
            spriteRenderer.color = nuevoColor;

            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que el rombo esté completamente visible
        spriteRenderer.color = colorInicial;
    }

    // Método para reiniciar el estado del rombo
    public void Reiniciar()
    {
        debeDesaparecer = false;
        spriteRenderer.enabled = true;  // Volver a activar el SpriteRenderer
        Color colorRestaurado = new Color(colorInicial.r, colorInicial.g, colorInicial.b, 1f);
        spriteRenderer.color = colorRestaurado;

        // Detener el sonido de interacción si está reproduciéndose
        if (audioSourceInteraccion != null && audioSourceInteraccion.isPlaying)
        {
            audioSourceInteraccion.Stop();
        }
    }
}
