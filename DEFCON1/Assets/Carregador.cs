using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using UnityEngine.U2D.Animation;
using Unity.VisualScripting;

public class Carregador : MonoBehaviour
{
    public string apiUrl = "http://defcon1.dam.inspedralbes.cat:27891/personatge";
    public string apiImatgesUrl = "http://defcon1.dam.inspedralbes.cat:27891/sprites/";
    public ParamsPersonatges paramsPersonatges;

    private int pendingSkins = 0;
    public SpriteLibraryAsset baseSpriteLibrary;

    void Start()
    {
        StartCoroutine(CarregarJocAmbStats());
    }

    IEnumerator CarregarJocAmbStats()
    {
        yield return new WaitForSeconds(3f);

        AsyncOperation operacioCarrega = SceneManager.LoadSceneAsync(1);
        operacioCarrega.allowSceneActivation = false;

        Debug.Log("Carregador: Sol·licitant estadístiques des de " + apiUrl);
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Debug.Log("Carregador: JSON rebut: " + json);
                StatsData[] statsArray = JsonHelper.FromJson<StatsData>(json);
                if (statsArray != null && statsArray.Length > 0)
                {
                    Debug.Log("Carregador: Stats Array Length: " + statsArray.Length);
                    paramsPersonatges.personatges.Clear();

                    pendingSkins = statsArray.Length; 

                    foreach (StatsData stats in statsArray)
                    {
                        Personatge personatge = new Personatge(stats.idPersonatge, stats.nom, stats.velocitat, stats.bombesSimultanies, stats.forcaExplosions);
                        paramsPersonatges.personatges.Add(personatge);
                        Debug.Log("Carregador: Personatge afegit: " + personatge.nom);
                        StartCoroutine(CarregarSpritesPersonatge(personatge));
                    }

                    yield return new WaitUntil(() => pendingSkins <= 0);
                }
                else
                {
                    Debug.LogWarning("Carregador: No s'han trobat personatges a les dades rebudes.");
                }
            }
            else
            {
                Debug.LogError($"Carregador: Error carregant les stats: {request.error}");
            }
        }

        operacioCarrega.allowSceneActivation = true; 
    }

    private IEnumerator CarregarSpritesPersonatge(Personatge personatge)
    {
        string skinUrl = $"{apiImatgesUrl}{personatge.idPersonatge}.png";

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(skinUrl))
        {
            yield return request.SendWebRequest();

            try
            {
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error carregant skin: {request.error}");
                    yield break;
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                if (texture != null)
                {
                    var newLibrary = ScriptableObject.CreateInstance<SpriteLibraryAsset>();


                    int spriteWidth = 64;
                    int spriteHeight = 98;
                    void AddSpriteToLibrary(int row, int col, string category, string label)
                {
                    Rect spriteRect = new Rect(
                        col * spriteWidth,           
                        texture.height - (row + 1) * spriteHeight, 
                        spriteWidth,
                        spriteHeight
                    );

                    Sprite sprite = Sprite.Create(
                        texture,
                        spriteRect,
                        new Vector2(0.5f, 0.5f),
                        100,
                        0,
                        SpriteMeshType.FullRect
                    );

                    newLibrary.AddCategoryLabel(sprite, category, label);
                }


                    AddSpriteToLibrary(0, 7, "Idle", "Idle_01");

                    AddSpriteToLibrary(0, 7, "Abaix", "Abaix_01");
                    AddSpriteToLibrary(0, 6, "Abaix", "Abaix_02");
                    AddSpriteToLibrary(0, 5, "Abaix", "Abaix_03");
                    AddSpriteToLibrary(1, 0, "Abaix", "Abaix_04");
                    AddSpriteToLibrary(1, 1, "Abaix", "Abaix_05");

                    AddSpriteToLibrary(0, 7, "Abaix_Parat", "Abaix_Parat01");

                    AddSpriteToLibrary(2, 1, "Adalt", "Adalt_01");
                    AddSpriteToLibrary(2, 0, "Adalt", "Adalt_02");
                    AddSpriteToLibrary(1, 7, "Adalt", "Adalt_03");
                    AddSpriteToLibrary(2, 2, "Adalt", "Adalt_04");
                    AddSpriteToLibrary(2, 3, "Adalt", "Adalt_05");

                    AddSpriteToLibrary(2, 1, "Adalt_Parat", "Adalt_Parat01");

                    AddSpriteToLibrary(0, 2, "Dreta", "Dreta_01");
                    AddSpriteToLibrary(0, 1, "Dreta", "Dreta_02");
                    AddSpriteToLibrary(0, 0, "Dreta", "Dreta_03");
                    AddSpriteToLibrary(0, 3, "Dreta", "Dreta_04");
                    AddSpriteToLibrary(0, 4, "Dreta", "Dreta_05");

                    AddSpriteToLibrary(0, 2, "Dreta_Parat", "Dreta_Parat01");

                    AddSpriteToLibrary(1, 4, "Esquerra", "Esquerra_01");
                    AddSpriteToLibrary(1, 3, "Esquerra", "Esquerra_02");
                    AddSpriteToLibrary(1, 2, "Esquerra", "Esquerra_03");
                    AddSpriteToLibrary(1, 5, "Esquerra", "Esquerra_04");
                    AddSpriteToLibrary(1, 6, "Esquerra", "Esquerra_05");

                    AddSpriteToLibrary(1, 4, "Esquerra_Parat", "Esquerra_Parat_01");

                    AddSpriteToLibrary(0, 7, "Posar_Bomba", "Posar_Bomba_01");
                    AddSpriteToLibrary(3, 3, "Posar_Bomba", "Posar_Bomba_02");

                    personatge.spriteLibraryAsset = newLibrary;
                    Debug.Log($"Sprites carregats manualment per {personatge.nom}");
                }
            }
            finally
            {
                pendingSkins--;
            }
        }
    }

    [System.Serializable]
    private class StatsData
    {
        public int idPersonatge;
        public string nom;
        public float velocitat;
        public int forcaExplosions;
        public int bombesSimultanies;
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string wrapper = "{\"array\":" + json + "}";
        Wrapper<T> wrapped = JsonUtility.FromJson<Wrapper<T>>(wrapper);
        return wrapped.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}