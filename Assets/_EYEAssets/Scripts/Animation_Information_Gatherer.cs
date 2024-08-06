using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Information_Gatherer : MonoBehaviour
{
    Animation_Manager _animManager;

    [Header("ANIMATORS")]
    [SerializeField] Animator _tabledCardsAnimator;
    [SerializeField] Animator _cardsInHandAnimator;
    Animator _playerAnimator;

    [Header("PlayerGrips")]
    [SerializeField] GameObject _gripR;
    [SerializeField] GameObject _gripL;

    [Header("DECK OF CARDS")]
    [SerializeField] GameObject _deckOnTable;


    //BUILT-IN FUNCTIONS
    void OnEnable()
    {
        _playerAnimator = GetComponentInChildren<Animator>();
        _animManager = FindObjectOfType<Animation_Manager>();

        SendAnimationInformation();
    }

    //CORE FUNCTIONS
    void SendAnimationInformation()
    {
        _animManager.ReceiveInformationFromSource(_playerAnimator, _tabledCardsAnimator, _cardsInHandAnimator, _gripR, _gripL, _deckOnTable);
    }
}
