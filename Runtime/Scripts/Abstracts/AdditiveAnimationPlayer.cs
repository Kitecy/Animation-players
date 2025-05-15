namespace AnimationPlayers
{
    using DG.Tweening;
    using System;
    using System.Threading.Tasks;
    using UnityEngine;

    public abstract class AdditiveAnimationPlayer : MonoBehaviour
    {
        protected const float ColorsDivider = 2f;

        [SerializeField] protected AdditiveAnimation TargetAnimation = new();

        protected Transform CurrentTransform;

        private void Awake()
        {
            CurrentTransform = transform;
        }

        private void OnEnable()
        {
            if (TargetAnimation.PlayOnEnable)
                Add();
        }

        public void Add()
        {
            switch (TargetAnimation.Type)
            {
                case AdditiveAnimation.AdditiveAnimationType.Position:
                    AddPosition();
                    break;

                case AdditiveAnimation.AdditiveAnimationType.Scale:
                    AddScale();
                    break;

                case AdditiveAnimation.AdditiveAnimationType.Rotation:
                    AddRotation();
                    break;

                case AdditiveAnimation.AdditiveAnimationType.Color:
                    AddColor();
                    break;
            }
        }

        public async void AsyncAdd(Action onCompleteCallback)
        {
            switch (TargetAnimation.Type)
            {
                case AdditiveAnimation.AdditiveAnimationType.Position:
                    await AsyncAddPosition();
                    break;

                case AdditiveAnimation.AdditiveAnimationType.Scale:
                    await AsyncAddScale();
                    break;

                case AdditiveAnimation.AdditiveAnimationType.Rotation:
                    await AsyncAddRotation();
                    break;

                case AdditiveAnimation.AdditiveAnimationType.Color:
                    await AsyncAddColor();
                    break;
            }

            onCompleteCallback?.Invoke();
        }

        protected abstract Task AsyncAddColor();

        protected abstract void AddColor();

        private void AddPosition()
        {
            Vector3 newPosition = CurrentTransform.position + TargetAnimation.AdditivePosition;
            CurrentTransform.DOMove(newPosition, TargetAnimation.Duration).SetDelay(TargetAnimation.Delay);
        }

        private async Task AsyncAddPosition()
        {
            Vector3 newPosition = CurrentTransform.position + TargetAnimation.AdditivePosition;
            Tween tween = CurrentTransform.DOMove(newPosition, TargetAnimation.Duration).SetDelay(TargetAnimation.Delay);

            await tween.AsyncWaitForCompletion();
        }

        private void AddScale()
        {
            Vector3 newScale = CurrentTransform.localScale + TargetAnimation.AdditiveScale;
            CurrentTransform.DOScale(newScale, TargetAnimation.Duration).SetDelay(TargetAnimation.Delay);
        }

        private async Task AsyncAddScale()
        {
            Vector3 newScale = CurrentTransform.localScale + TargetAnimation.AdditiveScale;
            Tween tween = CurrentTransform.DOScale(newScale, TargetAnimation.Duration).SetDelay(TargetAnimation.Delay);

            await tween.AsyncWaitForCompletion();
        }

        private void AddRotation()
        {
            Vector3 newRotation = transform.rotation.eulerAngles + TargetAnimation.AdditiveRotation;
            CurrentTransform.DORotate(newRotation, TargetAnimation.Duration).SetDelay(TargetAnimation.Delay);
        }

        private async Task AsyncAddRotation()
        {
            Vector3 newRotation = transform.rotation.eulerAngles + TargetAnimation.AdditiveRotation;
            Tween tween = CurrentTransform.DORotate(newRotation, TargetAnimation.Duration).SetDelay(TargetAnimation.Delay);

            await tween.AsyncWaitForCompletion();
        }
    }
}
