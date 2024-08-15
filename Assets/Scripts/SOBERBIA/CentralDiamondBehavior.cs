using UnityEngine;

public class CentralDiamondBehavior : MonoBehaviour
{
    public FallBehavior[] surroundingDiamonds; // Array de FallBehavior en los rombos alrededor
    public float targetScaleFactor = 1.5f; // Factor de escala objetivo para agrandar la estrella
    public float scalingSpeed = 0.5f; // Velocidad a la que la estrella se agranda
    private bool startScaling = false; // Para iniciar el escalado gradual
    private Vector3 initialScale; // Escala inicial de la estrella
    private Vector3 targetScale; // Escala objetivo de la estrella

    void Start()
    {
        initialScale = transform.localScale;
        targetScale = initialScale * targetScaleFactor;
    }

    void Update()
    {
        foreach (FallBehavior fallBehavior in surroundingDiamonds)
        {
            if (fallBehavior != null)
            {
                float distanceToCenter = Vector3.Distance(transform.position, fallBehavior.transform.position);
                if (distanceToCenter < fallBehavior.fallThreshold) // Usar el umbral específico del rombo
                {
                    fallBehavior.TriggerFall();
                }
            }
        }

        // Verificar si todos los rombos han caído
        if (!startScaling && AllDiamondsHaveFallen())
        {
            startScaling = true;
        }

        // Escalar la estrella gradualmente
        if (startScaling)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scalingSpeed * Time.deltaTime);

            // Detener el escalado cuando se alcanza la escala objetivo
            if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
            {
                transform.localScale = targetScale;
                startScaling = false;
            }
        }
    }

    // Método para verificar si todos los rombos han caído
    bool AllDiamondsHaveFallen()
    {
        foreach (FallBehavior fallBehavior in surroundingDiamonds)
        {
            if (fallBehavior != null && !fallBehavior.HasFallen())
            {
                return false;
            }
        }
        return true;
    }
}
