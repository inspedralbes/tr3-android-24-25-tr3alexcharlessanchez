using UnityEngine;

public class ImatgeScript : MonoBehaviour
{
    public Vector2 targetSize = new Vector2(2000f, 1592f);
    public float duration = 3f;

    private Vector2 initialSize;
    private float elapsedTime = 0f; 

    void Start()
    {
        initialSize = GetComponent<RectTransform>().sizeDelta;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime <= duration)
        {
            float t = elapsedTime / duration;
            Vector2 newSize = Vector2.Lerp(initialSize, targetSize, t);
            GetComponent<RectTransform>().sizeDelta = newSize;
        }
    }
}
