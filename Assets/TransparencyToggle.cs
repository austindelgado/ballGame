using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyToggle : MonoBehaviour
{
    public SpriteRenderer sprite;
    private bool visible = true;

    public void Toggle()
    {
        if (visible)
            StartCoroutine(FadeOut());
        else
            StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        float alphaVal = sprite.color.a;
         Color tmp = sprite.color;
 
         while (sprite.color.a < 1)
         {
             alphaVal += 0.01f;
             tmp.a = alphaVal;
             sprite.color = tmp;
 
             yield return new WaitForSeconds(0.05f); // update interval
         }
    }

    private IEnumerator FadeIn()
    {
         float alphaVal = sprite.color.a;
         Color tmp = sprite.color;
 
         while (sprite.color.a > 0)
         {
             alphaVal -= 0.01f;
             tmp.a = alphaVal;
             sprite.color = tmp;
 
             yield return new WaitForSeconds(0.05f); // update interval
         }
    }
}
