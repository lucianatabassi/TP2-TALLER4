using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinicioSoberbia : MonoBehaviour
{
    public Transform estrella; // Asigna el transform de la estrella en el inspector
    public Transform centro; // Asigna el transform del centro de la escena en el inspector
    public Vector3 tamanoOriginalEstrella = new Vector3(1f, 1f, 1f); // Tamaño original de la estrella
    public Animator[] romboAnimators; // Asigna los animadores de los rombos en el inspector
    public string[] idleAnimationNames; // Asigna los nombres de las animaciones Idle en el inspector

    public StarProximityScaling proximityScalingScript; // Asigna el script que maneja la escala y la interacción con los rombos

    // Método que se llama cuando se presiona el botón
    public void OnResetButtonPressed()
    {
        // Reiniciar la posición y tamaño de la estrella
        if (estrella != null && centro != null)
        {
            estrella.position = centro.position;
            estrella.localScale = tamanoOriginalEstrella;
        }

        // Asegúrate de que el número de animadores y nombres de animación coincidan
        if (romboAnimators.Length != idleAnimationNames.Length)
        {
            Debug.LogError("El número de animadores y nombres de animación no coincide.");
            return;
        }

        // Reiniciar las animaciones de los rombos a su animación Idle
        for (int i = 0; i < romboAnimators.Length; i++)
        {
            Animator romboAnimator = romboAnimators[i];
            string idleAnimationName = idleAnimationNames[i];

            if (romboAnimator != null)
            {
                // Cambiar manualmente al estado "Idle"
                romboAnimator.Play(idleAnimationName, 0, 0f); // Asegúrate de que el nombre de la animación coincida
            }
        }

        // Reiniciar la interacción de la estrella con los rombos
        if (proximityScalingScript != null)
        {
            proximityScalingScript.ResetInteractions();
        }
    }
}


