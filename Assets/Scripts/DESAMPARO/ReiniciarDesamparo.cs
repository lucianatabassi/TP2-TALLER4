using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ReiniciarDesamparo : MonoBehaviour
{
    public GameObject estrella;  // Referencia a la estrella
    public RombosDesaparecen[] rombosScripts;  // Referencia a los scripts RombosDesaparecen en los rombos
    public Button botonReiniciar;  // Referencia al botón de reinicio

    private Vector3 posicionInicialEstrella;  // Posición inicial de la estrella

    void Start()
    {
        // Guardar la posición inicial de la estrella
        if (estrella != null)
        {
            posicionInicialEstrella = estrella.transform.position;
        }

        // Asignar la función al botón
        if (botonReiniciar != null)
        {
            botonReiniciar.onClick.AddListener(Reiniciar);
        }
    }

    void Reiniciar()
    {
        // Reiniciar la posición de la estrella
        if (estrella != null)
        {
            estrella.transform.position = posicionInicialEstrella;
        }

        // Reiniciar los scripts RombosDesaparecen
        foreach (RombosDesaparecen script in rombosScripts)
        {
            if (script != null)
            {
                script.Reiniciar();
            }
        }
    }
}
