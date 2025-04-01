using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class LoginControllerScript : MonoBehaviour
{
    public TMP_InputField nom1;
    public TMP_InputField nom2;
    public TMP_InputField contrassenya1;
    public TMP_InputField contrassenya2;
    public Jugador1Script jugador1Script;
    public Jugador2Script jugador2Script;
    public TMP_Text resposta1;
    public TMP_Text resposta2;
    public JugarBotoScript botoJugar;
    public GameObject botoLogin1;
    public GameObject botoLogin2;
    public GameObject botoRegistre1;
    public GameObject botoRegistre2;
    private string apiUrl = "http://defcon1.dam.inspedralbes.cat:27891/jugador";

    void Start()
    {
        jugador1Script = FindObjectOfType<Jugador1Script>();
        jugador2Script = FindObjectOfType<Jugador2Script>();

    }
    public void loginJugador1()
    {
        string nom = nom1.text;
        string password = contrassenya1.text;

        if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(password))
        {
            resposta1.text = "Per favor, introdueix totes les dades.";
            return;
        }

        StartCoroutine(LoginRequest1(nom, password, jugador1Script, resposta1, botoLogin1, botoRegistre1, nom1, contrassenya1));
    }

    public void loginJugador2()
    {
        string nom = nom2.text;
        string password = contrassenya2.text;

        if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(password))
        {
            resposta1.text = "Per favor, introdueix totes les dades.";
            return;
        }

        StartCoroutine(LoginRequest2(nom, password, jugador2Script, resposta2, botoLogin2, botoRegistre2, nom2, contrassenya2));
    }

    public void registreJugador1()
    {
        string nom = nom1.text;
        string password = contrassenya1.text;

        if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(password))
        {
            resposta1.text = "Per favor, introdueix totes les dades.";
            return;
        }

        StartCoroutine(RegisterRequest1(nom, password, jugador1Script, resposta1, botoLogin1, botoRegistre1, nom1, contrassenya1));
    }

    public void registreJugador2()
    {
        string nom = nom2.text;
        string password = contrassenya2.text;

        if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(password))
        {
            resposta1.text = "Per favor, introdueix totes les dades.";
            return;
        }

        StartCoroutine(RegisterRequest2(nom, password, jugador2Script, resposta2, botoLogin2, botoRegistre2, nom2, contrassenya2));
    }

    private IEnumerator LoginRequest1(string nom, string password, Jugador1Script jugadorObject, TMP_Text respostaText, GameObject botoLogin, GameObject botoRegistre, TMP_InputField nomField, TMP_InputField contrassenyaField)
    {
        string jsonBody = $"{{\"nom\":\"{nom}\", \"password\":\"{password}\"}}";

        UnityWebRequest request = new UnityWebRequest(apiUrl, "GET");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            if (request.responseCode == 409)
            {
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                respostaText.text = $"Error: {errorResponse.error}";
            }
            else if (request.responseCode == 401)
            {
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                respostaText.text = $"Error: {errorResponse.error}";
                Debug.Log("Error 401: Nom o contrasenya incorrectes");
            }
            else
            {
                respostaText.text = "Error de connexió amb l'API.";
            }
        }
        else
        {

            if (string.IsNullOrEmpty(request.downloadHandler.text))
            {
                respostaText.text = "La resposta de l'API està buida.";
                yield break;
            }

            var jugadorResponse = JsonUtility.FromJson<JugadorResponse>(request.downloadHandler.text);

            if (jugadorResponse == null || jugadorResponse.idUsuari == 0)
            {
                respostaText.text = "No s'ha pogut obtenir la informació del jugador.";
                yield break;
            }

            if (jugador1Script != null)
            {
                jugador1Script.idUsuari = jugadorResponse.idUsuari;
                jugador1Script.nom = jugadorResponse.nom;
                jugador1Script.victories = jugadorResponse.victories;
                jugador1Script.derrotes = jugadorResponse.derrotes;
                jugador1Script.morts = jugadorResponse.morts;
                jugador1Script.kills = jugadorResponse.kills;
            }

            RectTransform rectTransform = respostaText.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 300);
            }


            respostaText.text = $"Benvingut/da, {jugador1Script.nom}!\n \n" +
                    $"Victòries: {jugador1Script.victories}\n" +
                    $"Derrotes: {jugador1Script.derrotes}\n" +
                    $"Morts: {jugador1Script.morts}\n" +
                    $"Kills: {jugador1Script.kills}";

            Destroy(nomField.gameObject);
            Destroy(contrassenyaField.gameObject);
            Destroy(botoLogin.gameObject);
            Destroy(botoRegistre.gameObject);

            if (botoJugar != null)
            {
                botoJugar.AugmentarValor();
            }
        }
    }
    private IEnumerator LoginRequest2(string nom, string password, Jugador2Script jugadorObject, TMP_Text respostaText, GameObject botoLogin, GameObject botoRegistre, TMP_InputField nomField, TMP_InputField contrassenyaField)
    {
        string jsonBody = $"{{\"nom\":\"{nom}\", \"password\":\"{password}\"}}";

        UnityWebRequest request = new UnityWebRequest(apiUrl, "GET");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            if (request.responseCode == 409)
            {
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                respostaText.text = $"Error: {errorResponse.error}";
            }
            else if (request.responseCode == 401)
            {
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                respostaText.text = $"Error: {errorResponse.error}";
                Debug.Log("Error 401: Nom o contrasenya incorrectes");
            }
            else
            {
                respostaText.text = "Error de connexió amb l'API.";
            }
        }
        else
        {

            if (string.IsNullOrEmpty(request.downloadHandler.text))
            {
                respostaText.text = "La resposta de l'API està buida.";
                yield break;
            }

            var jugadorResponse = JsonUtility.FromJson<JugadorResponse>(request.downloadHandler.text);

            if (jugadorResponse == null || jugadorResponse.idUsuari == 0)
            {
                respostaText.text = "No s'ha pogut obtenir la informació del jugador.";
                yield break;
            }

            if (jugador2Script != null)
            {
                jugador2Script.idUsuari = jugadorResponse.idUsuari;
                jugador2Script.nom = jugadorResponse.nom;
                jugador2Script.victories = jugadorResponse.victories;
                jugador2Script.derrotes = jugadorResponse.derrotes;
                jugador2Script.morts = jugadorResponse.morts;
                jugador2Script.kills = jugadorResponse.kills;
            }

            RectTransform rectTransform = respostaText.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 300);
            }


            respostaText.text = $"Benvingut/da, {jugador2Script.nom}!\n" +
                    $"Victòries: {jugador2Script.victories}\n" +
                    $"Derrotes: {jugador2Script.derrotes}\n" +
                    $"Morts: {jugador2Script.morts}\n" +
                    $"Kills: {jugador2Script.kills}";

            Destroy(nomField.gameObject);
            Destroy(contrassenyaField.gameObject);
            Destroy(botoLogin.gameObject);
            Destroy(botoRegistre.gameObject);

            if (botoJugar != null)
            {
                botoJugar.AugmentarValor();
            }
        }
    }

    private IEnumerator RegisterRequest1(string nom, string password, Jugador1Script jugador1Script, TMP_Text respostaText, GameObject botoLogin, GameObject botoRegistre, TMP_InputField nomField, TMP_InputField contrassenyaField)
    {
        string jsonBody = $"{{\"nom\":\"{nom}\", \"password\":\"{password}\"}}";

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            if (request.responseCode == 409)
            {
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                respostaText.text = $"Error: {errorResponse.error}";
            }
            else if (request.responseCode == 401)
            {
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                respostaText.text = $"Error: {errorResponse.error}";
                Debug.Log("Error 401: Nom o contrasenya incorrectes");
            }
            else
            {
                respostaText.text = "Error de connexió amb l'API.";
            }
        }
        else
        {

            if (string.IsNullOrEmpty(request.downloadHandler.text))
            {
                respostaText.text = "La resposta de l'API està buida.";
                yield break;
            }

            var jugadorResponse = JsonUtility.FromJson<JugadorResponse>(request.downloadHandler.text);

            if (jugadorResponse == null || jugadorResponse.idUsuari == 0)
            {
                respostaText.text = "No s'ha pogut obtenir la informació del jugador.";
                yield break;
            }

            if (jugador1Script != null)
            {
                jugador1Script.idUsuari = jugadorResponse.idUsuari;
                jugador1Script.nom = jugadorResponse.nom;
                jugador1Script.victories = jugadorResponse.victories;
                jugador1Script.derrotes = jugadorResponse.derrotes;
                jugador1Script.morts = jugadorResponse.morts;
                jugador1Script.kills = jugadorResponse.kills;
            }

            RectTransform rectTransform = respostaText.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 300);
            }

            respostaText.text = $"Benvingut/da, {jugador1Script.nom}!\n" +
                    $"Victòries: {jugador1Script.victories}\n" +
                    $"Derrotes: {jugador1Script.derrotes}\n" +
                    $"Morts: {jugador1Script.morts}\n" +
                    $"Kills: {jugador1Script.kills}";

            Destroy(nomField.gameObject);
            Destroy(contrassenyaField.gameObject);
            Destroy(botoLogin.gameObject);
            Destroy(botoRegistre.gameObject);

            if (botoJugar != null)
            {
                botoJugar.AugmentarValor();
            }
        }
    }
    private IEnumerator RegisterRequest2(string nom, string password, Jugador2Script jugador2Script, TMP_Text respostaText, GameObject botoLogin, GameObject botoRegistre, TMP_InputField nomField, TMP_InputField contrassenyaField)
    {
        string jsonBody = $"{{\"nom\":\"{nom}\", \"password\":\"{password}\"}}";

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            if (request.responseCode == 409)
            {
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                respostaText.text = $"Error: {errorResponse.error}";
            }
            else if (request.responseCode == 401)
            {
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                respostaText.text = $"Error: {errorResponse.error}";
                Debug.Log("Error 401: Nom o contrasenya incorrectes");
            }
            else
            {
                respostaText.text = "Error de connexió amb l'API.";
            }
        }
        else
        {

            if (string.IsNullOrEmpty(request.downloadHandler.text))
            {
                respostaText.text = "La resposta de l'API està buida.";
                yield break;
            }

            var jugadorResponse = JsonUtility.FromJson<JugadorResponse>(request.downloadHandler.text);

            if (jugadorResponse == null || jugadorResponse.idUsuari == 0)
            {
                respostaText.text = "No s'ha pogut obtenir la informació del jugador.";
                yield break;
            }

            if (jugador2Script != null)
            {
                jugador2Script.idUsuari = jugadorResponse.idUsuari;
                jugador2Script.nom = jugadorResponse.nom;
                jugador2Script.victories = jugadorResponse.victories;
                jugador2Script.derrotes = jugadorResponse.derrotes;
                jugador2Script.morts = jugadorResponse.morts;
                jugador2Script.kills = jugadorResponse.kills;
            }

            RectTransform rectTransform = respostaText.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 300);
            }

            respostaText.text = $"Benvingut/da, {jugador2Script.nom}!\n" +
                    $"Victòries: {jugador2Script.victories}\n" +
                    $"Derrotes: {jugador2Script.derrotes}\n" +
                    $"Morts: {jugador2Script.morts}\n" +
                    $"Kills: {jugador2Script.kills}";

            Destroy(nomField.gameObject);
            Destroy(contrassenyaField.gameObject);
            Destroy(botoLogin.gameObject);
            Destroy(botoRegistre.gameObject);

            if (botoJugar != null)
            {
                botoJugar.AugmentarValor();
            }
        }
    }

    [System.Serializable]
    private class JugadorResponse
    {
        public int victories;
        public int derrotes;
        public int morts;
        public int kills;
        public int idUsuari;
        public string nom;
    }

    [System.Serializable]
    private class ErrorResponse
    {
        public string error;
    }
}
