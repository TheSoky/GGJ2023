using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;

    [SerializeField]
    private TextMeshProUGUI nameTextMesh;

    [SerializeField]
    private GameObject[] goCharacters;

    [SerializeField]
    private string[] dialogCharacters;

    [SerializeField]
    private string[] names;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Button prevButton;

    [SerializeField]
    private Button playButton;

    private int nameCnt = 0;
    private int dialogCnt = 0;
    private int characterCnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("WaitForFunction", 1f);
        nextButton.onClick.AddListener(() =>
        {
            NextDialog();
        });

        prevButton.onClick.AddListener(() =>
        {
            PrevDialog();
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void WaitForFunction()
    {
        nameTextMesh.text = names[nameCnt];
        textMesh.text = dialogCharacters[dialogCnt];
        goCharacters[characterCnt].SetActive(true);
        goCharacters[characterCnt + 2].SetActive(false);
        nameCnt++;
        dialogCnt++;
        characterCnt++;
    }

    private void NextDialog()
    {
        if (dialogCnt < dialogCharacters.Length)
        {
            if (dialogCnt == 1)
            {
                prevButton.gameObject.SetActive(true);
            }
            
            if (dialogCnt % 2 == 0)
            {
                goCharacters[characterCnt].SetActive(true);
                goCharacters[characterCnt + 2].SetActive(false);
                characterCnt++;
                goCharacters[characterCnt].SetActive(false);
                goCharacters[characterCnt + 2].SetActive(true);
            }
            else
            {
                goCharacters[characterCnt].SetActive(true);
                goCharacters[characterCnt + 2].SetActive(false);
                characterCnt--;
                goCharacters[characterCnt].SetActive(false);
                goCharacters[characterCnt + 2].SetActive(true);
            }
            var cnt = nameCnt % 2;
            nameTextMesh.text = names[cnt];
            textMesh.text = dialogCharacters[dialogCnt];
            nameCnt++;
            dialogCnt++;
        }
        
        if (dialogCnt == dialogCharacters.Length)
        {
            nextButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
        }
    }

    private void PrevDialog()
    {
        if (dialogCnt > 0)
        {
            nameCnt-=2;
            dialogCnt-=2;
            
            if (dialogCnt == 5)
            {
                nextButton.gameObject.SetActive(true);
                playButton.gameObject.SetActive(false);
            }
            
            if (dialogCnt % 2 == 0)
            {
                goCharacters[characterCnt].SetActive(true);
                goCharacters[characterCnt + 2].SetActive(false);
                characterCnt++;
                goCharacters[characterCnt].SetActive(false);
                goCharacters[characterCnt + 2].SetActive(true);
            }
            else
            {
                goCharacters[characterCnt].SetActive(true);
                goCharacters[characterCnt + 2].SetActive(false);
                characterCnt--;
                goCharacters[characterCnt].SetActive(false);
                goCharacters[characterCnt + 2].SetActive(true);
            }
            var cnt = nameCnt % 2;
            nameTextMesh.text = names[cnt];
            textMesh.text = dialogCharacters[dialogCnt];
            nameCnt++;
            dialogCnt++;
        }

        if (dialogCnt == 1)
        {
            prevButton.gameObject.SetActive(false);
        }
    }
}
