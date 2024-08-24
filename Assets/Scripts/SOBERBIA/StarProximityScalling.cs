using UnityEngine;
using System.Collections;  // Añade esta línea para utilizar IEnumerator y corrutinas

public class StarProximityScaling : MonoBehaviour
{
    public Animator[] romboAnimators; // Asigna los animadores de los rombos en el inspector
    public Transform estrella; // Asigna el transform de la estrella en el inspector
    public float scaleIncrement = 0.5f; // Cantidad por la cual la estrella aumentará su escala
    public float scaleDuration = 1f; // Duración en segundos para completar el incremento de escala
    public AudioClip sonidoInteraccion; // AudioClip para el sonido de interacción

    private bool[] hasTriggered; // Para asegurarse de que cada rombo solo active la escala una vez
    private AudioSource audioSource; // AudioSource para reproducir sonidos

    void Start()
    {
        // Inicializa el array de bools según la cantidad de rombos
        hasTriggered = new bool[romboAnimators.Length];

        // Obtener el componente AudioSource o agregar uno si no existe
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rombo"))
        {
            int index = System.Array.IndexOf(romboAnimators, other.GetComponent<Animator>());

            if (index >= 0 && !hasTriggered[index])
            {
                // Activa la animación del rombo
                Animator romboAnimator = romboAnimators[index];
                if (romboAnimator != null)
                {
                    romboAnimator.SetTrigger("Achicar"); // Asegúrate de que "Achicar" es el trigger en el animador del rombo
                }

                // Iniciar la corutina para aumentar gradualmente la escala de la estrella
                StartCoroutine(IncrementarEscalaGradualmente());

                // Reproducir sonido de interacción si está definido
                if (sonidoInteraccion != null)
                {
                    audioSource.PlayOneShot(sonidoInteraccion);
                }

                // Marca este rombo como activado
                hasTriggered[index] = true;
            }
        }
    }

    // Corrutina para incrementar la escala de la estrella gradualmente
    IEnumerator IncrementarEscalaGradualmente()
    {
        Vector3 initialScale = estrella.localScale;
        Vector3 targetScale = initialScale + new Vector3(scaleIncrement, scaleIncrement, 0);

        float elapsedTime = 0f;

        while (elapsedTime < scaleDuration)
        {
            estrella.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Espera hasta el siguiente frame
        }

        // Asegura que la escala final sea exactamente la de destino
        estrella.localScale = targetScale;
    }

    // Método para reiniciar las interacciones
    public void ResetInteractions()
    {
        // Reiniciar el estado de los rombos
        for (int i = 0; i < hasTriggered.Length; i++)
        {
            hasTriggered[i] = false; // Permitir que los rombos vuelvan a activar la escala
        }
    }
}
