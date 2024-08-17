using UnityEngine;

public class RhombusAnimationController : MonoBehaviour
{
    private Animator animator;
    public string deformarStateName;  // Nombre del estado "Deformar" espec�fico para cada rombo
    public string TransformTrigger; // Nombre del trigger "Transformar" espec�fico para cada rombo
    private bool hasTransformed = false;

    void Start()
    {
        animator = GetComponent<Animator>();
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
