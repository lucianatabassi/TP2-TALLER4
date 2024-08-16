using UnityEngine;
using System.Collections;

public class CambiarAnimacion : MonoBehaviour
{
    public Animator animator;
    public string nombreAnimacionInicial = "Idle";
    public string nombreAnimacionSiguiente = "Deformacion";
    public float tiempoEspera = 3f; // Tiempo en segundos antes de cambiar la animaci�n

    void Start()
    {
        animator = GetComponent<Animator>();

        // Iniciar con la animaci�n inicial
        animator.Play(nombreAnimacionInicial);

        // Iniciar la corutina para cambiar a la siguiente animaci�n despu�s de cierto tiempo
        StartCoroutine(CambiarAnimacionDespuesDeTiempo());
    }

    IEnumerator CambiarAnimacionDespuesDeTiempo()
    {
        // Esperar cierto tiempo antes de cambiar la animaci�n
        yield return new WaitForSeconds(tiempoEspera);

        // Cambiar a la siguiente animaci�n
        animator.Play(nombreAnimacionSiguiente);
    }
}
