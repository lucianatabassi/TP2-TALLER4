using UnityEngine;

public class EstrellaProximidad : MonoBehaviour
{
    public string romboTag = "Rombo"; // Etiqueta asignada a los rombos
    public float umbralProximidad = 2.0f; // Distancia para activar la animaci�n

    private GameObject[] rombos; // Array para almacenar todos los rombos
    private bool[] animacionActiva; // Estado de la animaci�n para cada rombo

    void Start()
    {
        // Encontrar todos los rombos con la etiqueta especificada
        rombos = GameObject.FindGameObjectsWithTag(romboTag);

        // Inicializar el array de estado de animaci�n
        animacionActiva = new bool[rombos.Length];
    }

    void Update()
    {
        bool estrellaCerca = false;

        // Revisar la distancia a cada rombo
        for (int i = 0; i < rombos.Length; i++)
        {
            float distancia = Vector3.Distance(transform.position, rombos[i].transform.position);
            if (distancia < umbralProximidad)
            {
                estrellaCerca = true;
                break;
            }
        }

        // Si el estado de proximidad ha cambiado, actualiza las animaciones
        if (estrellaCerca)
        {
            ActivarAnimaciones(true);
        }
        else
        {
            ActivarAnimaciones(false);
        }
    }

    // M�todo para activar o desactivar las animaciones "Agrandar"
    void ActivarAnimaciones(bool activar)
    {
        for (int i = 0; i < rombos.Length; i++)
        {
            ControladorRombo controlador = rombos[i].GetComponent<ControladorRombo>();
            if (controlador != null)
            {
                controlador.ActivarAnimacionAgrandar(activar);
            }
        }
    }
}
