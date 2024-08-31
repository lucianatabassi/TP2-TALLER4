using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROTACION : MonoBehaviour
{
    public GameObject estrella;  // Referencia a la estrella
    public float velocidadRotacion = 5f;  // Velocidad de rotación de los rombos
    public float ajusteRotacionInicial = 45f;  // Ajuste de rotación para corregir el desajuste
    public float tiempoEsperaReinicio = 3f; // Tiempo de espera después de la interacción antes de reiniciar
    public float duracionTransicion = 2f; // Duración en segundos para la transición gradual

    private Quaternion rotacionOriginal;  // Para guardar la rotación original
    private Vector3 posicionInicialEstrella; // Para guardar la posición inicial de la estrella
    private bool enTransicion = false;  // Indica si la estrella está en transición
    private bool haInteractuado = false; // Indica si ha habido interacción

    void Start()
    {
        // Guarda la rotación original del rombo y la posición inicial de la estrella
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
            // Dirección hacia la cual debe rotar el rombo (en el plano 2D)
            Vector3 direccionHaciaEstrella = estrella.transform.position - transform.position;
            direccionHaciaEstrella.Normalize();  // Normalizamos la dirección

            // Convertimos la dirección en un ángulo (en grados) en el plano 2D
            float angulo = Mathf.Atan2(direccionHaciaEstrella.y, direccionHaciaEstrella.x) * Mathf.Rad2Deg;

            // Ajustamos el ángulo con la corrección de 45 grados
            angulo -= ajusteRotacionInicial;

            // Aplicamos la rotación al objeto
            Quaternion rotacionDeseada = Quaternion.Euler(0, 0, angulo);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * velocidadRotacion);

            // Detecta la interacción (puedes cambiar esta condición para definir qué es "interacción")
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

        // Inicia la transición de retorno gradual
        StartCoroutine(RetornarGradualmente());
    }

    IEnumerator RetornarGradualmente()
    {
        enTransicion = true;
        Vector3 posicionActual = estrella.transform.position;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionTransicion)
        {
            // Interpolación lineal para mover la estrella gradualmente a su posición inicial
            estrella.transform.position = Vector3.Lerp(posicionActual, posicionInicialEstrella, tiempoTranscurrido / duracionTransicion);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la estrella llegue exactamente a la posición inicial
        estrella.transform.position = posicionInicialEstrella;
        enTransicion = false;
        haInteractuado = false; // Reinicia la variable para futuras interacciones

        // Reinicia la rotación a la original
      //  Resetear();
    }

    // Método para reiniciar la rotación al estado original
   // public void Resetear()
  //  {
   //     transform.rotation = rotacionOriginal;
   // }
}
