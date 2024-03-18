using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace QVN.Story
{
    public class EventSetter : MonoBehaviour
    {
        [SerializeField]
        private StroyAssets _assets;
        [SerializeField]
        private Image _image;
        [SerializeField]
        private Image _subImage;
        [SerializeField]
        private CanvasGroup _fadeGroup;
        [SerializeField]
        private CanvasGroup _subFadeGroup;
        [SerializeField]
        private float _dissolveTime = 1.0f;
        private string cur_id = null;

        private void Awake()
        {
            _fadeGroup.alpha = 0.0f;
            _subFadeGroup.alpha = 0.0f;
        }
        IEnumerator dissolve(string id){
            _subImage.sprite = _assets.GetBackgroundAsset(id);
            _subFadeGroup.alpha = 1.0f;
            if (_subImage.sprite==null)
                Debug.LogError($"[{id}]에 해당하는 이벤트 cg가 존재하지 않습니다.");
	        Tween tween = _fadeGroup.DOFade(0.0f, _dissolveTime);
            yield return tween.WaitForCompletion();
            _image.sprite = _subImage.sprite;
            _fadeGroup.alpha = 1.0f;
            _subFadeGroup.alpha = 0.0f;
        }

        public void UpdateImage(string id)
        {
            /*String id에 해당하는 값으로 배경을 변경해줌
            효과는 dissolve. */
            cur_id = id;
            StartCoroutine(dissolve(id));
        }

        public void UpdateEvent(string id)
        {
            if (cur_id==null) {
                cur_id = id;
                _image.sprite = _assets.GetBackgroundAsset(id);
                if (_image.sprite==null)
                    Debug.LogError($"[{id}]에 해당하는 이벤트 cg가 존재하지 않습니다.");
                Tween tween = _fadeGroup.DOFade(1.0f, _dissolveTime);
            }
            else {
                cur_id = id;
                StartCoroutine(dissolve(id));
            }
        }

        public void DeleteEvent()
        {
            Tween tween = _fadeGroup.DOFade(0.0f, _dissolveTime);
        }

        public string GetID() {
            return cur_id;
        }
    }
}
