using UnityEngine;

namespace CodeBase.Behaviours.Player.Remote.Lerp
{
    public abstract class GenericLerp<T> : ILerpable<T>
    {
        private readonly float _lerpTime;

        private T _currentLerpValue;
        
        private bool _needToLerp;
        private T _initialValue;
        private T _targetValue;
        private float _timer;

        protected GenericLerp(float lerpTime)
        {
            _lerpTime = lerpTime;
        }

        public void SetNewCurrentAndTarget(T current, T target)
        {
            _initialValue = current;
            _targetValue = target;
            _timer = 0;
            _needToLerp = true;
        }

        public T CalculateLerpValue(float deltaTime)
        {
            if (!_needToLerp)
            {
                return _currentLerpValue;
            }

            _currentLerpValue = LerpFunction(_initialValue, _targetValue, _timer / _lerpTime);
            _timer += Time.deltaTime;

            if (_timer >= _lerpTime)
            {
                _currentLerpValue = _targetValue;
                _needToLerp = false;
            }

            return _currentLerpValue;
        }

        protected abstract T LerpFunction(T initialValue, T targetValue, float time);
    }
}