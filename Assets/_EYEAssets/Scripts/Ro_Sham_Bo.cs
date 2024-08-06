using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ro_Sham_Bo : MonoBehaviour
{    
    [Header("CARD SPECIFICS")]
    [SerializeField] private string[] _roShamBo;
    [SerializeField] private Sprite[] _itemImage;

    [Header("PLAYERS CARD")]
    [SerializeField] private TMP_Text _cardNamePlayer;
    [SerializeField] private Image _cardImagePlayer;
    [SerializeField] private GameObject _cardCoverPlayer;

    [Header("OPPONENTS CARD")]
    [SerializeField] private TMP_Text _cardNameOpponent;
    [SerializeField] private Image _cardImageOpponent;
    [SerializeField] private GameObject _cardCoverOpponent;

    [Header("PLAYER GRIPS")]
    [SerializeField] GameObject _gripR;
    [SerializeField] GameObject _gripL;

    int _selectionID;
    int _opponentSelection;

    string _opponentCard;
    string _playerCard;

    bool _dealerIsIdle;
    bool _readyForPlayerInput;
    bool _isPlayersTurn;
    bool _opponentHasSelected;


    //PROPERTIES
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
        TurnCardsFaceDown();

        DisallowPlayerInput();

        _gripL.SetActive(false);
        _gripR.SetActive(false);
    }

    void Update()
    {
        if (ReadyForPlayerInput)
        {
            _cardImagePlayer.sprite = _itemImage[_selectionID];
            _cardCoverPlayer.SetActive(false);
            UserInput();
            if(_opponentHasSelected == false)
                OpponentSelectsCard();

        }
        if (_isPlayersTurn)
            UserInput();
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
    void TurnCardsFaceDown()
    {
        _cardCoverOpponent.SetActive(true);
        _cardCoverPlayer.SetActive(true);
    }

    void OpponentSelectsCard()
    {
        _opponentHasSelected = true;

        UI_Manager.Instance.DisplayPlayerMessage("W/S chooses, Space selects");
        Debug.Log("Displaying UI Message");
        _opponentSelection = Random.Range(0, 3);
        _opponentCard = _roShamBo[_opponentSelection];
        _cardNameOpponent.text = _opponentCard;
        
        Debug.Log("Opponent Card: " + _opponentCard);
        _isPlayersTurn = true;
        
    }

    void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _selectionID++;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _selectionID--;
        }

        if (_selectionID < 0)
            _selectionID = 2;

        else if (_selectionID > 2)
            _selectionID = 0;

        if (Input.GetKeyDown(KeyCode.Space))
            PlayerSelectsCard();
    }

    void PlayerSelectsCard()
    {
        _isPlayersTurn = false;
        DisallowPlayerInput();

        _cardImagePlayer.sprite = _itemImage[_selectionID];
        _playerCard = _roShamBo[_selectionID];
        _cardNamePlayer.text = _playerCard;

        Debug.Log("Player: " + _playerCard);                  
    
        CompareOpponentsCard();        
    }


    void CompareOpponentsCard()
    {
        _cardCoverOpponent.SetActive(false);

        switch (_opponentCard)
        {
            case "ROCK":
                if (_playerCard == "ROCK")
                    Draw();
                else if (_playerCard == "PAPER")
                    Win();
                else if (_playerCard == "SCISSORS")
                    Lose();
                break;
            case "PAPER":
                if (_playerCard == "ROCK")
                    Lose();
                else if (_playerCard == "PAPER")
                    Draw();
                else if (_playerCard == "SCISSORS")
                    Win();
                break;
            case "SCISSORS":
                if (_playerCard == "ROCK")
                    Win();
                else if (_playerCard == "PAPER")
                    Lose();
                else if (_playerCard == "SCISSORS")
                    Draw();
                break;

            default:
                break;
        }

        _cardImageOpponent.sprite = _itemImage[_opponentSelection];
        _cardImagePlayer.sprite = _itemImage[_selectionID];

        StartCoroutine(DelayErasePlayerMessage());
    }


    //WIN, LOSE, DRAW FUNCTIONS
    void Win()
    {
        UI_Manager.Instance.DisplayPlayerMessage("YOU WON");
        Debug.Log("Win");
        Animation_Manager.Instance.SetPlayerWinsRoundTrigger();
    }

    void Lose()
    {
        UI_Manager.Instance.DisplayPlayerMessage("YOU LOST");
        Debug.Log("Lose");
    }

    void Draw()
    {
        Debug.Log("Draw");
        UI_Manager.Instance.DisplayPlayerMessage("DRAW");
    }


    //COROUTINES
    IEnumerator DelayErasePlayerMessage()
    {
        yield return new WaitForSeconds(2);

        UI_Manager.Instance.DisplayPlayerMessage("");

        Animation_Manager.Instance.SetShuffleTrigger();

        _cardCoverOpponent.SetActive(true);
        _opponentHasSelected = false;
        ReadyForPlayerInput = true;
    }
}
