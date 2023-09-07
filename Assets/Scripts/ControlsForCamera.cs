using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    // changes camera position to target position (player)
    void Update()
    {
        if(transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);

            targetPosition.x = Mathf.Clamp(target.position.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(target.position.y, minPosition.y, maxPosition.y);

            //transform.position = Vector3.Lerp(transform.position, target.position, smoothing);

            transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPosition.x, smoothing), Mathf.Lerp(transform.position.y, targetPosition.y, smoothing), -10);
        }
        
    }

}