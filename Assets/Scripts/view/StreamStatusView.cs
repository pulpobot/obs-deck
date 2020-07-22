using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StreamStatusView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txtStreaming;
    [SerializeField]
    private TextMeshProUGUI txtRecording;
    [SerializeField]
    private TextMeshProUGUI txtStreamTime;
    [SerializeField]
    private TextMeshProUGUI txtKbitsSec;
    [SerializeField]
    private TextMeshProUGUI txtBytesSec;
    [SerializeField]
    private TextMeshProUGUI txtFramerate;
    [SerializeField]
    private TextMeshProUGUI txtStrain;
    [SerializeField]
    private TextMeshProUGUI txtDroppedFrames;
    [SerializeField]
    private TextMeshProUGUI txtTotalFrames;

    [SerializeField]
    private Button returnBtn;

    [SerializeField]
    private MainController controller;

    private void OnEnable()
    {
        returnBtn.onClick.AddListener(OnReturn);
        txtStreaming.SetText("Streaming:\t\t" + controller.IsStreaming().ToString());
        txtRecording.SetText("Recording:\t\t" + controller.IsRecording().ToString());
    }

    private void OnDisable()
    {
        returnBtn.onClick.RemoveListener(OnReturn);
    }

    void OnReturn()
    {
        MainView.Return();
    }

    public void Setup(OBSWebsocketDotNet.Types.StreamStatus data)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        txtStreaming.SetText("Streaming:\t\t" + data.Streaming.ToString());
        txtRecording.SetText("Recording:\t\t" + data.Recording.ToString());
        txtStreamTime.SetText("Total Stream Time:\t\t" + data.TotalStreamTime.ToString() + " sec");
        txtKbitsSec.SetText("Kbits/sec:\t\t" + data.KbitsPerSec.ToString() + " kbit/s");
        txtBytesSec.SetText("Bytes/sec:\t\t" + data.BytesPerSec.ToString() + " bytes/s");
        txtFramerate.SetText("Framerate:\t\t" + data.FPS.ToString() + " FPS");
        txtStrain.SetText("Strain:\t\t" + (data.Strain * 100).ToString() + " %");
        txtDroppedFrames.SetText("Dropped Frames:\t\t" + data.DroppedFrames.ToString());
        txtTotalFrames.SetText("Total Frames:\t\t" + data.TotalFrames.ToString());
    }
}
