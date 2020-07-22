using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    public delegate void OnMainView();
    public static OnMainView Return;

    [SerializeField]
    private GameObject mainFolderDeck;
    [SerializeField]
    private StreamStatusView streamStatusView;
    [SerializeField]
    private TransitionsView transitionsView;

    [SerializeField]
    private Button streamStatusBtn;
    [SerializeField]
    private Button transitionsBtn;

    private void OnEnable()
    {
        Return += OnReturn;
        streamStatusView.gameObject.SetActive(false);
        streamStatusBtn.onClick.AddListener(OnStreamStatusClick);
        transitionsBtn.onClick.AddListener(OnTransitionsClick);
        mainFolderDeck.SetActive(true);
    }

    private void OnDisable()
    {
        Return -= OnReturn;
        streamStatusView.gameObject.SetActive(false);
        transitionsView.gameObject.SetActive(false);
    }

    private void OnReturn()
    {
        streamStatusView.gameObject.SetActive(false);
        transitionsView.gameObject.SetActive(false);
        mainFolderDeck.SetActive(true);
    }

    private void OnStreamStatusClick()
    {
        mainFolderDeck.SetActive(false);
        streamStatusView.gameObject.SetActive(true);
    }

    public void OnStreamData(OBSWebsocketDotNet.Types.StreamStatus data)
    {
        streamStatusView.Setup(data);
    }

    private void OnTransitionsClick()
    {
        mainFolderDeck.SetActive(false);
        transitionsView.gameObject.SetActive(true);
    }
}
