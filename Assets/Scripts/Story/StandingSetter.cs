using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace QVN.Story
{
    public class StandingSetter : MonoBehaviour
    {
        [SerializeField]
        private StroyAssets _assets;
        [SerializeField]
        private List<Image> _standingSlots = new List<Image>();
        private List<string> _names = new List<string>();
        private List<FEELING> _feels = new List<FEELING>();
        [SerializeField]
        private float _fadeTime = 0.5f;
        public void SetStandings(string contents, FEELING feel)
        {
            if (contents.Equals(string.Empty))
            {
                SetStandingSlots(null, feel);
            }
            SetStandingSlots(contents.Split(','), feel);
        }

        private void SetStandingSlots(string[] members, FEELING feel)
        {
            /*현재 화면에 표시되어 있는 멤버들에 인자로 받은 배열에 
            FEELING에 맞는 이미지를 n등분한 화면에 멤버들을 위치시킴.*/
            if (members==null) return;
            var distance = 1920.0f / (_standingSlots.Count+members.Length);
            var cur_dis = -960.0f;
            foreach (Image image in _standingSlots) {
                if (2*cur_dis + distance >= -960.0f && 2*cur_dis + distance <= 960.0f) image.gameObject.transform.DOLocalMoveX((2*cur_dis + distance)/2.0f,1.0f);
                cur_dis += distance;
            }
            for (int i = 0; i < members.Length; i++) {
                if (members[i]==null) continue;
                if (_names.Contains(members[i])) continue;
                var sprite = _assets.GetStandingAsset(members[i], feel);
                if (sprite == null) {
                    Debug.LogError($"[{members[i]},{feel}]에 해당하는 스탠딩 이미지가 존재하지 않습니다.");
                    sprite = _assets.GetStandingAsset("Oscar", feel);
                }
                GameObject parentObject = GameObject.Find("Standings");
                GameObject newGameObject = new GameObject("Standing (" + _standingSlots.Count.ToString()+")");
                newGameObject.transform.parent = parentObject.transform;
                RectTransform rectTransform = newGameObject.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0.5f, 0f);
                rectTransform.anchorMax = new Vector2(0.5f, 0f);
                Vector2 sizeDelta = rectTransform.sizeDelta;
                sizeDelta.x = 1100;
                sizeDelta.y = 1100;
                rectTransform.sizeDelta = sizeDelta;
                rectTransform.pivot = new Vector2(0.5f, 0.0f);
                newGameObject.transform.localPosition = new Vector3((2*cur_dis+distance)/2.0f,0.0f,0.0f);
                newGameObject.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
                Image newImage = newGameObject.AddComponent<Image>();
                newImage.sprite = sprite;
                newImage.preserveAspect = true;
                CanvasGroup newCanvas = newGameObject.AddComponent<CanvasGroup>();
                newCanvas.alpha = 0.0f;
                newCanvas.DOFade(1.0f, _fadeTime);
                _standingSlots.Add(newImage);
                _names.Add(members[i]);
                _feels.Add(feel);
                cur_dis += distance;
            }
        }
        IEnumerator Fadeout(int id) {
            var CurGameObject = _standingSlots[id].gameObject;
            Tween tween = CurGameObject.GetComponent<CanvasGroup>().DOFade(0.0f, _fadeTime);
            yield return tween.WaitForCompletion();
            Destroy(CurGameObject);
        }

        public void RemoveStandings(string contents) {
            if (contents.Equals(string.Empty))
            {
                RemoveStandingSlots(null);
            }
            RemoveStandingSlots(contents.Split(','));
        }

        private void RemoveStandingSlots(string[] members) {
            /*현재 화면에 표시되어 있는 멤버들 중 인자로 받은 
            string과 일치하는 것을 제거하고 n등분한 화면에 멤버들을 위치시킴.*/
            if (members==null) return;
            var distance = 1920.0f / (_standingSlots.Count-members.Length);
            var cur_dis = -960.0f;
            for (int i = 0; i < _standingSlots.Count; i++) {
                bool condition;
                do {
                    condition = false;
                    for (int j=0; j<members.Length; j++) {
                        if (members[j] == "") continue;
                        if (members[j].Equals(_names[i])) {
                            StartCoroutine(Fadeout(i));
                            _names.Remove(_names[i]);
                            _standingSlots.Remove(_standingSlots[i]);
                            _feels.Remove(_feels[i]);
                            members[j] = "";
                            condition = true;
                            break;
                        }
                    }
                } while(condition && i<_standingSlots.Count);
                if (i>=_standingSlots.Count) break;
                if (2*cur_dis + distance >= -960.0f && 2*cur_dis + distance <= 960.0f) _standingSlots[i].gameObject.transform.DOLocalMoveX((2*cur_dis + distance)/2.0f,1.0f);
                cur_dis += distance;
            }
        }
        
        public void ResetStandings() {
            for (int i=0; i<_standingSlots.Count;) {
                StartCoroutine(Fadeout(i));
                _standingSlots.Remove(_standingSlots[0]);
                _names.Remove(_names[0]);
                _feels.Remove(_feels[0]);
            }
        }

        private void SetSlot(Image image, string name, FEELING feeling)
        {
            /*해당하는 이미지를 FEELING 감정으로 바꿔줌*/
            if (name.Equals(string.Empty))
            {
                image.gameObject.SetActive(false);
                return;
            }
            var sprite = _assets.GetStandingAsset(name, feeling);
            if (sprite == null)
            {
                Debug.LogError($"[{name},{feeling}]에 해당하는 스탠딩 이미지가 존재하지 않습니다.");
                sprite = _assets.GetStandingAsset("Oscar", feeling);
            }
            image.gameObject.SetActive(true);
            image.sprite = sprite;
            for (int i=0; i<_standingSlots.Count; i++) {
                if (!name.Equals(string.Empty) && _names[i].Equals(name)) {
                    _feels[i] = feeling;
                    break;
                }
            }
        }

        public void SetTalker(string talker, FEELING feeling)
        {
            /*현재 말하는 사람 제외하고 살짝 어둡게 만듬 말하는 
            사람이 화면에 표시가 안되어 있다면 모두 밝게 해줌*/
            bool iswhite = false;
            for (int i = 0; i < _standingSlots.Count; i++)
            {
                _standingSlots[i].color = _names[i].Equals(talker) ? Color.white : Color.gray;
                if (!talker.Equals(string.Empty) && _names[i].Equals(talker))
                {
                    var sprite = _assets.GetStandingAsset(talker, feeling);
                    if (sprite != null)
                    {
                        _standingSlots[i].sprite = sprite;
                        _feels[i] = feeling;
                    }
                    iswhite = true;
                }
            }
            if (!iswhite) {
                for (int i = 0; i < _standingSlots.Count; i++) {
                    _standingSlots[i].color = Color.white;
                }
            }
        }

        public List<string> GetNames() {
            return _names;
        }
        public List<FEELING> GetFeel() {
            return _feels;
        }

        public void SetStanding(List<string> names, List<FEELING> feels) {
            ResetStandings();
            for (int i=0;i<names.Count;i++) {
                SetStandingSlots(new [] {names[i]}, feels[i]);
            }
        }
    }
}
