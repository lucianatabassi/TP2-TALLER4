using UnityEngine;

public class RomboAnimacion : MonoBehaviour
{
    public string estrellaTag = "Estrella"; // Etiqueta asignada a la estrella
    public float umbralProximidad = 2.0f; // Distancia para activar la animaci�n "Agrandar"
    public float umbralAlejamiento = 2.2f; // Distancia para activar la animaci�n "Idle"
    private Transform estrella;
    private Animator romboAnimator;
    private bool estaAgrandando = false;

    void Start()
    {
        // Buscar la estrella en la escena por su etiqueta
        GameObject estrellaObjeto = GameObject.FindWithTag(estrellaTag);
        if (estrellaObjeto != null)
        {
            estrella = estrellaObjeto.transform;
        }
        else
        {
            Debug.LogError("No se encontr� ning�n objeto con la etiqueta " + estrellaTag);
        }

        // Obtener el Animator del rombo
        romboAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Calcular la distancia entre la estrella y el rombo
        if (estrella != null && romboAnimator != null)
        {
            float distancia = Vector3.Distance(transform.position, estrella.position);

            // Cambiar a la animaci�n "Agrandar" si la estrella est� cerca
            if (distancia < umbralProximidad && !estaAgrandando)
            {
                romboAnimator.SetTrigger("Agrandar");
                estaAgrandando = true;
            }
            // Cambiar a la animaci�n "Idle" si la estrella est� lo suficientemente lejos
            else if (distancia > umbralAlejamiento && estaAgrandando)
            {
                romboAnimator.SetTrigger("Idle");
                estaAgrandando = false;
            }
        }
    }
}
