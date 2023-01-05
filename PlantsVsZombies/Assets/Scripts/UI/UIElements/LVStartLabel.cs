using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVStartLabel : MonoBehaviour
{
    public Sprite label1;
    public Sprite label2;
    public Sprite label3;
    IEnumerator ShowCoroutine()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = label1;
        yield return new WaitForSecondsRealtime(0.3f);
        spriteRenderer.sprite = label2;
        yield return new WaitForSecondsRealtime(0.3f);
        spriteRenderer.sprite = label3;
        yield return new WaitForSecondsRealtime(1f);
        Destroy(gameObject);
    }
    private void Start()
    {
       // AudioManager.Instance.PlayEffectAudio("readysetplant");
        StartCoroutine(ShowCoroutine());
    }
}
