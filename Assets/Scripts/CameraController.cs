using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform charTrasform;
    public Vector3 targetPos;
    public Vector3 rotatePos;
    public float lerpSpeed;
    public float roateLerpSpeed;
    
    private void FixedUpdate() {
        if(GameManager.instance.finishCamMove)
        {
            FinishMovement();
        }
    }
    

    public void FinishMovement()
    {
        StartCoroutine("WaitCoroutine");
    }

    IEnumerator WaitCoroutine()
    {
        yield return new  WaitForSecondsRealtime(0.5f);
        targetPos = new Vector3( charTrasform.position.x, 40f, charTrasform.position.z-40f);
        transform.SetParent(null);
       transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed*Time.deltaTime);
       transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotatePos),roateLerpSpeed*Time.deltaTime);
    }
}
