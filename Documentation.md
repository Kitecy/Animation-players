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

### Abstracts
`BasePlayer`

### Classes
`Animation`
`SimpleAnimationPlayer`
`AnimationsPlayer`
`AnimationPlayersQueue`
`GroupedAnimationsPlayers`

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

## Abstracts

### `BasePlayer : IAnimationPlayer`

This component is basic for all animation players. You may need it to separate all players or create your own.

|Methods|Description|
|-------|----------|
|```Play(Action onCompleteCallback)```|Play all animations|
|```AsyncPlay()```|Play all animations asynchronously|
|```Prepare()```|Prepares the player for animation playback|

|Field|Description|
|-------|----------|
|```IsUI```|Set the value to true if the object is part of the UI.|
|```PlayOnAwake```|Set the value to true if you want the animation to be played in the Awake method.|
|```PlayOnEnable```|Set the value to true if you want the animation to be played in the OnEnable method.|

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
|```Convert(BasePlayer player, bool isUI)```|The extension method of the `AnimationExtensions` class. Converts `Animation` to `Tween`|
|```Prepare(BasePlayer player, bool isUI)```|The extension method of the `AnimationExtensions` class. Sets the starting value for the player from the animation|
---

### `SimpleAnimationPlayer : BasePlayer`

The component is used to create a single animation on an object. The name and order of the animation are not specified in this case. You can achieve the same effect using AnimationPlayer, but for more convenience, we recommend using this component.

|Methods|Description|
|-------|----------|
|```Play(Action onAnimationEnded = null)```|Play animation|
|```AsyncPlay()```|Play animation asynchronously|
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
|```AsyncPlay()```|Play all animations asynchronously|
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
|```Play(Action onAnimationEnded = null)```|Play all animations|
|```AsyncPlay()```|Play all animations asynchronously|
|```Prepare()```|Prepares the player for animation playback|

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
|```AsyncPlay()```|Play all animations asynchronously|
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