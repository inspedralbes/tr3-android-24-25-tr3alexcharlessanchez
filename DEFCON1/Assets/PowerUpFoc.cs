using UnityEngine;

public class PowerUpFoc : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Moure playerMovement = col.GetComponent<Moure>();
            if (playerMovement != null)
            {
                AudioManagerScript.instance.PowerUpReproduir();
                playerMovement.AugmentarFoc();
            }
            
            Destroy(gameObject);
        }
    }
}
