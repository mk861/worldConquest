using UnityEngine;
using System.Collections;
using UnityEngine.UI;
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
    public class CardsManager : Singleton<CardsManager>
    {
        #region Inspector

        [SerializeField]
        private Image cardUi;

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
        /// Creates instance when script is enabled
        /// </summary>
        private void OnEnable()
        {
            BattleManager.OnTerritoryConquered += OnTerritoryConquered;
        }

        /// <summary>
        /// Assigns a card when a territory is conquered and begins the animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTerritoryConquered(object sender, BattleManager.OnTerritoryConqueredEventArgs e)
        {
            Debug.Log("In cardmanager onterritoryconquered");
            Card card = e.conqueredTerritory.TerritoryMonoBehaviour.territoryCard;
            if (card == null)
            {
                Debug.Log("In cardmanager if");
                return;
            }

            // Animate the card popup and add the card to the player's hand
            StartCoroutine(AnimateCardPopUp(card));
            e.conqueringPlayer.Cards.Add(card);

            // The territory no longer have a card
            e.conqueredTerritory.TerritoryMonoBehaviour.territoryCard = null;
        }

        /// <summary>
        /// Removes instance when the script is disabled
        /// </summary>
        private void OnDisable()
        {
            BattleManager.OnTerritoryConquered -= OnTerritoryConquered;
        }

        #endregion //Unity Engine & Events

        /// <summary>
        /// Coroutine for animation of card popping up, ran once when player recieves a card
        /// </summary>
        /// <param name="card"></param>
        /// <returns> null </returns>
        public IEnumerator AnimateCardPopUp(Card card)
        {
            cardUi.gameObject.SetActive(true);
            cardUi.sprite = card.cardSprite;

            float lerp = 0f;
            while (lerp < 1f)
            {
                lerp += Time.deltaTime / 0.3f;
                cardUi.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, lerp);
                yield return null;
            }

            yield return new WaitForSeconds(2f);

            lerp = 0f;
            while (lerp < 1f)
            {
                lerp += Time.deltaTime / 0.3f;
                cardUi.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, lerp);
                yield return null;
            }

            cardUi.gameObject.SetActive(false);
        }
    }

}
