using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace WorldDomination
{
#if ODIN_INSPECTOR
    [TypeInfoBox("There is no Info about this script")]
#endif

    [HelpURL("")]
    public class Troop : MonoBehaviour
    {
        #region Inspector

        public TroopType troopType = TroopType.Infantry;

        #endregion //Inspector

        public enum TroopType
        {
            Infantry = 1,
            Cavalry = 5,
            Artillery = 10
        }

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

        /// <summary>
        /// The player this troop is assigned to 
        /// </summary>
        public Player Owner { get; set; }

        /// <summary>
        /// The territory this troop is assigned
        /// </summary>
        public Territory TerritoryAssigned { get; set; }

        /// <summary>
        /// The troop is in combat with this territory
        /// </summary>
        public Territory AttackingTerritory { get; private set; }

        /// <summary>
        /// How many troop is this troop worth (cavalry-5, artillery-10)
        /// </summary>
        public int TroopValue
        {
            get
            {
                return (int)troopType;
            }
        }

        #endregion

        private Vector3 initialTerritoryPosition;
        private Vector3 clickOffset;

        #region Unity Engine & Events

        /// <summary>
        /// Sets the initial  territory position when this script is first loaded
        /// </summary>
        private void Start()
        {
            initialTerritoryPosition = Transform.position;
        }

        /// <summary>
        /// Moves the troop object to the position where the mouse is when clicked
        /// </summary>
        private void OnMouseDown()
        {
            clickOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        /// <summary>
        /// Troop follows position of the mouse when it's dragged
        /// </summary>
        public void OnMouseDrag()
        {
            if (!Owner.IsTurn)
                return;

            Vector3 newPosition = clickOffset + Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(newPosition.x, newPosition.y, 0);
        }

        /// <summary>
        /// Handles troop movements and initiates battles based on mouse release position
        /// </summary>
        public void OnMouseUp()
        {
            if (!Owner.IsTurn)
                return;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Collider2D[] colliders = Physics2D.OverlapPointAll(mousePosition);
            TerritoryMonoBehaviour territory = null;

            foreach (var collider in colliders)
            {
                territory = collider.GetComponent<TerritoryMonoBehaviour>();
                if (territory != null)
                    break;
            }

            if (territory == null)
            {
                ReturnToInitialPosition();
                Debug.Log("Troop dropped out of territory");
                return;
            }

            if (!TerritoryManager.Instance.AreTerritoriesAdjacent(territory.territory, TerritoryAssigned))
            {
                Debug.Log("You can only move troops to adjacent territories");
                ReturnToInitialPosition();
                return;
            }

            Player newTerritoryOwner = TerritoryManager.Instance.GetTerritoryOwner(territory.territory);
            // Territory is owned by another player
            if (newTerritoryOwner != null && newTerritoryOwner != Owner)
            {
                if (!BattleManager.Instance.IsCorrectBattleTerritory(territory.territory))
                {
                    Debug.Log("There is already a territory prepared for battle");
                    ReturnToInitialPosition();
                    return;
                }

                if (BattleManager.Instance.TryAddAttacker(this))
                {
                    AttackingTerritory = territory.territory;
                    BattleManager.Instance.SetBattleTerritories(TerritoryAssigned, territory.territory);
                    Transform.position = mousePosition;
                    Debug.Log(Owner.PlayerName + " is attacking " + newTerritoryOwner.PlayerName);
                }
                else
                {
                    ReturnToInitialPosition();
                }
            }
            else
            {
                // Territory was not owned by another player, add ownership to the player that moved the troops into it
                TerritoryAssigned.Troops.Remove(this);
                TerritoryAssigned = territory.territory;
                TerritoryManager.Instance.AssignTerritory(territory.territory, Owner);
                initialTerritoryPosition = Transform.position;
            }
        }

        /// <summary>
        /// Returns the troops to their initial position
        /// </summary>
        public void ReturnToInitialPosition()
        {
            Transform.position = initialTerritoryPosition;
        }
        #endregion //Unity Engine & Events
    }
}
