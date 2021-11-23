using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeDestroy : MonoBehaviour
{

    private bool FadeIn;
    private bool FadeOut;
    public int fadeSpeed;
    Transform[] allChildren;

    // Start is called before the first frame update
    void Start()
    {
        allChildren = this.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // OLD:
        //if (FadeOut)
        //{
        //    Color objectColor = this.GetComponent<Renderer>().material.color;
        //    float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

        //    objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
        //    this.GetComponent<Renderer>().material.color = objectColor;

        //    if(objectColor.a <= 0)
        //    {
        //        FadeOut = false;
        //    }
        //}
        //if (FadeIn)
        //{
        //    Color objectColor = this.GetComponent<Renderer>().material.color;
        //    float fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

        //    objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
        //    this.GetComponent<Renderer>().material.color = objectColor;

        //    if(objectColor.a >= 1)
        //    {
        //        FadeIn = false;
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.A))
        {
            // OLD:
            StartCoroutine(FadeOutObject());
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            // OLD:
            StartCoroutine(FadeInObject());
        }
    }

    public IEnumerator FadeOutObject()
    {
        foreach (Transform child in allChildren)
        {
            Debug.Log("child.name: " + child.name);
            // OLD:
            // FadeOut = true;
            Color objectColor = child.gameObject.GetComponent<MeshRenderer>().material.color;

            // OLD:
            //float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
            //objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            Color fadedColor = new Color(objectColor.r, objectColor.g, objectColor.b, 0);

            Lerp_MeshRenderer_Color(child.GetComponent<MeshRenderer>(), 2.0f, objectColor, fadedColor);

            // OLD:
            // this.GetComponent<MeshRenderer>().material.color = objectColor;

            // OLD:
            //if (objectColor.a <= 0)
            //{
            //    FadeOut = false;
            //}
        }

        yield return null;
    }

    public IEnumerator FadeInObject()
    {
        
        //Transform[] allChildren = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {

            // OLD:
            // FadeIn = true;
            
            Color objectColor = child.gameObject.GetComponent<MeshRenderer>().material.color;

            // OLD:
            //float fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
            //objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);

            Color fadedColor = new Color(objectColor.r, objectColor.g, objectColor.b, 0);

            Lerp_MeshRenderer_Color(child.GetComponent<MeshRenderer>(), 2.0f, fadedColor, objectColor);

            // OLD:
            //this.GetComponent<MeshRenderer>().material.color = objectColor;

            // OLD:
            //if (objectColor.a >= 1)
            //{
            //    FadeIn = false;
            //}
        }

        yield return null;
    }
    private IEnumerator Lerp_MeshRenderer_Color(MeshRenderer target_MeshRender, float lerpDuration, Color startLerp, Color targetLerp)
    {
        float lerpStart_Time = Time.time;
        float lerpProgress;
        bool lerping = true;
        while (lerping)
        {
            yield return new WaitForEndOfFrame();
            lerpProgress = Time.time - lerpStart_Time;
            if (target_MeshRender != null)
            {
                target_MeshRender.material.color = Color.Lerp(startLerp, targetLerp, lerpProgress / lerpDuration);
            }
            else
            {
                lerping = false;
            }


            if (lerpProgress >= lerpDuration)
            {
                lerping = false;
            }
        }
        yield break;
    }
}
