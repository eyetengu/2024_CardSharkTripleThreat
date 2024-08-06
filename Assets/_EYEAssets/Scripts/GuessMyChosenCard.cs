using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessMyChosenCard : MonoBehaviour
{

    int _opponentID;
    int _playerID;

    [SerializeField] private Sprite[] _objectImages;

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
        
    }

    void Update()
    {
        if (ReadyForPlayerInput)
        {

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



    void OpponentChoosesCard()
    {
        _opponentID = Random.Range(0, _objectImages.Length - 1);

    }

    void PlayerChoosesCard()
    {
        
    }

    void CompareCardValues()
    {

    }

    void Win()
    {
        UI_Manager.Instance.DisplayPlayerMessage("YOU WIN");
    }

    void Lose()
    {
        UI_Manager.Instance.DisplayPlayerMessage("YOU LOSE");
    }


    IEnumerator RefreshAndReset()
    {

        yield return new WaitForSeconds(2.5f);


    }












}
