using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReiniciarAcoso : MonoBehaviour
{
    public GameObject estrella;  // Asigna la estrella desde el Inspector
    public Transform centro;     // Asigna el objeto al centro de la escena desde el Inspector

    // M�todo que se llama cuando se presiona el bot�n
    public void OnReiniciarButtonPressed()
    {
        if (estrella != null && centro != null)
        {
            estrella.transform.position = centro.position;
        }
    }
}
