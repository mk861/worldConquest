using UnityEngine;
using System.Collections;
using System;
using TMPro;
using System.Linq;
using System.Collections.Generic;





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

        public static event Action<State> OnStateChanged;

        #region Inspector

        [SerializeField]
        private GameObject playersSelectionUi;

        [SerializeField]
        private GameObject turnUi;

        [SerializeField]
        private GameObject endGameUi;

        [SerializeField]
        private TMP_Text currentTurnText;

        [SerializeField]
        private TMP_Text timer;

        #endregion //Inspector

        public GameObject map;
        /*public GameObject playerNameSelector;*/

        public enum State
        {
            Idle,
            PlacingStartingTroops,
            PlacingExtraTroops,
            MoveTroops,
            Attack,
            Fortify,

        }

        #region Properties
        //public Timer timer;

        public State GameState;

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
        private void Start()
        {
            /*playerNameSelector.SetActive(false);*/
            map.SetActive(false);

            currentTurnText.text = "Waiting for players to be confirmed";
            Debug.Log("waiting for players to be chosen");
            GameState = State.Idle;
        }
        private void OnEnable()
        {
            PlayerSelectorUI.OnPlayersConfirmed += OnPlayersConfirmed;
            BattleManager.OnBattleCompleted += OnBattleCompleted;
        }
        private void OnDisable()
        {
            PlayerSelectorUI.OnPlayersConfirmed -= OnPlayersConfirmed;
            BattleManager.OnBattleCompleted -= OnBattleCompleted;
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
            setPlayerNames();
        }

        private void setPlayerNames()
        {
            playersSelectionUi.SetActive(true);



            playersSelectionUi.SetActive(false);

            map.SetActive(true);
            GameState = State.PlacingStartingTroops;
            CurrentTurnValue = 1;
            PlayerManager.Instance.StartFirstTurn();
            UpdateTurnText();
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

        public void Attack()
        {
            StartCoroutine(BattleManager.Instance.ExecuteBattle());
        }

        public void Fortify()
        {

        }

        //Call this, to make it most effective, create a UI script that toggles between different UI's when the OnStateChanged event is listened to.
        public void SetState(State newState)
        {
            if (GameState == newState)
                return;

            GameState = newState;
            OnStateChanged?.Invoke(GameState);
        }

        //Either automatically call this after the fortify action is finished or have it on a selectable button.
        public void EndTurn()
        {
            GoNextTurn();
        }

        private void UpdateTurnText()
        {
            currentTurnText.text = PlayerManager.Instance.playerList[PlayerManager.Instance.CurrentPlayerTurnIndex].PlayerName + "'s Turn";
        }




        [Button]
        public void DEBUG_SETPLAYERSPOSITIONS()
        {
            for (int i = 0; i < 99999; i++)
            {
                if (TerritoryManager.Instance.Territories.All(t => t.Owner))
                    break;

                for (int j = 0; j < PlayerManager.Instance.playerList.Count; j++)
                {
                    Player player = PlayerManager.Instance.playerList[j];
                    List<Territory> territoriesNotOwned = TerritoryManager.Instance.Territories.Where(t => t.Owner == null).ToList();
                    Territory t = territoriesNotOwned[UnityEngine.Random.Range(0, territoriesNotOwned.Count)];
                    player.PlacingStartingUnits(t, t.TerritoryMonoBehaviour.transform);
                }
            }
        }

    }
}








/* using UnityEngine;
using System.Collections;
using System;
using TMPro;
using System.Linq;
using System.Collections.Generic;





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

        public static event Action<State> OnStateChanged;

        #region Inspector

        [SerializeField]
        private GameObject playersSelectionUi;

        [SerializeField]
        private GameObject turnUi;

        [SerializeField]
        private GameObject endGameUi;

        [SerializeField]
        private TMP_Text currentTurnText;

        [SerializeField]
        private TMP_Text timer;

        #endregion //Inspector

        public enum State
        {
            Idle,
            PlacingStartingTroops,
            PlacingExtraTroops,
            MoveTroops,
            Attack,
            Fortify,

        }

        #region Properties
        //public Timer timer;

        public GameObject map;

        public State GameState;

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
        private void Start()
        {
            map.SetActive(false);
            currentTurnText.text = "Waiting for players to be confirmed";
            Debug.Log("waiting for players to be chosen");
            GameState = State.Idle;
        }
        private void OnEnable()
        {
            PlayerSelectorUI.OnPlayersConfirmed  += OnPlayersConfirmed;
            BattleManager.OnBattleCompleted += OnBattleCompleted;
        }
        private void OnDisable()
        {
            PlayerSelectorUI.OnPlayersConfirmed -= OnPlayersConfirmed;
            BattleManager.OnBattleCompleted -= OnBattleCompleted;
        }


        private void OnBattleCompleted(object sender, BattleManager.OnBattleCompletedEventArgs e)
        {
            Player winningPlayer = TerritoryManager.Instance.GetPlayerOwningAllTerritories();
            if (winningPlayer == null)
            {
                //added
                GameState = State.Fortify;
                return;
            }

            endGameUi.SetActive(true);
            // Game won
            onGameWonEventArgs.winningPlayer = winningPlayer;
            OnGameWon?.Invoke(this, onGameWonEventArgs);
        }

        private void OnPlayersConfirmed(object sender, PlayerSelectorUI.OnPlayersConfirmedEventArgs e)
        {
            playersSelectionUi.SetActive(false);
            map.SetActive(true);
            GameState = State.PlacingStartingTroops;
            CurrentTurnValue = 1;
            PlayerManager.Instance.StartFirstTurn();
            UpdateTurnText();
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

        public void Attack()
        {
            StartCoroutine(BattleManager.Instance.ExecuteBattle());
        }

        public void Fortify()
        {

        }


        //Either automatically call this after the fortify action is finished or have it on a selectable button.
        public void EndTurn()
        {
            if(BattleManager.Instance.IsStillInBattle())
            {
                Debug.Log("Still in battle mode!");
                return;
            }
            GoNextTurn();
        }

        private void UpdateTurnText()
        {
            currentTurnText.text = PlayerManager.Instance.playerList[PlayerManager.Instance.CurrentPlayerTurnIndex].PlayerName + "'s Turn";
        }

        //Call this, to make it most effective, create a UI script that toggles between different UI's when the OnStateChanged event is listened to.
        public void SetState(State newState)
        {
            if (GameState == newState)
                return;

            GameState = newState;
            OnStateChanged?.Invoke(GameState);
        }

    
        [Button]
        public void DEBUG_SETPLAYERSPOSITIONS()
        {
            for (int i = 0; i < 99999; i++)
            {
                if (TerritoryManager.Instance.Territories.All(t => t.Owner))
                    break;

                for (int j = 0; j < PlayerManager.Instance.playerList.Count; j++)
                {
                    Player player = PlayerManager.Instance.playerList[j];
                    List<Territory> territoriesNotOwned = TerritoryManager.Instance.Territories.Where(t => t.Owner == null).ToList();
                    Territory t = territoriesNotOwned[UnityEngine.Random.Range(0, territoriesNotOwned.Count)];
                    player.PlacingStartingUnits(t, t.TerritoryMonoBehaviour.transform);
                }
            }
        }

    }
}







*//* using UnityEngine;
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

        public void IgnorePlease()
{

}

        public void MoveTroops()
        {
            if (GameState == State.PlacingStartingTroops)
                return;

            GameState = State.MoveTroops;
        }
    }
}
*/
