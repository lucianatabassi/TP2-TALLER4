using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReiniciarTimidez : MonoBehaviour
{
    public GameObject estrella;  // Referencia a la estrella
    public TIMIDEZROMBO timidezRomboScript;  // Referencia al script TIMIDEZROMBO
    public ROTACION[] rotacionScripts;  // Array de scripts ROTACION
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

    public void Reiniciar()
    {
        // Reiniciar la posici�n de la estrella
        if (estrella != null)
        {
            estrella.transform.position = posicionInicialEstrella;
        }

        // Reiniciar el script TIMIDEZROMBO
        if (timidezRomboScript != null)
        {
            timidezRomboScript.Resetear();
        }

        // Reiniciar todos los scripts ROTACION
        foreach (ROTACION rotacionScript in rotacionScripts)
        {
            if (rotacionScript != null)
            {
                rotacionScript.Resetear();
            }
        }
    }
}
