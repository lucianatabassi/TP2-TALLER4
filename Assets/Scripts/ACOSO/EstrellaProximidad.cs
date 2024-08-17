using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstrellaProximidad : MonoBehaviour
{
    public string romboTag = "Rombo";  // Etiqueta para identificar los rombos
    public float umbralProximidad = 2.0f;  // Distancia para activar las animaciones
    public GameObject[] agrandarAnimaciones; // Asigna los GameObjects de animación "Agrandar"
    public GameObject[] idleAnimaciones;

    private GameObject[] rombos;  // Array para almacenar los rombos

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

        // Actualiza las animaciones solo cuando la estrella esté cerca
        ActivarAnimaciones(estrellaCerca);
    }

    // Método para inicializar las animaciones "Agrandar"
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

    // Método para activar o desactivar las animaciones "Agrandar"
    void ActivarAnimaciones(bool activar)
    {
        foreach (GameObject animacion in agrandarAnimaciones)
        {
            if (animacion != null)
            {
                animacion.SetActive(activar);
            }
        }

        // Desactivar las animaciones "Idle" si la estrella está cerca
        foreach (GameObject animacion in idleAnimaciones)
        {
            if (animacion != null)
            {
                animacion.SetActive(!activar);
            }
        }
    }
}
