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
            Debug.LogError("Second Instance of CameraController was created, this instance was destroyed.");
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

    [Header("Shake Settings")]
    public float minAngle = -3.0f;
    public float maxAngle = 3.0f;

    private Camera myCamera;
    private bool IsShaking = false;
    // Start is called before the first frame update
    void Start()
    {
        if (referenceObject == null)
            Debug.LogWarning("CameraControllers has no reference object!");

        myCamera = GetComponentInChildren<Camera>();
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
        if (!IsShaking && magintude > 0.0f)
        {
            IsShaking = true;

            float time = 0.0f;
            float initialMagnitude = magintude;
            Vector3 childPosition = myCamera.transform.localPosition;
            Vector3 shake = Vector3.zero;
            Vector3 shakeEuler = Vector3.zero;
            do
            {
                float magRatio = magintude / initialMagnitude;

                //Apply shake
                do
                {
                    shake += new Vector3(Random.Range(-255, 256), Random.Range(-255, 256), 0.0f);
                } while (shake == new Vector3(0, 0, 0));
                shake = shake.normalized;

                shakeEuler = new Vector3(0.0f, 0.0f, Random.Range(minAngle * magRatio, (maxAngle + 1.0f) * magRatio));

                myCamera.transform.localPosition = childPosition + shake * magintude;
                myCamera.transform.localRotation = Quaternion.Euler(shakeEuler);
                magintude -= (magintude * Time.deltaTime) / fixedTime;

                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            } while (time < fixedTime);

            myCamera.transform.localPosition = childPosition;
            myCamera.transform.localRotation = Quaternion.identity;
            IsShaking = false;
            yield return null;
        }
    }
}
