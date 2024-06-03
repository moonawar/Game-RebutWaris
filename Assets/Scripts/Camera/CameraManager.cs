using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera preGameCam;
    [SerializeField] private CinemachineVirtualCamera inGameCam;
    [SerializeField] private CinemachineVirtualCamera eventCam;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetPreGameCamActive();
    }

    public void SetPreGameCamActive()
    {
        preGameCam.Priority = 10;
        inGameCam.Priority = 0;
        eventCam.Priority = 0;
    }

    public void SetInGameCamActive()
    {
        preGameCam.Priority = 0;
        inGameCam.Priority = 10;
        eventCam.Priority = 0;
    }

    public void SetMikaEventCamActive()
    {
        preGameCam.Priority = 0;
        inGameCam.Priority = 0;
        eventCam.Priority = 10;
    }
}
