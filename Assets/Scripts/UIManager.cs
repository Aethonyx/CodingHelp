using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// If you know what SerializeField does, nice, if not it is a better way to show your
    /// variables in the inspector without unnecessarily opening it up to modification.
    /// It is best practice to only expose the variables/methods you need to and to keep
    /// everything else private. Use this for stuff you want to show in the inspector, but
    /// you don't want other scripts to be able to accidentally modify. These are our UI
    /// elements. The toggle box and the Image is a placeholder for whatever you have set
    /// up to hold your controls. 
    /// </summary>
    [SerializeField] private Toggle _toggleJoystickPosition;
    [SerializeField] private Image _imageDummyControls;
    
    /// <summary>
    /// Singleton Instance used for interacting with the UI Manager
    /// </summary>
    public static UIManager Instance { get; private set; }

    /// <summary>
    /// Here we are subscribing and unsubscribing to the events provided by the toggle box.
    /// If you know what => means nice, if not, you can use it if your method is only going
    /// to do one thing. Since OnEnable only contains one instruction, we can cut out some
    /// space by shrinking down out method call.
    /// </summary>
    private void OnEnable() => _toggleJoystickPosition.onValueChanged.AddListener(ToggleChanged);
    private void OnDisable() => _toggleJoystickPosition.onValueChanged.RemoveListener(ToggleChanged);
    /// <summary>
    /// Setting up the singleton so it doesn't destroy itself. I have attached it to the Canvas root so that no matter
    /// what scene we change to, the UI and thus UI Logic follows us too.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Here we send the settings singleton our original x-axis position so that we are able to mirror it.
    /// We can't do it in Awake because the Settings singleton also sets itself up in awake, so if we were
    /// to try and access it, we would get a null reference error.
    /// </summary>
    private void Start() => SettingsManager.Instance.SetOriginalPositionX(_imageDummyControls.rectTransform.anchoredPosition.x);
    /// <summary>
    /// This is the UI logic that handles moving the UI control to where it needs to be based on the toggle state.
    /// </summary>
    /// <param name="toggle">Toggle box state</param>
    private void ToggleJoystickPosition(bool toggle)
    {
        var values = SettingsManager.Instance.ToggleJoystickPosition(toggle);
        var imageRectTransform = _imageDummyControls.rectTransform;
        imageRectTransform.anchorMax = values.anchorMax;
        imageRectTransform.anchorMin = values.anchorMin;
        imageRectTransform.pivot = values.pivot;
        imageRectTransform.anchoredPosition = new Vector2(SettingsManager.Instance.GetJoystickPosition(toggle), imageRectTransform.anchoredPosition.y);
    }
    /// <summary>
    /// This is the method we subscribed to with the toggle box event handler. This will be called everytime the
    /// value of the toggle box changes, and passes it to our UI methods to handle.
    /// </summary>
    /// <param name="toggle">Toggle box state</param>
    private void ToggleChanged(bool toggle)
    {
        ToggleJoystickPosition(toggle);
    }
    

}
