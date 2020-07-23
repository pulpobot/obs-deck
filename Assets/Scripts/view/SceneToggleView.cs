using OBSWebsocketDotNet.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneToggleView : MonoBehaviour
{
    public delegate void OnSceneToggle(OBSScene scene);
    public static OnSceneToggle SceneSelected;

    OBSScene scene;
    
    public void Setup(OBSScene scene, string activeScene)
    {
        name = scene.Name;
        this.scene = scene;
        this.GetComponentInChildren<Text>().text = scene.Name;
        this.GetComponent<Toggle>().isOn = scene.Name.Equals(activeScene);
        this.GetComponent<Toggle>().onValueChanged.AddListener(OnSceneSelected);
    }

    private void OnDisable()
    {
        this.GetComponent<Toggle>().onValueChanged.RemoveListener(OnSceneSelected);
    }

    void OnSceneSelected(bool active)
    {
        if (active) { SceneSelected(scene); }
    }
}
