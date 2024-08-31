using UnityEngine;
using System.Collections;

public class StarProximityScaling : MonoBehaviour
{
    public Animator[] romboAnimators; // Asigna los animadores de los rombos en el inspector
    public Transform estrella; // Asigna el transform de la estrella en el inspector
    public float scaleIncrement = 0.5f; // Cantidad por la cual la estrella aumentará su escala
    public float scaleDuration = 1f; // Duración en segundos para completar el incremento de escala
    public float resetDelay = 10f; // Tiempo en segundos antes de reiniciar la estrella
    public float resetDuration = 2f; // Duración en segundos para volver a la posición y tamaño inicial
    public AudioClip sonidoInteraccion; // AudioClip para el sonido de interacción

    private Vector3 initialScale; // Escala inicial de la estrella
    private Vector3 initialPosition; // Posición inicial de la estrella
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

        // Guarda la escala y posición inicial de la estrella
        if (estrella != null)
        {
            initialScale = estrella.localScale;
            initialPosition = estrella.position;
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

                // Iniciar la corutina para reiniciar la estrella después de un retraso
                StartCoroutine(ReiniciarEstrella());
            }
        }
    }

    // Corrutina para incrementar la escala de la estrella gradualmente
    IEnumerator IncrementarEscalaGradualmente()
    {
        Vector3 targetScale = initialScale + new Vector3(scaleIncrement, scaleIncrement, 0);
        Vector3 scaleStart = estrella.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < scaleDuration)
        {
            estrella.localScale = Vector3.Lerp(scaleStart, targetScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Espera hasta el siguiente frame
        }

        // Asegura que la escala final sea exactamente la de destino
        estrella.localScale = targetScale;
    }

    // Corrutina para reiniciar la estrella y las animaciones
    IEnumerator ReiniciarEstrella()
    {
        yield return new WaitForSeconds(resetDelay); // Espera el retraso antes de reiniciar

        if (estrella == null) yield break;

        Vector3 scaleStart = estrella.localScale;
        Vector3 targetScale = initialScale;
        Vector3 positionStart = estrella.position;
        Vector3 targetPosition = initialPosition;

        float elapsedTime = 0f;

        // Mueve la estrella y cambia su escala gradualmente
        while (elapsedTime < resetDuration)
        {
            estrella.position = Vector3.Lerp(positionStart, targetPosition, elapsedTime / resetDuration);
            estrella.localScale = Vector3.Lerp(scaleStart, targetScale, elapsedTime / resetDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Espera hasta el siguiente frame
        }

        // Asegura que la estrella esté exactamente en la posición y escala iniciales
        estrella.position = targetPosition;
        estrella.localScale = targetScale;

        // Reiniciar el estado de los rombos
        for (int i = 0; i < romboAnimators.Length; i++)
        {
            Animator romboAnimator = romboAnimators[i];
            if (romboAnimator != null)
            {
                // Vuelve al estado inicial del rombo
                romboAnimator.SetTrigger("Reiniciar"); // Asegúrate de que "Reiniciar" es el trigger para volver al estado inicial
            }
        }

        // Reinicia las interacciones para permitir nuevas activaciones
        for (int i = 0; i < hasTriggered.Length; i++)
        {
            hasTriggered[i] = false;
        }
    }
}
