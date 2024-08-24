using UnityEngine;
using System.Collections;

public class CambiarAnimacion : MonoBehaviour
{
    public Animator animator;
    public string nombreAnimacionInicial = "Idle";
    public string nombreAnimacionSiguiente = "Deformacion";
    public float tiempoEspera = 3f; // Tiempo en segundos antes de cambiar la animaci�n
    public float duracionBucle = 1f; // Duraci�n en segundos que la animaci�n estar� en bucle

    void Start()
    {
        animator = GetComponent<Animator>();
        IniciarSecuencia();
    }

    public void IniciarSecuencia()
    {
        // Iniciar con la animaci�n inicial
        animator.Play(nombreAnimacionInicial);

        // Iniciar la corutina para cambiar a la siguiente animaci�n despu�s de cierto tiempo
        StartCoroutine(CambiarAnimacionDespuesDeTiempo());
    }

    public void ReiniciarSecuencia()
    {
        StopAllCoroutines();  // Detener cualquier corutina anterior
        IniciarSecuencia();   // Reiniciar la secuencia
    }

    IEnumerator CambiarAnimacionDespuesDeTiempo()
    {
        // Esperar cierto tiempo antes de cambiar la animaci�n
        yield return new WaitForSeconds(tiempoEspera);

        // Cambiar a la siguiente animaci�n (que est� en loop por defecto)
        animator.Play(nombreAnimacionSiguiente, -1, 0);

        // Esperar la duraci�n del bucle
        yield return new WaitForSeconds(duracionBucle);

        // Detener el bucle de la animaci�n configurando el tiempo de loop a cero
        animator.SetFloat("Loop", 0f);
    }
}
