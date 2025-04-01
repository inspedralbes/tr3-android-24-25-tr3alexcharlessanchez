using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.U2D.Animation;

public class DropDown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public ParamsPersonatges paramsPersonatges;
    public int playerId;
    public Jugador1Script jugador1Script;
    public Jugador2Script jugador2Script;
    public GameObject personatge1;
    public GameObject personatge2;
    public TMP_Text detalls1;
    public TMP_Text detalls2;
    public GameObject botoJugar;
    private bool limitBotoHelperNosequeMes = false;

    void Start()
    {
        StartCoroutine(PopulateDropdown());
        jugador1Script = FindObjectOfType<Jugador1Script>();
        jugador2Script = FindObjectOfType<Jugador2Script>();
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator PopulateDropdown()
    {
        while (paramsPersonatges == null || paramsPersonatges.personatges.Count == 0)
        {
            paramsPersonatges = FindObjectOfType<ParamsPersonatges>();
            yield return null;
        }

        List<string> options = new List<string>();

        options.Add("Selecciona");

        foreach (Personatge p in paramsPersonatges.personatges)
        {
            if (!string.IsNullOrEmpty(p.nom))
            {
                options.Add(p.nom);
            }
            else
            {
                options.Add("Personatge sense nom");
            }
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(options);

        dropdown.value = 0;
        dropdown.RefreshShownValue();

        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        if (index == 0)
        {
            Debug.Log("Placeholder seleccionat");
            botoJugar.GetComponent<JugarBotoScript>().DisminuirValor();
            limitBotoHelperNosequeMes = false;

            if (playerId == 1)
            {
                detalls1.text = "Selecciona el teu Personatge";
            }
            else if (playerId == 2)
            {
                detalls2.text = "Selecciona el teu Personatge";
            }
            return;
        }
        else if (!limitBotoHelperNosequeMes)
        {
            botoJugar.GetComponent<JugarBotoScript>().AugmentarValor();
            limitBotoHelperNosequeMes = true;
        }

        int personatgeIndex = index - 1;
        Personatge p = (Personatge)paramsPersonatges.personatges[personatgeIndex];

        if (playerId == 1)
        {
            jugador1Script.eleccio = personatgeIndex;
            detalls1.text = $"Velocitat: {p.velocitat}\n" +
                            $"Explosions: {p.forcaExplosions} casella/es\n" +
                            $"Bombes Simultànies: {p.bombesSimultanies}\n";

            ActualitzarSkinPersonatge(personatge1, p);
        }
        else if (playerId == 2)
        {
            jugador2Script.eleccio = personatgeIndex;
            detalls2.text = $"Velocitat: {p.velocitat}\n" +
                            $"Explosions: {p.forcaExplosions} casella/es\n" +
                            $"Bombes Simultànies: {p.bombesSimultanies}\n";

            ActualitzarSkinPersonatge(personatge2, p);
        }
    }

    private void ActualitzarSkinPersonatge(GameObject personatge, Personatge p)
    {
        if (personatge == null || p.spriteLibraryAsset == null)
        {
            Debug.LogError("Personatge o SpriteLibraryAsset és null!");
            return;
        }

        SpriteLibrary spriteLibrary = personatge.GetComponent<SpriteLibrary>();
        SpriteResolver spriteResolver = personatge.GetComponent<SpriteResolver>();
        SpriteRenderer spriteRenderer = personatge.GetComponent<SpriteRenderer>();

        if (spriteLibrary == null || spriteResolver == null || spriteRenderer == null)
        {
            Debug.LogError("Falten components al personatge!");
            return;
        }

        StartCoroutine(CanviarSkinCoroutine(spriteLibrary, spriteResolver, spriteRenderer, p));
    }

    IEnumerator CanviarSkinCoroutine(SpriteLibrary library, SpriteResolver resolver, SpriteRenderer renderer, Personatge p)
    {
        Debug.Log("Iniciant CanviarSkinCoroutine per a " + p.nom);
        
        library.spriteLibraryAsset = p.spriteLibraryAsset;
        
        resolver.SetCategoryAndLabel("Idle", "Idle_01");
        resolver.ResolveSpriteToSpriteRenderer();
        yield return null;

    }
}
