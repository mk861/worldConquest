using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace WorldDomination
{
#if ODIN_INSPECTOR
    [TypeInfoBox("There is no Info about this script")]
#endif

    [HelpURL("")]
    public class BattleManager : Singleton<BattleManager>
    {
        public class OnBattleCompletedEventArgs : System.EventArgs
        {
            public Territory winningTerritory;
            public Territory losingTerritory;
        }
        private OnBattleCompletedEventArgs onBattleCompletedEventArgs = new OnBattleCompletedEventArgs();
        public static event System.EventHandler<OnBattleCompletedEventArgs> OnBattleCompleted;

        //added
        public class OnTerritoryConqueredEventArgs : System.EventArgs
        {
            public Territory conqueredTerritory;
            public Player conqueringPlayer;
        }
        private OnTerritoryConqueredEventArgs onTerritoryConqueredEventArgs = new OnTerritoryConqueredEventArgs();
        public static event System.EventHandler<OnTerritoryConqueredEventArgs> OnTerritoryConquered;

        #region Inspector

        [SerializeField]
        private float diceShowDuration = 2f;

        [SerializeField]
        private float battleEndDelay = 2f;

        [SerializeField]
        private Dice[] attackDice;

        [SerializeField]
        private Dice[] defendDice;

        #endregion //Inspector

        #region Properties

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

        public List<Troop> AttackingTroops { get; private set; }

        public List<Troop> DefendingTroops { get; private set; }

        public Territory AttackingTerritory { get; private set; }

        public Territory DefendingTerritory { get; private set; }

        #endregion

        #region Unity Engine & Events

        /// <summary>
        /// Resets state of game when script instance is loaded
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            ResetBattle();
        }

        #endregion //Unity Engine & Events

        /// <summary>
        /// Resets the current battle state
        /// </summary>
        public void ResetBattle()
        {
            AttackingTerritory = null;
            DefendingTerritory = null;
            AttackingTroops = new List<Troop>();
            DefendingTroops = new List<Troop>();
        }

        /// <summary>
        /// Tries to add an attacker to a territory, if there are less than 3 
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns> true if an attacker was added, false otherwise </returns>
        public bool TryAddAttacker(Troop attacker)
        {
            if (AttackingTroops.Contains(attacker))
            {
                Debug.Log("Can't add this attacker, it's already in the list");
                return false;
            }

            if (AttackingTroops.Count >= 3)
            {
                Debug.Log("Max number of attackers reached");
                return false;
            }

            AttackingTroops.Add(attacker);
            return true;
        }

        /// <summary>
        /// Sets the attacking and defending territories for upcoming battle
        /// </summary>
        /// <param name="attacking"></param>
        /// <param name="defending"></param>
        public void SetBattleTerritories(Territory attacking, Territory defending)
        {
            AttackingTerritory = attacking;
            DefendingTerritory = defending;
            DefendingTroops = new List<Troop>(DefendingTerritory.Troops);
            for (int i = 0; i < DefendingTroops.Count; i++)
            {
                DefendingTroops[i].Owner = TerritoryManager.Instance.GetTerritoryOwner(DefendingTerritory);
            }
        }

        /// <summary>
        /// Checks to see if given territory is the correct battle territory
        /// </summary>
        /// <param name="territory"></param>
        /// <returns> true if not correct territory, false otherwise </returns>
        public bool IsCorrectBattleTerritory(Territory territory)
        {
            return (DefendingTerritory == null) || string.IsNullOrEmpty(DefendingTerritory.TerritoryID) || territory == DefendingTerritory;
        }

        /// <summary>
        /// Becins battle couroutine
        /// </summary>
        /// <returns> Begins ExecuteBattle() </returns>
        public IEnumerator ExecuteBattle()
        {
            yield return StartCoroutine(BattleRoutine());
            GameManager.Instance.SetState(GameManager.State.Fortify);
        }

        /// <summary>
        /// Battle couroutine, rolls dice for attacker and defender, higer roll wins and troops are 
        /// removed of loser from the territory
        /// </summary>
        /// <returns> wait or null </returns>
        private IEnumerator BattleRoutine()
        {
            if (AttackingTroops.Count == 0 || DefendingTroops.Count == 0)
            {
                yield break;
            }

            List<int> attackerDiceRolls = new List<int>();
            List<int> defenderDiceRolls = new List<int>();

            int maxAttackDice = Mathf.Clamp(AttackingTroops.Count, 1, 3);
            int maxDefendDice = Mathf.Clamp(DefendingTroops.Count, 1, 2);

            // Roll dice
            for (int i = 0; i < maxAttackDice; i++)
            {
                attackDice[i].gameObject.SetActive(true);
                yield return null;
                yield return attackDice[i].RollTheDice(x => attackerDiceRolls.Add(x));
            }
            for (int i = 0; i < maxDefendDice; i++)
            {
                defendDice[i].gameObject.SetActive(true);
                yield return null;
                yield return defendDice[i].RollTheDice(x => defenderDiceRolls.Add(x));
            }

            // Sort dice rolls from highest to lowest
            attackerDiceRolls.Sort();
            defenderDiceRolls.Sort();
            // Reverse lists to get highest to lowest
            attackerDiceRolls.Reverse();
            defenderDiceRolls.Reverse();
            Debug.Log("Attacker highest: " + attackerDiceRolls[0] + " Defender highest: " + defenderDiceRolls[0]);

            yield return new WaitForSeconds(diceShowDuration);

            // Compare dice rolls
            if (attackerDiceRolls[0] > defenderDiceRolls[0])
            {
                GameObject troopObject = DefendingTroops[0].gameObject;
                // Attacker won
                DefendingTroops.RemoveAt(0);
                DefendingTerritory.RemoveTroops(1);
                Destroy(troopObject);
                // Remove a troop from the defending territory and player


                // No troop left in the territory, set owner to null
                //CHANGE THIS!!!!!!!!!!!!!!!!!!!!! TERRITORY OWNER CAN'T BE NULL
                if (DefendingTerritory.TroopsCount <= 0)
                {
                    // TerritoryManager.Instance.RemoveTerritory(DefendingTerritory);

                    // Territory conquered
                    onTerritoryConqueredEventArgs.conqueredTerritory = DefendingTerritory;
                    onTerritoryConqueredEventArgs.conqueringPlayer = AttackingTroops[0].Owner;
                    OnTerritoryConquered?.Invoke(this, onTerritoryConqueredEventArgs);
                    DefendingTerritory.AddTroops(AttackingTroops);
                }
                else
                {
                    // Return all troops to their original position
                    for (int i = 0; i < AttackingTroops.Count; i++)
                    {
                        AttackingTroops[i].ReturnToInitialPosition();
                    }
                }

                onBattleCompletedEventArgs.winningTerritory = AttackingTerritory;
                onBattleCompletedEventArgs.losingTerritory = DefendingTerritory;
                OnBattleCompleted?.Invoke(this, onBattleCompletedEventArgs);
            }
            else
            {
                // Defender won
                GameObject troopObject = AttackingTroops[0].gameObject;
                AttackingTroops.RemoveAt(0);
                AttackingTerritory.RemoveTroops(1);
                Destroy(troopObject);

                //AGAIN, TERRITORY OWNER CAN'T BE NULL
                if (AttackingTerritory.TroopsCount <= 0)
                {
                    TerritoryManager.Instance.RemoveTerritory(AttackingTerritory);
                }
                if (DefendingTerritory.TroopsCount <= 0)
                {
                    TerritoryManager.Instance.RemoveTerritory(DefendingTerritory);
                }

                onBattleCompletedEventArgs.winningTerritory = DefendingTerritory;
                onBattleCompletedEventArgs.losingTerritory = AttackingTerritory;
                OnBattleCompleted?.Invoke(this, onBattleCompletedEventArgs);

                // Return all troops to their original position
                for (int i = 0; i < AttackingTroops.Count; i++)
                {
                    AttackingTroops[i].ReturnToInitialPosition();
                }
            }

            yield return new WaitForSeconds(battleEndDelay);

            DisableDice();

            yield return new WaitForSeconds(battleEndDelay);

            ResetBattle();
        }

        /// <summary>
        /// Disables the dice
        /// </summary>
        private void DisableDice()
        {
            for (int i = 0; i < attackDice.Length; i++)
            {
                attackDice[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < defendDice.Length; i++)
            {
                defendDice[i].gameObject.SetActive(false);
            }
        }
    }
}