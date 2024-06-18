using System.Collections;
using System.Linq;
using UnityEngine;

public class FishingController : MonoBehaviour
{
    public Vector2Int fishingPoint;
    public float fishingDuration = 5.0f;
    public float catchProbability = 0.5f;
    public float bobberSinkTime = 2.0f;
    public float flySpeed = 2.0f;
    public AnimationCurve flyCurve;

    public GameObject bobber;
    public bool isFishing = false;
    public bool fishOnHook = false;

    private Vector3 startPoint;

    void Start()
    {
        bobber.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isFishing)
        {
            fishingPoint = Utils.GetMousePoisionInt();
            StartCoroutine(MoveBobber());
        }

        if (fishOnHook && Input.GetKeyDown(KeyCode.Space))
        {
            CatchFish();
        }

        if (!fishOnHook && Input.GetKeyDown(KeyCode.Space))
        {
            EndFishing();
        }
    }

    private IEnumerator Fish()
    {
        bobber.SetActive(true);
        isFishing = true;
        Debug.Log("Fishing started...");

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
        bobber.transform.position = new Vector3(startPoint.x, startPoint.y, 1);
        bobber.SetActive(true);
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
        Debug.Log("Fish on the hook!");
        fishOnHook = true;
        bobber.SetActive(false);

        yield return new WaitForSeconds(bobberSinkTime);

        if (fishOnHook)
        {
            Debug.Log("Fish got away!");
            EndFishing();
            StartCoroutine(Fish());
        }
    }

    private void CatchFish()
    {
        Debug.Log("Fish caught!");
        EndFishing();
    }

    private void EndFishing()
    {
        isFishing = false;
        fishOnHook = false;
        bobber.SetActive(false);
    }
}
