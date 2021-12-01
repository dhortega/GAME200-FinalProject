using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeDisable : MonoBehaviour
{

    // Local sprite renderer that will be grabbed from the object that this is attached to
    SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeOut()
    {
        #region APPROACH FADING OUT MULTIPLE OBJECTS HERE INSTEAD OF FADESIGNALLER.CS
        //for (int i = 0; i < targetsToFade.Length; i++)
        //{
        //    Debug.Log("Entered FadeOut() coroutine");
        //    spriteRenderer = targetsToFade[i].GetComponent<SpriteRenderer>();

        //    float alphaValue = spriteRenderer.color.a;
        //    Color temp = spriteRenderer.color;

        //    while (spriteRenderer.color.a > 0)
        //    {
        //        alphaValue -= 0.2f;
        //        temp.a = alphaValue;
        //        spriteRenderer.color = temp;

        //        yield return new WaitForSeconds(0.05f);
        //    }

        //}
        #endregion

        float alphaValue = this.spriteRenderer.color.a;
        Color temp = this.spriteRenderer.color;

        while (spriteRenderer.color.a > 0)
        {
            alphaValue -= 0.2f;
            temp.a = alphaValue;
            spriteRenderer.color = temp;

            yield return new WaitForSeconds(0.05f);
        }
        this.gameObject.SetActive(false);
    }

    public IEnumerator FadeIn()
    {
        #region APPROACH FADING IN MULTIPLE OBJECTS HERE INSTEAD OF FADESIGNALLER.CS
        //for (int i = 0; i < targetsToFade.Length; i++)
        //{
        //    Debug.Log("Entered FadeOut() coroutine");
        //    spriteRenderer = targetsToFade[i].GetComponent<SpriteRenderer>();

        //    float alphaValue = spriteRenderer.color.a;
        //    Color temp = spriteRenderer.color;

        //    while (spriteRenderer.color.a < 1)
        //    {
        //        alphaValue += 0.01f;
        //        temp.a = alphaValue;
        //        spriteRenderer.color = temp;

        //        yield return new WaitForSeconds(0.05f);
        //    }

        //}
        #endregion

        float alphaValue = this.spriteRenderer.color.a;
        Color temp = this.spriteRenderer.color;

        while (spriteRenderer.color.a < 1)
        {
            alphaValue += 0.2f;
            temp.a = alphaValue;
            spriteRenderer.color = temp;

            yield return new WaitForSeconds(0.05f);
        }

    }

    #region LOCAL CALL TO FADEOUT HERE
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Entered Fade Collision");
    //    StartCoroutine(FadeOut());
    //}
    #endregion
}
