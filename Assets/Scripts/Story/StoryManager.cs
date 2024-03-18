using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading;

namespace QVN.Story
{
    using Models;
    using DefaultUI;

    public class StoryManager : MonoBehaviour
    {
        [SerializeField]
        private SceneLoader _loader;
        [SerializeField]
        private TalkDialogSetter _dialogSetter;
        [SerializeField]
        private SystemDialogSetter _systemDialogSetter;
        [SerializeField]
        private StandingSetter _standingSetter;
        [SerializeField]
        private BackgroundSetter _backgroundSetter;
        [SerializeField]
        private EventSetter _eventSetter;
        [SerializeField]
        private BgmSetter _bgmSetter;
        [SerializeField]
        private SelectionSetter _selectionSetter;
        [SerializeField]
        private Maple _maple;
        [SerializeField]
        private Image _fade;
        private List<ScenarioLine> _scenarioList;
        private int _pin;
        private int _flag;
        private bool _isOnLoading;
        private List<ScenarioLine> _selectionLines;
        private List<int> _selectionLinesID;
        private Dictionary<string, int> _liking = new Dictionary<string, int>();
        [SerializeField]
        private int _skipDelay = 100;
        private int _lastTalk = 0;
        [SerializeField]
        private CanvasGroup _UiCanvas;
        private bool _isAuto = false;
        private bool _isFast = false;
        private int _timeCulm = 0;

        private void Awake()
        {
            /* 오브젝트 시작시에 호출되며 시나리오 csv를 불러와 
            _scenarioList에 저장해준 후 시나리오를 초기화하여 불러올 준비를 한다. */
            _selectionSetter.Init();
            _systemDialogSetter.Init();
            int targetScenario = Data.StoryBookmark.GetScenarioID();
            Data.StoryStaticData.ReadData();
            Data.NameStaticData.ReadData();
            ShowScenario(targetScenario);
            _UiCanvas.DOFade(1.0f,1.0f);
        }

        private void Update () {
            /* 인풋으로 
            Control -> _skipDelay에 맞게 빠르게 스킵함.
            Space or Enter -> 다음 시나리오를 불러옴 */
            if (_isAuto) {
                if (_timeCulm> (_isFast ? 1 : 100)) {
                    GetNextInput();
                    _timeCulm=0;
                }
                _timeCulm++;
                return;
            }

            if (Input.GetKey(KeyCode.LeftControl)) {
                GetNextInput();
                Thread.Sleep(_skipDelay);
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                GetNextInput();
            }
            if (Input.GetKeyDown(KeyCode.Return)) {
                GetNextInput();
            }
        }

        public void AutoPlay(bool fast) {
            _isAuto = !_isAuto;
            _isFast = fast;
        }

        private void CheckLike() {
            bool condition = false;
            foreach (string like in _scenarioList[_pin].Info.Split(",")) {
                var tlike = like.TrimStart();
                if (tlike.Equals(string.Empty)) continue;
                string[] likes = tlike.Split(" ");
                if (_liking.ContainsKey(likes[0])) {
                    if (System.Int32.TryParse(likes[1], out int count)) {
                        if (_liking[likes[0]] < count) {
                            condition = true;
                            break;
                        }
                    }
                }
                else {
                    if (System.Int32.TryParse(likes[1], out int count)) {
                        if (count == 0) continue;
                    }
                    condition= true;
                    break;
                }
            }
            if (condition) {
                while (true) {
                    _pin++;
                    if (_scenarioList.Count <= _pin)
                    {
                        EndScenario();
                        return;
                    }
                    var line = _scenarioList[_pin];
                    if (line.Code.Equals("SCENE")) break;
                }
                ReadScenario();
            }
            else Next();
        }

        private void ShowScenario(int id)
        {
            /* 해당 시나리오를 불러와주고 시나리오를 실행해줌 */
            _scenarioList = Data.StoryStaticData.GetScenario(id);
            _fade.gameObject.SetActive(false);
            _isOnLoading = false;
            _selectionLines = new List<ScenarioLine>();
            _selectionLinesID = new List<int>();
            _pin = 0;
            _flag = 0;
            ReadScenario();
        }
        public void EndAllProduction() {
            //소멸자 넣어주세요
            _maple.MeshCutEnd();
        }

        private void Production(ScenarioLine line) {
            /* 연출을 생성자와 소멸자로 나눠서 관리하는 곳 
            개발 해야하는 곳 */
            if (line.SubContents != "") {
                switch (line.Contents) {
                    case "0":
                        //TODO 생성자
                        _maple.MeshCut();
                        break;
                }
            }
            else {
                switch (line.Contents) {
                    case "0":
                        //TODO 소멸자
                        _maple.MeshCutEnd();
                        break;
                }
            }
        }
        private void ReadScenario()
        {
            /*_scenarioList에서 하나씩 불러와서 그에 맞게 실행함 
            슬라이드 참조 */
            var line = _scenarioList[_pin];
            switch (line.Code)
            {
                case "SET":
                    switch (line.Info)
                    {
                        case "Member":
                            _standingSetter.SetStandings(line.Contents, line.GetFeeling());
                            break;
                        case "BG":
                            _standingSetter.ResetStandings();
                            _backgroundSetter.UpdateImage(line.Contents);
                            break;
                        case "Remove":
                            _standingSetter.RemoveStandings(line.Contents);
                            break;
                        case "BGM":
                            _bgmSetter.PlayBgm(line.Contents);
                            break;
                        case "Sound":
                            _bgmSetter.PlaySoundEffect(line.Contents);
                            break;
                        case "Event":
                            if (line.Contents.Equals(string.Empty))
                                _eventSetter.DeleteEvent();
                            else
                                _eventSetter.UpdateEvent(line.Contents);
                            break;
                        case "Name":
                            // TODO: 사용자 이름 입력받기
                            break;
                    }
                    Next();
                    break;

                case "TALK":
                    _lastTalk = _pin;
                    _dialogSetter.SetDialog(line.GetName(), line.Contents);
                    _standingSetter.SetTalker(line.Info, line.GetFeeling());
                    break;

                case "RADIO":
                    _lastTalk = _pin;
                    _dialogSetter.SetRadioDialog(line.GetName(), line.Contents);
                    _standingSetter.SetTalker(line.Info, line.GetFeeling());
                    break;

                case "SYSTEM":
                    _systemDialogSetter.SetDialog(string.Empty, line.Contents);
                    break;

                case "SELECT":
                    bool condition = false;
                    foreach (string like in line.SubInfo.Split(",")) {
                        var tlike = like.TrimStart();
                        if (tlike.Equals(string.Empty)) continue;
                        string[] likes = tlike.Split(" ");
                        if (_liking.ContainsKey(likes[0])) {
                            if (System.Int32.TryParse(likes[1], out int count)) {
                                if (_liking[likes[0]] < count) {
                                    condition = true;
                                    break;
                                }
                            }
                        }
                        else {
                            if (System.Int32.TryParse(likes[1], out int count)) {
                                if (count == 0) continue;
                            }
                            condition= true;
                            break;
                        }
                    }
                    var nextLine = _scenarioList[_pin + 1];
                    if (condition) {
                        if (!nextLine.Code.Equals("SELECT"))
                        {
                            _selectionSetter.SetSelections(_selectionLines);
                        }
                        else
                        {
                            Next();
                        }
                        break;
                    }
                    if (!nextLine.Code.Equals("SELECT"))
                    {
                        _selectionLines.Add(line);
                        _selectionLinesID.Add(_pin);
                        _selectionSetter.SetSelections(_selectionLines);
                    }
                    else
                    {
                        _selectionLines.Add(line);
                        _selectionLinesID.Add(_pin);
                        Next();
                    }
                    break;
                case "PRODUCTION":
                    Production(line);
                    break;
                case "SCENE":
                    CheckLike();
                    break;
                case "END":
                    EndScenario();
                    break;
            }
        }

        private void EndScenario()
        {
            /*현재 시나리오 종료 즉 게임을 종료함*/
            if (_isOnLoading) return;
            _pin = 0;
            _fade.gameObject.SetActive(true);
            _fade.DOFade(1, 0.5f).From(0).OnComplete(() => _loader.Load(SceneName.Home));
        }

        private void Next()
        {
            /*다음 시나리오 즉 csv에서 다음 줄을 실행한다*/
            _pin++;
            if (_scenarioList.Count <= _pin)
            {
                EndScenario();
            }
            else
            {
                ReadScenario();
            }
        }

        public void GetNextInput()
        {
            /* 현재 진행중인 애니메이션 (채팅, 시스템)을
             중단하고 다음 시나리오를 실행함 */
            if (_scenarioList.Count <= _pin)
            {
                EndScenario();
                return;
            }
            var line = _scenarioList[_pin];
            switch (line.Code)
            {
                case "SELECT":
                    return;
                case "TALK":
                    if (_dialogSetter.IsAnimationPlaying())
                    {
                        _dialogSetter.ForceCompleteAnimation();
                        return;
                    }
                    break;
                case "RADIO":
                    if (_dialogSetter.IsAnimationPlaying())
                    {
                        _dialogSetter.ForceCompleteAnimation();
                        return;
                    }
                    break;
                case "SYSTEM":
                    if (_systemDialogSetter.IsAnimationPlaying())
                    {
                        _systemDialogSetter.ForceCompleteAnimation();
                        return;
                    }
                    else
                    {
                        var nextLine = _scenarioList[_pin + 1];
                        if (!nextLine.Code.Equals(line.Code))
                            _systemDialogSetter.Close();
                    }
                    break;
            }
            Next();
        }

        public void GetSelect(int index)
        {
            /*선택지를 선택하면 그 선택지에 맞는 변수를 추가 및 증감해줌*/
            foreach (var like in _selectionLines[index].Info.Split(",")) {
                var tlike = like.TrimStart();
                if (tlike.Equals(string.Empty)) continue;
                string[] likes = tlike.Split(" ");
                if (_liking.ContainsKey(likes[0])) {
                    if (System.Int32.TryParse(likes[1], out int count)) _liking[likes[0]] += count;
                }
                else {
                    if (System.Int32.TryParse(likes[1], out int count)) _liking.Add(likes[0],count);
                }
            }
            _selectionLines.Clear();
            _selectionLinesID.Clear();
            Next();
        }

        public void Skip()
        {
            EndScenario();
        }

        public int GetPin() {
            return _pin;
        }

        public void SetPin(int pin, int LastTalk) {

            var line = _scenarioList[LastTalk];
            if (_dialogSetter.IsAnimationPlaying())
                _dialogSetter.ForceCompleteAnimation();
            if (_systemDialogSetter.IsAnimationPlaying())
                _systemDialogSetter.ForceCompleteAnimation();
            _dialogSetter.SetDialog(line.GetName(), line.Contents);
            _standingSetter.SetTalker(line.Info, line.GetFeeling());
            _pin = pin;
            if (!_scenarioList[_pin].Code.Equals("SELECT")) ReadScenario();
        }
        public int GetLastTalk() {
            return _lastTalk;
        }
        public List<int> GetSelectID() {
            return _selectionLinesID;
        }
        public Dictionary<string, int> GetLiking() {
            return _liking;
        }
        public void SetLiking(Dictionary<string, int> Liking) {
            _liking.Clear();
            _liking = Liking;
        }
        public void SetSelectID(List<int> ID) {
            _selectionLines.Clear();
            _selectionLinesID.Clear();
            _selectionLinesID = ID;
            foreach (int i in ID) {
                _selectionLines.Add(_scenarioList[i]);
            }
             _selectionSetter.SetSelections(_selectionLines);
        }

        public void End() {
            Application.Quit();
        }
    }
}
