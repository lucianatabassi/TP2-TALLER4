using UnityEngine;

public class RhombusAnimationController : MonoBehaviour
{
    private Animator animator;
    public string deformarStateName;  // Nombre del estado "Deformar" espec�fico para cada rombo
    public string TransformTrigger; // Nombre del trigger "Transformar" espec�fico para cada rombo
    private bool hasTransformed = false;

    public AudioClip sonidoInteraccion; // AudioClip para el sonido de interacci�n
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

        // Inicia la animaci�n Idle si es necesario
        // animator.Play("Idle");
        Invoke("StartDeformAnimation", 2f); // Ajusta el tiempo seg�n sea necesario
    }

    void StartDeformAnimation()
    {
        if (!hasTransformed)
        {
            animator.Play(deformarStateName); // Usa el nombre del estado espec�fico
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

            // Reproducir sonido de interacci�n
            if (sonidoInteraccion != null)
            {
                audioSource.PlayOneShot(sonidoInteraccion);
            }
        }
    }

    void ActivateTransformTrigger()
    {
        animator.SetTrigger(TransformTrigger); // Usa el nombre del trigger espec�fico
    }

    // M�todo para reiniciar el estado del script
    public void ResetAnimation()
    {
        // Reiniciar el estado de la animaci�n
        animator.ResetTrigger(TransformTrigger);
        animator.Play("Idle");  // Vuelve a la animaci�n "Idle"
        hasTransformed = false; // Restablecer el estado de transformaci�n
        CancelInvoke();  // Cancelar cualquier invocaci�n pendiente
        Invoke("StartDeformAnimation", 2f); // Reiniciar la animaci�n "Deformar" despu�s de un tiempo
    }
}
