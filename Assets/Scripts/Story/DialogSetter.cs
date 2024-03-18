using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

namespace QVN.Story
{
    public class DialogSetter : MonoBehaviour
    {
        [SerializeField]
        protected TextMeshProUGUI _dialog;
        protected object _dialogAnimation;
        private Coroutine _keyboardSound;
        [SerializeField]
        public float _dialogShowingSpeed = 2.0f;
        [SerializeField]
        private Slider _slider;

        void Start() {
            _slider.value = _dialogShowingSpeed;
        }

        public virtual void SetDialog(string name, string dialog)
        {
            dialog = dialog.Replace("{n}", Data.PlayerSaveData.Name);
            float duration = dialog.Length * 0.2f / _dialogShowingSpeed;
            _dialogAnimation = _dialog.DOText(dialog, duration).From("").SetEase(Ease.Linear).target;
            PlayKeyboardSound(duration);
        }

        public bool IsAnimationPlaying()
        {
            /*Dialog 애니메이션이 실행 중인지 여부를 반환함*/
            if (_dialogAnimation == null)
            {
                return false;
            }
            return DOTween.IsTweening(_dialogAnimation);
        }

        public void ForceCompleteAnimation()
        {
            /*실행중인 Dialog 애니메이션이 있다면 중단함*/
            if (_dialogAnimation == null)
            {
                return;
            }
            DOTween.Kill(_dialogAnimation, true);
            if (_keyboardSound != null)
            {
                StopCoroutine(_keyboardSound);
            }
        }

        protected void PlayKeyboardSound(float duration)
        {
            if (_keyboardSound != null)
            {
                StopCoroutine(_keyboardSound);
            }
            _keyboardSound = StartCoroutine(KeyboardSoundCoroutine(0.1f, duration));
        }

        protected IEnumerator KeyboardSoundCoroutine(float term, float duration)
        {
            WaitForSeconds wait = new WaitForSeconds(term);
            float timeBucket = Time.time;

            while (Time.time - timeBucket < duration)
            {
                DefaultSystem.EffectSoundSystem.GetInstance?.PlayEffect("keyboard");
                yield return wait;
            }
        }

        public void SetSpeed() {
            _dialogShowingSpeed = _slider.value;
        }
    }
}
