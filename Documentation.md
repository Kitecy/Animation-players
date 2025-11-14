# 🎬 Animation Players Documentation

This package gives you the opportunity to use
simple DOTween animations without writing almost any code.

## 🚀 Install

You can install this package using a special `https://github.com/Kitecy/Animation-players.git`.

1. Open Unity
2. Click the `Windows` button
3. Open the 'Package Manager` window
4. Press the `+` button
5. Select `Install package from git URL...`
6. Specify the link above.

You also need to install [DOTween](http://dotween.demigiant.com/) in your project for the package to work. and in its settings (`Tools/Demigiant`), click the "Create ASMDEF..." button. You can install the package from the [AssetStore](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)

You also need to install the [UniTask](https://github.com/Cysharp/UniTask?tab=readme-ov-file#install-via-git-url) package in your project.

### Interfaces
`IAnimationPlayer`

`IReadOnlyAnimation` 

`ITrigger`

### Abstracts
`BasePlayer`

### Classes
`Animation`

`SimpleAnimationPlayer`

`AnimationsPlayer`

`AnimationPlayersQueue`

`GroupedAnimationsPlayers`

`TriggeredAnimationPlayer`

`DynamicAnimationPlayer`

`BaseTrigger`

`TriggerObject`

## Interfaces

### `IAnimationPlayer`

This interface allows you to create your own player or separate all the others.

|Methods|Description|
|-------|----------|
|```Play(Action onCompleteCallback)```|Play all animations|
|```AsyncPlay()```|Play all animations asynchronously|
|```Prepare()```|Prepares the player for animation playback|

### `IReadOnlyAnimation`

This interface allows you to access all the public properties of Animation without giving access to change it.
You can read more about its properties in `Animation`.

### `ITrigger`

Using this interface, you can create your own Trigger that will allow you to work with TriggeredAnimationPlayer the way you need it. It is recommended to create your own unique triggers using the BaseTrigger class (read more in the classes section).

|Events|Description|
|-------|----------|
|```Triggered```|An event that is triggered at a necessary and specified moment.|

|Methods|Description|
|-------|----------|
|```Invoke()```|Triggers an event that other objects can subscribe to.|

## Abstracts

### `BasePlayer : IAnimationPlayer`

This component is basic for all animation players. You may need it to separate all players or create your own.

|Methods|Description|
|-------|----------|
|```Play(Action onCompleteCallback)```|Play all animations|
|```AsyncPlay(CancellationToken token)```|Play all animations asynchronously|
|```Prepare()```|Prepares the player for animation playback|
|```protected CancellationToken GetOnDisableCancellationToken()```|Returns the cancellation token for the OnDisable method. All methods that receive it will stop executing when the object is turned off.|
|```protected CancellationTokenSource CombineTokensWithOnDisableToken(params CancellationToken[] tokens)```|Combines the specified tokens with the shutdown token in OnDisable and returns a new source. Excludes all repetitions of tokens.|


|Field|Description|
|-------|----------|
|```bool IsUI```|Set the value to true if the object is part of the UI.|
|```AutoCall AutoCall```|set one of the values if you want to start the animation automatically in Awake or OnEnable. Leave None if you don't need it.|

|Property|Description|
|-------|----------|
|```IsUsingInUI```|Returns the value of the IsUI field.|

#### Example

```csharp
public class MyPlayer : BasePlayer
{
    public override void Play(Action onCompleteCallback)
    {
        //Do something

        onCompleteCallback?.Invoke();
    }

    public override async UniTask AsyncPlay()
    {
        //Do something and wait;
    }
}
```
---
## Classes

### `Animation`

The object is used to represent the required animation.

|Properties|Description|
|----------|-----------|
|`string Name`|Animation name|
|`int Order`|Animation order|
|`float Duration`|Animation duration|
|`float Delay`|Delay in animation playback|
|`float TotalDuration`|The sum of the animation duration and its delay. Read-only|
|`int IsEternalLoop`|The animation becomes infinite if set to true.|
|`int Loops`|The number of animation cycles (at -1, the animation repeats indefinitely)|
|`LoopType LoopType`|Loops type|
|`Ease Ease`|Ease type|
|`Type Sort`|Animation type|
|`Vector3 StartPosition`|Starting position for position animation|
|`Vector3 EndPosition`|The final position to animate the position|
|`Vector3 StartScale`|Starting scale for scale animation|
|`Vector3 EndScale`|The final scale to animate the scale|
|`Vector3 StartRotation`|Starting rotation for rotation animation|
|`Vector3 EndRotation`|The final rotation to animate the rotation|
|`float StartFade`|Initial value for fade animation|
|`float EndFade`|The final value for the fade animation|
|`Renderer Renderer`|The Renderer component for 2d and 3d color animation|
|`Graphic Graphic`|Graphic component for UI color animation|
|`Color StartColor`|Starting color for color animation|
|`Color EndColor`|The final color to animate the color|
---
|Methods|Description|
|-------|----------|
|```Convert(BasePlayer player, bool isUI, CancellationToken token)```|The extension method of the `AnimationExtensions` class. Converts `Animation` to `Tween`|
|```Prepare(BasePlayer player, bool isUI)```|The extension method of the `AnimationExtensions` class. Sets the starting value for the player from the animation|
|```SetName(string value)```|Sets a new value for the "Name" parameter|
|```SetOrder(int value)```|Sets a new value for the "Order" parameter|
|```SetDuration(float value)```|Sets a new value for the "Duration" parameter|
|```SetDelay(float value)```|Sets a new value for the "Delay" parameter|
|```SetIsEternalLoop(bool value)```|Sets a new value for the "IsEternalLoop" parameter|
|```SetLoops(int value)```|Sets a new value for the "Loops" parameter|
|```SetLoopType(LoopType value)```|Sets a new value for the "LoopType" parameter|
|```SetEase(Ease value)```|Sets a new value for the "Ease" parameter|
|```SetType(Animation.Type value)```|Sets a new value for the "Sort" parameter|
|```SetStartPosition(Vector3 value)```|Sets a new value for the "StartPosition" parameter|
|```SetEndPosition(Vector3 value)```|Sets a new value for the "EndPosition" parameter|
|```SetStartRotation(Vector3 value)```|Sets a new value for the "StartRotation" parameter|
|```SetEndRotation(Vector3 value)```|Sets a new value for the "EndRotation" parameter|
|```SetStartScale(Vector3 value)```|Sets a new value for the "StartScale" parameter|
|```SetEndScale(Vector3 value)```|Sets a new value for the "EndScale" parameter|
|```SetStartColor(Color value)```|Sets a new value for the "StartColor" parameter|
|```SetEndColor(Color value)```|Sets a new value for the "EndColor" parameter|
|```SetRenderer(Renderer value)```|Sets a new value for the "Renderer" parameter|
|```SetGraphic(Graphic value)```|Sets a new value for the "Graphic" parameter|
|```SetStartFade(float value)```|Sets a new value for the "StartFade" parameter|
|```SetEndFade(float value)```|Sets a new value for the "EndFade" parameter|
|```SetStartAnchoredPosition(Vector3 value)```|Sets a new value for the "StartAnchorPosition" parameter|
|```SetEndAnchoredPosition(Vector3 value)```|Sets a new value for the "EndAnchorPosition" parameter|
---

### `SimpleAnimationPlayer : BasePlayer`

The component is used to create a single animation on an object. The name and order of the animation are not specified in this case. You can achieve the same effect using AnimationPlayer, but for more convenience, we recommend using this component.

|Methods|Description|
|-------|----------|
|```Play(Action onAnimationEnded = null)```|Play animation|
|```AsyncPlay(CancellationToken token)```|Play animation asynchronously|
|```Prepare()```|Prepares the player for animation playback|

|Properties|Description|
|-------|----------|
|```IReadOnlyAnimation Animation```|A read-only animation instance|

#### Example

```csharp
public class Example : MonoBehaviour 
{
    [SerializeField] private SimpleAnimationPlayer _player;

    private void Start() 
    {
        Play();
        PlayWithCallback();
    } 

    //It just plays the animation.
    private void Play() 
    {
        _player.Play();
    }

    //When the animation is completed, a message will be displayed
    private void PlayWithCallback() 
    {
        _player.Play(() => print("Ended"));
    }
}
```

### `AnimationsPlayer : BasePlayer`

The component is used to play multiple animations. You can play them simultaneously or one after the other, as well as play them in groups.

|Methods|Description|
|-------|----------|
|```Play(Action onAnimationEnded = null)```|Play all animations|
|```Play(string name, Action onAnimationEnded = null)```|Play a specific animation from the list (only the first animation with that name will be played)|
|```AsyncPlay(CancellationToken token)```|Play all animations asynchronously|
|```AsyncPlay(string name, CancellationToken token)```|Play a specific animation asynchronously from the list (only the first animation with that name will be played)|
|```Prepare()```|Prepares the player for animation playback|

|Properties|Description|
|-------|----------|
|```IReadOnlyList<IReadOnlyAnimation> Animations```|A read-only animations list instance|

#### Example

```csharp
public class Example : MonoBehaviour 
{
    [SerializeField] private AnimationsPlayer _player;

    private void Start() 
    {
        Play();
        PlayWithCallback();
    } 

    //It just plays the all animations.
    private void Play() 
    {
        _player.Play();
    }

    //When the animations is completed, a message will be displayed
    private void PlayWithCallback() 
    {
        _player.Play(() => print("Ended"));
    }
}
```

### `AnimationPlayersQueue : BasePlayer`

Replays all the `BasePlayer` one after another, waiting for each one to be completed.

|Methods|Description|
|-------|----------|
|```Play(Action onAnimationEnded = null)```|Launches all the players one after the other|
|```AsyncPlay(CancellationToken token)```|Launches all the players one after the other asynchronously|
|```Prepare()```|Prepares the players for animation playback|

#### Example
```csharp
public class Example : MonoBehaviour 
{
    [SerializeField] private AnimationQueuePlayer _player;

    private void Start() 
    {
        _player.Play(); //A regular call
        _player.Play(OnAnimationsEnded); //Calling the method with callback
    }

    private void OnAnimationsEnded() 
    {
        print("Ended");
    }
}
```
---
### `AnimationPlayersGroup`

This component calls all child players one after the other immediately or at a specified interval (it can be changed in runtime via the inspector). You can also create any player on the same object, and after it finishes playing, all the others will be played.

|Methods|Description|
|-------|----------|
|```Play(Action onAnimationEnded = null)```|Play all animations|
|```AsyncPlay(CancellationToken token)```|Play all animations asynchronously|
|```Prepare()```|Prepares the player for animation playback|

#### Example

```csharp
public class Example : MonoBehaviour 
{
    [SerializeField] private AnimationPlayersGroup _playersGroup;

    //It just plays the all animations.
    private void Start() 
    {
        _playersGroup.Play();
    }
}
```

### `DynamicAnimationPlayer`

This component is necessary if you don't know which animation you want to start or want to generate it in runtime. In this case, you add this component to the required object and start an animation for it during the game.

|Methods|Description|
|-------|----------|
|```SetAnimation(Animation animation)```|Sets the animation for playback. Call this method before starting the animation!|
|```Play(Action onAnimationEnded = null)```|Play animation|
|```AsyncPlay(CancellationToken token)```|Play animation asynchronously|
|```Prepare()```|Prepares the player for animation playback|

```csharp
public class Example : MonoBehaviour
{
    [SerializeField] private DynamicAnimationPlayer _player;
    [SerializeField] private Animation _animation;
    [SerializeField] private Animation _animation2;

    private void Start() 
    {
        _player.SetAnimation(_animation);
        _player.Play(() => {
            _player.SetAnimation(_animation2);
            _player.Play();
        })
    }
}
```

### `TriggeredAnimationPlayer`

This component allows you to trigger certain animations when certain triggers are called. The component itself subscribes to their call (you only need to specify what exactly to subscribe to). You can also specify which animation to play for a specific trigger. Please note that in this version, only the animation of the last trigger is played, that is, if two are called at the same time, the one that was later will be played! Animation triggering methods work only after the first trigger call and repeat the last animation.

|Fields|Description|
|-------|----------|
|```List<TriggerObject> Triggers```|A list that stores all trigger animation pairs|

|Methods|Description|
|-------|----------|
|```AddTrigger(BaseTrigger trigger, Animation animation)```|Creates a new subscription in runtime|
|```Play(Action onAnimationEnded = null)```|Play animation|
|```AsyncPlay(CancellationToken token)```|Play animation asynchronously|
|```Prepare()```|Prepares the player for animation playback|

```csharp
public class TriggerExample : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private BaseTrigger _trigger; //The trigger that the players are subscribed to

    private void Start()
    {
        _button.onClick.AddListener(() => _trigger.Invoke()); //Notification when the button is pressed that the trigger has been triggered
    }
}
```

You can use this for pressing a button (as in the example above), as well as for a variety of unique situations, and you can also create your own unique trigger by inheriting it from BaseTrigger.

### `BaseTrigger`

This component is necessary for TriggeredAnimationPlayer to work. You can call it in any situations you need and thus play all the players that are subscribed to it. You can also write your own unique trigger by inheriting from BaseTrigger.

|Methods|Description|
|-------|----------|
|```public Invoke()```|A method for calling an event from outside|
|```protected InternalInvoke()```|A method for interacting with an event call with inherited elements|

```csharp
public class Example : MonoBehaviour
{
    [SerializeField] private BaseTrigger _trigger;

    private void Update() 
    {
        if (Time.time == 10)
            _trigger.Invoke(); // When 10 seconds have passed, animations subscribed to this trigger will start.
    }
}
```

### `TriggerObject`

This object just saves a couple of triggers and animations.

|Fields|Description|
|-------|----------|
|```BaseTrigger Trigger```|It`s just a trigger|
|```Animation Animation```|It`s just a animation|

```csharp
public class Example : MonoBehaviour
{
    [SerializeField] private BaseTrigger _trigger;
    [SerializeField] private Animation _animation;

    private TriggerObject _object

    private void Start()
    {
        _object = new TriggerObject(_trigger, _animation); // Now it just keeps them together.
    }
}
```
