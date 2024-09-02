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

    public Transform estrella; // Campo público para asignar la estrella en el inspector
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
            Debug.LogError("La estrella no está asignada en el inspector.");
            return;
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("El Animator no está asignado en el GameObject.");
            return;
        }

        posicionInicial = estrella.position; // Captura la posición inicial de la estrella

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
            Debug.LogError("La estrella no está asignada.");
            yield break;
        }

        Debug.Log("Iniciando retorno a la posición inicial...");
        float tiempo = 0f;
        Vector3 posicionActual = estrella.position;

        // Cambia el valor de tiempoDeEspera para ajustar la velocidad del retorno
        float velocidadDeRetorno = 1f; // Ajusta este valor para hacer el retorno más rápido

        while (tiempo < 1f)
        {
            tiempo += Time.deltaTime * velocidadDeRetorno; // Multiplicador para acelerar el retorno
            estrella.position = Vector3.Lerp(posicionActual, posicionInicial, tiempo);
            Debug.Log($"Posición actual: {estrella.position}");
            yield return null;
        }

        estrella.position = posicionInicial;
        Debug.Log("Posición inicial alcanzada.");

        // Reproduce el sonido de reinicio cuando la estrella regresa a la posición inicial
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
            // Reiniciar al estado de animación "pelea"
            romboAnimator.Play("Pelea"); // Asegúrate de que "Pelea" sea el nombre exacto de la animación de pelea
            romboAnimator.SetBool("IsFighting", true); // Activa el estado de pelea
            romboAnimator.SetBool("IsCalm", false); // Desactiva el estado tranquilo
        }
        else
        {
            Debug.LogWarning("Animator no encontrado en el rombo.");
        }
    }
}
