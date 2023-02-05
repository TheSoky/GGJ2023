using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndPanManager : MonoBehaviour
{
    [SerializeField]
    private Image beetRoot;

    [SerializeField]
    private Transform beetRootTransform;

    [SerializeField]
    private float _speed = 1.0f;

    [SerializeField]
    private float _border = -29.0f;

    [SerializeField]
    private GameObject beetRootPanel;

    [SerializeField]
    private GameObject EndPanel;

    private Coroutine myCoroutine;
    private bool finished = false;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("WaitForFunction", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(finished)
        {
            StopCoroutine(myCoroutine);
            finished = false;
            Invoke("SetEverything", 2f);

        }
    }

    private void WaitForFunction()
    {
        myCoroutine = StartCoroutine(TransformCoroutine());
    }

    private void SetEverything()
    {
        beetRootPanel.SetActive(false);
        EndPanel.SetActive(true);
        beetRoot.gameObject.SetActive(false);
    }

    IEnumerator TransformCoroutine(){
        while (beetRootTransform.position.y > _border)
        {
            beetRootTransform.position += Vector3.down * _speed * Time.deltaTime;
            yield return null;
        }
        finished = true;
    }
}
