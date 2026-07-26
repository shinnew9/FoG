# FoG 프로젝트 - 시나리오 빠른 참조 가이드

## 📊 전체 시나리오 요약

총 **6개 시나리오**가 두 프로젝트에 분산되어 있습니다:
- **FoG_Revised**: 5개 시나리오 (메뉴 포함)
- **FoG_Walkways**: 3개 시나리오 (중복 제외)

---

## 🎯 시나리오 한눈에 보기

### FoG_Revised (보행동결 특화)

| # | 시나리오 | 파일명 | 크기 | 목적 | 난이도 |
|----|---------|--------|------|------|--------|
| 1 | 메뉴 | ScenarioMenu.unity | 50 KB | 시나리오 선택 | - |
| 2 | 기본 | BasicScene.unity | 112 KB | VR 환경 테스트 | ⭐ 낮음 |
| 3 | 보행동결 | Freeze_of_Gait.unity | 510 KB | 주요 평가 | ⭐⭐⭐ 중간 |
| 4 | 거리 변형 | Freeze_of_Gait_6m_3m.unity | 536 KB | 거리별 평가 | ⭐⭐⭐ 중간 |
| 5 | 출입구 | Freeze_of_Gait_Doorway.unity | 525 KB | 출입구 통과 | ⭐⭐⭐⭐ 높음 |
| 6 | 닫힌 문 | Freeze_of_Gait_Closed_Door.unity | 535 KB | 응급 상황 | ⭐⭐⭐⭐ 높음 |

### FoG_Walkways (보행 환경 다양성)

| # | 시나리오 | 파일명 | 크기 | 목적 | 난이도 |
|----|---------|--------|------|------|--------|
| 7 | 메뉴 | MainMenu.unity | 67 KB | 시나리오 선택 | - |
| 8 | 복잡한 보도 | ClutteredWalkway.unity | 302 KB | 장애물 회피 | ⭐⭐⭐ 중간 |
| 9 | 좁은 보도 | NarrowedWalkway.unity | 522 KB | 공간 제약 | ⭐⭐⭐⭐ 높음 |

---

## 🚀 각 시나리오별 시작 방법

### FoG_Revised 시나리오 실행
```powershell
# 프로젝트 경로
cd C:\Users\user\workspaces\unity\FoG_Revised

# Unity 에디터에서
1. Project > Assets/Scenes > [시나리오명].unity 더블클릭
2. Play 버튼 클릭
3. VR 컨트롤러 Y 버튼으로 메뉴 복귀
```

### FoG_Walkways 시나리오 실행
```powershell
# 프로젝트 경로
cd C:\Users\user\workspaces\unity\FoG_Walkways

# Unity 에디터에서
1. Project > Assets/Scenes > MainMenu.unity 더블클릭
2. Play 버튼 클릭
3. ClutteredWalkway 또는 NarrowedWalkway 선택
```

---

## 🎮 컨트롤 방법

### 키보드 (에디터 테스트)
```
W           - 전진
S           - 후진
A           - 좌측 이동
D           - 우측 이동
마우스      - 시점 회전
Y / ESC     - 메뉴로 복귀
```

### VR 컨트롤러 (Meta Quest)
```
아날로그 스틱 위      - 전진
아날로그 스틱 아래    - 후진
아날로그 스틱 좌      - 좌측 이동
아날로그 스틱 우      - 우측 이동
Y 버튼 (왼쪽)        - 메뉴로 복귀
X / A 버튼           - 상호작용 (시나리오별)
```

---

## 📈 평가 항목 비교

### 세 가지 평가 카테고리

**1️⃣ 보행동결 평가 (FoG_Revised)**
- 대상: 파킨슨병 환자, 신경계 질환자
- 측정: 보행 속도, 정지/동결 시간, 경로 안정성
- 환경: 실내 아파트, 문 통과, 다양한 거리

**2️⃣ 장애물 회피 능력 (FoG_Walkways - ClutteredWalkway)**
- 대상: 낙상 위험 환자, 고령자
- 측정: 회피 성공률, 경로 효율성, 반응 시간
- 환경: 복수 장애물, 개방형 공간

**3️⃣ 공간 제약 적응력 (FoG_Walkways - NarrowedWalkway)**
- 대상: 불안정성, 균형 장애 환자
- 측정: 균형 유지도, 신체 흔들림, 이동 속도
- 환경: 좁은 복도, 폐쇄형 공간

---

## ⏱️ 권장 평가 시간표

### 단일 세션 (30분)
```
전체 평가 프로토콜
├─ 준비 및 설정: 5분
│  ├─ 시스템 초기화
│  ├─ VR 헤드셋 착용
│  └─ 칼리브레이션
│
├─ BasicScene: 2분
│  └─ VR 환경 적응
│
├─ Freeze_of_Gait: 5분
│  └─ 주요 보행동결 평가
│
├─ 선택 시나리오: 5분
│  ├─ Doorway 또는 Closed_Door
│  └─ ClutteredWalkway 또는 NarrowedWalkway
│
├─ 데이터 수집 및 저장: 3분
│  └─ 평가 결과 분석
│
└─ 정리: 5분
   ├─ 장비 정리
   └─ 피드백 수집
```

### 전체 포괄 평가 (60분)
```
모든 시나리오 평가
├─ 준비: 5분
├─ BasicScene: 2분
├─ Freeze_of_Gait: 5분
├─ Freeze_of_Gait_6m_3m: 5분
├─ Freeze_of_Gait_Doorway: 5분
├─ Freeze_of_Gait_Closed_Door: 5분
├─ ClutteredWalkway: 5분
├─ NarrowedWalkway: 10분
├─ 데이터 처리: 5분
└─ 정리 및 피드백: 8분
```

---

## 💾 데이터 저장 구조

```
프로젝트 루트
├── FoG_Revised/
│   ├── Assets/Scenes/
│   │   ├── ScenarioMenu.unity
│   │   ├── BasicScene.unity
│   │   ├── Freeze_of_Gait.unity
│   │   ├── Freeze_of_Gait_6m_3m.unity
│   │   ├── Freeze_of_Gait_Doorway.unity
│   │   ├── Freeze_of_Gait_Closed_Door.unity
│   │   └── SCENARIOS_DOCUMENTATION.md ✓
│   └── Assets/Scripts/
│       └── ReturnToMenu.cs
│
├── FoG_Walkways/
│   ├── Assets/Scenes/
│   │   ├── MainMenu.unity
│   │   ├── ClutteredWalkway.unity
│   │   ├── NarrowedWalkway.unity
│   │   ├── Floor.prefab
│   │   └── WALKWAY_SCENARIOS_DOCUMENTATION.md ✓
│   └── Assets/Scripts/
│       └── [환경 제어 스크립트]
│
└── SCENARIOS_QUICK_REFERENCE.md ✓ (이 파일)
```

---

## 🔄 시나리오 선택 플로우차트

```
┌──────────────────┐
│   평가 목표       │
└────────┬─────────┘
         │
    ┌────┴─────────────────────────┐
    │                              │
    ▼                              ▼
┌─────────────────┐        ┌──────────────┐
│ 보행동결 평가   │        │ 일상 보행    │
│  (FoG_Revised)  │        │  (FoG_Walkways)
└────────┬────────┘        └──────┬───────┘
         │                        │
    ┌────┴────────────┐      ┌────┴─────────┐
    │                 │      │              │
    ▼                 ▼      ▼              ▼
[기본]      [거리/문]  [장애물]  [공간제약]
Basic      6m_3m/    Cluttered Narrowed
Scene      Doorway   Walkway   Walkway
           /Closed
           Door
    │         │         │       │
    └────┬────┴────┬────┴───┬───┘
         │         │        │
         ▼         ▼        ▼
    [데이터 수집]
         │
         ▼
    [결과 분석]
         │
         ▼
    [피드백 제공]
```

---

## 🎯 임상 목표별 권장 시나리오

| 임상 목표 | 권장 시나리오 | 주요 평가 항목 |
|----------|------------|------------|
| **파킨슨병 보행동결** | Freeze_of_Gait + Doorway + Closed_Door | 동결 시간, 회복 시간, 문 통과 능력 |
| **낙상 위험 평가** | BasicScene + ClutteredWalkway + NarrowedWalkway | 장애물 회피, 균형, 반응 시간 |
| **신경재활** | Freeze_of_Gait_6m_3m + ClutteredWalkway | 거리별 성능, 회피 능력 |
| **정상 기준선 수립** | BasicScene + Freeze_of_Gait | 기본 보행 속도, 시간 |
| **포괄적 평가** | 모든 시나리오 | 통합 점수 |

---

## 🔧 기본 설정 체크리스트

### VR 헤드셋 준비
- [ ] Oculus Runtime 설치 확인
- [ ] 헤드셋 배터리 충전
- [ ] 컨트롤러 배터리 확인
- [ ] Guardian 경계 설정 (안전 거리 2m×2m 이상)
- [ ] 카메라 칼리브레이션 실행

### 환경 준비
- [ ] 보호 공간 확보 (3m×3m 최소)
- [ ] 바닥 장애물 제거
- [ ] 조명 충분 (밝기 300 럭스 이상)
- [ ] 선풍기/에어컨 음소거
- [ ] 비상 연락처 준비

### 소프트웨어 준비
- [ ] Unity 프로젝트 열기
- [ ] 해당 씬 로드
- [ ] VR 연결 확인
- [ ] 데이터 저장 폴더 확인
- [ ] 로그 초기화

---

## 📱 모바일 테스트

### 안드로이드 빌드
```
File > Build Settings > Android 선택
File > Build and Run

또는

adb install -r FoG_Project.apk
adb shell am start -n com.unity.fog/com.unity.fog.MainActivity
```

---

## 🔗 상세 문서 참조

더 자세한 정보는 각 프로젝트의 전용 문서를 참고하세요:

### FoG_Revised (보행동결 특화)
📄 **[SCENARIOS_DOCUMENTATION.md](C:\Users\user\workspaces\unity\FoG_Revised\SCENARIOS_DOCUMENTATION.md)**
- 5개 시나리오 상세 설명
- 메뉴 복귀 메커니즘
- 기술 스택 및 성능 사양

### FoG_Walkways (환경 다양성)
📄 **[WALKWAY_SCENARIOS_DOCUMENTATION.md](C:\Users\user\workspaces\unity\FoG_Walkways\WALKWAY_SCENARIOS_DOCUMENTATION.md)**
- 3개 시나리오 상세 설명
- 환경 에셋 및 구조
- 데이터 기록 형식

---

## ✅ 성공 체크리스트

### 준비 완료
- [ ] 두 프로젝트 모두 다운로드 및 열림
- [ ] SCENARIOS_DOCUMENTATION.md 읽음
- [ ] WALKWAY_SCENARIOS_DOCUMENTATION.md 읽음
- [ ] VR 장비 테스트 완료
- [ ] 데이터 저장 경로 확인

### 실행 준비
- [ ] BasicScene에서 VR 환경 테스트
- [ ] 적어도 하나의 Freeze_of_Gait 시나리오 실행
- [ ] ClutteredWalkway 또는 NarrowedWalkway 실행
- [ ] 메뉴 복귀 메커니즘 테스트
- [ ] 데이터 저장 확인

### 평가 준비
- [ ] 평가 프로토콜 선택 (30분 또는 60분)
- [ ] 참가자 안전 교육 완료
- [ ] 동의서 확보 (의료 평가의 경우)
- [ ] 기록 양식 준비
- [ ] 응급 상황 대응 계획 수립

---

## 📞 문제 해결

### 씬 로드 실패
```
오류: "Scene 'ScenarioMenu' not found"
해결: Build Settings > Scenes In Build에 모든 씬 추가
```

### VR 헤드셋 인식 안 됨
```
오류: Oculus 또는 Meta 헤드셋 감지 안 됨
해결: 
1. Oculus Runtime 재설치
2. 헤드셋 USB 재연결
3. Unity > Edit > Project Settings > XR Plug-in Management 확인
```

### 프레임 드롭
```
오류: FPS 90 이하로 떨어짐
해결:
1. Graphics 설정 감소
2. Oculus 성능 모드 활성화
3. 배경 앱 종료
```

---

## 📊 데이터 분석 팁

### 수집 데이터
```json
{
  "session_id": "FOG20260722_001",
  "scenario": "Freeze_of_Gait",
  "participant_id": "P001",
  "age": 65,
  "condition": "Parkinson",
  "metrics": {
    "total_time_seconds": 245,
    "distance_meters": 8.5,
    "average_speed_mps": 0.035,
    "freeze_events": 3,
    "total_freeze_time": 18.5,
    "path_efficiency": 0.92
  }
}
```

### 비교 분석
```
대상자 간 비교:
- 평균 속도 (m/s)
- 동결 시간 합계 (s)
- 경로 효율성 (%)

시간 경과 추적:
- 주간 진행 상황
- 월간 개선도
- 재활 효과 측정
```

---

**최종 업데이트**: 2026-07-22  
**프로젝트 상태**: 완성 및 문서화 완료  
**총 시나리오**: 6개 (메뉴 제외) + 2개 메뉴 = 8개
