using OBSWebsocketDotNet.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionsView : MonoBehaviour
{
    [SerializeField]
    private Button returnBtn;

    [SerializeField]
    private MainController controller;

    [SerializeField]
    private GameObject scenesListParent;

    [SerializeField]
    private GameObject scenePrefab;

    private ToggleGroup toggleGroup;
    private void Awake()
    {
        toggleGroup = scenesListParent.GetComponent<ToggleGroup>();
    }

    private void OnEnable()
    {
        returnBtn.onClick.AddListener(OnReturn);

        List<OBSScene> scenes = controller.GetScenes();

        for (int i = 0; i < scenesListParent.transform.childCount; i++)
        {
            Destroy(scenesListParent.transform.GetChild(i).gameObject);
        }

        if (null != scenes)
        {
            OBSScene activeScene = controller.GetActiveScene();

            foreach (OBSScene scene in scenes)
            {
                GameObject sceneObj = Instantiate<GameObject>(scenePrefab, scenesListParent.transform);
                sceneObj.GetComponent<Toggle>().group = toggleGroup;
                sceneObj.GetComponent<SceneToggleView>().Setup(scene, activeScene.Name);
            }
        }

        SceneToggleView.SceneSelected += OnSceneSelected;

    }

    private void OnDisable()
    {
        returnBtn.onClick.RemoveListener(OnReturn);
        SceneToggleView.SceneSelected -= OnSceneSelected;
    }

    void OnReturn()
    {
        MainView.Return();
    }

    void OnSceneSelected(OBSScene scene)
    {
        controller.SetActiveScene(scene);
    }
}
