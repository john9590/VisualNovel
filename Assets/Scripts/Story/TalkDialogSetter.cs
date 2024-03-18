using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

namespace QVN.Story
{
    public class TalkDialogSetter : DialogSetter
    {
        [SerializeField]
        private TextMeshProUGUI _name;
        [SerializeField]
        private DialogConfig _config;


        public override void SetDialog(string name, string dialog)
        {
            base.SetDialog(name, dialog);
            _name.text = name;
            _dialog.font = _config.GetBaseFont;
        }

        public void SetRadioDialog(string name, string dialog)
        {
            _name.text = name;
            dialog = dialog.Replace("{n}", Data.PlayerSaveData.Name);
            float duration = dialog.Length * 0.2f / _dialogShowingSpeed;
            _dialogAnimation = _dialog.DOText(dialog, duration).From("").SetEase(Ease.Linear).target;
            _dialog.font = _config.GetRadioFont;
            PlayKeyboardSound(duration);
        }

        public string GetCurName() {
            return _name.text;
        }

        public string GetCurDialog() {
            return _dialog.text;
        }
        
        public void SetBreak(string name, string dialog) {
            _name.text = name;
            _dialog.text = dialog;
        }

    }

    [System.Serializable]
    public class DialogConfig
    {
        [SerializeField]
        private TMP_FontAsset _baseFont;
        [SerializeField]
        private TMP_FontAsset _radioFont;

        public TMP_FontAsset GetBaseFont => _baseFont;
        public TMP_FontAsset GetRadioFont => _radioFont;
    }
}
