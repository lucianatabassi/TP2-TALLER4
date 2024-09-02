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
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Inicia la animaci�n Idle si es necesario
        Invoke("StartDeformAnimation", 2f);
    }

    void StartDeformAnimation()
    {
        if (!hasTransformed)
        {
            animator.Play(deformarStateName);
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

            if (sonidoInteraccion != null)
            {
                audioSource.PlayOneShot(sonidoInteraccion);
            }

            // Llama a ReiniciarEstado despu�s de 10 segundos
            Invoke("ReiniciarEstado", 10f);
        }
    }

    void ActivateTransformTrigger()
    {
        animator.SetTrigger(TransformTrigger);
    }

    public void ReiniciarEstado()
    {
        // Reiniciar el estado de la animaci�n
        animator.ResetTrigger(TransformTrigger);
        animator.Play("Idle");
        hasTransformed = false;
        CancelInvoke();
        Invoke("StartDeformAnimation", 2f);
    }
}
