/*==============================================================================

==============================================================================*/

using UnityEngine;
using System.Collections;

[DefaultExecutionOrder(-30000)]
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Public Variables

    private static T _instance;

    /// <summary>
    /// Access the instance, if no instance is available one will be automatically be created
    /// </summary>
	public static T Instance
    {
        get
        {
            // Check if the instance is null.
            if (_instance == null)
            {
                // Couldn't find the singleton in the scene, so make it.
                GameObject singleton = new GameObject(typeof(T).Name);
                _instance = singleton.AddComponent<T>();
            }

            return _instance;
        }
    }

    /// <summary>
    /// Return the available instance, this property will not create an instance, only use it if you are sure the singleton is ready
    /// </summary>
    /// <remarks>Useful to call when accessing the Instance from OnDisable to avoid unity error when creating objects from closing the editor</remarks>
    public static T InstanceAvailable
    {
        get
        {
            return _instance;
        }
    }

    #endregion //Public Variables



    #region Private Variables



    #endregion //Private Variables



    #region Unity Engine & Events

    protected virtual void Awake()
    {
        CreateInstance();
    }

    #endregion //Unity Engine & Events

    private void CreateInstance()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T;
        }
    }

}
