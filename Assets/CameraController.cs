﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject referenceObject;

    [Header("Camera Settings")]
    public float maxDistance = 1.0f;
    public float minSize = 6.0f;
    public float maxSize = 8.0f;
    public float physicalMotionLerp = 0.05f;
    public float sizeMotionLerp = 0.05f;

    private Camera myCamera;
    // Start is called before the first frame update
    void Start()
    {
        if (referenceObject == null)
            Debug.LogWarning("CameraControllers has no reference object!");

        myCamera = GetComponentInChildren<Camera>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(Shake(1.0f, 1.0f));
        }
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

    public IEnumerator Shake(float magintude, float fixedTime = 1.0f)
    {
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
        yield return null;
    }
}
