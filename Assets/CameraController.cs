using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Singleton

    public static CameraController instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Debug.LogError("Second Instance of GameManager was created, this instance was destroyed.");
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    #endregion

    public GameObject referenceObject;

    [Header("Camera Settings")]
    public float maxDistance = 1.0f;
    public float minSize = 6.0f;
    public float maxSize = 8.0f;
    public float physicalMotionLerp = 0.05f;
    public float sizeMotionLerp = 0.05f;

    private Camera myCamera;
    private bool IsShaking = false;
    // Start is called before the first frame update
    void Start()
    {
        if (referenceObject == null)
            Debug.LogWarning("CameraControllers has no reference object!");

        myCamera = GetComponentInChildren<Camera>();
    }
    private void Update()
    {
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (referenceObject != null)
        {
            Vector3 targetPosition = referenceObject.transform.position;
            targetPosition.z = -10.0f;
            float distance = Vector3.Distance(transform.position, targetPosition);

            float scale = distance / maxDistance;
            
            transform.position = Vector3.Lerp(transform.position, targetPosition, physicalMotionLerp * scale);

            float targetSize = scale * (maxSize - minSize) + minSize;

            myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, targetSize, sizeMotionLerp * scale);
        }
    }

    public void StartShake(float magintude, float fixedTime = 1.0f)
    {
        StartCoroutine(Shake(magintude, fixedTime));
    }

    private IEnumerator Shake(float magintude, float fixedTime = 1.0f)
    {
        if (IsShaking)
            yield return null;

        IsShaking = true;

        float time = 0.0f;
        Vector3 childPosition = myCamera.transform.localPosition;
        do
        {
            //Apply shake
            Vector3 shake = new Vector3(Random.Range(-255, 256), Random.Range(-255, 256), 0.0f).normalized;
            myCamera.transform.localPosition = childPosition + shake * magintude;
            magintude -= (magintude * Time.deltaTime)/fixedTime;

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        } while (time < fixedTime);

        myCamera.transform.localPosition = childPosition;
        IsShaking = false;
        yield return null;
    }
}
