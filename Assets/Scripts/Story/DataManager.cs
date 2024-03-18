using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

namespace QVN.Story
{
    public class PlayerData
    {
        public int _pin = 0;
        public int _lastTalk = 0;
        public List<FEELING> _feels;
        public List<string> _names;
        public List<int> _selectID;
        public List<string> _likingKey = new List<string>();
        public List<int> _likingValue = new List<int>();
        public string _bgID;
        public string _date;
    }
    public class DataManager : MonoBehaviour
    {
        [SerializeField]
            private TextMeshProUGUI[] _slot;
        [SerializeField]
            private StoryManager _storyPin;
        [SerializeField]
            private StandingSetter _standing;
        [SerializeField]
            private BackgroundSetter _background;
        private bool[] _savefile = new bool[4];	// 세이브파일 존재유무 저장
        private string _path;
        private bool start = false;
        private PlayerData nowPlayer = new PlayerData();
        void Start()
        {
            _path = Application.persistentDataPath + "/save";
            // 슬롯별로 저장된 데이터가 존재하는지 판단.
            for (int i = 0; i < 4; i++)
            {
                if (File.Exists(_path + $"{i}"))	// 데이터가 있는 경우
                {
                    _savefile[i] = true;			// 해당 슬롯 번호의 bool배열 true로 변환
                    //_nowSlot = i;	// 선택한 슬롯 번호 저장
                    LoadData(i);	// 해당 슬롯 데이터 불러옴
                    _slot[i].text = nowPlayer._date;	// 버튼에 정보 표시
                }
                else	// 데이터가 없는 경우
                {
                    _slot[i].text = "Empty";
                }
            }
            // 불러온 데이터를 초기화시킴.(버튼에 닉네임을 표현하기위함이었기 때문)
            start = true;
            DataClear();
        }
    
        private void LoadPlayer() {
            nowPlayer._pin = _storyPin.GetPin();
            nowPlayer._lastTalk = _storyPin.GetLastTalk();
            nowPlayer._selectID = _storyPin.GetSelectID();
            nowPlayer._feels = _standing.GetFeel();
            nowPlayer._names = _standing.GetNames();
            nowPlayer._bgID = _background.GetID();
            nowPlayer._date = DateTime.Now.ToString("MM/dd HH:mm");
            var liking = _storyPin.GetLiking();
            nowPlayer._likingKey.Clear();
            nowPlayer._likingValue.Clear();
            foreach (var item in liking) {
                nowPlayer._likingKey.Add(item.Key);
                nowPlayer._likingValue.Add(item.Value);
            }
        }
    
        public void SaveData(int id)
        {
            LoadPlayer();
            string data = JsonUtility.ToJson(nowPlayer);
            File.WriteAllText(_path+ id.ToString(), data);
            _savefile[id] = true;
            _slot[id].text = nowPlayer._date;
        }
    
        private void SetPlayer() {
            _storyPin.EndAllProduction();
            _standing.SetStanding(nowPlayer._names, nowPlayer._feels);
            _background.UpdateImage(nowPlayer._bgID);
            _storyPin.SetSelectID(nowPlayer._selectID);
            Dictionary<string, int> Liking = new Dictionary<string, int>();
            for (int i=0;i<nowPlayer._likingKey.Count;i++) {
                Liking.Add(nowPlayer._likingKey[i],nowPlayer._likingValue[i]);
            }
            _storyPin.SetLiking(Liking);
            _storyPin.SetPin(nowPlayer._pin, nowPlayer._lastTalk);
        }

        public void LoadData(int id)
        {
            string data = File.ReadAllText(_path + id.ToString());
            nowPlayer = JsonUtility.FromJson<PlayerData>(data);
            if (start) SetPlayer();
        }
    
        public void DataClear()
        {
            nowPlayer = new PlayerData();
        }
    }
}
