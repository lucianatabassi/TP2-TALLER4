using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReiniciarAcoso : MonoBehaviour
{
    public GameObject estrella;  // Asigna la estrella desde el Inspector
    public Transform centro;     // Asigna el objeto al centro de la escena desde el Inspector

    // Método que se llama cuando se presiona el botón
    public void OnReiniciarButtonPressed()
    {
        if (estrella != null && centro != null)
        {
            estrella.transform.position = centro.position;
        }
    }
}
