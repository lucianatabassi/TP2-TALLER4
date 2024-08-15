using UnityEngine;

public class FallBehavior : MonoBehaviour
{
    public float fallThreshold = 1.0f; // Distancia umbral para caer
    public float groundLevel = -5.0f; // Nivel del suelo en el eje Y
    private bool hasFallen = false; // Para evitar que el rombo caiga m�s de una vez

    void Start()
    {
        // No es necesario verificar centralDiamond aqu�, ya que la verificaci�n se hace en CentralDiamondBehavior
    }

    void Update()
    {
        // Limitar la ca�da del rombo
        if (transform.position.y < groundLevel)
        {
            transform.position = new Vector3(transform.position.x, groundLevel, transform.position.z);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero; // Detener cualquier movimiento adicional
                rb.gravityScale = 0; // Desactivar la gravedad una vez que alcanza el suelo
            }
        }
    }

    // M�todo para activar la ca�da
    public void TriggerFall()
    {
        if (!hasFallen)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 1; // Activa la gravedad
                hasFallen = true;
            }
            else
            {
                Debug.LogError("Rigidbody2D component is missing on this GameObject.");
            }
        }
    }

    // M�todo para verificar si ha ca�do
    public bool HasFallen()
    {
        return hasFallen;
    }
}
