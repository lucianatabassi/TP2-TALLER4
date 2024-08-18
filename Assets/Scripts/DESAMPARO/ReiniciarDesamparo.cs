using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ReiniciarDesamparo : MonoBehaviour
{
    public GameObject estrella;  // Referencia a la estrella
    public RombosDesaparecen[] rombosScripts;  // Referencia a los scripts RombosDesaparecen en los rombos
    public Button botonReiniciar;  // Referencia al bot�n de reinicio

    private Vector3 posicionInicialEstrella;  // Posici�n inicial de la estrella

    void Start()
    {
        // Guardar la posici�n inicial de la estrella
        if (estrella != null)
        {
            posicionInicialEstrella = estrella.transform.position;
        }

        // Asignar la funci�n al bot�n
        if (botonReiniciar != null)
        {
            botonReiniciar.onClick.AddListener(Reiniciar);
        }
    }

    void Reiniciar()
    {
        // Reiniciar la posici�n de la estrella
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
