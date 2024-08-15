using UnityEngine;

public class ControladorRombo : MonoBehaviour
{
    public GameObject animacionAgrandar; // Asigna el GameObject con la animación "Agrandar"
    public GameObject animacionIdle; // Asigna el GameObject con la animación "Idle"
    public Transform estrella; // Asigna el Transform del GameObject estrella
    public float anguloRotacionLeve = 10f; // Ángulo de rotación leve para ajustar la orientación

    private bool animacionActiva = false;

    void Start()
    {
        // Inicialmente, desactiva la animación "Agrandar" y muestra la animación "Idle"
        if (animacionAgrandar != null)
        {
            animacionAgrandar.SetActive(false);
        }
        if (animacionIdle != null)
        {
            animacionIdle.SetActive(true);
        }
    }

    void Update()
    {
        // Si la animación está activa, rota el rombo hacia la estrella
        if (animacionActiva && estrella != null)
        {
            RotarHaciaEstrella();
        }
    }

    // Método para activar o desactivar la animación "Agrandar"
    public void ActivarAnimacionAgrandar(bool activar)
    {
        if (animacionAgrandar != null && animacionIdle != null)
        {
            animacionAgrandar.SetActive(activar);
            animacionIdle.SetActive(!activar);
            Debug.Log($"{gameObject.name} - Animación Agrandar: {activar}");
        }
        animacionActiva = activar;
    }

    // Método para rotar el rombo hacia la estrella
    private void RotarHaciaEstrella()
    {
        Vector3 direccion = (estrella.position - transform.position).normalized;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        Debug.Log($"Rombo: {gameObject.name}, Ángulo: {angulo}, Dirección: {direccion}");
        transform.rotation = Quaternion.Euler(0, 0, angulo + anguloRotacionLeve);
    }
}
