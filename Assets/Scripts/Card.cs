using UnityEngine;
using System.Collections;
using static WorldDomination.Troop;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace WorldDomination
{
#if ODIN_INSPECTOR
    [TypeInfoBox("There is no Info about this script")]
#endif

    [HelpURL("")]
    [CreateAssetMenu(fileName = "Card", menuName = "WorldDomination/Card", order = 1)]
    public class Card : ScriptableObject
    {
        #region Inspector

        public TroopType troopType;
        public Sprite cardSprite;

        #endregion //Inspector
    }
}
