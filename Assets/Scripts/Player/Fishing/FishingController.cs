using System.Collections;
using UnityEngine;

public class FishingController : MonoBehaviour
{
    public float fishingDuration = 5.0f;
    public float catchProbability = 0.5f;
    public float bobberSinkTime = 2.0f;

    public bool isFishing = false;
    public bool fishOnHook = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isFishing)
        {
            StartCoroutine(Fish());
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

    private IEnumerator BobberSink()
    {
        Debug.Log("Fish on the hook!");
        fishOnHook = true;

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
    }
}
