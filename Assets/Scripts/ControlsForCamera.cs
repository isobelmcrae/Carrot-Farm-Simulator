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
    void LateUpdate()
    {
        
        
        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, -10f);

        targetPosition.x = Mathf.Clamp(target.position.x, minPosition.x, maxPosition.x);
        targetPosition.y = Mathf.Clamp(target.position.y, minPosition.y, maxPosition.y);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
    }

}