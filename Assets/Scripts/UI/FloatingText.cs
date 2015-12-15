using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

    private float speed;
    private Vector3 direction;
    private float fadeTime;
    	
	void Update ()
    {
        float translation = speed * Time.deltaTime;
        transform.Translate(direction * translation);
	}

    public void Initialize(float speed, Vector3 direct, float fadeTime)
    {
        this.speed = speed;
        this.fadeTime = fadeTime;
        direction = direct;

        StartCoroutine(Fadeout());
    }

    private IEnumerator Fadeout()
    {
        float startalpha = GetComponent<Text>().color.a;
        float rate = 1.0f / fadeTime;
        float progress = 0.0f;
        while (progress < 1.0f)
        {
            Color tmpColor = GetComponent<Text>().color;
            GetComponent<Text>().color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(startalpha, 0, progress));
            progress += rate * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
