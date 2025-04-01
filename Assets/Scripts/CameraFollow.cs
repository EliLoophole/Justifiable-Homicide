using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float followIntensity = 0.1f;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            AdjustPosition();
        }
    }

    private void AdjustPosition()
    {
        Vector3 targetPos = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
        transform.position = Vector3.Lerp(transform.position,targetPos,followIntensity);
    }
}
