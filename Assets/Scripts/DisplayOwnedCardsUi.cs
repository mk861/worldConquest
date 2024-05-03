using UnityEngine;
using System.Collections;
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
    public class DisplayOwnedCardsUi : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private GameObject cardUiTemplate;

        [SerializeField]
        private Transform container;

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

        #endregion

        #region Unity Engine & Events

        /// <summary>
        /// Calls UpdateUi method when this object class is enabled
        /// </summary>
        private void OnEnable()
        {
            UpdateUi();
        }

        /// <summary>
        /// Turns off cardUiTemplate when script is first loaded
        /// </summary>
        private void Start()
        {
            //ERROR FOR WHAT?? 
            cardUiTemplate.SetActive(false);
        }

        #endregion //Unity Engine & Events

        /// <summary>
        /// Displays current players cards, and clears previous entries
        /// </summary>
        private void UpdateUi()
        {
            ClearUi();

            List<Card> cards = PlayerManager.Instance.CurrentPlayer.Cards;
            if (cards == null || cards.Count == 0)
                return;

            for (int i = 0; i < cards.Count; i++)
            {
                GameObject cardObj = Instantiate(cardUiTemplate, container, false);
                cardObj.SetActive(true);
                CardUi cardUi = cardObj.GetComponent<CardUi>();
                cardUi.Card = cards[i];
            }
        }

        /// <summary>
        /// Clears the Ui of objects
        /// </summary>
        private void ClearUi()
        {
            //ERROR FOR WHAT???????
            for (int i = 0; i < container.childCount; i++)
            {
                if (container.GetChild(i).gameObject != cardUiTemplate.gameObject)
                    Destroy(container.GetChild(i).gameObject);
            }
        }
    }
}
