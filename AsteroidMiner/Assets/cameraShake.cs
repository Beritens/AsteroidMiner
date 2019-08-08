using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour
{
    public static cameraShake instance;
    void Start()
    {
        instance = this;
    }
    public void Shake(float duration, float magnitude){
        StartCoroutine(shake(duration,magnitude));
    }
    
    IEnumerator shake(float duration, float magnitude){
        Vector2 originalPos = transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed<duration)
        {
            elapsed += Time.deltaTime;
            float j = Mathf.Pow(0.9f,elapsed);
            float x = Random.Range(-1f,1f)*magnitude*j-transform.localPosition.x*0.1f*j;
            float y = Random.Range(-1f,1f)*magnitude*j-transform.localPosition.y*0.1f*j;
            transform.localPosition += new Vector3(x,y,0);
            
            yield return null;
        }
        transform.localPosition = Vector2.zero;

    }
}
