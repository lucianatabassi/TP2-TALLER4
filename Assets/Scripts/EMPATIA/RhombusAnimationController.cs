using UnityEngine;

public class RhombusAnimationController : MonoBehaviour
{
    private Animator animator;
    public string deformarStateName;  // Nombre del estado "Deformar" específico para cada rombo
    public string TransformTrigger; // Nombre del trigger "Transformar" específico para cada rombo
    private bool hasTransformed = false;

    void Start()
    {
        animator = GetComponent<Animator>();
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
        }
    }

    void ActivateTransformTrigger()
    {
        animator.SetTrigger(TransformTrigger); // Usa el nombre del trigger específico
    }
}
