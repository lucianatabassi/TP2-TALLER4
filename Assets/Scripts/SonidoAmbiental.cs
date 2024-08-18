using UnityEngine;

public class SonidoAmbiental : MonoBehaviour
{
    public AudioClip sonidoAmbiental;  // Clip de audio para el sonido ambiental
    public float volumen = 1f;  // Volumen del sonido ambiental

    private AudioSource audioSource;

    void Awake()
    {
        // Verifica si ya existe un GameObject de Sonido Ambiental
        if (FindObjectsOfType<SonidoAmbiental>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        // Configura el componente AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sonidoAmbiental;
        audioSource.volume = volumen;
        audioSource.loop = true;

        // Reproduce el sonido ambiental
        audioSource.Play();

        // Asegúrate de que el GameObject persista en todas las escenas
        DontDestroyOnLoad(gameObject);
    }
}
