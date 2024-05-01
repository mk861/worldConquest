using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace WorldDomination
{
    [HelpURL("")]
    public class CardUi : MonoBehaviour, IPointerDownHandler
    {
        #region Inspector

        [SerializeField]
        private Image cardImage;

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

        private Card _Card;
        public Card Card
        {
            get
            {
                return _Card;
            }
            set
            {
                cardImage.sprite = value.cardSprite;
                _Card = value;
            }
        }

        #endregion

        #region Unity Engine & Events

        public void OnPointerDown(PointerEventData eventData)
        {
            string message = "Do you want to use the card " + Card.name + "? You will obtain " + (int)Card.troopType + " troops";
            PromptPopup.Instance.ShowMessage(message, "Yes", "No", () =>
            {
                // If prompt is confirmed add the amount of troops to the player and remove the card
                PlayerManager.Instance.CurrentPlayer.AddTroops((int)Card.troopType);
                PlayerManager.Instance.CurrentPlayer.Cards.Remove(Card);
                gameObject.SetActive(false);
            }, null);
        }

        #endregion //Unity Engine & Events

    }
}
