# Flow Framework

Unity용 게임 플로우 관리 프레임워크입니다. 게임의 생명주기, 업데이트 루프, 인터럽션(중단) 처리를 체계적으로 관리할 수 있는 구조를 제공합니다.

## 목차

- [개요](#개요)
- [핵심 개념](#핵심-개념)
  - [GameFlow](#gameflow)
  - [GameState](#gamestate)
  - [Update System](#update-system)
  - [Interruption System](#interruption-system)
  - [Completion Actions](#completion-actions)
- [설치 및 설정](#설치-및-설정)
- [사용 예시](#사용-예시)
- [API 레퍼런스](#api-레퍼런스)

---

## 개요

Flow Framework는 다음과 같은 기능을 제공합니다:

- **상태 기반 게임 플로우 관리**: Initialization → Running → End
- **업데이트 시스템**: Update/FixedUpdate/LateUpdate 분리 및 순서 제어
- **인터럽션 메커니즘**: 특정 게임 상태에서 비동기 작업 실행 (튜토리얼, 컷신 등)
- **완료 액션**: 게임 종료 시 실행할 동기/비동기 작업 등록
- **의존성 주입**: Provider 패턴을 통한 깔끔한 의존성 관리

---

## 핵심 개념

### GameFlow

게임의 전체 생명주기를 관리하는 핵심 컴포넌트입니다.

#### 주요 메서드

```csharp
public class GameFlow : MonoBehaviour
{
    // 의존성 초기화
    public void InitializeDependencies(
        IGameContext context,
        IGameStateHandler gameStateHandler,
        IFlowServiceProvider serviceProvider);
    
    // 게임 시작
    public async UniTask RunAsync();
    
    // 게임 종료
    public async UniTask QuitAsync();
    
    // 현재 게임 상태
    public GameState State { get; }
}
```

#### 실행 흐름

1. `InitializeDependencies()` - 의존성 주입
2. `RunAsync()` - 게임 시작
   - Initialization 인터럽션 실행
   - Running 상태로 전환
   - 업데이트 루프 시작
3. `QuitAsync()` - 게임 종료
   - 완료 액션 실행
   - End 상태로 전환

---

### GameState

게임의 현재 상태를 나타내는 열거형입니다.

```csharp
public enum GameState
{
    Undefined,       // 초기화되지 않은 상태
    Initialization,  // 초기화 진행 중
    Running,         // 게임 실행 중
    End              // 게임 종료됨
}
```

#### 상태 전환

```
Undefined → Initialization → Running → End
```

---

### Update System

Unity의 업데이트 루프를 관리하는 시스템입니다.

#### UpdateType

```csharp
public enum UpdateType
{
    Update,       // 매 프레임
    FixedUpdate,  // 고정 시간 간격 (물리)
    LateUpdate    // Update 이후
}
```

#### IUpdate 인터페이스

```csharp
public interface IUpdate
{
    bool Enabled { get; set; }
    void Update(float deltaTime);
}
```

#### Attribute를 통한 설정

```csharp
[ManualUpdate(UpdateType.Update)]
[ManualUpdateOrder(100)]
public class PlayerSystem : IUpdate
{
    public bool Enabled { get; set; }
    
    public void Update(float deltaTime)
    {
        // 플레이어 로직
    }
}
```

- `ManualUpdateAttribute`: 업데이트 타입 지정
- `ManualUpdateOrderAttribute`: 실행 순서 지정 (낮을수록 먼저 실행)

#### Update Group 구현

```csharp
public class UpdateGroup : BaseUpdateGroup
{
    public override UpdateType UpdateType => UpdateType.Update;
    
    public UpdateGroup(IEnumerable<IUpdate> systems) : base(systems)
    {
    }
}
```

각 UpdateType별로 UpdateGroup, FixedUpdateGroup, LateUpdateGroup이 존재합니다.

---

### Interruption System

게임의 특정 시점에 비동기 작업을 실행하고 게임을 일시 중단시키는 메커니즘입니다.

#### 사용 사례

- 튜토리얼 진행
- 컷신 재생
- 로딩 화면
- 대화 이벤트
- 게임 시작/종료 연출

#### InterruptionState

```csharp
public enum InterruptionState
{
    Undefined,   // 정의되지 않음
    RequireRun,  // 실행 대기 중
    Running,     // 실행 중
    Done         // 완료됨
}
```

#### IInterruption 인터페이스

```csharp
public interface IInterruption
{
    int Order { get; }                    // 실행 순서
    InterruptionState State { get; }      // 현재 상태
    IInterruptAt At { get; }              // 실행 시점
    UniTask RunAsync(CancellationToken ct);
}
```

#### InterruptAt 구현

특정 게임 상태에서 실행되도록 지정:

```csharp
public abstract record InterruptAtGameState(GameState State) : IInterruptAt
{
    public GameState State { get; } = State;
}

// 사용 예시
public record InterruptAtInit() : InterruptAtGameState(GameState.Initialization);
public record InterruptAtRunning() : InterruptAtGameState(GameState.Running);
```

#### Interruption 구현 방법

**1. Interruption 클래스 사용**

```csharp
var tutorialInterruption = new Interruption(
    at: new InterruptAtInit(),
    task: async () => 
    {
        await ShowTutorialAsync();
    },
    order: 100
);
```

**2. BaseInterruptionBehaviour 상속**

```csharp
public class TutorialInterruption : BaseInterruptionBehaviour
{
    public override int Order => 100;
    public override IInterruptAt At => new InterruptAtInit();

    protected override async UniTask RunAsyncInternal(CancellationToken ct)
    {
        // 튜토리얼 로직
        await ShowTutorialPanelAsync();
        await WaitForUserInputAsync(ct);
    }
}
```

**3. InterruptionFromCallback 사용**

```csharp
var callbackInterruption = new InterruptionFromCallback(
    order: 100,
    initialState: InterruptionState.RequireRun,
    at: new InterruptAtInit(),
    callback: (onComplete) =>
    {
        // 콜백 기반 로직
        StartCoroutine(SomeCoroutine(onComplete));
    }
);
```

#### 주요 특징

- **단일 실행 보장**: 한 번에 하나의 Interruption만 실행됩니다
- **순서 보장**: Order가 낮을수록 먼저 실행됩니다
- **업데이트 중단**: Interruption 실행 중에는 Update 시스템이 일시 정지됩니다
- **상태별 실행**: GameState에 따라 자동으로 필터링되어 실행됩니다

---

### Completion Actions

게임 종료 시 실행되는 정리 작업입니다.

#### ICompletionAction (동기)

```csharp
public interface ICompletionAction
{
    void Execute(IGameContext context);
}

// 구현 예시
public class SaveGameAction : ICompletionAction
{
    public void Execute(IGameContext context)
    {
        var result = context.GetResult();
        SaveToFile(result);
    }
}
```

#### IAsyncCompletionAction (비동기)

```csharp
public interface IAsyncCompletionAction
{
    UniTask ExecuteAsync(IGameContext context);
}

// 구현 예시
public class UploadScoreAction : IAsyncCompletionAction
{
    public async UniTask ExecuteAsync(IGameContext context)
    {
        var result = context.GetResult();
        await UploadToServerAsync(result);
    }
}
```

---

## 설치 및 설정

### 필수 의존성

- [UniTask](https://github.com/Cysharp/UniTask) (비동기 처리)

### 기본 설정

#### 1. Provider 구현

```csharp
public class GameServiceProvider : IFlowServiceProvider
{
    public IInterruptionProvider Interruption { get; }
    public ICompleteActionProvider CompleteAction { get; }
    public IUpdateGroupProvider UpdateGroup { get; }

    public GameServiceProvider()
    {
        // 각 Provider 초기화
        Interruption = new MyInterruptionProvider();
        CompleteAction = new MyCompleteActionProvider();
        UpdateGroup = new MyUpdateGroupProvider();
    }
}
```

#### 2. InterruptionProvider 구현

```csharp
public class MyInterruptionProvider : IInterruptionProvider
{
    public List<IInterruption> ProvideInterruptions()
    {
        return new List<IInterruption>
        {
            new Interruption(
                at: new InterruptAtInit(),
                task: async () => await LoadResourcesAsync(),
                order: 0
            ),
            // 추가 인터럽션들...
        };
    }
}
```

#### 3. UpdateGroupProvider 구현

```csharp
public class MyUpdateGroupProvider : IUpdateGroupProvider
{
    public IReadOnlyList<BaseUpdateGroup> GetUpdateGroups()
    {
        var playerSystem = new PlayerSystem();
        var enemySystem = new EnemySystem();
        
        return new List<BaseUpdateGroup>
        {
            new UpdateGroup(new[] { playerSystem, enemySystem }),
            new FixedUpdateGroup(new[] { /* 물리 시스템들 */ }),
            new LateUpdateGroup(new[] { /* 카메라 등 */ })
        };
    }
}
```

#### 4. CompleteActionProvider 구현

```csharp
public class MyCompleteActionProvider : ICompleteActionProvider
{
    public (IReadOnlyList<ICompletionAction>, IReadOnlyList<IAsyncCompletionAction>) GetActions()
    {
        var syncActions = new List<ICompletionAction>
        {
            new SaveGameAction()
        };
        
        var asyncActions = new List<IAsyncCompletionAction>
        {
            new UploadScoreAction()
        };
        
        return (syncActions, asyncActions);
    }
}
```

---

## 사용 예시

### 기본 사용법

```csharp
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameFlow _gameFlow;
    
    private async void Start()
    {
        // 컨텍스트 준비
        var context = new MyGameContext();
        var stateHandler = new MyGameStateHandler();
        var serviceProvider = new GameServiceProvider();
        
        // 의존성 주입
        _gameFlow.InitializeDependencies(context, stateHandler, serviceProvider);
        
        // 게임 시작
        await _gameFlow.RunAsync();
    }
    
    private async void OnApplicationQuit()
    {
        await _gameFlow.QuitAsync();
    }
}
```

### 튜토리얼 구현 예시

```csharp
public class TutorialInterruption : BaseInterruptionBehaviour
{
    [SerializeField] private TutorialUI _ui;
    
    public override int Order => 100;
    public override IInterruptAt At => new InterruptAtRunning();

    protected override async UniTask RunAsyncInternal(CancellationToken ct)
    {
        // 튜토리얼 UI 표시
        _ui.Show();
        
        // 단계별 진행
        await ShowStep1(ct);
        await ShowStep2(ct);
        await ShowStep3(ct);
        
        // 완료
        _ui.Hide();
    }
    
    private async UniTask ShowStep1(CancellationToken ct)
    {
        _ui.SetMessage("이동하려면 WASD를 누르세요");
        
        // 사용자가 이동할 때까지 대기
        await UniTask.WaitUntil(() => Input.anyKey, cancellationToken: ct);
    }
}
```

### 커스텀 Update System 구현

```csharp
[ManualUpdate(UpdateType.Update)]
[ManualUpdateOrder(100)]
public class PlayerMovementSystem : IUpdate
{
    public bool Enabled { get; set; }
    private Transform _playerTransform;
    private float _speed = 5f;
    
    public PlayerMovementSystem(Transform player)
    {
        _playerTransform = player;
    }
    
    public void Update(float deltaTime)
    {
        if (!Enabled) return;
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        _playerTransform.position += movement * _speed * deltaTime;
    }
}
```

### GameState 변경 감지

```csharp
public class MyGameStateHandler : IGameStateHandler
{
    public void OnGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Initialization:
                Debug.Log("게임 초기화 중...");
                break;
                
            case GameState.Running:
                Debug.Log("게임 시작!");
                // UI 표시, BGM 재생 등
                break;
                
            case GameState.End:
                Debug.Log("게임 종료");
                // 결과 화면 표시 등
                break;
        }
    }
}
```

---

## API 레퍼런스

### Core Classes

#### GameFlow

| 메서드 | 설명 |
|--------|------|
| `InitializeDependencies(IGameContext, IGameStateHandler, IFlowServiceProvider)` | 의존성 주입 |
| `RunAsync()` | 게임 시작 (Initialization → Running) |
| `QuitAsync()` | 게임 종료 및 완료 액션 실행 |
| `State` | 현재 게임 상태 (읽기 전용) |

#### BaseUpdateGroup

| 속성/메서드 | 설명 |
|-------------|------|
| `Systems` | 등록된 IUpdate 목록 |
| `UpdateType` | Update 타입 (Update/FixedUpdate/LateUpdate) |
| `DeltaTime` | 마지막 프레임의 deltaTime |
| `Time` | 누적 시간 |
| `Update(float)` | 모든 시스템 업데이트 |

### Interfaces

#### IUpdate

```csharp
bool Enabled { get; set; }  // 활성화 여부
void Update(float deltaTime);
```

#### IInterruption

```csharp
int Order { get; }                       // 실행 순서
InterruptionState State { get; }         // 현재 상태
IInterruptAt At { get; }                 // 실행 시점
UniTask RunAsync(CancellationToken ct);  // 실행 메서드
```

#### ICompletionAction / IAsyncCompletionAction

```csharp
void Execute(IGameContext context);              // 동기 실행
UniTask ExecuteAsync(IGameContext context);      // 비동기 실행
```

### Attributes

#### ManualUpdateAttribute

```csharp
[ManualUpdate(UpdateType.Update)]
```

Update 타입을 지정합니다.

#### ManualUpdateOrderAttribute

```csharp
[ManualUpdateOrder(100)]
```

실행 순서를 지정합니다 (낮을수록 먼저 실행).

---

## 주의사항

### Interruption 실행 중 업데이트 중단

Interruption이 실행되는 동안 모든 Update 시스템이 일시 정지됩니다. 이는 의도된 동작이며, 튜토리얼이나 컷신 중에 게임 로직이 실행되지 않도록 하기 위함입니다.

### CancellationToken 처리

모든 비동기 작업은 CancellationToken을 올바르게 처리해야 합니다:

```csharp
protected override async UniTask RunAsyncInternal(CancellationToken ct)
{
    await SomeAsyncOperation()
        .AttachExternalCancellation(ct)
        .SuppressCancellationThrow();
}
```

### Update 시스템 순서

같은 Order를 가진 시스템의 실행 순서는 보장되지 않습니다. 명확한 순서가 필요한 경우 서로 다른 Order 값을 사용하세요.

### FixedUpdate deltaTime

FixedUpdate에서는 `Time.fixedDeltaTime`이 자동으로 사용됩니다.

---
