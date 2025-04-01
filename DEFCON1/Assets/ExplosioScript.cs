using UnityEngine;

public class ExplosioScript : MonoBehaviour
{
    public LayerMask jugadorMask;
    public LayerMask bombesMask;
    public LayerMask powerUpsMask;

    void Start()
    {
        Invoke(nameof(Destroy), 2.5f);
    }

    void Update()
    {
        Collider2D[] jugadorsAfectats = Physics2D.OverlapCircleAll(transform.position, 0.5f, jugadorMask);
        foreach (Collider2D obj in jugadorsAfectats)
        {
            if (obj.CompareTag("Player"))
            {
                Destroy(obj.gameObject);
            }
        }

        Collider2D[] bombesAfectades = Physics2D.OverlapCircleAll(transform.position, 0.5f, bombesMask);
        foreach (Collider2D obj in bombesAfectades)
        {
            if (obj.CompareTag("Bomba"))
            {
                BombaScript bomba = obj.GetComponent<BombaScript>();
                if (bomba != null)
                {
                    Debug.Log("Bomba trobada! Explosió activada.");
                    bomba.Explota();
                }
                else
                {
                    Debug.LogError("ERROR: L'objecte amb tag 'Bomba' no té el component Bomba!");
                }
            }
        }

    }

     void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("PowerUps"))
            {
                Destroy(other.gameObject);
            }
        }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
