using UnityEngine;
using UnityEngine.UI;

public class JugarBotoScript : MonoBehaviour
{
    [Header("Configuració")]
    public int valorEsperat = 2;         
    public Color colorInactiu = Color.grey; 
    
    [Header("Estat Actual")]
    public int valorActual = 0;

    private Button _boto;
    private Color _colorOriginal;

    void Start()
    {
        _boto = GetComponent<Button>();
        _colorOriginal = _boto.image.color;
        ActualitzarEstatBoto();
    }

    void Update()
    {
        ActualitzarEstatBoto();
    }

    public void AugmentarValor(int quantitat = 1)
    {
        valorActual += quantitat;
        ActualitzarEstatBoto();
    }

    public void DisminuirValor(int quantitat = 1)
    {
        valorActual -= quantitat;
        ActualitzarEstatBoto();
    }

    void ActualitzarEstatBoto()
    {
        bool estaDesbloquejat = (valorActual >= valorEsperat);
        
        _boto.interactable = estaDesbloquejat;
        _boto.image.color = estaDesbloquejat ? _colorOriginal : colorInactiu;
    }
}