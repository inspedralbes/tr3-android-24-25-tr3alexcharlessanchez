using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class ParamsPersonatges : MonoBehaviour
{
    public ArrayList personatges = new ArrayList();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

public class Personatge
{
    public int idPersonatge;
    public string nom;
    public float velocitat;
    public int bombesSimultanies;
    public int forcaExplosions;
    public SpriteLibraryAsset spriteLibraryAsset;

    public Personatge(int idPersonatge, string nom, float velocitat, int bombesSimultanies, int forcaExplosions)
    {
        this.idPersonatge = idPersonatge;
        this.nom = nom;
        this.velocitat = velocitat;
        this.bombesSimultanies = bombesSimultanies;
        this.forcaExplosions = forcaExplosions;
        this.spriteLibraryAsset = null;
    }
    
}