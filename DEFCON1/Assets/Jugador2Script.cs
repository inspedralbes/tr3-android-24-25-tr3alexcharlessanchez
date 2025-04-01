using UnityEngine;

public class Jugador2Script : MonoBehaviour
{
    public int idUsuari;
    public string nom;
    public int victories;
    public int derrotes;
    public int morts;
    public int kills;
    public int eleccio;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
