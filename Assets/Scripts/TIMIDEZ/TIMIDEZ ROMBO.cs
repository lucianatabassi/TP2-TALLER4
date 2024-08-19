using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIMIDEZROMBO : MonoBehaviour
{
    public GameObject[] rombos;  // Arreglo de rombos en la escena
    public float distanciaCercana = 5f;  // Distancia a la cual la estrella comenzar� a achicarse
    public Vector3 escalaAchicada = new Vector3(0.5f, 0.5f, 0.5f);  // Tama�o achicado de la estrella
    public float velocidadAchicamiento = 2f;  // Velocidad de achicamiento
    public AudioClip sonidoInteraccion;  // Clip de audio para la interacci�n
    public float volumenSonido = 1f;  // Volumen del sonido

    private Vector3 escalaOriginal;
    private bool enProximidad = false;
    private bool sonidoReproducido = false;  // Controla si el sonido ya se ha reproducido
    private AudioSource audioSource;  // Componente AudioSource

    void Start()
    {
        // Guardamos la escala original de la estrella
        escalaOriginal = transform.localScale;

        // Obt�n el componente AudioSource (aseg�rate de que est� agregado al mismo GameObject)
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

                // Reproduce el sonido si est� en proximidad y no ha sonado antes
                if (!sonidoReproducido)
                {
                    audioSource.Play();
                    sonidoReproducido = true;  // Marca que el sonido ha sido reproducido
                }
                break;  // Si un rombo est� lo suficientemente cerca, no necesitamos seguir buscando
            }
        }

        // Si est� en proximidad, achicamos la estrella
        if (enProximidad)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, escalaAchicada, Time.deltaTime * velocidadAchicamiento);
        }
        else
        {
            // Si no est� en proximidad, volvemos al tama�o original
            transform.localScale = Vector3.Lerp(transform.localScale, escalaOriginal, Time.deltaTime * velocidadAchicamiento);
            sonidoReproducido = false;  // Reseteamos la variable para que el sonido pueda reproducirse nuevamente cuando se acerque a un rombo
        }
    }

    // M�todo para reiniciar la escala de la estrella
    public void Resetear()
    {
        transform.localScale = escalaOriginal;
    }
}
