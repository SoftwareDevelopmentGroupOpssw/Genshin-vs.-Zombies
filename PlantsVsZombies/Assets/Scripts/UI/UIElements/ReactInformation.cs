using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 反应文字的脚本
/// </summary>
public class ReactInformation : MonoBehaviour
{
    public Sprite Text;
    IEnumerator DisappearCoroutine()
    {
        float displayTime = 0.5f;
        yield return new WaitForSecondsRealtime(displayTime);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Text;
        float startY = transform.position.y;
        float speed = 2;//字移动的速度
        float upOffset = 0.5f;//字向上移动的距离
        for(float i = 0; i < 1; i+= Time.deltaTime * speed)
        {
            transform.position = new Vector2(transform.position.x, startY + i * upOffset);
            Color c = spriteRenderer.color;
            c .a = 1 - i;
            spriteRenderer.color = c;
            yield return 1;
        }
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisappearCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
