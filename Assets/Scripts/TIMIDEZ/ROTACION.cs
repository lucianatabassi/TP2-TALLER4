using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROTACION : MonoBehaviour
{
    public GameObject estrella;  // Referencia a la estrella
    public float velocidadRotacion = 5f;  // Velocidad de rotaci�n de los rombos
    public float ajusteRotacionInicial = 45f;  // Ajuste de rotaci�n para corregir el desajuste

    private Quaternion rotacionOriginal;  // Para guardar la rotaci�n original

    void Start()
    {
        // Guarda la rotaci�n original del rombo
        rotacionOriginal = transform.rotation;
    }

    void Update()
    {
        // Aseg�rate de que la estrella est� asignada
        if (estrella == null) return;

        // Direcci�n hacia la cual debe rotar el rombo (en el plano 2D)
        Vector3 direccionHaciaEstrella = estrella.transform.position - transform.position;
        direccionHaciaEstrella.Normalize();  // Normalizamos la direcci�n

        // Convertimos la direcci�n en un �ngulo (en grados) en el plano 2D
        float angulo = Mathf.Atan2(direccionHaciaEstrella.y, direccionHaciaEstrella.x) * Mathf.Rad2Deg;

        // Ajustamos el �ngulo con la correcci�n de 45 grados
        angulo -= ajusteRotacionInicial;

        // Aplicamos la rotaci�n al objeto
        Quaternion rotacionDeseada = Quaternion.Euler(0, 0, angulo);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * velocidadRotacion);
    }

    // M�todo para reiniciar la rotaci�n al estado original
    public void Resetear()
    {
        transform.rotation = rotacionOriginal;
    }
}
