using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class ReinicioMediacion : MonoBehaviour
{
    public GameObject estrella; // Arrastra aquí el objeto estrella desde el inspector
    public PELEAROMBOS peleaRombos; // Arrastra aquí el script PELEAROMBOS desde el inspector
    public CambiarAnimacion[] cambiarAnimacion; // Array de scripts CambiarAnimacion desde el inspector

    private Vector3 posicionInicialEstrella; // Define la posición inicial deseada para la estrella

    void Start()
    {
        // Al iniciar, guarda la posición inicial de la estrella
        if (estrella != null)
        {
            posicionInicialEstrella = estrella.transform.position;
        }
    }

    public void Reiniciar()
    {
        // Reiniciar la posición de la estrella
        if (estrella != null)
        {
            estrella.transform.position = posicionInicialEstrella;
        }

        // Reiniciar los estados de los rombos hasta la animación de pelea
        if (peleaRombos != null)
        {
            peleaRombos.ReiniciarHastaPelea(); // Llama al método para reiniciar el estado del script PELEAROMBOS hasta la animación de pelea
        }

        // Reiniciar la secuencia de animación para cada CambiarAnimacion en el array
        if (cambiarAnimacion != null && cambiarAnimacion.Length > 0)
        {
            foreach (CambiarAnimacion animacion in cambiarAnimacion)
            {
                if (animacion != null)
                {
                    animacion.ReiniciarSecuencia(); // Llama al método para reiniciar la secuencia de animación en cada componente CambiarAnimacion
                }
            }
        }
    }
}
