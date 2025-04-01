using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.U2D.Animation;


public class SkinChanger : MonoBehaviour
{
    public ParamsPersonatges paramsPersonatges;
    private Moure moureScript;
    private SpriteResolver spriteResolver;
    private SpriteLibrary spriteLibrary;
    private Animator animator;
    private int idEleccio;

    void Start()
    {
        spriteResolver = GetComponent<SpriteResolver>();
        spriteLibrary = GetComponent<SpriteLibrary>();
        animator = GetComponent<Animator>();
        
        if (spriteResolver == null || spriteLibrary == null || animator == null)
        {
            Debug.LogError("SkinChanger: Falten components essentials!");
            return;
        }

        moureScript = GetComponent<Moure>();
        if (moureScript != null)
        {
            idEleccio = moureScript.idEleccio;
            Debug.Log("ID d'elecció: " + idEleccio);
        }
        
        StartCoroutine(ApplySkinWhenReady());
    }

    IEnumerator ApplySkinWhenReady()
    {

        while (paramsPersonatges == null || paramsPersonatges.personatges.Count <= idEleccio)
        {
            paramsPersonatges = FindObjectOfType<ParamsPersonatges>();
            yield return null;
        }

        Personatge personatge = (Personatge)paramsPersonatges.personatges[idEleccio];
        
        while (personatge.spriteLibraryAsset == null)
        {
            yield return null;
        }

        spriteLibrary.spriteLibraryAsset = personatge.spriteLibraryAsset;
        
        ResetAnimationSystem();
        
        Debug.Log("Skin aplicada correctament per a " + personatge.nom);
    }

    void ResetAnimationSystem()
{
    animator.enabled = false;
    animator.Rebind();
    animator.Update(0f);

    foreach (var category in spriteLibrary.spriteLibraryAsset.GetCategoryNames())
    {
        var labels = spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(category);
        if (labels.Any())
        {
            spriteResolver.SetCategoryAndLabel(category, labels.First());
            spriteResolver.ResolveSpriteToSpriteRenderer();
        }
    }

    animator.enabled = true;
    animator.Play("Idle");
}
}