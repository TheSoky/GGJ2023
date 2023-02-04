using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentTreeChoice : MonoBehaviour
{
    [SerializeField]
    private Image offenseImage;

    [SerializeField]
    private Image defenseImage;

    [SerializeField]
    private Button nextLevelButton;

    [SerializeField]
    private Button offenseButton;

    [SerializeField]
    private Button defenseButton;

    [SerializeField]
    private Sprite markedCheckSprite;

    [SerializeField]
    private Sprite unMarkedCheckSprite;

    [SerializeField]
    private Sprite[] offenseSprites;

    [SerializeField]
    private Sprite[] defenseSprites;


    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Offense", 0);
        PlayerPrefs.SetInt("Defense", 0);
        PlayerPrefs.SetInt("Selected", 0);

        offenseButton.onClick.AddListener(() =>
        {
            SpriteSwap(offenseButton.name);
        });
        defenseButton.onClick.AddListener(() =>
        {
            SpriteSwap(defenseButton.name);
        });

        //on click listener for next level button that calls function NextSprite
        nextLevelButton.onClick.AddListener(() =>
        {
            NextSprite();
        });
        //on click listener for next level button that calls funcstion Selected
        nextLevelButton.onClick.AddListener(() =>
        {
            Selected();
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpriteSwap(string name)
    {
        if (name == "OffenseButton")
        {
            offenseButton.GetComponent<Image>().sprite = markedCheckSprite;
            defenseButton.GetComponent<Image>().sprite = unMarkedCheckSprite;
            PlayerPrefs.SetInt("Selected", 1);
        }
        if (name == "DefenseButton")
        {
            defenseButton.GetComponent<Image>().sprite = markedCheckSprite;
            offenseButton.GetComponent<Image>().sprite = unMarkedCheckSprite;
            PlayerPrefs.SetInt("Selected", 2);
        }
    }

    private void NextSprite()
    {
        Sprite offenseSprite = offenseImage.sprite;
        for (int i = 0; i < offenseSprites.Length; i++)
        {
            if (offenseSprite == offenseSprites[i])
            {
                if (i == offenseSprites.Length - 1)
                {
                    offenseImage.sprite = offenseSprites[0];
                    defenseImage.sprite = defenseSprites[0];
                    break;
                }
                offenseImage.sprite = offenseSprites[i + 1];
                defenseImage.sprite = defenseSprites[i + 1];
                break;
            }
        }
        offenseButton.GetComponent<Image>().sprite = unMarkedCheckSprite;
        defenseButton.GetComponent<Image>().sprite = unMarkedCheckSprite;
    }

    private void Selected()
    {
        int offenseLevel = PlayerPrefs.GetInt("Offense");
        int defenseLevel = PlayerPrefs.GetInt("Defense");

        if (PlayerPrefs.GetInt("Selected")==1)
        {
            offenseLevel++;
            PlayerPrefs.SetInt("Offense", offenseLevel);
        }

        if (PlayerPrefs.GetInt("Selected") == 2)
        {
            defenseLevel++;
            PlayerPrefs.SetInt("Defense", defenseLevel);
        }
    }
}
