using UnityEngine;

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
    public float distanciaActivacion = 2.0f; // Ajusta esta distancia según sea necesario
    public AudioSource audioSource; // Referencia al componente AudioSource
    public AudioClip sonidoInteraccion; // Referencia al AudioClip para reproducir

    private Vector3 posicionInicialEstrella;
    private bool sonidoReproducido = false; // Variable para controlar la reproducción del sonido

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

        // Cambia la animación para todos los rombos y maneja el sonido
        if (estrellaCerca)
        {
            foreach (RomboConfig rombo in rombos)
            {
                rombo.animator.Play(rombo.animacionProteccion);
            }

            if (!sonidoReproducido)
            {
                ReproducirSonido(); // Reproduce el sonido si no se ha reproducido aún
                sonidoReproducido = true; // Marca que el sonido ha sido reproducido
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

    // Método para reiniciar el estado del script y la posición de la estrella
    public void Reiniciar()
    {
        if (estrella != null)
        {
            estrella.transform.position = posicionInicialEstrella;
        }

        foreach (RomboConfig rombo in rombos)
        {
            rombo.animator.Play(rombo.animacionIdle);
        }

        sonidoReproducido = false; // Resetea la variable de sonido al reiniciar
    }

    // Método para reproducir el sonido
    void ReproducirSonido()
    {
        if (audioSource != null && sonidoInteraccion != null)
        {
            audioSource.PlayOneShot(sonidoInteraccion);
        }
    }
}
