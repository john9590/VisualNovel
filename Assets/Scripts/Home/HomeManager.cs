using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace QVN.Home
{
    public class HomeManager : MonoBehaviour
    {
        [SerializeField]
        private DefaultUI.SceneLoader _sceneLoader;
        [Header("UI")]
        [SerializeField]
        private ScenarioList _scenarioList;
        [SerializeField]
        private DropdownSetter _dropdownSetter;
        [SerializeField]
        private CanvasGroup _UiCanvas;

        private void Start()
        {
            Data.LocalData.ReadLocalData();
            RefreshStoryDataAndUI();

            var lanIndex = Data.LocalData.GetLanguage;
            _dropdownSetter.SetIndex((int)lanIndex);
        }

        public void ChangeLanguage(int value)
        {
            Data.LocalData.ChangeLanguage((Data.LANGUAGE)value);
            Data.StoryStaticData.DeleteData();
            RefreshStoryDataAndUI();
        }

        IEnumerator FadeIn(int id) {
            Tween tween = _UiCanvas.DOFade(0.0f, 1.0f);
            yield return tween.WaitForCompletion();
            Data.StoryBookmark.SetScenarioID(id);
            _sceneLoader.Load(DefaultUI.SceneName.Story);
        }

        public void MoveToStoryScene(int scenarioID)
        {
            StartCoroutine(FadeIn(scenarioID));
            /*Data.StoryBookmark.SetScenarioID(scenarioID);
            _sceneLoader.Load(DefaultUI.SceneName.Story);*/
        }

        private void RefreshStoryDataAndUI()
        {
            Data.StoryStaticData.ReadData();
            _scenarioList.Init();
        }
    }
}
