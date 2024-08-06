using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ro_Sham_Bo : MonoBehaviour
{
    [Header("ANIMATORS")]
    [SerializeField] Animator _tabledCardsAnimator;
    [SerializeField] Animator _cardsInHandAnimator;
    Animator _animator_Player;

    [Header("CARD SPECIFICS")]
    [SerializeField] private string[] _roShamBo;
    [SerializeField] private Sprite[] _itemImage;

    [Header("PLAYERS CARD")]
    [SerializeField] private TMP_Text _cardNamePlayer;
    [SerializeField] private Image _cardImagePlayer;

    [Header("OPPONENTS CARD")]
    [SerializeField] private TMP_Text _cardNameOpponent;
    [SerializeField] private Image _opponentMesh;
    [SerializeField] private GameObject _opponentCardCover;

    [Header("PLAYER MESSAGE")]
    [SerializeField] private TMP_Text _playerMessage;

    [Header("PLAYER GRIPS")]
    [SerializeField] GameObject _gripR;
    [SerializeField] GameObject _gripL;

    int _selectionID;
    int _opponentSelection;

    string _opponentCard;
    string _playerCard;

    bool _readyForPlayerInput;
    bool _dealerIsIdle;


    void Start()
    {
        _animator_Player = GetComponentInChildren<Animator>();

        _opponentCardCover.SetActive(true);
        _readyForPlayerInput = true;

        _gripL.SetActive(false);
        _gripR.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        AnimationAssess();

        if(_readyForPlayerInput)
        {
            _cardImagePlayer.sprite = _itemImage[_selectionID];

            UserInput();
            PlayerSelectsCard();
        }
    }

    void AnimationAssess()
    {
        if (_animator_Player.GetCurrentAnimatorStateInfo(0).IsName("DealerShuffle"))
        {
            _gripR.SetActive(true);
            _gripL.SetActive(true);

            _tabledCardsAnimator.SetTrigger("CollectCards"); 
        }

        if (_animator_Player.GetCurrentAnimatorStateInfo(0).IsName("DealerDeal"))
        {
            Debug.Log("Dealer is Dealing Cards");
            _tabledCardsAnimator.SetTrigger("DealCards");
        }


        if (_animator_Player.GetCurrentAnimatorStateInfo(0).IsName("DealerIdle"))
        {
            _gripR.SetActive(false);
            //_gripL.SetActive(false);

            _dealerIsIdle = true;
        }

        if (_dealerIsIdle)
        {
            if (!_animator_Player.GetCurrentAnimatorStateInfo(0).IsName("DealerIdle"))
            {
                _dealerIsIdle = false;

                var randomIdleAnim = Random.Range(0, 4);
                _animator_Player.SetInteger("IdleValue", randomIdleAnim);
            } 
        }

        if (_animator_Player.GetCurrentAnimatorStateInfo(0).IsName("DealerFan"))
        {
            _cardsInHandAnimator.SetBool("FanCards", true);
            StartCoroutine(ResetDealerFanTrigger());
        }
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
    }

    void PlayerSelectsCard()
    {
        _cardImagePlayer.sprite = _itemImage[_selectionID];
        _playerCard = _roShamBo[_selectionID];
        _cardNamePlayer.text = _playerCard;

        Debug.Log("Player: " + _playerCard);
        if (Input.GetKeyDown(KeyCode.Space))
        {            
            OpponentSelect();
            _readyForPlayerInput = false;
        }
    }

    void OpponentSelect()
    {
        _opponentSelection = Random.Range(0, 3);
        _opponentCard = _roShamBo[_opponentSelection];
        _cardNameOpponent.text = _opponentCard;

        Debug.Log("Opponent Card: " + _opponentCard);
        CompareOpponentsCard();
    }

    void CompareOpponentsCard()
    {
        _opponentCardCover.SetActive(false);
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

        _opponentMesh.sprite = _itemImage[_opponentSelection];
        _cardImagePlayer.sprite = _itemImage[_selectionID];

        StartCoroutine(DelayErasePlayerMessage());
    }

    void Win()
    {
        _playerMessage.text = "YOU WON";
        Debug.Log("Win");
    }

    void Lose()
    {
        _playerMessage.text = "YOU LOST";
        Debug.Log("Lose");
    }

    void Draw()
    {
        Debug.Log("Draw");
        _playerMessage.text = "DRAW";
    }

    //COROUTINES
    IEnumerator DelayErasePlayerMessage()
    {
        yield return new WaitForSeconds(2);

        _playerMessage.text = "";

        _opponentCardCover.SetActive(true);
        _readyForPlayerInput = true;
    }

    IEnumerator ResetDealerFanTrigger()
    {
        yield return new WaitForSeconds(2.5f);
        _cardsInHandAnimator.SetBool("FanCards", false);
    }
}
