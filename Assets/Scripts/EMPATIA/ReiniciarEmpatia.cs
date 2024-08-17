using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReiniciarEmpatia : MonoBehaviour
{
    public GameObject estrella;  // Asigna la estrella desde el Inspector
    public Transform centro;     // Asigna el objeto al centro de la escena desde el Inspector
    public CambiarAnimacion[] rombosCambiarAnimacion;  // Asigna los scripts de animación de los rombos
    public RhombusAnimationController[] rombosAnimationController;  // Asigna los controladores de animación de los rombos

    // Método que se llama cuando se presiona el botón
    public void OnReiniciarButtonPressed()
    {
        if (estrella != null && centro != null)
        {
            // Reiniciar la posición de la estrella al centro
            estrella.transform.position = centro.position;
        }

        // Reiniciar las animaciones de los rombos a "Idle" y reiniciar su secuencia de animaciones
        foreach (CambiarAnimacion rombo in rombosCambiarAnimacion)
        {
            if (rombo != null && rombo.animator != null)
            {
                // Reiniciar a la animación "Idle"
                rombo.animator.Play(rombo.nombreAnimacionInicial);

                // Reiniciar la secuencia de animaciones en el script `CambiarAnimacion`
                rombo.ReiniciarSecuencia();
            }
        }

        // Reiniciar los scripts `RhombusAnimationController` para cada rombo
        foreach (RhombusAnimationController romboController in rombosAnimationController)
        {
            if (romboController != null)
            {
                romboController.ResetAnimation();
            }
        }
    }
}
