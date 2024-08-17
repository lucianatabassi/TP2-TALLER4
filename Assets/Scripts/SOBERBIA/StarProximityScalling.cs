using UnityEngine;

public class StarProximityScaling : MonoBehaviour
{
    public Animator[] romboAnimators; // Asigna los animadores de los rombos en el inspector
    public Transform estrella; // Asigna el transform de la estrella en el inspector
    public float scaleIncrement = 0.5f; // Cantidad por la cual la estrella aumentar� su escala

    private bool[] hasTriggered; // Para asegurarse de que cada rombo solo active la escala una vez

    void Start()
    {
        // Inicializa el array de bools seg�n la cantidad de rombos
        hasTriggered = new bool[romboAnimators.Length];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rombo"))
        {
            int index = System.Array.IndexOf(romboAnimators, other.GetComponent<Animator>());

            if (index >= 0 && !hasTriggered[index])
            {
                Debug.Log("Activando animaci�n de achicar para el rombo " + index);
                // Activa la animaci�n del rombo
                Animator romboAnimator = romboAnimators[index];
                if (romboAnimator != null)
                {
                    Debug.Log("Activando animaci�n de achicar para el rombo " + index);
                    romboAnimator.SetTrigger("Achicar"); // Aseg�rate de que "Achicar" es el trigger en el animador del rombo
                }

                // Aumenta la escala de la estrella
                estrella.localScale += new Vector3(scaleIncrement, scaleIncrement, 0);

                // Marca este rombo como activado
                hasTriggered[index] = true;
            }
        }
    }

    // M�todo para reiniciar las interacciones
    public void ResetInteractions()
    {
        // Reiniciar el estado de los rombos
        for (int i = 0; i < hasTriggered.Length; i++)
        {
            hasTriggered[i] = false; // Permitir que los rombos vuelvan a activar la escala
        }
    }
}
