using UnityEngine;
using DG.Tweening;
using TMPro;

namespace QVN.Story
{
    public class OptionManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _button1;
        [SerializeField]
        private GameObject _button2;
        [SerializeField]
        private GameObject _button3;
        [SerializeField]
        private GameObject _button4;
        [SerializeField]
        private GameObject _button5;
        [SerializeField]
        private GameObject _button6;
        [SerializeField]
        private CanvasGroup _canvas;
        [SerializeField]
        private GameObject _slot;
        [SerializeField]
        private CanvasGroup _slotCanvas;
        [SerializeField]
        private DataManager _dataManager;
        [SerializeField]
        private TextMeshProUGUI _title;
        private object _animation;
        private bool cur_save = false;
        private bool is_open = false;

        public void ClickedID(int id) {
            if(cur_save) _dataManager.SaveData(id);
            else _dataManager.LoadData(id);
        }

        public void OpenSlot(bool save) {
            if (save) _title.text = "SAVE";
            else _title.text = "LOAD";
            cur_save = save;
            DefaultSystem.EffectSoundSystem.GetInstance?.PlayEffect("button");
            _slot.SetActive(true);
            if (_animation != null && DOTween.IsTweening(_animation))
                return;

            _animation = _slotCanvas.DOFade(1, 0.5f).target;
        }
        
        public void CloseSlot() {
            if (_animation != null && DOTween.IsTweening(_animation))
                return;

            _animation = _slotCanvas.DOFade(0, 0.5f).OnComplete(() => _slot.SetActive(false)).target;
        }

        public void Open()
        {
            if (is_open) {
                Close();
                return;
            }
            is_open = true;
            DefaultSystem.EffectSoundSystem.GetInstance?.PlayEffect("button");
            _button1.SetActive(true);
            _button2.SetActive(true);
            _button4.SetActive(true);
            _button5.SetActive(true);
            _button6.SetActive(true);
            if (_animation != null && DOTween.IsTweening(_animation))
                return;

            _animation = _canvas.DOFade(1, 0.5f).target;
        }

        public void Close()
        {
            is_open = false;
            _button1.SetActive(false);
            _button2.SetActive(false);
            _button3.SetActive(false);
            _button4.SetActive(false);
            _button5.SetActive(false);
            _button6.SetActive(false);
            if (_animation != null && DOTween.IsTweening(_animation))
                return;
        }
    }
}
