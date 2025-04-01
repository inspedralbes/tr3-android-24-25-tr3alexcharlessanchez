using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript instance;
    public AudioSource audioSource;
    public AudioClip soPowerUp;
    public AudioClip soPosarBomba;
    public AudioClip soPowerDown;
    public AudioClip soExplosio;

    public void PowerUpReproduir()
    {
        audioSource.PlayOneShot(soPowerUp);
    }
    public void PowerDownReproduir()
    {
        audioSource.PlayOneShot(soPowerDown);
    }
    public void ExplosioReproduir()
    {
        audioSource.PlayOneShot(soExplosio);
    }
    public void PosarBombaReproduir()
    {
        audioSource.PlayOneShot(soPosarBomba);
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
