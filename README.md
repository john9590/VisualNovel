# Project VisualNovel Engine
### 규격에 맞게 작성한 스프레드 시트를 추출하여 자동으로 비주얼 노벨을 만들어주는 엔진
    스프레드시트만 잘 작성해서 비주얼 노벨을 뚝딱 만들어 보자!
- 개발 규모 : ?인
- 버전 : Unity 2022.3.4f1
- 사용한 프로그램 : Visual Studio Code

# 사용법
## 스프레드시트 작성 예시
![c7257211202522973a6847d1b1d0b068-1](https://github.com/john9590/VisualNovel/assets/64019851/bcc37863-d42e-4278-9380-08e83e3bb5cd)
![c7257211202522973a6847d1b1d0b068-2](https://github.com/john9590/VisualNovel/assets/64019851/10188fdc-ec5a-482a-8ce8-3e846fdf0995)
![c7257211202522973a6847d1b1d0b068-3](https://github.com/john9590/VisualNovel/assets/64019851/148949ab-8f27-4047-bc0a-d8dc851f9569)
![c7257211202522973a6847d1b1d0b068-4](https://github.com/john9590/VisualNovel/assets/64019851/c57f0d42-7f50-47c0-b4dc-d3500cfec350)
![c7257211202522973a6847d1b1d0b068-5](https://github.com/john9590/VisualNovel/assets/64019851/035962e8-b586-467d-ba5c-9b077efd3f13)
![c7257211202522973a6847d1b1d0b068-6](https://github.com/john9590/VisualNovel/assets/64019851/3c298972-3b40-46f9-a067-77c707eb3317)
![c7257211202522973a6847d1b1d0b068-7](https://github.com/john9590/VisualNovel/assets/64019851/5f04e35d-0223-455a-aae1-69a2c895b100)
![c7257211202522973a6847d1b1d0b068-8](https://github.com/john9590/VisualNovel/assets/64019851/4b9c2fa2-d07b-4945-bd34-7cc43b5c3ca2)

    Program.py로 엔진에 맞게 .csv 파일로 변환 후 프로젝트의 Scenario파일에 넣고 실행

# 개발 범위
## Maple
기본적인 비주얼 노벨 시스템 이외에 새로운 연출이 필요할때 만든 코드로 기술팀 각자가 이런 형태의 코드를 하나씩 만들 예정이었음

대화창을 갈라서 2등분 해주는 코드
## BackgroundSetter
해당 id를 가지는 배경 어셋을 불러와서 DOTween을 이용하여 Dissolve하게 표시해주는 코드
## BgmSettser
해당 id를 가지는 사운드 에셋을 불러와서 Play시켜주는 코드 
## DataManager
시작시에 세이브 데이터를 불러오고 Json 형태로 세이브하고 로드하는 코드 OptionManager과 연동하여 사용한다
## DialogSetter
DOTween을 이용하여 타이핑 하듯이 대사를 출력해주는 코드, 화자의 이름과 대화 내용을 들고와서 출력해준다
## EventSetter
DOTween을 이용하여 Dissolve하게 컷씬을 출력해주는 코드
## OptionManager
세이브 로드 등의 옵션을 클릭했을 때 띄워주는 코드
## SelectionSetter
선택지들을 띄워주는 함수로 특정 선택지가 선택되었을 시 그 정보를 전달해 준다
## SelectionSlot
SelectionSetter를 통해 선택지들을 띄워주는 것으로 해당 선택지 슬롯에 버튼 클릭 이벤트와 내용을 넣어준다
## StandingSlot
해당 ID를 가지는 캐릭터 에셋을 가져와서 현재 출력되고 있는 캐릭터를 움직이고 빈자리에 캐릭터를 dissolve 하게 출력하여 화면을 n등분 한 자리에 캐릭터들이 위치하도록 한다

캐릭터가 퇴장할때도 마찬가지로 dissolve하게 퇴장하고 n등분 한 자리에 캐릭터들이 위치하도록 이동한다
## StoryManager
스프레드 시트를 하나씩 불러와서 해당하는 기능을 실행시켜주는 역할을 한다

클릭이나 엔터, 컨트롤(스킵)시에 다음 스프레드 시트 내용을 불러와서 실행 시켜준다

특정 조건 변수가 만족되지 않으면 씬을 건너뛰고 선택지를 통해 조건 변수의 증감을 조절한다
### 주요 변수
- _liking : 조건 변수로 Dictionary<string, int>타입으로 설정되어 있으며 선택지 선택을 통해 증감 할 수 있으며 씬의 실행 여부를 판단한다

## StoryAssets
캐릭터, 배경, 사운드의 어셋들을 찾아서 id로 반환하고 이를 위의 코드들을 통해 가져올 수 있도록 한다

## SystemDialogSetter
전체 화면을 덮은 나레이션 같은 대화를 표현하기 위해 만들었다

위에서부터 순차적으로 대화를 쌓아간다
## TalkDialogSetter
대화를 세팅하기 위한 코드이다