using UnityEngine;
using System.Collections;

[DefaultExecutionOrder(-30000)]
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Singleton Management

    private static T _instance;

    /// <summary>
    /// Accesses the instance, if no instance is available one will be automatically be created
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
    /// Returns the available instance, this property will not create an instance and only use it if we are sure the singleton is ready
    /// </summary>
    /// <remarks> can be useful to call when accessing the Instance from OnDisable to avoid unity error when creating objects from closing the editor </remarks>
    public static T InstanceAvailable
    {
        get
        {
            return _instance;
        }
    }

    #endregion // Singleton Management


    #region Unity Specific Functions

    /// <summary>
    /// Instance is created when Instance of Singleton script is loaded
    /// </summary>
    protected virtual void Awake()
    {
        CreateInstance();
    }

    #endregion // Unity Specific Functions

    /// <summary>
    /// Creates an Instance if there isn't already one present, otherwise destroys it
    /// </summary>
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
