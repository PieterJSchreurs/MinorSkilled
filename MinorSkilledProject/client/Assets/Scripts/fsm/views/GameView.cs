using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Wraps all elements and functionality required for the GameView.
 */
public class GameView : View
{
    [SerializeField] private GameBoard _gameboard = null;
    public GameBoard gameBoard => _gameboard;
    [SerializeField] private TMP_Text _player1Label = null;
    public TMP_Text playerLabel1 => _player1Label;
    [SerializeField] private TMP_Text _player2Label = null;
    public TMP_Text playerLabel2 => _player2Label;
    [SerializeField] private TMP_Text _gameMessageLabel = null;
    public TMP_Text gameMessageLabel1 => _gameMessageLabel;
    [SerializeField] private Button _exitGameButton = null;
    public Button exitGameButton1 => _exitGameButton;
    [SerializeField] private Button _exitGameButton2 = null;
    public Button exitGameButton2 => _exitGameButton2;
    [SerializeField] private GameObject _gameTicTactoe = null;
    public GameObject _gameTicTacToeObject => _gameTicTactoe;
    [SerializeField] private GameObject _gamePong = null;
    public GameObject _gamePongObject => _gamePong;
}

