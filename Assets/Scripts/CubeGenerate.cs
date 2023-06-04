using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerate : MonoBehaviour
{
    public static CubeGenerate instance{set; get;}
    public Vector3 position;
    public Transform cubeParent;
    
    private void Awake() {
        instance = this;
    }

    public GameObject cube ;

    public void Generate(Vector3 pos, Transform parent)
    {
      position = pos;
      cubeParent = parent;
       GameObject g = Instantiate(cube);
        g.SetActive(false);
        g.transform.SetParent(cubeParent);
        g.transform.localPosition = position;
        print("generated");
        StartCoroutine("GenerateCoroutine", g);
      
    }

    IEnumerator GenerateCoroutine(GameObject g)
    {
        print("start coroutine");
        yield return new WaitForSecondsRealtime(0.2f);
        
        g.SetActive(true);
        print("finish coroutine");
    }

    
    
}
