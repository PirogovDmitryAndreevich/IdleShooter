using System;
using TMPro;
using UnityEngine;
using YG;

public class UserInfo : MonoBehaviour
{
    [SerializeField] private ImageLoadYG imageLoad;
    [SerializeField] private Sprite _defaultAvatar;
    [SerializeField] private TMP_Text _crownsText;

    private ICrownsManager _crownManger;

    private void OnDestroy() => 
        _crownManger.CrownChanged -= OnCrownChanged;

    public void Construct(ICrownsManager crownManager)
    {
        _crownManger = crownManager;

        _crownManger.CrownChanged += OnCrownChanged;

        OnCrownChanged();
        LoadPlayerAvatar();
    }

    private void LoadPlayerAvatar()
    {
        if (imageLoad == null)
            return;

        string photoUrl = YG2.player.photo;

        if (string.IsNullOrEmpty(photoUrl))
        {
            if (_defaultAvatar != null)
                imageLoad.SetTexture(_defaultAvatar.texture);
            else
                imageLoad.ClearTexture();

            return;
        }

        imageLoad.Load(photoUrl);
    }

    private void OnCrownChanged()
    {
        _crownsText.text = _crownManger
            .GetCurrentCrowns()
            .ToString();
    }
}