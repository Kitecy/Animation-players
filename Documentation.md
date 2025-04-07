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

## Classes

### Abstracts
1. AnimationPlayer
2. SimpleAnimationPlayer

---
### Objects
1. Animation

### AnimationPlayer components
1. AnimationPlayer2D
2. AnimationPlayer3D
3. AnimationPlayerUI
---
### SimpleAnimationPlayer components
1. Simple2DAnimationPlayer
2. Simple3DAnimationPlayer
3. SimpleUIAnimationPlayer
---
### Other components
1. AnimationQueuePlayer 

## Abstracts

### AnimationPlayer.cs

The component is used to create multiple animations on the same object. You can specify their order and duration. You can also specify multiple animations in the same order to play them at the same time. The animations of the next order will start playing after the longest animation of the previous order is completed.

|Methods|Description|
|-------|----------|
|```Play(string animationName, Action onAnimationEnded = null)```|Play one animation|
|```Play(int order, Action onAnimationEnded = null)```|Play all animations in a certain order|
|```PlayAll(Action onAnimationEnded = null)```|Play all animations|
|```AsyncPlay(string animationName)```|Play one animation asynchronously|
|```AsyncPlayAll()```|Play all animations asynchronously|

### Example

```csharp
public class Example : MonoBehaviour
{
    [SerializeField] private AnimationPlayer _player;

    private void Start()
    {
        _player.Play(() => print("Ended"));
    }
}
```
---
### SimpleAnimationPlayer.cs

The component is used to create a single animation on an object. The name and order of the animation are not specified in this case. You can achieve the same effect using AnimationPlayer, but for more convenience, we recommend using this component.

|Methods|Description|
|-------|----------|
|```Play(Action onAnimationEnded = null)```|Play animation|

### Example

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

## Objects

### `Animation`

The object is used to represent the required animation.

|Properties|Description|
|----------|-----------|
|`string Name`|Animation name|
|`int Order`|Animation order|
|`float Duration`|Animation duration|
|`bool PlayOnEnable`|Whether to enable animation when activating an object|
|`AnimationType Type`|Animation type|
|`Vector3 StartPosition`|Starting position for position animation|
|`Vector3 EndPosition`|The final position to animate the position|
|`Vector3 StartScale`|Starting scale for scale animation|
|`Vector3 EndScale`|The final scale to animate the scale|
|`Vector3 StartRotation`|Starting rotation for rotation animation|
|`Vector3 EndRotation`|The final rotation to animate the rotation|
|`Renderer Renderer`|The Renderer component for 3d color animation|
|`SpriteRenderer SpriteRenderer`|SpriteRenderer component for 2d color animation|
|`Graphic Graphic`|Graphic component for UI color animation|
|`Color StartColor`|Starting color for color animation|
|`Color EndColor`|The final color to animate the color|
---
|Methods|Description|
|-------|----------|
|```SetStartPosition(Vector3 position)```|Sets the value for StartPosition|
|```SetEndPosition(Vector3 position)```|Sets the value for EndPosition|

## AnimationPlayer components

###  `AnimationPlayer2D : AnimationPlayer`

The component is designed to interact with 2D objects. Using it with 3D or UI objects can lead to unexpected errors! Inherits methods of the `AnimationPlayer` class.

### Example
```csharp
public class Example : MonoBehaviour 
{
    private const string AnimationName = "Base2DAnimation";

    [SerializeField] private AnimationPlayer2D _player2D;

    private void Start() 
    {
        _player2D.Play(AnimationName);
    }
}
```
---
### `AnimationPlayer3D : AnimationPlayer`

The component is designed to interact with 3D objects. Using it with 2D or UI objects can lead to unexpected errors! Inherits methods of the `AnimationPlayer` class.

### Example
```csharp
public class Example : MonoBehaviour 
{
    private const string AnimationName = "Base3DAnimation";

    [SerializeField] private AnimationPlayer3D _player3D;

    private void Start() 
    {
        _player3D.Play(AnimationName);
    }
}
```
---
### `AnimationPlayerUI : AnimationPlayer`

The component is designed to interact with UI objects. Using it with 2D or 3D objects can lead to unexpected errors! Inherits methods of the `AnimationPlayer` class. The position for using the position animation type is specified relative to the lower-left corner of the canvas.

### Example
```csharp
public class Example : MonoBehaviour 
{
    private const string AnimationName = "BaseUIAnimation";

    [SerializeField] private AnimationPlayerUI _playerUI;

    private void Start() 
    {
        _playerUI.Play(AnimationName);
    }
}
```

## SimpleAnimationPlayer components

### `Simple2DAnimationPlayer : SimpleAnimationPlayer`

The component is designed to interact with 2D objects. Using it with 3D or UI objects can lead to unexpected errors! Inherits methods of the `SimpleAnimationPlayer` class

### Example
```csharp
public class Example : MonoBehaviour 
{
    private const string AnimationName = "Base2DAnimation";

    [SerializeField] private Simple2DAnimationPlayer _player2D;

    private void Start() 
    {
        _player2D.Play(AnimationName);
    }
}
```
---
### `Simple3DAnimationPlayer : SimpleAnimationPlayer`

The component is designed to interact with 3D objects. Using it with 2D or UI objects can lead to unexpected errors! Inherits methods of the `SimpleAnimationPlayer` class

### Example
```csharp
public class Example : MonoBehaviour 
{
    private const string AnimationName = "Base3DAnimation";

    [SerializeField] private Simple3DAnimationPlayer _player3D;

    private void Start() 
    {
        _player3D.Play(AnimationName);
    }
}
```
---
### `SimpleUIAnimationPlayer : SimpleAnimationPlayer`

The component is designed to interact with UI objects. Using it with 2D or 3D objects can lead to unexpected errors! Inherits methods of the `SimpleAnimationPlayer` class.

### Example
```csharp
public class Example : MonoBehaviour 
{
    private const string AnimationName = "BaseUIAnimation";

    [SerializeField] private SimpleUIAnimationPlayer _playerUI;

    private void Start() 
    {
        _playerUI.Play(AnimationName);
    }
}
```

## Other components

### `AnimationQueuePlayer`

The component plays all the `AnimationPlayer` in order one after the other (waits until all the animations of the previous `AnimationPlayer` are completed and only then starts the next one).

|Methods|Description|
|-------|----------|
|```Play(Action onAnimationEnded = null)```|Plays the queue of all AnimationPlayer |

### Example
```csharp
public class Example : MonoBehaviour 
{
    [SerializeField] private AnimationQueuePlayer _player;

    private void Start() 
    {
        _player.Play(); //A regular call
        _player.Play(OnAnimationEnded); //Calling the method with callback
    }

    private void OnAnimationEnded() 
    {
        print("Ended");
    }
}
```
