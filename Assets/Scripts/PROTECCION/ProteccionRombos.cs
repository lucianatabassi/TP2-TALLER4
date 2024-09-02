using UnityEngine;
using System.Collections;

public class ProteccionRombos : MonoBehaviour
{
    [System.Serializable]
    public class RomboConfig
    {
        public Animator animator;
        public string animacionIdle;
        public string animacionProteccion;
    }

    public GameObject estrella;
    public RomboConfig[] rombos;
    public float distanciaActivacion = 2.0f; // Ajusta esta distancia seg�n sea necesario
    public AudioSource audioSource; // Referencia al componente AudioSource
    public AudioClip sonidoInteraccion; // Sonido cuando la estrella est� cerca
    public AudioClip sonidoReinicio; // Sonido de reinicio

    private Vector3 posicionInicialEstrella;
    private bool sonidoReproducido = false; // Variable para controlar la reproducci�n del sonido
    private bool sonidoReinicioReproducido = false; // Variable para controlar la reproducci�n del sonido de reinicio

    void Start()
    {
        if (estrella != null)
        {
            posicionInicialEstrella = estrella.transform.position;
        }
    }

    void Update()
    {
        bool estrellaCerca = false;
        foreach (RomboConfig rombo in rombos)
        {
            float distancia = Vector3.Distance(estrella.transform.position, rombo.animator.gameObject.transform.position);
            if (distancia <= distanciaActivacion)
            {
                estrellaCerca = true;
                break;
            }
        }

        // Cambia la animaci�n para todos los rombos y maneja el sonido
        if (estrellaCerca)
        {
            foreach (RomboConfig rombo in rombos)
            {
                rombo.animator.Play(rombo.animacionProteccion);
            }

            if (!sonidoReproducido)
            {
                ReproducirSonido(sonidoInteraccion); // Reproduce el sonido cuando la estrella est� cerca
                sonidoReproducido = true; // Marca que el sonido ha sido reproducido
            }

            if (!sonidoReinicioReproducido)
            {
                StartCoroutine(MoverEstrellaAtras()); // Inicia la coroutine para mover la estrella
                sonidoReinicioReproducido = true; // Marca que el sonido de reinicio ha sido reproducido
            }
        }
        else
        {
            foreach (RomboConfig rombo in rombos)
            {
                rombo.animator.Play(rombo.animacionIdle);
            }

            sonidoReproducido = false; // Resetea la variable si la estrella se aleja
        }
    }

    // Coroutine para mover la estrella de vuelta a su posici�n inicial gradualmente
    IEnumerator MoverEstrellaAtras()
    {
        yield return new WaitForSeconds(9f); // Espera 10 segundos

        if (estrella == null) yield break;

        Vector3 posicionActual = estrella.transform.position;
        float tiempoTranscurrido = 0f;
        float duracionMovimiento = 2f; // Duraci�n del movimiento gradual

        // Reproduce el sonido de reinicio cuando la estrella comienza a moverse
        ReproducirSonido(sonidoReinicio);

        while (tiempoTranscurrido < duracionMovimiento)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / duracionMovimiento;
            estrella.transform.position = Vector3.Lerp(posicionActual, posicionInicialEstrella, t);
            yield return null;
        }

        // Aseg�rate de que la estrella est� exactamente en la posici�n inicial
        estrella.transform.position = posicionInicialEstrella;

        // Reinicia la animaci�n de los rombos al estado idle despu�s de mover la estrella
        foreach (RomboConfig rombo in rombos)
        {
            rombo.animator.Play(rombo.animacionIdle);
        }

        sonidoReproducido = false; // Resetea la variable de sonido al reiniciar
        sonidoReinicioReproducido = false; // Resetea la variable del sonido de reinicio
    }

    // M�todo para reproducir un sonido dado
    void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
