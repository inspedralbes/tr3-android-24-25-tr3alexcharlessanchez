using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DropDown2 : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public ParamsPersonatges paramsPersonatges;
    public int eleccio;

    void Start()
    {
        StartCoroutine(PopulateDropdown());
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

        dropdown.RefreshShownValue();

        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        Debug.Log("Personatge seleccionat: " + ((Personatge)paramsPersonatges.personatges[index]).nom);
        eleccio = index;
    }
}
