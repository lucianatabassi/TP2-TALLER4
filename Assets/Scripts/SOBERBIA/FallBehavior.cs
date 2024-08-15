using UnityEngine;

public class FallBehavior : MonoBehaviour
{
    public float fallThreshold = 1.0f; // Distancia umbral para caer
    public float groundLevel = -5.0f; // Nivel del suelo en el eje Y
    private bool hasFallen = false; // Para evitar que el rombo caiga más de una vez

    void Start()
    {
        // No es necesario verificar centralDiamond aquí, ya que la verificación se hace en CentralDiamondBehavior
    }

    void Update()
    {
        // Limitar la caída del rombo
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

    // Método para activar la caída
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

    // Método para verificar si ha caído
    public bool HasFallen()
    {
        return hasFallen;
    }
}
