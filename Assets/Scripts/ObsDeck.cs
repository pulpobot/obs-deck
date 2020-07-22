using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsDeck : MonoBehaviour
{
    protected OBSWebsocket _obs;
    [SerializeField]
    private ConnectionView connectionView;
    [SerializeField]
    private MainView mainView;
    
    // Start is called before the first frame update
    void Start()
    {
        //view callbacks
        ConnectionView.OnConnect += Connect;

        _obs = new OBSWebsocket();
        _obs.Connected += OnConnect;
        _obs.WSTimeout = TimeSpan.FromSeconds(10);
        _obs.Disconnected += OnDisconnect;

        //_obs.SceneChanged += onSceneChange;
        //_obs.SceneCollectionChanged += onSceneColChange;
        //_obs.ProfileChanged += onProfileChange;
        //_obs.TransitionChanged += onTransitionChange;
        //_obs.TransitionDurationChanged += onTransitionDurationChange;

        //_obs.StreamingStateChanged += onStreamingStateChange;
        //_obs.RecordingStateChanged += onRecordingStateChange;

        _obs.StreamStatus += OnStreamData;
        connectionView.gameObject.SetActive(true);
        mainView.gameObject.SetActive(false);
    }

    public void Connect(string ip, string password)
    {
        if (!_obs.IsConnected)
        {
            try
            {
                _obs.Connect(ip, password);
            }
            catch (AuthFailureException)
            {
                Debug.LogError("Authentication failed. Error");
                return;
            }
            catch (ErrorResponseException ex)
            {
                Debug.LogError("Authentication failed. Error: " + ex.Message);
                return;
            }
        }
        else
        {
            _obs.Disconnect();
        }
    }

    private void OnDisconnect(object sender, EventArgs e)
    {
        connectionView.gameObject.SetActive(true);
        mainView.gameObject.SetActive(false);
    }

    private void OnConnect(object sender, EventArgs e)
    {
        Debug.Log("Connected: " + _obs.GetVersion().OBSStudioVersion);
        connectionView.gameObject.SetActive(false);
        mainView.gameObject.SetActive(true);
        //gbControls.Enabled = true;

        //var versionInfo = _obs.GetVersion();
        //tbPluginVersion.Text = versionInfo.PluginVersion;
        //tbOBSVersion.Text = versionInfo.OBSStudioVersion;

        //btnListScenes.PerformClick();
        //btnGetCurrentScene.PerformClick();

        //btnListSceneCol.PerformClick();
        //btnGetCurrentSceneCol.PerformClick();

        //btnListProfiles.PerformClick();
        //btnGetCurrentProfile.PerformClick();

        //btnListTransitions.PerformClick();
        //btnGetCurrentTransition.PerformClick();

        //btnGetTransitionDuration.PerformClick();

        //var streamStatus = _obs.GetStreamingStatus();
        //if (streamStatus.IsStreaming) 
        //    onStreamingStateChange(_obs, OutputState.Started);
        //else
        //    onStreamingStateChange(_obs, OutputState.Stopped);

        //if (streamStatus.IsRecording)
        //    onRecordingStateChange(_obs, OutputState.Started);
        //else
        //    onRecordingStateChange(_obs, OutputState.Stopped);
    }

    public bool IsRecording()
    {
        if (null == _obs) { return false; }
        return _obs.GetStreamingStatus().IsRecording;
    }

    public bool IsStreaming()
    {
        if (null == _obs) { return false; }
        return _obs.GetStreamingStatus().IsStreaming;
    }

    public List<OBSScene> GetScenes()
    {
        if (null == _obs) { return null; }
        return _obs.ListScenes();
    }

    public OBSScene GetActiveScene()
    {
        if (null == _obs) { return null; }
        return _obs.GetCurrentScene();
    }

    public void SetActiveScene(OBSScene scene)
    {
        _obs.SetCurrentScene(scene.Name);
    }

    private void OnStreamData(OBSWebsocket sender, StreamStatus data)
    {
        mainView.OnStreamData(data);
    }
}
