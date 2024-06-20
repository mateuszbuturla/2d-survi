using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float moveSpeed = 30f;
    public float fadeSpeed = 0.75f;

    public TextMeshProUGUI textMesh;

    void Update()
    {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        Color textColor = textMesh.color;
        textColor.a -= fadeSpeed * Time.deltaTime;
        textMesh.color = textColor;

        if (textColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text, Color color)
    {
        textMesh.text = text;
        textMesh.color = color;
    }
}
