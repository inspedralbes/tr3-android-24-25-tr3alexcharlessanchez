using UnityEngine;
using UnityEngine.SceneManagement;

public class CanviEscena : MonoBehaviour
{
    public void CarregarMenu()
    {
        SceneManager.LoadScene(1);
    }
    
    public void CarregarPersonatges()
    {
        SceneManager.LoadScene(2);
    }

    public void CarregarMenuJocs()
    {
        SceneManager.LoadScene(3);
    }

    public void CarregarJocClassic()
    {
        SceneManager.LoadScene(4);
    }

    public void CarregarJocBatallaMort()
    {
        SceneManager.LoadScene(5);
    }

    public void CarregarGameOver()
    {
        SceneManager.LoadScene(6);
    }

    public void SortirJoc()
    {
        Application.Quit(); 
    }
}