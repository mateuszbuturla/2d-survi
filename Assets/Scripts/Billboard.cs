using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (cam != null)
        {
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                             cam.transform.rotation * Vector3.up);
        }
    }
}
