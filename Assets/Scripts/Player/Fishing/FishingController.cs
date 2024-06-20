using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FishingController : MonoBehaviour
{
    public GameObject crate;

    [Header("Fishing Settings")]
    public Fish[] fishes;
    public Vector2Int fishingPoint;
    public float fishingDuration = 5.0f;
    public float catchProbability = 0.5f;
    public float bobberSinkTime = 2.0f;
    public float flySpeed = 2.0f;
    public float fishTravelSpeed = 2.0f;
    public AnimationCurve flyCurve;

    [Header("Fishing Objects")]
    public GameObject bobber;
    public GameObject fishingString;
    public GameObject fishingRod;
    public GameObject fishingReward;
    public Tilemap tilemap;
    public TileBase waterTile;

    [Header("Player Settings")]
    public PlayerStatusInfoManager playerStatusInfoManager;
    public Transform player;

    private Vector3 startPoint;
    private bool isFishing = false;
    private bool fishOnHook = false;

    void Start()
    {
        SetFishingActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleFishingInput();
        }
    }


    private void StartFishing()
    {
        fishingPoint = Utils.GetMousePoisionInt();
        fishingRod.GetComponent<Animator>().SetTrigger("Start");
        StopAllCoroutines();
        StartCoroutine(MoveBobber());
    }

    public void HandleFishingInput()
    {
        if (!isFishing)
        {
            Vector2Int mousePos = Utils.GetMousePoisionInt();

            TileBase tile = tilemap.GetTile((Vector3Int)mousePos);

            if (tile == waterTile)
            {
                StartFishing();
            }
        }
        else
        {
            StopAllCoroutines();

            if (fishOnHook)
            {
                StartCoroutine(MoveFishToPlayer());
            }
            else
            {
                EndFishing();
            }
        }
    }

    private IEnumerator Fish()
    {
        fishingString.GetComponent<FishingString>().isBobberInWater = true;
        SetFishingActive(true);

        yield return new WaitForSeconds(fishingDuration);

        if (Random.value < catchProbability)
        {
            StartCoroutine(BobberSink());
        }
        else
        {
            RetryFishing();
        }
    }

    private IEnumerator MoveBobber()
    {
        PrepareBobberForFlight();
        yield return FlyBobberToTarget();

        StartCoroutine(Fish());
    }

    private void PrepareBobberForFlight()
    {
        startPoint = transform.position;
        bobber.GetComponent<FishingBobber>().PrepareBobber(fishingPoint);
        bobber.transform.position = new Vector3(startPoint.x, startPoint.y, 1);
        SetFishingActive(true);
        fishingString.GetComponent<FishingString>().isBobberInWater = false;
    }

    private IEnumerator FlyBobberToTarget()
    {
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

        bobber.transform.position = targetPoint;
    }

    private IEnumerator BobberSink()
    {
        fishOnHook = true;
        bobber.GetComponent<FishingBobber>().FishOnHook();

        yield return new WaitForSeconds(bobberSinkTime);

        if (fishOnHook)
        {
            RetryFishing();
        }
    }

    private IEnumerator MoveFishToPlayer()
    {
        Fish fish = GetRandomFish();
        PrepareFishSprite(fish);

        fishingReward.SetActive(true);
        fishingReward.GetComponent<FishingReward>().ShowReward(fish.sprite);
        fishingString.GetComponent<FishingString>().isBobberInWater = false;
        Vector3 bobberPosition = bobber.transform.position;
        Vector3 playerPosition = player.position;
        float distance = Vector3.Distance(bobberPosition, playerPosition);
        float fishTravelDuration = distance / fishTravelSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < fishTravelDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fishTravelDuration;

            bobber.transform.position = Vector3.Lerp(bobberPosition, playerPosition, t);
            fishingReward.transform.position = Vector3.Lerp(bobberPosition, playerPosition, t);

            yield return null;
        }

        CatchFish(fish);
    }

    private Fish GetRandomFish()
    {
        int randomIndex = Random.Range(0, fishes.Length);
        return fishes[randomIndex];
    }

    private void PrepareFishSprite(Fish fish)
    {
        fishingString.GetComponent<FishingString>().isBobberInWater = false;
        fishingReward.SetActive(true);
        fishingReward.GetComponent<SpriteRenderer>().sprite = fish.sprite;
    }

    private void CatchFish(Fish fish)
    {
        Color color = GetColor(fish.fishRarity);
        playerStatusInfoManager.ShowStatusInfo(fish.name, color);

        player.GetComponent<Player>().playerInventory.AddItem(Instantiate(crate));

        EndFishing();
    }

    private Color GetColor(FishRarity fishRarity)
    {
        switch (fishRarity)
        {
            case FishRarity.COMMON: return Color.gray;
            case FishRarity.RARE: return Color.cyan;
            case FishRarity.EPIC: return Color.magenta;
            case FishRarity.LEGENDARY: return Color.yellow;
            default: return Color.gray;
        }
    }

    private void RetryFishing()
    {
        EndFishing();
        StartCoroutine(Fish());
    }

    private void EndFishing()
    {
        fishingReward.SetActive(false);
        SetFishingActive(false);
        fishOnHook = false;
        bobber.GetComponent<FishingBobber>().ResetBobber();
    }

    private void SetFishingActive(bool isActive)
    {
        bobber.SetActive(isActive);
        fishingString.SetActive(isActive);
        isFishing = isActive;
    }
}