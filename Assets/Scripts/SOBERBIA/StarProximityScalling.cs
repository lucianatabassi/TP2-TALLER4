using UnityEngine;
using System.Collections;

public class StarProximityScaling : MonoBehaviour
{
    public Animator[] romboAnimators; // Asigna los animadores de los rombos en el inspector
    public Transform estrella; // Asigna el transform de la estrella en el inspector
    public float scaleIncrement = 0.5f; // Cantidad por la cual la estrella aumentar� su escala cada vez
    public float scaleDuration = 1f; // Duraci�n en segundos para completar el incremento de escala
    public float resetDelay = 10f; // Tiempo en segundos antes de reiniciar la estrella
    public float resetDuration = 2f; // Duraci�n en segundos para volver a la posici�n y tama�o inicial
    public AudioClip sonidoInteraccion; // AudioClip para el sonido de interacci�n
    public AudioClip sonidoReinicio; // AudioClip para el sonido de reinicio

    private Vector3 initialScale; // Escala inicial de la estrella
    private Vector3 initialPosition; // Posici�n inicial de la estrella
    private bool[] hasTriggered; // Para asegurarse de que cada rombo solo active la escala una vez
    private bool isResetting = false; // Indica si la estrella est� en el proceso de reinicio
    private int triggerCount = 0; // Contador de interacciones
    private AudioSource audioSource; // AudioSource para reproducir sonidos

    void Start()
    {
        // Inicializa el array de bools seg�n la cantidad de rombos
        hasTriggered = new bool[romboAnimators.Length];

        // Obtener el componente AudioSource o agregar uno si no existe
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Guarda la escala y posici�n inicial de la estrella
        if (estrella != null)
        {
            initialScale = estrella.localScale;
            initialPosition = estrella.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rombo") && !isResetting)
        {
            int index = System.Array.IndexOf(romboAnimators, other.GetComponent<Animator>());

            if (index >= 0 && !hasTriggered[index])
            {
                // Activa la animaci�n del rombo
                Animator romboAnimator = romboAnimators[index];
                if (romboAnimator != null)
                {
                    romboAnimator.SetTrigger("Achicar"); // Aseg�rate de que "Achicar" es el trigger en el animador del rombo
                }

                // Iniciar la corutina para aumentar gradualmente la escala de la estrella
                StartCoroutine(IncrementarEscalaGradualmente());

                // Reproducir sonido de interacci�n si est� definido
                if (sonidoInteraccion != null)
                {
                    audioSource.PlayOneShot(sonidoInteraccion);
                }

                // Marca este rombo como activado
                hasTriggered[index] = true;
                triggerCount++; // Incrementa el contador de interacciones
                Debug.Log("Trigger Count: " + triggerCount); // Mensaje de depuraci�n


                // Si se ha activado 4 veces, iniciar la corutina para reiniciar despu�s del retraso
                if (triggerCount >= romboAnimators.Length)
                {
                    StartCoroutine(ReiniciarDespu�sDeRetraso());
                }
            }
        }
    }

    // Corrutina para incrementar la escala de la estrella gradualmente
    IEnumerator IncrementarEscalaGradualmente()
    {
        // Aseg�rate de que la escala no exceda el l�mite m�ximo deseado
        Vector3 targetScale = initialScale + new Vector3(scaleIncrement * triggerCount, scaleIncrement * triggerCount, 0);
        Vector3 scaleStart = estrella.localScale;
        float elapsedTime = 0f;

        Debug.Log("Iniciando incremento de escala: escala objetivo = " + targetScale);

        while (elapsedTime < scaleDuration)
        {
            estrella.localScale = Vector3.Lerp(scaleStart, targetScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Espera hasta el siguiente frame
        }

        // Asegura que la escala final sea exactamente la de destino
        estrella.localScale = targetScale;
        Debug.Log("Incremento de escala completado: escala actual = " + estrella.localScale);
    }

    // Corrutina para reiniciar la estrella y las animaciones despu�s de un retraso
    IEnumerator ReiniciarDespu�sDeRetraso()
    {
        isResetting = true; // Establece que estamos en el proceso de reinicio
        yield return new WaitForSeconds(resetDelay); // Espera el retraso antes de reiniciar

        if (estrella == null) yield break;

        // Reiniciar la estrella a su escala y posici�n inicial
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

        // Asegura que la estrella est� exactamente en la posici�n y escala iniciales
        estrella.position = targetPosition;
        estrella.localScale = targetScale;

        // Reiniciar el estado de los rombos
        for (int i = 0; i < romboAnimators.Length; i++)
        {
            Animator romboAnimator = romboAnimators[i];
            if (romboAnimator != null)
            {
                // Vuelve al estado inicial del rombo
                romboAnimator.SetTrigger("Reiniciar"); // Aseg�rate de que "Reiniciar" es el trigger para volver al estado inicial
            }
        }

        // Reproduce el sonido de reinicio despu�s de que la estrella haya vuelto a la posici�n inicial
        if (sonidoReinicio != null)
        {
            audioSource.PlayOneShot(sonidoReinicio);
        }

        // Reinicia las interacciones para permitir nuevas activaciones
        for (int i = 0; i < hasTriggered.Length; i++)
        {
            hasTriggered[i] = false;
        }

        triggerCount = 0; // Reinicia el contador de interacciones
        isResetting = false; // Marca que ya no estamos en el proceso de reinicio
    }
}
