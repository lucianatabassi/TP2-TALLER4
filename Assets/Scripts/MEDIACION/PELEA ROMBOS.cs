using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PELEAROMBOS : MonoBehaviour
{
    public GameObject rombo1;
    public GameObject rombo2;
    public GameObject rombo3;
    public GameObject rombo4;

    public float distanciaActivacion = 2.0f; // Distancia a partir de la cual cambia la animación

    public AudioClip sonidoInteraccion; // AudioClip para el sonido de interacción
    private AudioSource audioSource; // Referencia al AudioSource

    void Start()
    {
        // Agrega o encuentra el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Ejemplo básico de detección de proximidad con distancia cuadrada
        float distancia1 = (rombo1.transform.position - transform.position).sqrMagnitude;
        float distancia2 = (rombo2.transform.position - transform.position).sqrMagnitude;
        float distancia3 = (rombo3.transform.position - transform.position).sqrMagnitude;
        float distancia4 = (rombo4.transform.position - transform.position).sqrMagnitude;

        if (distancia1 < distanciaActivacion * distanciaActivacion)
        {
            CambiarAnimacion(rombo1);
        }
        if (distancia2 < distanciaActivacion * distanciaActivacion)
        {
            CambiarAnimacion(rombo2);
        }
        if (distancia3 < distanciaActivacion * distanciaActivacion)
        {
            CambiarAnimacion(rombo3);
        }
        if (distancia4 < distanciaActivacion * distanciaActivacion)
        {
            CambiarAnimacion(rombo4);
        }
    }

    void CambiarAnimacion(GameObject rombo)
    {
        Animator animator = rombo.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsFighting", false); // Cambia "IsFighting" según sea necesario
            animator.SetBool("IsCalm", true); // Activa el estado tranquilo
        }

        // Reproducir sonido de interacción si no se está reproduciendo ya
        if (!audioSource.isPlaying && sonidoInteraccion != null)
        {
            audioSource.PlayOneShot(sonidoInteraccion);
        }
    }

    // Método para reiniciar el estado del script hasta la animación de pelea
    public void ReiniciarHastaPelea()
    {
        // Reiniciar las animaciones de los rombos al estado de pelea
        ReiniciarRombo(rombo1);
        ReiniciarRombo(rombo2);
        ReiniciarRombo(rombo3);
        ReiniciarRombo(rombo4);
    }

    private void ReiniciarRombo(GameObject rombo)
    {
        if (rombo == null)
        {
            Debug.LogWarning("Rombo no asignado.");
            return;
        }

        Animator animator = rombo.GetComponent<Animator>();
        if (animator != null)
        {
            // Reiniciar al estado de animación "pelea"
            animator.Play("Pelea"); // Asegúrate de que "Pelea" sea el nombre exacto de la animación de pelea
            animator.SetBool("IsFighting", true); // Activa el estado de pelea
            animator.SetBool("IsCalm", false); // Desactiva el estado tranquilo
        }
        else
        {
            Debug.LogWarning("Animator no encontrado en el rombo.");
        }
    }
}
