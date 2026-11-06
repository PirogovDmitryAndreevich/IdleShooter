using UnityEngine;
using UnityEngine.UI;

public class MoveRunSwitcher : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _outline;

    private bool _isRun;
    private IInputService _inputService;

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    public void Construct(IInputService inputService)
    {
        _inputService = inputService;

        _isRun = false;
        _outline.SetActive(_isRun);

        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _isRun = !_isRun;
        _inputService.SwitchMove();

        _outline.SetActive(_isRun);
    }
}
