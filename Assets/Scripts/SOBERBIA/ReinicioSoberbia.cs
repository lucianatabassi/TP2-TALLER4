using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinicioSoberbia : MonoBehaviour
{
    public Transform estrella; // Asigna el transform de la estrella en el inspector
    public Transform centro; // Asigna el transform del centro de la escena en el inspector
    public Vector3 tamanoOriginalEstrella = new Vector3(1f, 1f, 1f); // Tama�o original de la estrella
    public Animator[] romboAnimators; // Asigna los animadores de los rombos en el inspector
    public string[] idleAnimationNames; // Asigna los nombres de las animaciones Idle en el inspector

    public StarProximityScaling proximityScalingScript; // Asigna el script que maneja la escala y la interacci�n con los rombos

    // M�todo que se llama cuando se presiona el bot�n
    public void OnResetButtonPressed()
    {
        // Reiniciar la posici�n y tama�o de la estrella
        if (estrella != null && centro != null)
        {
            estrella.position = centro.position;
            estrella.localScale = tamanoOriginalEstrella;
        }

        // Aseg�rate de que el n�mero de animadores y nombres de animaci�n coincidan
        if (romboAnimators.Length != idleAnimationNames.Length)
        {
            Debug.LogError("El n�mero de animadores y nombres de animaci�n no coincide.");
            return;
        }

        // Reiniciar las animaciones de los rombos a su animaci�n Idle
        for (int i = 0; i < romboAnimators.Length; i++)
        {
            Animator romboAnimator = romboAnimators[i];
            string idleAnimationName = idleAnimationNames[i];

            if (romboAnimator != null)
            {
                // Cambiar manualmente al estado "Idle"
                romboAnimator.Play(idleAnimationName, 0, 0f); // Aseg�rate de que el nombre de la animaci�n coincida
            }
        }

        // Reiniciar la interacci�n de la estrella con los rombos
        if (proximityScalingScript != null)
        {
            proximityScalingScript.ResetInteractions();
        }
    }
}


