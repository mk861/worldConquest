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

        private void OnEnable()
        {
            UpdateUi();
        }

        private void Start()
        {
            //ERROR FOR WHAT?? 
            cardUiTemplate.SetActive(false);
        }

        #endregion //Unity Engine & Events

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
