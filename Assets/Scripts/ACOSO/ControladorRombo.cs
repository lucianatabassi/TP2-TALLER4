using UnityEngine;

public class ControladorRombo : MonoBehaviour
{
    public Transform estrella;  // Asigna el Transform del GameObject estrella
    public float anguloRotacionLeve = 10f;  // �ngulo de rotaci�n leve para ajustar la orientaci�n

    void Update()
    {
        RotarRomboHaciaEstrella();
    }

    // M�todo para rotar el rombo hacia la estrella
    private void RotarRomboHaciaEstrella()
    {
        if (estrella != null)
        {
            Vector3 direccion = (estrella.position - transform.position).normalized;
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

            // Imprimir valores para diagn�stico
            Debug.Log($"Rombo: {gameObject.name}, Direcci�n: {direccion}, �ngulo Calculado: {angulo}");

            // Ajusta la rotaci�n para que considere correctamente el eje Z
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo + anguloRotacionLeve));
        }
    }
}
