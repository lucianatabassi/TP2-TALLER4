using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstrellaProximidad : MonoBehaviour
{
    public string romboTag = "Rombo"; // Etiqueta asignada a los rombos
    public float umbralProximidad = 2.0f; // Distancia para activar la animación

    private GameObject[] rombos; // Array para almacenar todos los rombos

    void Start()
    {
        // Encontrar todos los rombos con la etiqueta especificada
        rombos = GameObject.FindGameObjectsWithTag(romboTag);
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

        // Si el estado de proximidad ha cambiado, actualiza las animaciones
        ActivarAnimaciones(estrellaCerca);
    }

    // Método para activar o desactivar las animaciones "Agrandar"
    void ActivarAnimaciones(bool activar)
    {
        for (int i = 0; i < rombos.Length; i++)
        {
            ControladorRombo controlador = rombos[i].GetComponent<ControladorRombo>();
            if (controlador != null)
            {
                controlador.ActivarAnimacionAgrandar(activar);
            }

        }
    }
}
