using UnityEngine;
using UnityEngine.Tilemaps;

public class PosaBombes : MonoBehaviour
{
    public GameObject bombaPrefab;
    public Grid grid;
    public Tilemap mapa;
    public Tilemap tilemap;
    public Tilemap obstacleTrencable;
    public int maxBombes; 
    private int bombesActives = 0; 

    public int radiExplosio; 
    


    public void PosarBomba()
    {
        if (bombesActives >= maxBombes) return; 

        AudioManagerScript.instance.PosarBombaReproduir();

        Vector3 posicioJugador = transform.position;
        Vector3 ajustPosicio = posicioJugador - tilemap.tileAnchor;
        Vector3Int tilePos = tilemap.WorldToCell(ajustPosicio);
        Vector3 posicioCentrada = tilemap.GetCellCenterWorld(tilePos);

        GameObject bomba = Instantiate(bombaPrefab, posicioCentrada, Quaternion.identity);
        bomba.GetComponent<BombaScript>().grid = grid;
        bomba.GetComponent<BombaScript>().obstacleTrencable = obstacleTrencable;
        bomba.GetComponent<BombaScript>().obstacleIndestructible = tilemap;
        bomba.GetComponent<BombaScript>().mapa = mapa;
        bomba.GetComponent<BombaScript>().radiExplosio = this.radiExplosio; 

        bombesActives++; 
        bomba.GetComponent<BombaScript>().onExplosio += BombaExplotada; 

        
    }

    void BombaExplotada()
    {
        bombesActives--;
    }

    public void AugmentarRadiExplosio()
    {
        radiExplosio++;
    }

    public void DisminuirRadiExplosio()
    {   
        if (radiExplosio != 1){
            radiExplosio--;
        }
    }

    public void AugmentarMaxBombes()
    {
        maxBombes++;
    }

    public void DisminuirMaxBombes()
    {   
        if (maxBombes != 1){
            maxBombes--;
        }
    }


    void Update()
    {
        transform.position = transform.parent.position;
    }
}
