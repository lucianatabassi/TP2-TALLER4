using UnityEngine;

public class StarProximityScaling : MonoBehaviour
{
    public Animator[] romboAnimators; // Asigna los animadores de los rombos en el inspector
    public Transform estrella; // Asigna el transform de la estrella en el inspector
    public float scaleIncrement = 0.5f; // Cantidad por la cual la estrella aumentará su escala

    private bool[] hasTriggered; // Para asegurarse de que cada rombo solo active la escala una vez

    void Start()
    {
        // Inicializa el array de bools según la cantidad de rombos
        hasTriggered = new bool[romboAnimators.Length];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rombo"))
        {
            int index = System.Array.IndexOf(romboAnimators, other.GetComponent<Animator>());

            if (index >= 0 && !hasTriggered[index])
            {
                Debug.Log("Activando animación de achicar para el rombo " + index);
                // Activa la animación del rombo
                Animator romboAnimator = romboAnimators[index];
                if (romboAnimator != null)
                {
                    Debug.Log("Activando animación de achicar para el rombo " + index);
                    romboAnimator.SetTrigger("Achicar"); // Asegúrate de que "Achicar" es el trigger en el animador del rombo
                }

                // Aumenta la escala de la estrella
                estrella.localScale += new Vector3(scaleIncrement, scaleIncrement, 0);

                // Marca este rombo como activado
                hasTriggered[index] = true;
            }
        }
    }
}
