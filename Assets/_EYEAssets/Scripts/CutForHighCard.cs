using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutForHighCard : MonoBehaviour
{
    [Header("MANAGERS")]
    UI_Manager _uiManager;

    [Header("CARD FEATURES")]
    [SerializeField] int[] _cardValues;
    [SerializeField] Sprite[] _cardSuit;

    [Header("CONTENDER CARDS")]
    [SerializeField] Image[] _opponentCardImage;
    [SerializeField] Image[] _playerCardImage;

    [Header("CARDS IN PLAY")]
    [SerializeField] GameObject _opponentCoverCard;
    [SerializeField] GameObject _playerCoverCard;

    Sprite _opponentSuit;
    Sprite _playerSuit;

    int _opponentValue;
    int _playerValue;
    int _opponentCardValue;
    int _playerCardValue;

    bool _isPlayersCut;
    bool _opponentHasSelectedCard;


    //PROPERTIES
    bool _readyForPlayerInput;
    public bool ReadyForPlayerInput
    {
        get { return _readyForPlayerInput; }
        set { _readyForPlayerInput = value; }
    }


    //BUILT-IN FUNCTIONS
    private void OnEnable()
    {
        Animation_Manager.playerInputAllowed += AllowPlayerInput;
        Animation_Manager.playerInputDisallowed += DisallowPlayerInput;
    }

    private void OnDisable()
    {
        Animation_Manager.playerInputAllowed -= AllowPlayerInput;
        Animation_Manager.playerInputDisallowed -= DisallowPlayerInput;
    }

    void Start()
    {
        _uiManager = FindObjectOfType<UI_Manager>();

        _cardValues = new int[13] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

        _opponentCoverCard.SetActive(true);
        _playerCoverCard.SetActive(true);
        _readyForPlayerInput = true;

        OpponentCutsForHighCard();
    }

    void Update()
    {
        if (_readyForPlayerInput && _isPlayersCut)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                PlayerCutsForHighCard();
        }
    }


    //EVENT FUNCTIONS
    void AllowPlayerInput()
    {
        ReadyForPlayerInput = true;
    }

    void DisallowPlayerInput()
    {
        ReadyForPlayerInput = false;
    }


    //CORE FUNCTIONS
    void OpponentCutsForHighCard()
    {
        _readyForPlayerInput = false;
        var randomCard = Random.Range(0, 13);
        var randomSuit = Random.Range(0, 4);

        _opponentValue = _cardValues[randomCard];
        _opponentSuit = _cardSuit[randomSuit];

        DisplayOpponentCard();
    }

    void DisplayOpponentCard()
    {
        _opponentCardImage[0].sprite = _opponentSuit;
        _opponentCardImage[1].sprite = _opponentSuit;

        _opponentCardValue = _opponentValue;        

        _isPlayersCut = true;
        Debug.Log("Player Cut: " + _isPlayersCut);
    }

    void PlayerCutsForHighCard()
    {
        _isPlayersCut = false;

        int randomValue = Random.Range(0, 13);
        int randomSuit = Random.Range(0, 4);

        _playerValue = _cardValues[randomValue];
        _playerSuit = _cardSuit[randomSuit];

        DisplayPlayerCard();
    }
    
    void DisplayPlayerCard()
    {
        _playerCardImage[0].sprite = _playerSuit;
        _playerCardImage[1].sprite = _playerSuit;

        _playerCardValue = _playerValue;

        CompareBothCards();
    }

    void CompareBothCards()
    {
        _opponentCoverCard.SetActive(false);
        _playerCoverCard.SetActive(false);

        if (_playerValue > _opponentValue)
            Win();        
        else if (_playerValue == _opponentValue)
            Draw();
        else if (_playerValue < _opponentValue)
            Lose();

        StartCoroutine(RefreshAndReset());
    }


    //WIN, LOSE, DRAW FUNCTIONS
    void Win()
    {
        _uiManager.DisplayPlayerMessage("GREAT");
    }

    void Lose()
    {
        _uiManager.DisplayPlayerMessage("TOO BAD");
    }

    void Draw()
    {
        _uiManager.DisplayPlayerMessage("IMPASSE");
    }


    //COROUTINES
    IEnumerator RefreshAndReset()
    {
        yield return new WaitForSeconds(3.0f);
        
        _uiManager.DisplayPlayerMessage("");

        _opponentCoverCard.SetActive(true);
        _playerCoverCard.SetActive(true);

        OpponentCutsForHighCard();

    }
}
