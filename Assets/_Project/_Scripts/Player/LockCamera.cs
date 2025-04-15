using UnityEngine;
using Cinemachine;

public class LockCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    public CinemachineVirtualCamera TriggerCamera;
    private LockXCameraPosition _lockXExtension;
    private bool _isLocked;
    private Transform _lookAtGameObject;
    private Transform _playerShadow;

    public bool CanTrigger = true;

    void Start()
    {
        _virtualCamera = GameObject.Find("Virtual Camera")?.GetComponent<CinemachineVirtualCamera>();
        _lockXExtension = _virtualCamera?.GetComponent<LockXCameraPosition>();
        _isLocked = false;
        _lookAtGameObject = _virtualCamera.LookAt;

        _playerShadow = transform.GetChild(0);
    }

    private void Update()
    {
        if (GameManager.Instance.Player != null)
        {
            _playerShadow.transform.localPosition = new Vector3(0.0f, GameManager.Instance.Player.transform.position.y, GameManager.Instance.Player.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CanTrigger)
            return;

        if (other.GetComponent<Player>() != null)
        {
            _isLocked = !_isLocked;
            _lockXExtension.IsLocked = _isLocked;

            if (_isLocked)
            {
                _virtualCamera.LookAt = _playerShadow;
                TriggerCamera.gameObject.SetActive(true); 
                _virtualCamera.gameObject.SetActive(false); 

                _lockXExtension.LockedX = transform.position.x;
            }
            else
            {
                _virtualCamera.LookAt = _lookAtGameObject;
                _virtualCamera.gameObject.SetActive(true);
                TriggerCamera.gameObject.SetActive(false);

                _lockXExtension.LockedX = 0.0f;
            }
        }
    }
}
