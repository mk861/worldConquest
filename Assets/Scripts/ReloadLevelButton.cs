using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace MS.UI
{
#if ODIN_INSPECTOR
	[TypeInfoBox("Reload the current level")]
#endif

	[HelpURL("")]
	public class ReloadLevelButton : MonoBehaviour
	{
		#region Inspector

		[SerializeField]
		private bool useAddressable = true;

		#endregion //Inspector

		#region Unity Engine & Events

		/// <summary>
		/// Reloads the scene
		/// </summary>
		public void Reload()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		#endregion //Unity Engine & Events

	}
}
