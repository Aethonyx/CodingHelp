using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }
    
    /// <summary>
    /// These are Named Tuples, if you know what they are nice, if not then these are basically like a cheat code
    /// to be able to group multiple variables together in a neat package. You can use unnamed tuples if you'd like
    /// but I prefer to be able to get the variable I'm looking for by name, rather than Item1, Item2, etc. So with
    /// these I can just say _anchorLeft.anchorMin instead of _anchorLeft.Item1. These variables are the positions
    /// for anchoring a UI element to either the left or right side using the unity anchor system so that no matter
    /// the screen size, they are just anchored properly.
    /// </summary>
    private readonly (Vector2 anchorMin, Vector2 anchorMax) _anchorLeft = new (new Vector2(0, 0.5f), new Vector2(0f, 0.5f));
    private readonly (Vector2 anchorMin, Vector2 anchorMax) _anchorRight = new (new Vector2(1, 0.5f), new Vector2(1f, 0.5f));
    private readonly Vector2 _pivot = new (0.5f, 0.5f);

    /// <summary>
    /// These variables are for getting the anchored position on the x-axis. We have to invert the value when we anchor
    /// to the right so that it is mirrored (just the position)
    /// </summary>
    private float _joystickPosLeft;
    private float _joystickPosRight;
    
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
    /// Sets the original position on the x-axis so we can mirror control position.
    /// </summary>
    /// <param name="x">Position on the x-axis</param>
    public void SetOriginalPositionX(float x)
    {
        _joystickPosLeft = x;
        _joystickPosRight = -x;
    }
    /// <summary>
    /// Gets the current joystick position based on the current state of the toggle box bool.
    /// If you know what the ternary operator is nice, if not, it is a handy way to set or
    /// return a value based on a test. Its like:
    /// variable = condition ? valueIfTrue : valueIfFalse;
    /// So if toggle is true, it returns _joystickPosLeft. If toggle is false, it would return
    /// _joystickPosRight.
    /// </summary>
    /// <param name="toggle">Boolean that comes from toggle control</param>
    /// <returns>Float joystick position, either original or mirrored</returns>
    public float GetJoystickPosition(bool toggle)
    {
        return toggle ? _joystickPosLeft : _joystickPosRight;
    }
    /// <summary>
    /// Returns the correct anchor values based on the current state of the toggle box bool
    /// </summary>
    /// <param name="toggle">Boolean that comes from toggle control</param>
    /// <returns>Tuple containing the anchor points</returns>
    public (Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot) ToggleJoystickPosition(bool toggle)
    {
        return toggle switch
        {
            true => (_anchorLeft.anchorMin, _anchorLeft.anchorMax, _pivot),
            false => (_anchorRight.anchorMin, _anchorRight.anchorMax, _pivot),
        };
    }
}
