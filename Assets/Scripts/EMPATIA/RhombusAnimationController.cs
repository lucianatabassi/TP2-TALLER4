using UnityEngine;

public class RhombusAnimationController : MonoBehaviour
{
    private Animator animator;
    public string deformarStateName;  // Nombre del estado "Deformar" específico para cada rombo
    public string TransformTrigger; // Nombre del trigger "Transformar" específico para cada rombo
    private bool hasTransformed = false;

    public AudioClip sonidoInteraccion; // AudioClip para el sonido de interacción
    private AudioSource audioSource; // Referencia al AudioSource

    void Start()
    {
        animator = GetComponent<Animator>();

        // Agrega o encuentra el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Inicia la animación Idle si es necesario
        // animator.Play("Idle");
        Invoke("StartDeformAnimation", 2f); // Ajusta el tiempo según sea necesario
    }

    void StartDeformAnimation()
    {
        if (!hasTransformed)
        {
            animator.Play(deformarStateName); // Usa el nombre del estado específico
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collider detected: " + other.gameObject.name);
        if (other.CompareTag("Estrella") && !hasTransformed)
        {
            Debug.Log("Star detected. Activating transform animation.");
            hasTransformed = true;
            ActivateTransformTrigger();

            // Reproducir sonido de interacción
            if (sonidoInteraccion != null)
            {
                audioSource.PlayOneShot(sonidoInteraccion);
            }
        }
    }

    void ActivateTransformTrigger()
    {
        animator.SetTrigger(TransformTrigger); // Usa el nombre del trigger específico
    }

    // Método para reiniciar el estado del script
    public void ResetAnimation()
    {
        // Reiniciar el estado de la animación
        animator.ResetTrigger(TransformTrigger);
        animator.Play("Idle");  // Vuelve a la animación "Idle"
        hasTransformed = false; // Restablecer el estado de transformación
        CancelInvoke();  // Cancelar cualquier invocación pendiente
        Invoke("StartDeformAnimation", 2f); // Reiniciar la animación "Deformar" después de un tiempo
    }
}
