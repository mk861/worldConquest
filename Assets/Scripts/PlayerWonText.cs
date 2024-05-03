using UnityEngine;
using System.Collections;
using TMPro;
using System;



#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace WorldDomination
{
#if ODIN_INSPECTOR
    [TypeInfoBox("Show a text about the player who won the game.")]
#endif

    [HelpURL("")]
    public class PlayerWonText : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private string prefix = "Player ";

        [SerializeField]
        private string suffix = " won!";

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

        private TMP_Text _TMP_Text;
        private TMP_Text TMP_Text
        {
            get
            {
                if (_TMP_Text == null) _TMP_Text = GetComponent<TMP_Text>();
                return _TMP_Text;
            }
            set
            {
                _TMP_Text = value;
            }
        }

        #endregion

        #region Unity Engine & Events

        /// <summary>
        /// Creates OnGameWon when this object is enabled
        /// </summary>
        private void OnEnable()
        {
            GameManager.OnGameWon += OnGameWon;
        }

        /// <summary>
        /// Displays winning text when the game is won
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameWon(object sender, GameManager.OnGameWonEventArgs e)
        {
            TMP_Text.text = prefix + e.winningPlayer.PlayerName + suffix;
        }

        /// <summary>
        /// removes OnGameWon creation when MonoBehaviour is destroyed
        /// </summary>
        private void OnDisable()
        {
            GameManager.OnGameWon -= OnGameWon;
        }

        #endregion //Unity Engine & Events

    }
}
