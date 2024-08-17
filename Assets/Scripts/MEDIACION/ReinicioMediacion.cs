using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class ReinicioMediacion : MonoBehaviour
{
    public GameObject estrella; // Arrastra aqu� el objeto estrella desde el inspector
    public PELEAROMBOS peleaRombos; // Arrastra aqu� el script PELEAROMBOS desde el inspector
    public CambiarAnimacion[] cambiarAnimacion; // Array de scripts CambiarAnimacion desde el inspector

    private Vector3 posicionInicialEstrella; // Define la posici�n inicial deseada para la estrella

    void Start()
    {
        // Al iniciar, guarda la posici�n inicial de la estrella
        if (estrella != null)
        {
            posicionInicialEstrella = estrella.transform.position;
        }
    }

    public void Reiniciar()
    {
        // Reiniciar la posici�n de la estrella
        if (estrella != null)
        {
            estrella.transform.position = posicionInicialEstrella;
        }

        // Reiniciar los estados de los rombos hasta la animaci�n de pelea
        if (peleaRombos != null)
        {
            peleaRombos.ReiniciarHastaPelea(); // Llama al m�todo para reiniciar el estado del script PELEAROMBOS hasta la animaci�n de pelea
        }

        // Reiniciar la secuencia de animaci�n para cada CambiarAnimacion en el array
        if (cambiarAnimacion != null && cambiarAnimacion.Length > 0)
        {
            foreach (CambiarAnimacion animacion in cambiarAnimacion)
            {
                if (animacion != null)
                {
                    animacion.ReiniciarSecuencia(); // Llama al m�todo para reiniciar la secuencia de animaci�n en cada componente CambiarAnimacion
                }
            }
        }
    }
}
