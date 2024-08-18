using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; // Necesario para trabajar con UI
using UnityEngine;

public class ReiniciarProteccion : MonoBehaviour
{
    public Button botonReiniciar; // Referencia al botón en la UI
    public ProteccionRombos proteccionRombos; // Referencia al script que debe reiniciarse

    void Start()
    {
        if (botonReiniciar != null && proteccionRombos != null)
        {
            botonReiniciar.onClick.AddListener(Reiniciar); // Añade el listener al botón
        }
    }

    // Método para reiniciar el estado del script
    public void Reiniciar()
    {
        if (proteccionRombos != null)
        {
            proteccionRombos.Reiniciar();
        }
    }
}
