using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReiniciarDiscriminacion : MonoBehaviour
{
    public GameObject estrella; // Asigna la estrella desde el Inspector
    public DiscriminarEstrella[] rombos; // Asigna los rombos con sus scripts ProximityAnimation desde el Inspector

    private Vector3 posicionOriginalEstrella;

    void Start()
    {
        // Guarda la posici�n original de la estrella
        posicionOriginalEstrella = estrella.transform.position;
    }

    public void Reiniciar()
    {
        // Reiniciar la posici�n de la estrella
        estrella.transform.position = posicionOriginalEstrella;

        // Reiniciar la posici�n y animaci�n de cada rombo
        foreach (DiscriminarEstrella romboScript in rombos)
        {
            romboScript.ReiniciarPosicionYAnimacion();
        }
    }
}
