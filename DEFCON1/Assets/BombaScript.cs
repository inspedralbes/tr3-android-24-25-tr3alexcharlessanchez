using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class BombaScript : MonoBehaviour
{
    public int probabilitatSpawnPowerup = 20;
    public float tempsExplosio = 3f; 
    public int radiExplosio = 1; 
    public Grid grid;
    public Tilemap mapa;
    public Tilemap obstacleTrencable; 
    public Tilemap obstacleIndestructible; 
    public GameObject efecteExplosioCentralPrefab;
    public GameObject efecteExplosioHortizontalMigEsquerra;
    public GameObject efecteExplosioHortizontalMigDreta;
    public GameObject efecteExplosioHortizontalFinalEsquerra;
    public GameObject efecteExplosioHortizontalFinalDreta;
    public GameObject efecteExplosioAdaltMig;
    public GameObject efecteExplosioAbaixMig;
    public GameObject efecteExplosioAdaltFinal;
    public GameObject efecteExplosioAbaixFinal;
    public GameObject[] powerUps;

    public delegate void ExplosioHandler();
    public event ExplosioHandler onExplosio;

    private Vector3 midaInicial;
    private float increment = 0.02f;
    private bool augmentant = true;

    void Start()
    {
        midaInicial = transform.localScale * 0.9f;
        transform.localScale = midaInicial;
        InvokeRepeating(nameof(PulsarMida), 0.5f, 0.5f);
        Invoke(nameof(Explota), tempsExplosio);
    }

    public void Explota()
    {
        AudioManagerScript.instance.ExplosioReproduir();
        Vector3Int centreTile = grid.WorldToCell(transform.position);
        Vector3 posicioCentral = grid.GetCellCenterWorld(centreTile);

        Instantiate(efecteExplosioCentralPrefab, posicioCentral, Quaternion.identity);
        DestrueixTile(centreTile);

        Vector3Int[] direccions = { Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down };

        foreach (Vector3Int direccio in direccions)
        {
            for (int i = 1; i <= radiExplosio; i++)
            {
                Vector3Int posTile = centreTile + direccio * i;
                Vector3 posicioMundial = grid.GetCellCenterWorld(posTile);

                if (obstacleIndestructible.HasTile(posTile))
                {
                    break;
                }

                bool esFinal = (i == radiExplosio);
                GameObject prefabExplosion = null;

                if (obstacleTrencable.HasTile(posTile))
                {
                    esFinal = true;
                    DestrueixTile(posTile);
                    GameObject spawner = new GameObject("PowerupSpawner");
                    spawner.AddComponent<PowerupSpawner>().Init(powerUps, posicioMundial, probabilitatSpawnPowerup);
                    
                    if (direccio == Vector3Int.right)
                        prefabExplosion = efecteExplosioHortizontalFinalDreta;
                    else if (direccio == Vector3Int.left)
                        prefabExplosion = efecteExplosioHortizontalFinalEsquerra;
                    else if (direccio == Vector3Int.up)
                        prefabExplosion = efecteExplosioAdaltFinal;
                    else if (direccio == Vector3Int.down)
                        prefabExplosion = efecteExplosioAbaixFinal;

                    if (prefabExplosion != null)
                        Instantiate(prefabExplosion, posicioMundial, Quaternion.identity);
                    
                    break;
                }

                if (direccio == Vector3Int.right)
                {
                    prefabExplosion = esFinal ? efecteExplosioHortizontalFinalDreta : efecteExplosioHortizontalMigDreta;
                }
                else if (direccio == Vector3Int.left)
                {
                    prefabExplosion = esFinal ? efecteExplosioHortizontalFinalEsquerra : efecteExplosioHortizontalMigEsquerra;
                }
                else if (direccio == Vector3Int.up)
                {
                    prefabExplosion = esFinal ? efecteExplosioAdaltFinal : efecteExplosioAdaltMig;
                }
                else if (direccio == Vector3Int.down)
                {
                    prefabExplosion = esFinal ? efecteExplosioAbaixFinal : efecteExplosioAbaixMig;
                }

                if (prefabExplosion != null)
                {
                    Instantiate(prefabExplosion, posicioMundial, Quaternion.identity);
                }
            }
        }

        onExplosio?.Invoke();

        Destroy(gameObject);
    }

    void DestrueixTile(Vector3Int posicio)
    {
        if (obstacleTrencable.HasTile(posicio))
        {
            obstacleTrencable.SetTile(posicio, null);
        }
    }

    void PulsarMida()
    {
        if (augmentant)
        {
            transform.localScale += new Vector3(increment, increment, 0);
        }
        else
        {
            transform.localScale -= new Vector3(increment, increment, 0);
        }

        augmentant = !augmentant;
    }
}

public class PowerupSpawner : MonoBehaviour
{
    public void Init(GameObject[] powerUps, Vector3 posicio, int probabilitat)
    {
        StartCoroutine(Spawn(powerUps, posicio, probabilitat));
    }

    IEnumerator Spawn(GameObject[] powerUps, Vector3 posicio, int probabilitat)
    {
        yield return new WaitForSeconds(2.5f);
        if (Random.Range(0, 100) < probabilitat)
        {
            GameObject powerUp = powerUps[Random.Range(0, powerUps.Length)];
            Instantiate(powerUp, posicio, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
