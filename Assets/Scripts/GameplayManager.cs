using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[System.Serializable]
public class PlayerUIComponent {
    [ReadOnly] public string PlayerName;
    public GameObject Fill {get {return fill;}}
    public Slider LoveMeter {get {return loveMeter;}}

    [SerializeField] private GameObject fill;
    [SerializeField] private Slider loveMeter;
    [SerializeField] private GameObject heart;

    public PlayerUIComponent(string playerName) {
        PlayerName = playerName;
        fill = null;
        loveMeter = null;
    }
}

public class GameplayManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private PlayerUIComponent[] playerUIComponents = { 
        new PlayerUIComponent("Player 1"), new PlayerUIComponent("Player 2"),
    };

    private int playerJoined = 0;

    public void OnPlayerJoined() {

    }


}

#region ReadOnlyAttribute
public class ReadOnlyAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endregion