using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIMIDEZROMBO : MonoBehaviour
{
    public GameObject[] rombos;  // Arreglo de rombos en la escena
    public float distanciaCercana = 5f;  // Distancia a la cual la estrella comenzará a achicarse
    public Vector3 escalaAchicada = new Vector3(0.5f, 0.5f, 0.5f);  // Tamaño achicado de la estrella
    public float velocidadAchicamiento = 2f;  // Velocidad de achicamiento
    public AudioClip sonidoInteraccion;  // Clip de audio para la interacción
    public float volumenSonido = 1f;  // Volumen del sonido
    public float tiempoDeEspera = 10f;  // Tiempo en segundos para esperar antes de restaurar la posición

    private Vector3 posicionOriginal;  // Posición original de la estrella
    private Vector3 escalaOriginal;     // Escala original de la estrella
    private bool enProximidad = false;
    private bool sonidoReproducido = false;  // Controla si el sonido ya se ha reproducido
    private AudioSource audioSource;  // Componente AudioSource
    private bool corutinaEnEspera = false;  // Para evitar iniciar la corutina múltiples veces

    void Start()
    {
        // Guardamos la posición y escala original de la estrella
        posicionOriginal = transform.position;
        escalaOriginal = transform.localScale;

        // Obtén el componente AudioSource (asegúrate de que esté agregado al mismo GameObject)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Asignamos el clip de audio al AudioSource
        audioSource.clip = sonidoInteraccion;
        audioSource.volume = volumenSonido;
    }

    void Update()
    {
        enProximidad = false;

        // Verificamos la proximidad con cada rombo
        foreach (GameObject rombo in rombos)
        {
            // Calculamos la distancia entre la estrella y el rombo
            float distancia = Vector3.Distance(transform.position, rombo.transform.position);

            // Si la distancia es menor o igual a la distancia especificada
            if (distancia <= distanciaCercana)
            {
                enProximidad = true;

                // Reproduce el sonido si está en proximidad y no ha sonado antes
                if (!sonidoReproducido)
                {
                    audioSource.Play();
                    sonidoReproducido = true;  // Marca que el sonido ha sido reproducido
                    // Inicia la corutina para reiniciar la posición después de un retraso
                    if (!corutinaEnEspera)
                    {
                        StartCoroutine(ResetearPosicionConRetraso(tiempoDeEspera));
                    }
                }
                break;  // Si un rombo está lo suficientemente cerca, no necesitamos seguir buscando
            }
        }

        // Si está en proximidad, achicamos la estrella
        if (enProximidad)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, escalaAchicada, Time.deltaTime * velocidadAchicamiento);
        }
        else
        {
            // Si no está en proximidad, volvemos a la escala original
            transform.localScale = Vector3.Lerp(transform.localScale, escalaOriginal, Time.deltaTime * velocidadAchicamiento);
            sonidoReproducido = false;  // Reseteamos la variable para que el sonido pueda reproducirse nuevamente cuando se acerque a un rombo
        }
    }

    // Corutina para reiniciar la posición de la estrella después de un retraso
    private IEnumerator ResetearPosicionConRetraso(float delay)
    {
        corutinaEnEspera = true;  // Marcamos que la corutina está en espera
        Vector3 posicionInicial = transform.position;
        float tiempoTranscurrido = 0f;

        // Espera el tiempo especificado antes de empezar a mover
        yield return new WaitForSeconds(delay);

        // Mueve gradualmente la estrella a la posición original
        while (tiempoTranscurrido < 1f)  // Duración del movimiento (1 segundo)
        {
            tiempoTranscurrido += Time.deltaTime;
            float porcentaje = tiempoTranscurrido / 1f;
            transform.position = Vector3.Lerp(posicionInicial, posicionOriginal, porcentaje);
            yield return null;  // Espera hasta el siguiente frame
        }

        // Asegúrate de que la posición final sea la original
        transform.position = posicionOriginal;
        corutinaEnEspera = false;  // Permitimos que la corutina se inicie nuevamente
    }
}
