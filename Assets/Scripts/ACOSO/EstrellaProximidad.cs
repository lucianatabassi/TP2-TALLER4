using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstrellaProximidad : MonoBehaviour
{
    public string romboTag = "Rombo";  // Etiqueta para identificar los rombos
    public float umbralProximidad = 2.0f;  // Distancia para activar las animaciones
    public GameObject[] agrandarAnimaciones; // Asigna los GameObjects de animaci�n "Agrandar"
    public GameObject[] idleAnimaciones;

    public AudioSource audioSource; // Referencia al componente AudioSource
    public AudioClip sonidoInteraccion; // Referencia al AudioClip para reproducir

    private GameObject[] rombos;  // Array para almacenar los rombos
    private bool sonidoReproducido = false; // Variable para controlar la reproducci�n del sonido

    void Start()
    {
        // Encontrar todos los rombos con la etiqueta especificada
        rombos = GameObject.FindGameObjectsWithTag(romboTag);

        // Inicializar las animaciones "Agrandar" como desactivadas
        InicializarAnimaciones();
    }

    void Update()
    {
        bool estrellaCerca = false;

        // Revisar la distancia a cada rombo
        for (int i = 0; i < rombos.Length; i++)
        {
            float distancia = Vector3.Distance(transform.position, rombos[i].transform.position);
            if (distancia < umbralProximidad)
            {
                estrellaCerca = true;
                Debug.Log("Rombo dentro del umbral: " + rombos[i].name);
                break;
            }
        }

        // Actualiza las animaciones solo cuando la estrella est� cerca
        ActivarAnimaciones(estrellaCerca);

        // Reproduce el sonido solo si la estrella est� cerca y no se ha reproducido a�n
        if (estrellaCerca && !sonidoReproducido)
        {
            ReproducirSonido();
            sonidoReproducido = true; // Marca que el sonido ha sido reproducido
        }
        else if (!estrellaCerca)
        {
            sonidoReproducido = false; // Resetea la variable si la estrella se aleja
        }
    }

    // M�todo para inicializar las animaciones "Agrandar"
    void InicializarAnimaciones()
    {
        foreach (GameObject animacion in agrandarAnimaciones)
        {
            if (animacion != null)
            {
                animacion.SetActive(false); // Desactivar animaciones "Agrandar" al inicio
            }
        }

        // Desactivar todas las animaciones "Idle"
        foreach (GameObject animacion in idleAnimaciones)
        {
            if (animacion != null)
            {
                animacion.SetActive(true); // Suponiendo que las animaciones "Idle" deben estar activas al inicio
            }
        }
    }

    // M�todo para activar o desactivar las animaciones "Agrandar"
    void ActivarAnimaciones(bool activar)
    {
        foreach (GameObject animacion in agrandarAnimaciones)
        {
            if (animacion != null)
            {
                animacion.SetActive(activar);
            }
        }

        // Desactivar las animaciones "Idle" si la estrella est� cerca
        foreach (GameObject animacion in idleAnimaciones)
        {
            if (animacion != null)
            {
                animacion.SetActive(!activar);
            }
        }
    }

    // M�todo para reproducir el sonido
    void ReproducirSonido()
    {
        if (audioSource != null && sonidoInteraccion != null)
        {
            audioSource.PlayOneShot(sonidoInteraccion);
        }
    }
}
