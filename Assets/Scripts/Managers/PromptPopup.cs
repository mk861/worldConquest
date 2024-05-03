using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
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
    public class PromptPopup : Singleton<PromptPopup>
    {
        #region Inspector

        [SerializeField]
        private GameObject popup;

        [SerializeField]
        private TMP_Text messageText;

        [SerializeField]
        private Button confirmButton;

        [SerializeField]
        private TMP_Text confirmButtonLabelText;

        [SerializeField]
        private Button cancelButton;

        [SerializeField]
        private TMP_Text cancelButtonLabelText;

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

        private Action confirmAction;
        private Action cancelAction;

        #region Unity Engine & Events



        #endregion //Unity Engine & Events

        /// <summary>
        /// Shows a specific message and sets up all buttons
        /// </summary>
        /// <param name="message"></param>
        /// <param name="confirmButtonLabel"></param>
        /// <param name="cancelButtonLabel"></param>
        /// <param name="confirmAction"></param>
        /// <param name="cancelAction"></param>
        public void ShowMessage(string message, string confirmButtonLabel, string cancelButtonLabel, Action confirmAction, Action cancelAction = null)
        {
            popup.transform.SetAsLastSibling();
            popup.SetActive(true);
            messageText.text = message;
            confirmButtonLabelText.text = confirmButtonLabel;
            cancelButtonLabelText.text = cancelButtonLabel;
            cancelButton.gameObject.SetActive(!string.IsNullOrEmpty(cancelButtonLabel));
            this.confirmAction = confirmAction;
            this.cancelAction = cancelAction;
        }

        /// <summary>
        /// Invokes the action when confirmed
        /// </summary>
        public void Confirm()
        {
            confirmAction?.Invoke();
        }

        /// <summary>
        /// Cancels the action
        /// </summary>
        public void Cancel()
        {
            cancelAction?.Invoke();
        }
    }
}
