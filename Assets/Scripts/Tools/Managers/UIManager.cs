using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject blackScreen;

    public void Update()
    {

    }

    public void fadeToBlack(bool fade) {
        StartCoroutine(FadeBlack(fade));
    }

    public IEnumerator FadeBlack (bool fadeToBlack = true, int fadeSpeed = 5) {
        Color objectColor = blackScreen.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack) {
            while (blackScreen.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackScreen.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        } else {
            while (blackScreen.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackScreen.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
    }

}
