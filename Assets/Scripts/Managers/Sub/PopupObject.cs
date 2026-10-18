using UnityEngine;

namespace AD
{
    /// <summary>
    /// PopupObject는 팝업 관리와 관련된 컴포넌트로, 
    /// 활성화 시 PopupManager와 상호작용하여 팝업 스택에 등록하거나 예외/Flow 상태를 설정
    /// </summary>
    public class PopupObject : MonoBehaviour
    {
        private enum CheckType
        {
            Normal,
            Exception, // Exception인 경우, 팝업이 사라질 때 Release 처리
            Flow       // Flow 예외인 경우, 상황에 맞게 Flow 종료 후 Release 처리
        }

        [SerializeField] private CheckType _checkType = CheckType.Normal;
        private PopupManager _owner;
        internal bool IsFlowPopup => _checkType == CheckType.Flow;

        private void OnEnable()
        {
            _owner = AD.Managers.Instance != null ? AD.Managers.PopupM : null;
            if (_owner == null) return;
            switch (_checkType)
            {
                case CheckType.Normal:
                    _owner.EnablePop(gameObject);
                    break;
                case CheckType.Exception:
                    _owner.SetException();
                    break;
                case CheckType.Flow:
                    _owner.EnablePop(gameObject);
                    _owner.SetFlow();
                    break;
            }
        }

        /// <summary>
        /// Normal 타입일 경우 팝업을 비활성화 처리합니다.
        /// </summary>
        public void DisablePop()
        {
            if (_checkType == CheckType.Normal && _owner != null)
            {
                _owner.ClosePopup(gameObject);
            }
        }

        private void OnDisable()
        {
            // Use the manager that registered this popup, even during teardown.
            var owner = _owner;
            _owner = null;
            if (owner == null) return;
            if (_checkType == CheckType.Normal)
            {
                owner.NotifyPopupDisabled(gameObject);
            }
            else if (_checkType == CheckType.Exception)
            {
                owner.ReleaseException();
            }
            else if (_checkType == CheckType.Flow)
            {
                owner.NotifyPopupDisabled(gameObject);
                owner.ReleaseFlow();
            }
        }
    }
}
