using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RombosDesaparecen : MonoBehaviour
{
    public GameObject estrella;  // Referencia al objeto estrella
    public float distanciaDesaparicion = 1f;  // Distancia para que comience la desaparición
    public float velocidadDesaparicion = 2f;  // Velocidad de desaparición
    public AudioClip sonidoInteraccion;  // Clip de sonido a reproducir
    public float volumenSonido = 1f;  // Volumen del sonido

    private SpriteRenderer spriteRenderer;
    private bool debeDesaparecer = false;  // Bandera para activar la desaparición
    private Color colorInicial;
    private AudioSource audioSource;  // Fuente de audio para reproducir el sonido

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
        }
        else
        {
            // Si la estrella se aproxima al rombo, activa la desaparición
            float distancia = Vector3.Distance(transform.position, estrella.transform.position);
            if (distancia <= distanciaDesaparicion)
            {
                debeDesaparecer = true;

                // Reproducir el sonido de interacción
                if (audioSource != null && !audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
    }

    // Método para reiniciar el estado del rombo
    public void Reiniciar()
    {
        debeDesaparecer = false;
        spriteRenderer.enabled = true;  // Volver a activar el SpriteRenderer
        Color colorRestaurado = new Color(colorInicial.r, colorInicial.g, colorInicial.b, 1f);
        spriteRenderer.color = colorRestaurado;

        // Detener el sonido si está reproduciéndose
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
