using UnityEngine;

public class ControladorRombo : MonoBehaviour
{
    public Transform estrella;  // Asigna el Transform del GameObject estrella
    public float anguloRotacionLeve = 10f;  // Ángulo de rotación leve para ajustar la orientación

    void Update()
    {
        RotarRomboHaciaEstrella();
    }

    // Método para rotar el rombo hacia la estrella
    private void RotarRomboHaciaEstrella()
    {
        if (estrella != null)
        {
            Vector3 direccion = (estrella.position - transform.position).normalized;
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

            // Imprimir valores para diagnóstico
            Debug.Log($"Rombo: {gameObject.name}, Dirección: {direccion}, Ángulo Calculado: {angulo}");

            // Ajusta la rotación para que considere correctamente el eje Z
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo + anguloRotacionLeve));
        }
    }
}
