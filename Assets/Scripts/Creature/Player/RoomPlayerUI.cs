using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayerUI : MonoBehaviour
{

    [SerializeField] private TMP_Text _nickName;
    [SerializeField] private Image _teamImage;
    [SerializeField] private GameObject _readyStateObject;

    public void UpdateTeamUI(int team)
    {
        _teamImage.color = team == 0
            ? new Color(58f / 255f, 40f / 255f, 44f / 255f, 1f)
            : new Color(35f / 255f, 53f / 255f, 69f / 255f, 1f);
    }

    public void SetNickName(string nickName)
    {
        _nickName.richText = false;
        _nickName.parseCtrlCharacters = false;
        _nickName.text = (nickName ?? string.Empty).Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ');
    }

    public void SetReadyState(bool active)
    {
        _readyStateObject.SetActive(active);
    }
}
