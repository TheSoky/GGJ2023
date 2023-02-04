using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundChange : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;

    [SerializeField]
    private Image[] backgroundImages;
    
    [SerializeField]
    private float _speed = 1.0f;
    
    [SerializeField]
    private string[] _introTexts;

    [SerializeField]
    private Button nextButton;

    private Coroutine alphaCoroutine;
    private Coroutine betaCoroutine;

    private bool finished = false;
    private bool betaFinished = false;
    private int index = 0;
    private int indexStr = 0;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("WaitForFunction", 6f);
    }

    // Update is called once per frame
    void Update()
    {
        if (finished)
        {
            StopCoroutine(alphaCoroutine);
            finished = false;
            if (index < 5)
            {
                Invoke("WaitForFunction", 3f);
            }
        }
        if (betaFinished)
        {
            StopCoroutine(betaCoroutine);
            betaFinished = false;
        }
        if (index >= 5)
        {
            nextButton.gameObject.SetActive(true);
        }
    }
    
    private void WaitForFunction()
    {
        alphaCoroutine = StartCoroutine(ChangeAlpha(backgroundImages[index]));
        if(index == 0 || index == 1 || index == 3)
        {
            textMesh.text = _introTexts[indexStr];
            indexStr++;
        }
        if (index == 3)
        {
            betaCoroutine = StartCoroutine(DecreaseAlpha(backgroundImages[0]));
        }
        if (index == 4)
        {
            betaCoroutine = StartCoroutine(DecreaseAlpha(backgroundImages[1]));
            backgroundImages[2].gameObject.SetActive(false);
            backgroundImages[3].gameObject.SetActive(false);
        }
        index++;

    }

    IEnumerator ChangeAlpha(Image image)
    {
        while (image.color.a <= 1)
        {
            Color color = image.color;
            color.a += Time.deltaTime * _speed;
            image.color = color;
            yield return null;
        }
        finished = true;
    }

    IEnumerator DecreaseAlpha(Image image)
    {
        while (image.color.a >0 )
        {
            Color color = image.color;
            color.a -= Time.deltaTime * _speed;
            image.color = color;
            yield return null;
        }
        betaFinished = true;
    }
}
