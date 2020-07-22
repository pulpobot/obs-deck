using OBSWebsocketDotNet.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public ObsDeck obsDeck;

    public bool IsRecording()
    {
        return obsDeck.IsRecording();
    }

    public bool IsStreaming()
    {
        return obsDeck.IsStreaming();
    }

    public List<OBSScene> GetScenes()
    {
        return obsDeck.GetScenes();
    }

    public OBSScene GetActiveScene()
    {
        return obsDeck.GetActiveScene();
    }

    public void SetActiveScene(OBSScene scene)
    {
        obsDeck.SetActiveScene(scene);
    }
}