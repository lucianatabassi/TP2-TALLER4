using UnityEngine;

public class StarController : MonoBehaviour
{
    public Transform[] containers; // Asigna los contenedores en el inspector
    public float detectionRadius = 1.0f; // Radio de detección para cambiar tamaño
    public Vector3 initialScale = new Vector3(1f, 1f, 1f); // Escala inicial de los contenedores
    public Vector3 smallScale = new Vector3(0.5f, 0.5f, 0.5f); // Escala pequeña para el rombo cercano
    public Vector3 largeScale = new Vector3(2f, 2f, 2f); // Escala grande para los rombos lejanos

    void Start()
    {
        // Asegúrate de que todos los contenedores comiencen con la escala inicial
        foreach (Transform container in containers)
        {
            container.localScale = initialScale; // Debe estar en (1,1,1)
        }
    }

    void Update()
    {
        bool anyClose = false;
        Transform closestContainer = null;
        float closestDistance = detectionRadius;

        // Encuentra el contenedor más cercano a la estrella
        foreach (Transform container in containers)
        {
            float distance = Vector3.Distance(transform.position, container.position);

            if (distance < detectionRadius)
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestContainer = container;
                }
                anyClose = true;
            }
        }

        // Actualiza la escala de todos los contenedores
        foreach (Transform container in containers)
        {
            if (container == closestContainer && anyClose)
            {
                // Achicar el contenedor más cercano
                container.localScale = smallScale;
            }
            else
            {
                // Agrandar los demás contenedores solo si hay al menos un contenedor cerca
                if (anyClose)
                {
                    container.localScale = largeScale;
                }
                else
                {
                    // Si no hay contenedores cerca, mantener la escala inicial
                    container.localScale = initialScale;
                }
            }
        }
    }
}