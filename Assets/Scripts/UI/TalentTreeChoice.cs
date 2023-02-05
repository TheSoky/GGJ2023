using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentTreeChoice : MonoBehaviour
{
    [SerializeField]
    private ScriptableReference _references;

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

    private Image _offenseButtonImage;
    private Image _defenseButtonImage;

    private void Awake()
    {
        _offenseButtonImage = offenseButton.GetComponent<Image>();
        _defenseButtonImage = defenseButton.GetComponent<Image>();
    }

    void Start()
    {
        offenseButton.onClick.AddListener(() =>
        {
            SpriteSwap(true);
        });
        defenseButton.onClick.AddListener(() =>
        {
            SpriteSwap(false);
        });

        //on click listener for next level button that calls function NextSprite
        nextLevelButton.onClick.AddListener(() =>
        {
            NextSprite();
        });
        //on click listener for next level button that calls function Selected
        nextLevelButton.onClick.AddListener(() =>
        {
            Selected();
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpriteSwap(bool isOffense)
    {
        if (isOffense)
        {
            _offenseButtonImage.sprite = markedCheckSprite;
            _defenseButtonImage.sprite = unMarkedCheckSprite;
            _references.PlayerData.IsOffenseChosen = false;
        }
        else
        {
            _defenseButtonImage.sprite = markedCheckSprite;
            _offenseButtonImage.sprite = unMarkedCheckSprite;
            _references.PlayerData.IsOffenseChosen = true;
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
        _offenseButtonImage.sprite = unMarkedCheckSprite;
        _defenseButtonImage.sprite = unMarkedCheckSprite;
    }

    private void Selected()
    {
        int offenseLevel = PlayerPrefs.GetInt("Offense");
        int defenseLevel = PlayerPrefs.GetInt("Defense");

        if (_references.PlayerData.IsOffenseChosen)
        {
            _references.PlayerData.OffenseLevel++;
        }
        else
        {
            _references.PlayerData.DefenseLevel++;
        }
    }
}
