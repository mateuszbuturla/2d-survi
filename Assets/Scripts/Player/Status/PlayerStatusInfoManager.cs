using UnityEngine;

public class PlayerStatusInfoManager : MonoBehaviour
{
    public GameObject textPrefab;
    public Transform canvasTransform;
    public Vector3 offset;

    public void ShowStatusInfo(string message, Color color)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position) + offset;
        GameObject damageTextInstance = Instantiate(textPrefab, screenPosition, Quaternion.identity, canvasTransform);
        PlayerStatus playerStatus = damageTextInstance.GetComponent<PlayerStatus>();
        playerStatus.SetText(message, color);
    }
}
