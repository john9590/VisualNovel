using System.Collections.Generic;
using UnityEngine;

namespace QVN.Story
{
    public enum FEELING
    {
        IDLE, TALK, SMILE, ANGRY, SAD, HURT, TIRED, alcohol, cool, disappointed, drunkhappy, drunksad, funny, shy, somethingwrong, cry, shiny, suprised, laugh, tired, tiredbut
    }

    public class StroyAssets : MonoBehaviour
    {
        [SerializeField]
        private List<StandingAssetSet> _standingAssets;
        [SerializeField]
        private List<BackgroundAsset> _backgroundAssets;
        [SerializeField]
        private List<BgmAsset> _bgmAssets;

        public Sprite GetStandingAsset(string name, FEELING feeling)
        {
            /*String ID와 감정에 해당하는 Asset의 sprite를 반환한다.*/
            //var id = Data.NameStaticData.GetCharacterNameData().GetID(name);
            //if (id == null){
            //    return null;
            //}
            var standingData = _standingAssets.Find(x => x.ID.Equals(name));
            if (standingData.Equals(null)){
                return null;
            }
            var standings = standingData.standingSprites;
            var matchingSprite = standings.Find(x => x.feeling.Equals(feeling));
            if (matchingSprite.Equals(null)){
                return null;
            }
            return matchingSprite.standingSprite;
        }
        
        public Sprite GetBackgroundAsset(string id)
        {
            /*String ID에 해당하는 배경 Asset의 sprit를 반환한다.*/
            var backgroundData = _backgroundAssets.Find(x => x.ID.Equals(id));
            if (backgroundData.Equals(null)){
                return null;
            }
            return backgroundData.backgroundSprite;
        }

        public AudioClip GetBgmAsset(string id)
        {
            /*String ID에 해당하는 오디오 Asset의 AudioClip을 반환한다*/
            var bgmData = _bgmAssets.Find(x => x.ID.Equals(id));
            if (bgmData.Equals(null)){
                return null;
            }
            return bgmData.bgmClip;
        }

    }

    [System.Serializable]
    public struct StandingAssetSet
    {
        public string ID;
        public List<StandingAsset> standingSprites;
    }

    [System.Serializable]
    public struct StandingAsset
    {
        public FEELING feeling;
        public Sprite standingSprite;
    }

    [System.Serializable]
    public struct BackgroundAsset
    {
        public string ID;
        public Sprite backgroundSprite;
    }
    [System.Serializable]
    public struct BgmAsset
    {
        public string ID;
        public AudioClip bgmClip;
    }
}
