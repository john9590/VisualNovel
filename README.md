# Project VisualNovel Engine
### 규격에 맞게 작성한 스프레드 시트를 추출하여 자동으로 비주얼 노벨을 만들어주는 엔진
    스프레드시트만 잘 작성해서 비주얼 노벨을 뚝딱 만들어 보자!
- 개발 규모 : ?인
- 버전 : Unity 2022.3.4f1
- 사용한 프로그램 : Visual Studio Code

# 규격
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

# 기능
 - 대사를 선형적으로 출력 및 현재 화자를 제외하고 캐릭터를 어둡게
 - 배경을 Dissolve하게 전환
 - 캐릭터 등/퇴장 시에 화면을 기준으로 n명이 일정 거리만큼 떨어진 곳으로 선형적으로 이동 후 추가/삭제
 - 조건이 미충족시 씬과 선택지 미표시
 - 선택지 선택시 해당하는 조건 변수 증감
 - 기본적인 저장, 불러오기, 빨리감기, 자동진행 등의 기능
