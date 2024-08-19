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
    private AudioSource audioSource;  // Componente de AudioSource

    private bool sonidoReproducido = false;  // Para evitar reproducir el sonido varias veces

    void Start()
    {
        // Inicializa el componente AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sonidoInteraccion;
        audioSource.volume = volumen;  // Ajusta el volumen
    }

    void Update()
    {
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
        }

        // Restablece la posibilidad de reproducir el sonido si la distancia aumenta nuevamente
        if (distancia >= distanciaReferencia)
        {
            sonidoReproducido = false;
        }
    }
}
