using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QVN.Story
{
    public class BgmSetter : MonoBehaviour
    {
        [SerializeField]
        private StroyAssets _assets;
        [SerializeField]
        private AudioClip audioClip;
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private Slider _slider;
        public float volume = 1.0f;

        public void PlayBgm(string id) {
            if (id.Equals("")) {
                StopBgm();
                return;
            }
            var clip = _assets.GetBgmAsset(id);
            if (clip.Equals(null)) {
                Debug.LogError($"[{id}]에 해당하는 BGM이 존재하지 않습니다.");
                return;
            }
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.volume = volume;
            audioSource.Play();
        }
        public void StopBgm() {
            audioSource.Stop();
        }
        public void PlaySoundEffect(string id) {
            var clip = _assets.GetBgmAsset(id);
            if (clip.Equals(null)) {
                Debug.LogError($"[{id}]에 해당하는 효과음이 존재하지 않습니다.");
                return;
            }
            audioSource.PlayOneShot(clip, volume);
        }

        public void SetVolume() {
            volume = _slider.value;
            audioSource.volume = volume;
        }
    }
}
