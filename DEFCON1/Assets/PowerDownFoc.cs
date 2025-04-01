using UnityEngine;

public class PowerDownFoc : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Moure playerMovement = col.GetComponent<Moure>();
            if (playerMovement != null)
            {
                AudioManagerScript.instance.PowerDownReproduir();
                playerMovement.DisminuirFoc();
            }
            
            Destroy(gameObject);
        }
    }
}
