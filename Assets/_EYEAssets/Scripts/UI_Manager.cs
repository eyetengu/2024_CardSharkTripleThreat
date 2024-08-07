using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] GameObject _startScreen;
    [SerializeField] GameObject _mainMenuScreen;

    [SerializeField] GameObject[] _gameScenes;
    [SerializeField] GameObject[] _cardDisplays;

    [SerializeField] private TMP_Text _playerMessage;


    private static UI_Manager _instance;
    public static UI_Manager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("UI_Manager Not Found");

            return _instance;
        }
    }

    //BUILT-IN Functions
    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _startScreen.SetActive(true);

        HideAllGameScenesAndCardDisplays();
        StartCoroutine(DelayHideStartScreen());
    }


    //GAME SCENES & CARD DISPLAYS
    public void ShowCurrentGameSceneAndCardDisplay(int value)
    {
        HideAllGameScenesAndCardDisplays();

        _gameScenes[value - 1].SetActive(true);
        _cardDisplays[value - 1].SetActive(true);
    }

    void HideAllGameScenesAndCardDisplays()
    {
        foreach (GameObject scene in _gameScenes)
            scene.SetActive(false);
        foreach(GameObject display in _cardDisplays)
            display.SetActive(false);
    }


    //PLAYER MESSAGES
    public void DisplayPlayerMessage(string message)
    {
        StopCoroutine(ResetPlayerMessage());
        _playerMessage.text = message;
        StartCoroutine(ResetPlayerMessage());
    }


    //COROUTINES
    IEnumerator DelayHideStartScreen()
    {
        yield return new WaitForSeconds(2.5f);
        _startScreen.SetActive(false);
        _mainMenuScreen.SetActive(true);
    }

    IEnumerator ResetPlayerMessage()
    {
        yield return new WaitForSeconds(3.0f);
        DisplayPlayerMessage("");
    }

}
