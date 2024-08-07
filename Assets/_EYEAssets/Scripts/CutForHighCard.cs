using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutForHighCard : MonoBehaviour
{
    [Header("CARD FEATURES")]
    [SerializeField] int[] _cardValues;
    [SerializeField] Sprite[] _cardSuit;

    [Header("CONTENDER CARDS")]
    [SerializeField] Image[] _opponentCardImage;
    [SerializeField] Image[] _playerCardImage;

    [Header("CARDS IN PLAY")]
    [SerializeField] GameObject _opponentCoverCard;
    [SerializeField] GameObject _playerCoverCard;

    [Header("CONTENDER CARD VALUES")]
    [SerializeField] TMP_Text _opponentCardValue;
    [SerializeField] TMP_Text _playerCardValue;

    Sprite _opponentSuit;
    Sprite _playerSuit;

    int _opponentValue;
    int _playerValue;

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
        _cardValues = new int[13] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

        BeginRound();
    }

    void Update()
    {
        if (ReadyForPlayerInput)
        {
            if (_isPlayersCut)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    PlayerCutsForHighCard();
            }
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

    void BeginRound()
    {
        //AllowPlayerInput();

        _opponentCoverCard.SetActive(true);
        _playerCoverCard.SetActive(true);
        
        StartCoroutine(OneSecondPause(2.0f));
    }


    //CORE FUNCTIONS
    void OpponentCutsForHighCard()
    {
        DisallowPlayerInput();

        _opponentValue = Random.Range(1, 14);
        var randomSuit = Random.Range(0, 4);

        _opponentCardValue.text = _opponentValue.ToString();
        _opponentSuit = _cardSuit[randomSuit];

        DisplayOpponentCard();
    }

    void DisplayOpponentCard()
    {
        _opponentCardImage[0].sprite = _opponentSuit;
        _opponentCardImage[1].sprite = _opponentSuit;        
        
        _opponentCoverCard.SetActive(false);

        UI_Manager.Instance.DisplayPlayerMessage("Player's Cut");
        
        AllowPlayerInput();
        _isPlayersCut = true;

        Debug.Log("Player Cut: " + _isPlayersCut);
    }

    void PlayerCutsForHighCard()
    {
        _playerValue = Random.Range(1, 14);
        int randomSuit = Random.Range(0, 4);

        _playerCardValue.text = _playerValue.ToString();
        _playerSuit = _cardSuit[randomSuit];

        DisplayPlayerCard();

        DisallowPlayerInput();
        _isPlayersCut = false;
    }
    
    void DisplayPlayerCard()
    {
        _playerCardImage[0].sprite = _playerSuit;
        _playerCardImage[1].sprite = _playerSuit;

        _playerCoverCard.SetActive(false);

        CompareBothCards();
    }

    void CompareBothCards()
    {
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
        UI_Manager.Instance.DisplayPlayerMessage($"GREAT. \n{_playerValue} Beats {_opponentValue}");
    }

    void Lose()
    {
        UI_Manager.Instance.DisplayPlayerMessage($"TOO BAD.\n{_playerValue} Is Less Than {_opponentValue}");
    }

    void Draw()
    {
        UI_Manager.Instance.DisplayPlayerMessage("IMPASSE. \nNeither Contender Has The High Card");
    }


    //COROUTINES
    IEnumerator RefreshAndReset()
    {
        yield return new WaitForSeconds(3.0f);
        
        UI_Manager.Instance.DisplayPlayerMessage("");

        BeginRound();
    }

    IEnumerator OneSecondPause(float value)
    {
        yield return new WaitForSeconds(value);
        OpponentCutsForHighCard();
    }
}
