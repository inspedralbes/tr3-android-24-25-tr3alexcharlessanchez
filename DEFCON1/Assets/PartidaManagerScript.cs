using UnityEngine;

public class PartidaManagerScript : MonoBehaviour
{
    public GameObject bomberman1;
    public GameObject bomberman2;
    public int guanyador;
    public CanviEscena canviEscena;
    public int comptadorBombes;
    public int comptadorDistancia;
    public int comptadorPowerUps;
    public int duracio;

    void Start()
    {
        InvokeRepeating(nameof(AugmentarComptadorDuracio), 1f, 1f);
    }
    void Update()
    {
        if (bomberman1 == null && bomberman2 != null){
            guanyador = 2;
            canviEscena.CarregarGameOver();
            
        } else if (bomberman2 == null && bomberman1 != null) {
            guanyador = 1;
            canviEscena.CarregarGameOver();
        } else if (bomberman1 == null && bomberman2 == null) {
            int randomNumber = Random.Range(1, 3);
            guanyador = randomNumber;
            canviEscena.CarregarGameOver();
        }
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void AugmentarComptadorDistancia(){
        comptadorDistancia += 1;
    }

    public void AugmentarComptadorBombes(){
        comptadorBombes += 1;
    }

    public void AugmentarComptadorPowerups(){
        comptadorPowerUps += 1;
    }

    private void AugmentarComptadorDuracio(){
        duracio += 1;
    }
}
