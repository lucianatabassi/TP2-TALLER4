using UnityEngine;
using System.Collections;

public class CambiarAnimacion : MonoBehaviour
{
    public Animator animator;
    public string nombreAnimacionInicial = "Idle";
    public string nombreAnimacionSiguiente = "Deformacion";
    public float tiempoEspera = 3f; // Tiempo en segundos antes de cambiar la animación
    public float duracionBucle = 1f; // Duración en segundos que la animación estará en bucle

    private AnimatorOverrideController overrideController;
    private AnimationClip clipToModify;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Crear un AnimatorOverrideController basado en el controlador de animación original
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;

        // Buscar y almacenar el AnimationClip a modificar
        foreach (AnimationClip clip in overrideController.animationClips)
        {
            if (clip.name == nombreAnimacionSiguiente)
            {
                clipToModify = clip;
                break;
            }
        }

        IniciarSecuencia();
    }

    public void IniciarSecuencia()
    {
        // Iniciar con la animación inicial
        animator.Play(nombreAnimacionInicial);

        // Iniciar la corutina para cambiar a la siguiente animación después de cierto tiempo
        StartCoroutine(CambiarAnimacionDespuesDeTiempo());
    }

    public void ReiniciarSecuencia()
    {
        StopAllCoroutines();  // Detener cualquier corutina anterior
        IniciarSecuencia();   // Reiniciar la secuencia
    }

    IEnumerator CambiarAnimacionDespuesDeTiempo()
    {
        // Esperar cierto tiempo antes de cambiar la animación
        yield return new WaitForSeconds(tiempoEspera);

        // Cambiar a la siguiente animación (que está en loop por defecto)
        animator.Play(nombreAnimacionSiguiente, -1, 0);

        // Esperar la duración del bucle
        yield return new WaitForSeconds(duracionBucle);

        // Desactivar el loop del AnimationClip modificando su wrapMode
        if (clipToModify != null)
        {
            clipToModify.wrapMode = WrapMode.Once; // Desactivar el loop
        }
    }
}
