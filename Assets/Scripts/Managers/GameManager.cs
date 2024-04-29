using UnityEngine;
using System.Collections;
using System;
using TMPro;



#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace WorldDomination
{
#if ODIN_INSPECTOR
    [TypeInfoBox("There is no Info about this script")]
#endif

    [HelpURL("")]
    public class GameManager : Singleton<GameManager>
    {
        public class OnGameWonEventArgs : System.EventArgs
        {
            public Player winningPlayer;
        }
        private OnGameWonEventArgs onGameWonEventArgs = new OnGameWonEventArgs();
        public static event System.EventHandler<OnGameWonEventArgs> OnGameWon;

        public class OnTurnEndEventArgs : System.EventArgs
        {
            public Player lastTurnPlayer;
        }
        private OnTurnEndEventArgs onTurnEndEventArgs = new OnTurnEndEventArgs();
        public static event System.EventHandler<OnTurnEndEventArgs> OnTurnEnd;

        #region Inspector

        [SerializeField]
        private GameObject playersSelectionUi;

        [SerializeField]
        private GameObject turnUi;

        [SerializeField]
        private GameObject endGameUi;

        [SerializeField]
        private TMP_Text currentTurnText;

        #endregion //Inspector

        public enum State
        {
            Idle,
            PlacingStartingTroops,
            PlacingExtraTroops,
            MoveTroops
        }

        #region Properties

        public State GameState { get; private set; }

        public int CurrentTurnValue { get; private set; } // The current turn value

        private Transform _Transform;
        public Transform Transform
        {
            get
            {
                if (_Transform == null) _Transform = GetComponent<Transform>();
                return _Transform;
            }
            set
            {
                _Transform = value;
            }
        }

        #endregion

        #region Unity Engine & Events

        private void OnEnable()
        {
            PlayerSelectorUI.OnPlayersConfirmed += OnPlayersConfirmed;
            BattleManager.OnBattleCompleted += OnBattleCompleted;
        }

        private void OnBattleCompleted(object sender, BattleManager.OnBattleCompletedEventArgs e)
        {
            Player winningPlayer = TerritoryManager.Instance.GetPlayerOwningAllTerritories();
            if (winningPlayer == null)
                return;

            endGameUi.SetActive(true);
            // Game won
            onGameWonEventArgs.winningPlayer = winningPlayer;
            OnGameWon?.Invoke(this, onGameWonEventArgs);
        }

        private void OnPlayersConfirmed(object sender, PlayerSelectorUI.OnPlayersConfirmedEventArgs e)
        {
            playersSelectionUi.SetActive(false);
            GameState = State.PlacingStartingTroops;
            CurrentTurnValue = 1;
            PlayerManager.Instance.StartFirstTurn();
            UpdateTurnText();
        }

        private void OnDisable()
        {
            PlayerSelectorUI.OnPlayersConfirmed -= OnPlayersConfirmed;
            BattleManager.OnBattleCompleted -= OnBattleCompleted;
        }

        private void Start()
        {
            GameState = State.Idle;
        }

        #endregion //Unity Engine & Events

        public void GoNextTurn()
        {
            if (GameState == State.PlacingStartingTroops)
            {
                if (TerritoryManager.Instance.AreAllTerritoriesOccupied())
                {
                    GameState = State.MoveTroops;
                    turnUi.SetActive(true);
                }
            }
            PlayerManager.Instance.GoNextTurn();
            CurrentTurnValue++;
            UpdateTurnText();
        }

        public void EndTurn()
        {
            onTurnEndEventArgs.lastTurnPlayer = PlayerManager.Instance.playerList[PlayerManager.Instance.CurrentPlayerTurnIndex];
            OnTurnEnd?.Invoke(this, onTurnEndEventArgs);
        }

        private void UpdateTurnText()
        {
            currentTurnText.text = PlayerManager.Instance.playerList[PlayerManager.Instance.CurrentPlayerTurnIndex].PlayerName + "'s Turn";
        }

        public void PlaceExtraTroops()
        {
            if (GameState == State.PlacingStartingTroops)
                return;

            GameState = State.PlacingExtraTroops;
        }

        public void MoveTroops()
        {
            if (GameState == State.PlacingStartingTroops)
                return;

            GameState = State.MoveTroops;
        }
    }
}
