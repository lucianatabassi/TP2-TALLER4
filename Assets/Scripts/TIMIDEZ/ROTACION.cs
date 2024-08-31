using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROTACION : MonoBehaviour
{
    public GameObject estrella;  // Referencia a la estrella
    public float velocidadRotacion = 5f;  // Velocidad de rotaci�n de los rombos
    public float ajusteRotacionInicial = 45f;  // Ajuste de rotaci�n para corregir el desajuste
    public float tiempoEsperaReinicio = 3f; // Tiempo de espera despu�s de la interacci�n antes de reiniciar
    public float duracionTransicion = 2f; // Duraci�n en segundos para la transici�n gradual

    private Quaternion rotacionOriginal;  // Para guardar la rotaci�n original
    private Vector3 posicionInicialEstrella; // Para guardar la posici�n inicial de la estrella
    private bool enTransicion = false;  // Indica si la estrella est� en transici�n
    private bool haInteractuado = false; // Indica si ha habido interacci�n

    void Start()
    {
        // Guarda la rotaci�n original del rombo y la posici�n inicial de la estrella
        rotacionOriginal = transform.rotation;
        if (estrella != null)
        {
            posicionInicialEstrella = estrella.transform.position;
        }
    }

    void Update()
    {
        if (estrella == null) return;

        if (!enTransicion)
        {
            // Direcci�n hacia la cual debe rotar el rombo (en el plano 2D)
            Vector3 direccionHaciaEstrella = estrella.transform.position - transform.position;
            direccionHaciaEstrella.Normalize();  // Normalizamos la direcci�n

            // Convertimos la direcci�n en un �ngulo (en grados) en el plano 2D
            float angulo = Mathf.Atan2(direccionHaciaEstrella.y, direccionHaciaEstrella.x) * Mathf.Rad2Deg;

            // Ajustamos el �ngulo con la correcci�n de 45 grados
            angulo -= ajusteRotacionInicial;

            // Aplicamos la rotaci�n al objeto
            Quaternion rotacionDeseada = Quaternion.Euler(0, 0, angulo);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * velocidadRotacion);

            // Detecta la interacci�n (puedes cambiar esta condici�n para definir qu� es "interacci�n")
            if (Vector3.Distance(transform.position, estrella.transform.position) < 1f && !haInteractuado)
            {
                haInteractuado = true;
                StartCoroutine(ReiniciarDespuesDeInteraccion());
            }
        }
    }

    IEnumerator ReiniciarDespuesDeInteraccion()
    {
        // Espera el tiempo especificado antes de iniciar el retorno gradual
        yield return new WaitForSeconds(tiempoEsperaReinicio);

        // Inicia la transici�n de retorno gradual
        StartCoroutine(RetornarGradualmente());
    }

    IEnumerator RetornarGradualmente()
    {
        enTransicion = true;
        Vector3 posicionActual = estrella.transform.position;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionTransicion)
        {
            // Interpolaci�n lineal para mover la estrella gradualmente a su posici�n inicial
            estrella.transform.position = Vector3.Lerp(posicionActual, posicionInicialEstrella, tiempoTranscurrido / duracionTransicion);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la estrella llegue exactamente a la posici�n inicial
        estrella.transform.position = posicionInicialEstrella;
        enTransicion = false;
        haInteractuado = false; // Reinicia la variable para futuras interacciones

        // Reinicia la rotaci�n a la original
      //  Resetear();
    }

    // M�todo para reiniciar la rotaci�n al estado original
   // public void Resetear()
  //  {
   //     transform.rotation = rotacionOriginal;
   // }
}
