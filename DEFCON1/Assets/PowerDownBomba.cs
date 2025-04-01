using UnityEngine;

public class PowerDownBomba : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Moure playerMovement = col.GetComponent<Moure>();
            if (playerMovement != null)
            {
                AudioManagerScript.instance.PowerDownReproduir();
                playerMovement.DisminuirBombes();
            }
            
            Destroy(gameObject);
        }
    }
}
