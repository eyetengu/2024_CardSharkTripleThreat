using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class GuessMyChosenCard : MonoBehaviour
{
    [Header("COVER CARDS")]
    [SerializeField] private GameObject _coverCard_opponent;
    [SerializeField] private GameObject _coverCard_player;

    [Header("CARD FACE IMAGES")]
    [SerializeField] private Image _opponentCardImage;
    [SerializeField] private Image _playerCardImage;

    [Header("CARD IMAGE OPTIONS")]
    [SerializeField] private Sprite[] _objectImages;

    [Header("TAUNTS")]
    [SerializeField] private string[] _tauntsWin;
    [SerializeField] private string[] _tauntsLose;

    string _message;

    bool _isPlayersTurn;

    int _opponentID;
    int _playerID;


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
        BeginRound();
    }

    void Update()
    {
        if (ReadyForPlayerInput)
        {
            if (_isPlayersTurn)
            {
                UserInput();
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
        Debug.Log("Begin Round");
        _coverCard_opponent.SetActive(true);
        _coverCard_player.SetActive(true);

        OpponentChoosesCard();
    }

    void OpponentChoosesCard()
    {
        Debug.Log("Opponent Choosint");
        DisallowPlayerInput();

        _opponentID = Random.Range(0, _objectImages.Length - 1);

        _opponentCardImage.sprite = _objectImages[_opponentID];
        AllowPlayerInput();
        _isPlayersTurn = true;
        _coverCard_player.SetActive(false);
    }

    void UserInput()
    {
        Debug.Log("User Input");
        if (Input.GetKeyDown(KeyCode.W))
            _playerID++;
        else if (Input.GetKeyDown(KeyCode.S))
            _playerID--;

        if (_playerID > _objectImages.Length - 1)
            _playerID = 0;
        else if (_playerID < 0)
            _playerID = _objectImages.Length - 1;

        _playerCardImage.sprite = _objectImages[_playerID];

        if (Input.GetKeyDown(KeyCode.Space))
            PlayerChoosesCard();
    }

    void PlayerChoosesCard()
    {
        Debug.Log("Player Choosing");
        _isPlayersTurn = false;

        //_playerCardImage.sprite = _objectImages[_playerID];
    
        CompareCardValues();
    }

    void CompareCardValues()
    {
        Debug.Log("Comparing Values");
        _coverCard_opponent.SetActive(false);

        if (_playerID == _opponentID)
            Win();
        else
            Lose();

        StartCoroutine(RefreshAndReset());
    }

    void Win()
    {
        _message = _tauntsWin[Random.Range(0, _tauntsWin.Length-1)];
        UI_Manager.Instance.DisplayPlayerMessage(_message);
    }

    void Lose()
    {
        _message = _tauntsLose[Random.Range(0, _tauntsLose.Length - 1)];
        UI_Manager.Instance.DisplayPlayerMessage(_message);
    }


    IEnumerator RefreshAndReset()
    {
        Debug.Log("Refreshing");
        yield return new WaitForSeconds(2.5f);

        BeginRound();
    }












}
