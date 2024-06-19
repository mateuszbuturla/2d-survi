using System.Collections;
using System.Linq;
using UnityEngine;

public class FishingController : MonoBehaviour
{
    public Fish[] fishes;
    public Vector2Int fishingPoint;
    public float fishingDuration = 5.0f;
    public float catchProbability = 0.5f;
    public float bobberSinkTime = 2.0f;
    public float flySpeed = 2.0f;
    public AnimationCurve flyCurve;
    public GameObject bobber;
    public GameObject fishingString;
    public bool isFishing = false;
    public bool fishOnHook = false;

    private Vector3 startPoint;
    public PlayerStatusInfoManager playerStatusInfoManager;

    void Start()
    {
        bobber.SetActive(false);
        fishingString.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isFishing)
        {
            fishingPoint = Utils.GetMousePoisionInt();
            StopAllCoroutines();
            StartCoroutine(MoveBobber());
        }

        if (fishOnHook && Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            CatchFish();
        }

        if (!fishOnHook && Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            EndFishing();
        }
    }

    private IEnumerator Fish()
    {
        bobber.SetActive(true);
        fishingString.SetActive(true);
        fishingString.GetComponent<FishingString>().isBobberInWater = true;
        isFishing = true;

        yield return new WaitForSeconds(fishingDuration);

        if (Random.value < catchProbability)
        {
            StartCoroutine(BobberSink());
        }
        else
        {
            EndFishing();
            StartCoroutine(Fish());
        }
    }

    private IEnumerator MoveBobber()
    {
        startPoint = transform.position;
        bobber.GetComponent<FishingBobber>().PrepareBobber(fishingPoint);
        bobber.transform.position = new Vector3(startPoint.x, startPoint.y, 1);
        bobber.SetActive(true);
        fishingString.SetActive(true);
        fishingString.GetComponent<FishingString>().isBobberInWater = false;
        Vector3 targetPoint = new Vector3(fishingPoint.x, fishingPoint.y, startPoint.z);
        float distance = Vector3.Distance(startPoint, targetPoint);
        float bobberFlyDuration = distance / flySpeed;

        float elapsedTime = 0f;

        while (elapsedTime < bobberFlyDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / bobberFlyDuration;
            float height = flyCurve.Evaluate(t);

            bobber.transform.position = Vector3.Lerp(startPoint, targetPoint, t) + Vector3.up * height;

            yield return null;
        }

        bobber.transform.position = new Vector3(fishingPoint.x, fishingPoint.y, startPoint.z);

        StartCoroutine(Fish());
    }

    private IEnumerator BobberSink()
    {
        fishOnHook = true;
        bobber.GetComponent<FishingBobber>().FishOnHook();

        yield return new WaitForSeconds(bobberSinkTime);

        if (fishOnHook)
        {
            EndFishing();
            StartCoroutine(Fish());
        }
    }

    private Color GetColor(FishRarity fishRarity)
    {
        switch (fishRarity)
        {
            case FishRarity.COMMON:
                return Color.gray;
            case FishRarity.RARE:
                return Color.cyan;
            case FishRarity.EPIC:
                return Color.magenta;
            case FishRarity.LEGENDARY:
                return Color.yellow;
            default:
                return Color.gray;
        }
    }

    private void CatchFish()
    {
        int random = Random.Range(0, fishes.Length);
        Fish fish = fishes[random];
        Color color = GetColor(fish.fishRarity);

        playerStatusInfoManager.ShowStatusInfo(fish.name, color);

        EndFishing();
    }

    private void EndFishing()
    {
        fishingString.GetComponent<FishingString>().isBobberInWater = false;
        fishingString.SetActive(false);
        isFishing = false;
        fishOnHook = false;
        bobber.SetActive(false);
        bobber.GetComponent<FishingBobber>().ResetBobber();
    }
}
