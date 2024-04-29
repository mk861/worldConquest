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

		private Button _Button;
		private Button Button
		{
			get
			{
				if (_Button == null) _Button = GetComponent<Button>();
				return _Button;
			}
			set
			{
				_Button = value;
			}
		}

		#region Unity Engine & Events

		private void OnEnable()
		{
			Button.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		private void OnDisable()
		{
			Button.onClick.RemoveListener(OnClick);
		}

		#endregion //Unity Engine & Events

	}
}
