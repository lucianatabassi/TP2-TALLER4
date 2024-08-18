using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROTACION : MonoBehaviour
{
    public GameObject estrella;  // Referencia a la estrella
    public float velocidadRotacion = 5f;  // Velocidad de rotación de los rombos
    public float ajusteRotacionInicial = 45f;  // Ajuste de rotación para corregir el desajuste

    private Quaternion rotacionOriginal;  // Para guardar la rotación original

    void Start()
    {
        // Guarda la rotación original del rombo
        rotacionOriginal = transform.rotation;
    }

    void Update()
    {
        // Asegúrate de que la estrella esté asignada
        if (estrella == null) return;

        // Dirección hacia la cual debe rotar el rombo (en el plano 2D)
        Vector3 direccionHaciaEstrella = estrella.transform.position - transform.position;
        direccionHaciaEstrella.Normalize();  // Normalizamos la dirección

        // Convertimos la dirección en un ángulo (en grados) en el plano 2D
        float angulo = Mathf.Atan2(direccionHaciaEstrella.y, direccionHaciaEstrella.x) * Mathf.Rad2Deg;

        // Ajustamos el ángulo con la corrección de 45 grados
        angulo -= ajusteRotacionInicial;

        // Aplicamos la rotación al objeto
        Quaternion rotacionDeseada = Quaternion.Euler(0, 0, angulo);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * velocidadRotacion);
    }

    // Método para reiniciar la rotación al estado original
    public void Resetear()
    {
        transform.rotation = rotacionOriginal;
    }
}
