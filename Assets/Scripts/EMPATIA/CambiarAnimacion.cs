using UnityEngine;
using System.Collections;

public class CambiarAnimacion : MonoBehaviour
{
    public Animator animator;
    public string nombreAnimacionInicial = "Idle";
    public string nombreAnimacionSiguiente = "Deformacion";
    public float tiempoEspera = 3f;
    public float duracionBucle = 1f;
    public float tiempoDeEspera = 10f;

    public Transform estrella; // Campo p�blico para asignar la estrella en el inspector
    private Vector3 posicionInicial;

    public GameObject rombo1;
    public GameObject rombo2;
    public GameObject rombo3;
    public GameObject rombo4;

    public AudioSource audioSource; // Referencia al componente AudioSource
    public AudioClip sonidoReinicio; // Referencia al AudioClip para el sonido de reinicio
    public float volumenSonidoReinicio = 1.0f; // Volumen del sonido de reinicio

    void Start()
    {
        if (estrella == null)
        {
            Debug.LogError("La estrella no est� asignada en el inspector.");
            return;
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("El Animator no est� asignado en el GameObject.");
            return;
        }

        posicionInicial = estrella.position; // Captura la posici�n inicial de la estrella

        IniciarSecuencia();
    }

    public void IniciarSecuencia()
    {
        animator.Play(nombreAnimacionInicial);
        StartCoroutine(CambiarAnimacionDespuesDeTiempo());
    }

    public void ReiniciarSecuencia()
    {
        StopAllCoroutines();
        IniciarSecuencia();
    }

    IEnumerator CambiarAnimacionDespuesDeTiempo()
    {
        yield return new WaitForSeconds(tiempoEspera);
        animator.Play(nombreAnimacionSiguiente, -1, 0);
        yield return new WaitForSeconds(duracionBucle);

        yield return new WaitForSeconds(tiempoDeEspera);
        StartCoroutine(VolverAlaPosicionInicial());
    }

    IEnumerator VolverAlaPosicionInicial()
    {
        if (estrella == null)
        {
            Debug.LogError("La estrella no est� asignada.");
            yield break;
        }

        Debug.Log("Iniciando retorno a la posici�n inicial...");
        float tiempo = 0f;
        Vector3 posicionActual = estrella.position;

        // Cambia el valor de tiempoDeEspera para ajustar la velocidad del retorno
        float velocidadDeRetorno = 1f; // Ajusta este valor para hacer el retorno m�s r�pido

        while (tiempo < 1f)
        {
            tiempo += Time.deltaTime * velocidadDeRetorno; // Multiplicador para acelerar el retorno
            estrella.position = Vector3.Lerp(posicionActual, posicionInicial, tiempo);
            Debug.Log($"Posici�n actual: {estrella.position}");
            yield return null;
        }

        estrella.position = posicionInicial;
        Debug.Log("Posici�n inicial alcanzada.");

        // Reproduce el sonido de reinicio cuando la estrella regresa a la posici�n inicial
        if (audioSource != null && sonidoReinicio != null)
        {
            audioSource.PlayOneShot(sonidoReinicio, volumenSonidoReinicio);
        }

        ReiniciarSecuencia();

        // Reiniciar los rombos al estado de pelea
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

        Animator romboAnimator = rombo.GetComponent<Animator>();
        if (romboAnimator != null)
        {
            // Reiniciar al estado de animaci�n "pelea"
            romboAnimator.Play("Pelea"); // Aseg�rate de que "Pelea" sea el nombre exacto de la animaci�n de pelea
            romboAnimator.SetBool("IsFighting", true); // Activa el estado de pelea
            romboAnimator.SetBool("IsCalm", false); // Desactiva el estado tranquilo
        }
        else
        {
            Debug.LogWarning("Animator no encontrado en el rombo.");
        }
    }
}
