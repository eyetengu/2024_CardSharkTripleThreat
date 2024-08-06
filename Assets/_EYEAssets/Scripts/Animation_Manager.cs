using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Manager : MonoBehaviour
{
    //ANIMATORS
    Animator _animator_Player;
    Animator _tabledCardsAnimator;
    Animator _cardsInHandAnimator;

    //PLAYER GRIPS
    GameObject _gripR;
    GameObject _gripL;

    //CARD DECKS
    GameObject _deckOnTable;
    GameObject _deckInHand;

    //CONDITIONS
    bool _dealerIsIdle;
    bool _cardsDealt;

    //DELEGATES & EVENTS
    public delegate void AllowPlayerInput();
        public static event AllowPlayerInput playerInputAllowed;
    public delegate void DisallowPlayerInput();
        public static event DisallowPlayerInput playerInputDisallowed;


    private static Animation_Manager _instance;
    public static Animation_Manager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("No Animation_Manager Found");
            
            return _instance;
        }
    }

    //BUILT-IN FUNCTIONS
    void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        AnimationAssess();
    }


    //RECEIVE ANIMATION & GAME OBJECT INFORMATION
    public void ReceiveInformationFromSource(Animator player, Animator tableCards, Animator handCards, GameObject rGrip, GameObject lGrip, GameObject tableDeck)
    {
        _animator_Player = player;
        _tabledCardsAnimator = tableCards;
        _cardsInHandAnimator = handCards;
        _deckInHand = _cardsInHandAnimator.gameObject;

        _gripR = rGrip;
        _gripL = lGrip;

        _deckOnTable = tableDeck;
    }


    //ANIMATION PLAYING CHECKS
    void AnimationAssess()
    {
        //If Dealer Is Shuffling Cards
        if (_animator_Player.GetCurrentAnimatorStateInfo(0).IsName("DealerShuffle"))
        {
            _cardsDealt = false;

            _deckOnTable.SetActive(false);
            _deckInHand.SetActive(true);

            _gripR.SetActive(true);
            _gripL.SetActive(true);
            
            SetCollectCardsTrigger();
        }

        //If Dealer Is Dealing Cards
        if (_animator_Player.GetCurrentAnimatorStateInfo(0).IsName("DealerDeal"))
        {
            Debug.Log("Dealer is Dealing Cards");
            SetDealCardsTrigger();
        }

        //If Dealer Is Idle Set Idle Bool
        if (_animator_Player.GetCurrentAnimatorStateInfo(0).IsName("DealerIdle"))
        {
            _gripR.SetActive(false);
            //_gripL.SetActive(false);

            _dealerIsIdle = true;
        }

        //If Dealer Is Idle Select Random Idle Animation
        if (_dealerIsIdle)
        {
            if (!_animator_Player.GetCurrentAnimatorStateInfo(0).IsName("DealerIdle"))
            {
                _dealerIsIdle = false;

                var randomIdleAnim = Random.Range(0, 5);
                SelectRandomIdleAnimation(randomIdleAnim);
            }
        }

        //Idle Option- Fan Cards While Waiting
        if (_animator_Player.GetCurrentAnimatorStateInfo(0).IsName("DealerFan"))
        {
            SetFanCardsBool(true);
            StartCoroutine(ResetFanCardsBool());
        }

       
    }


    //TABLED CARDS ANIMATIONS
    void SetCollectCardsTrigger()
    {
        _tabledCardsAnimator.SetTrigger("ShuffleCards");
    }

    void SetDealCardsTrigger()
    {
        Debug.Log("Dealing Cards");
        _tabledCardsAnimator.SetTrigger("DealCards");
        _cardsDealt = true;
        playerInputAllowed?.Invoke();
    }


    //CARDS IN HAND ANIMATIONS
    void SetFanCardsBool(bool value)
    {
        _cardsInHandAnimator.SetBool("FanCards", value);
    }


    //PLAYER ANIMATIONS
    void SelectRandomIdleAnimation(int value)
    {
        _animator_Player.SetInteger("IdleValue", value);
    }

    public void SetShuffleTrigger()
    {
        _animator_Player.SetTrigger("ShuffleCards");
    }

    public void SetPlayerWinsRoundTrigger()
    {
        _animator_Player.SetTrigger("PlayerWinsRound");
    }


    //COROUTINES
    IEnumerator ResetFanCardsBool()
    {
        yield return new WaitForSeconds(2.5f);
        SetFanCardsBool(false);
    }
}
