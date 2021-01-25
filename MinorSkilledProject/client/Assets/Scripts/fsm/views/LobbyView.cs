using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * Wraps all elements and functionality required for the LobbyView.
 */
public class LobbyView : View
{
    //all components that need to be hooked up
    [SerializeField] private TMP_Text _textHeading = null;
    [SerializeField] private InputField _inputFieldChat = null;
    [SerializeField] private Text _textOutput = null;
    [SerializeField] private ScrollRect _scrollRect = null;
    [SerializeField] private Toggle _toggleReady = null;
    [SerializeField] public Dropdown _gameSelecter = null;

    private bool _focusedRequested = false;     //weird unity stuff as usual 
    private bool _scrollRequested = false;      //weird unity stuff as usual 

    //the events that needs to be monitored.
    public event Action<string> OnChatTextEntered = delegate { };
    public event Action<bool> OnReadyToggleClicked = delegate { };
    public event Action<Dropdown> OnDropDownValueChanged = delegate { };

    private void Start()
    {
        //setup chat input listener to trigger on enter
        _inputFieldChat.onEndEdit.AddListener(
            (value) => {
                    if (Input.GetKeyDown(KeyCode.Return))  OnChatTextEntered(value);
                }
         );

        //setup 
        _toggleReady.onValueChanged.AddListener((value) => OnReadyToggleClicked(value));

        //Setup change listener on dropdown box.
        _gameSelecter.options.Clear();
        _gameSelecter.onValueChanged.AddListener((value) => OnDropDownValueChanged(_gameSelecter));
        //clear title by default
        _textHeading.text = "";
    }

    private void Update()
    {
        checkFocus();
    }

    /// <summary>
    /// 
    /// </summary>
    private void checkFocus()
    {
        if (_focusedRequested)
        {
            _inputFieldChat.ActivateInputField();
            _inputFieldChat.Select();
            _focusedRequested = false;
        }

        if (_scrollRequested)
        {
            _scrollRect.verticalNormalizedPosition = 0;
            _scrollRequested = false;
        }
    }

    /// <summary>
    /// Sets the title of the lobby.
    /// </summary>
    /// <param name="pHeading"></param>
    public void SetLobbyHeading (string pHeading)
    {
        _textHeading.text = pHeading;
    }

    /// <summary>
    /// Adds a string to the chatbox.
    /// </summary>
    /// <param name="pOutput"></param>
    public void AddOutput(string pOutput)
    {
        _textOutput.text += pOutput + "\n";
        _scrollRequested = true;
    }

    /// <summary>
    /// Clears the chatbox.
    /// </summary>
    public void ClearOutput()
    {
        _textOutput.text = "";
        _scrollRequested = true;
    }

    /// <summary>
    /// Clears the chatbox input.
    /// </summary>
    public void ClearInput()
    {
        _inputFieldChat.text = "";
        _focusedRequested = true;
    }

    /// <summary>
    /// Sets a ready toggle to on or off.
    /// </summary>
    /// <param name="pValue"></param>
    public void SetReadyToggle (bool pValue)
    {
        _toggleReady.SetIsOnWithoutNotify(pValue);
    }

    /// <summary>
    /// Adds a value to the dropdown box in the view.
    /// </summary>
    /// <param name="pGameName"></param>
    public void AddGameToDropdown(string pGameName)
    {
        _gameSelecter.options.Add(new Dropdown.OptionData() {text = pGameName});
    }

}
