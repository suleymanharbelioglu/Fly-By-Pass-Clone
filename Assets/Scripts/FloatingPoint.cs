using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPoint : MonoBehaviour
{
    public float speed;
    private void Start() {
        Destroy(transform.gameObject,0.5f);
    }
    private void FixedUpdate() {
        transform.Translate((new Vector3(0f, 1f,1f)) * speed*Time.deltaTime);
    }
}
