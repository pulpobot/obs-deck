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

    private string lastIP;
    private string lastPort;
    private string lastPassword;

    public const string LAST_IP_KEY = "lastIP";
    public const string LAST_PORT_KEY = "lastPort";

    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
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

    public void Connect(string ip, string port, string password)
    {
        if (!_obs.IsConnected)
        {
            try
            {
                lastIP = ip;
                lastPort = port;
                lastPassword = password;
                _obs.Connect("ws://" + ip + ":" + port, password);
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

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            //If the app when to the background check the connectivity
            if (_obs != null && !_obs.IsConnected)
            {
                OnDisconnect(null, null);
                Connect(lastIP, lastPort, lastPassword);
            }
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
        //store in player prefs latest successful data
        PlayerPrefs.SetString(LAST_IP_KEY, lastIP);
        PlayerPrefs.SetString(LAST_PORT_KEY, lastPort);
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
