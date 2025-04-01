using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Text;
using System;
public class nomGuanyadorScript : MonoBehaviour
{
    public TMP_Text text;
    public Jugador1Script jugador1Script;
    public Jugador2Script jugador2Script;
    public PartidaManagerScript partidaManagerScript;
    private int nextPartidaId;
    private int idJugadorGuanyador;
    private int idJugadorPerdedor;
    private string endpointPartida = "http://defcon1.dam.inspedralbes.cat:27891/partida";
    private string endpointGuanyar = "http://defcon1.dam.inspedralbes.cat:27891/guanyar";
    private string endpointPerdre = "http://defcon1.dam.inspedralbes.cat:27891/perdre";
    private string endpointKill = "http://defcon1.dam.inspedralbes.cat:27891/kill";
    private string endpointMort = "http://defcon1.dam.inspedralbes.cat:27891/mort";
    private string endpointSeguentPartida = "http://defcon1.dam.inspedralbes.cat:27891/seguentPartida";
    private string endpointBombes = "http://defcon1.dam.inspedralbes.cat:27893/numeroBombesPartida";
    private string endpointDistancia = "http://defcon1.dam.inspedralbes.cat:27893/distanciaPartida";
    private string endpointPowerups =  "http://defcon1.dam.inspedralbes.cat:27893/powerupsPartida";

    void Start()
    {
        partidaManagerScript = FindObjectOfType<PartidaManagerScript>();
        if (partidaManagerScript.guanyador == 1)
        {
            jugador1Script = FindObjectOfType<Jugador1Script>();
            jugador2Script = FindObjectOfType<Jugador2Script>();
            idJugadorGuanyador = jugador1Script.idUsuari;
            idJugadorPerdedor = jugador2Script.idUsuari;
            text.text = jugador1Script.nom;
            Debug.LogError(jugador1Script.nom);
            
        }
        if (partidaManagerScript.guanyador == 2)
        {
            jugador1Script = FindObjectOfType<Jugador1Script>();
            jugador2Script = FindObjectOfType<Jugador2Script>();
            idJugadorGuanyador = jugador2Script.idUsuari;
            idJugadorPerdedor = jugador1Script.idUsuari;
            text.text = jugador2Script.nom;
            Debug.LogError(jugador2Script.nom);
            
        }
        IniciarEnviament();
        Destroy(partidaManagerScript);
    }

    public void IniciarEnviament()
    {
        StartCoroutine(EnviarTotaPartida());
    }

    IEnumerator EnviarTotaPartida()
    {
        yield return StartCoroutine(GetNextPartidaId());
        yield return StartCoroutine(PostPartida());
        yield return StartCoroutine(PostNumeroBombes());
        yield return StartCoroutine(PostDistancia());
        yield return StartCoroutine(PostPowerups());
        yield return StartCoroutine(EnviarPUTRequests());
    }
    IEnumerator GetNextPartidaId()
    {
        UnityWebRequest request = UnityWebRequest.Get(endpointSeguentPartida);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error GET: " + request.error);
        }
        else
        {
            string idStr = request.downloadHandler.text;
            if (int.TryParse(idStr, out nextPartidaId))
            {
                Debug.Log("Proper ID de partida: " + nextPartidaId);
            }
            else
            {
                Debug.LogError("No s'ha pogut convertir l'ID rebut: " + idStr);
            }
        }
    }
    IEnumerator PostPartida()
    {
        ParamsPersonatges paramsPersonatges = FindObjectOfType<ParamsPersonatges>();
        PartidaData partidaData = new PartidaData();
        partidaData.jugador1 = jugador1Script.idUsuari;
        partidaData.jugador2 = jugador2Script.idUsuari;
        partidaData.duracio = partidaManagerScript.duracio;

        if (partidaManagerScript.guanyador == 1)
        {
            Personatge personatgeGuanyador = (Personatge)paramsPersonatges.personatges[jugador1Script.eleccio];
            Personatge personatgePerdedor = (Personatge)paramsPersonatges.personatges[jugador2Script.eleccio];
            partidaData.idGuanyador = jugador1Script.idUsuari;
            partidaData.idPersonatgeGuanyador = personatgeGuanyador.idPersonatge;
            partidaData.idPersonatgePerdedor = personatgePerdedor.idPersonatge;
        }
        else if (partidaManagerScript.guanyador == 2)
        {
            Personatge personatgeGuanyador = (Personatge)paramsPersonatges.personatges[jugador2Script.eleccio];
            Personatge personatgePerdedor = (Personatge)paramsPersonatges.personatges[jugador1Script.eleccio];
            partidaData.idGuanyador = jugador2Script.idUsuari;
            partidaData.idPersonatgeGuanyador = personatgeGuanyador.idPersonatge;
            partidaData.idPersonatgePerdedor = personatgePerdedor.idPersonatge;
        }

        string jsonData = JsonUtility.ToJson(partidaData);
        Debug.Log("Dades JSON: " + jsonData);

        UnityWebRequest request = new UnityWebRequest(endpointPartida, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error POST: " + request.error);
        }
        else
        {
            Debug.Log("POST correcte: " + request.downloadHandler.text);
        }
    }

    IEnumerator PostNumeroBombes()
    {
        NumeroBombesData bombesData = new NumeroBombesData { idPartida = nextPartidaId, numeroBombes = partidaManagerScript.comptadorBombes };
        string jsonData = JsonUtility.ToJson(bombesData);
        yield return StartCoroutine(PostRequest(endpointBombes, jsonData));
    }

    IEnumerator PostDistancia()
    {
        DistanciaData distanciaData = new DistanciaData { idPartida = nextPartidaId, distancia = partidaManagerScript.comptadorDistancia };
        string jsonData = JsonUtility.ToJson(distanciaData);
        yield return StartCoroutine(PostRequest(endpointDistancia, jsonData));
    }

    IEnumerator PostPowerups()
    {
        PowerupsData powerupsData = new PowerupsData { idPartida = nextPartidaId, powerups = partidaManagerScript.comptadorPowerUps };
        string jsonData = JsonUtility.ToJson(powerupsData);
        yield return StartCoroutine(PostRequest(endpointPowerups, jsonData));
    }

    IEnumerator PostRequest(string url, string jsonData)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error POST a {url}: {request.error}");
        }
        else
        {
            Debug.Log($"POST correcte a {url}: {request.downloadHandler.text}");
        }
    }

    IEnumerator EnviarPUTRequests()
    {

        yield return StartCoroutine(PutRequest(endpointGuanyar, "{\"idJugador\":" + idJugadorGuanyador + "}"));
        
        yield return StartCoroutine(PutRequest(endpointPerdre, "{\"idJugador\":" + idJugadorPerdedor + "}"));
        
        yield return StartCoroutine(PutRequest(endpointKill, "{\"idJugador\":" + idJugadorGuanyador + "}"));
        
        yield return StartCoroutine(PutRequest(endpointMort, "{\"idJugador\":" + idJugadorPerdedor + "}"));
    }

    IEnumerator PutRequest(string url, string jsonData)
    {
        UnityWebRequest request = UnityWebRequest.Put(url, jsonData);
        request.SetRequestHeader("Content-Type", "application/json");
        
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error PUT a " + url + ": " + request.error);
        }
        else
        {
            Debug.Log("PUT correcte a " + url + ": " + request.downloadHandler.text);
        }
    }

    [System.Serializable]
    public class NumeroBombesData
    {
        public int idPartida;
        public int numeroBombes;
    }

    [System.Serializable]
    public class DistanciaData
    {
        public int idPartida;
        public int distancia;
    }

    [System.Serializable]
    public class PowerupsData
    {
        public int idPartida;
        public int powerups;
    }
    public class PartidaData
    {
        public int jugador1;
        public int jugador2;
        public int duracio;
        public int idGuanyador;
        public int idPersonatgeGuanyador;
        public int idPersonatgePerdedor;
    }
}
