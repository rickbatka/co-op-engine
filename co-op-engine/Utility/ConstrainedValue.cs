using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Utility
{
    public class ConstrainedValue
    {
        private float _value;
        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                var previous = _value;
                _value = value > MaxValue ? MaxValue : value < MinValue ? MinValue : value; //meh turnaryaryaryaryary

                if (previous == _value)
                {
                    return; //don't fire off events if nothing happened...
                }

                if (OnValueChanged != null)
                {
                    OnValueChanged(this, new ConstrainedValueEventArgs(_value, previous));
                }

                if (_value == MaxValue && OnFilled != null)
                {
                    OnFilled(this, new ConstrainedValueEventArgs(_value, previous));
                }
                else if (_value == MinValue && OnDepleted != null)
                {
                    OnDepleted(this, new ConstrainedValueEventArgs(_value, previous));
                }
            }
        }

        private float _maxValue;
        public float MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                var prev = _maxValue;
                _maxValue = value;

                if (OnMaxValueChanged != null)
                {
                    OnMaxValueChanged(this, new ConstrainedValueEventArgs(_maxValue, prev));
                }
            }
        }

        private float _minValue;
        public float MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                var prev = _minValue;
                _minValue = value;

                if (OnMinValueChanged != null)
                {
                    OnMinValueChanged(this, new ConstrainedValueEventArgs(_minValue, prev));
                }
            }
        }

        public event EventHandler<ConstrainedValueEventArgs> OnMaxValueChanged;
        public event EventHandler<ConstrainedValueEventArgs> OnMinValueChanged;
        public event EventHandler<ConstrainedValueEventArgs> OnValueChanged;
        public event EventHandler<ConstrainedValueEventArgs> OnDepleted;
        public event EventHandler<ConstrainedValueEventArgs> OnFilled;

        public ConstrainedValue(float min, float max, float initial = 0f)
        {
            _value = initial;
            _maxValue = max;
            _minValue = min;
        }

        public static implicit operator float(ConstrainedValue cv)
        {
            return cv.Value;
        }
        
        public static implicit operator int(ConstrainedValue cv)
        {
            return (int)cv.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class ConstrainedValueEventArgs : EventArgs
    {
        public float NewValue;
        public float OldValue;

        public ConstrainedValueEventArgs(float newValue, float oldValue)
        {
            NewValue = newValue;
            OldValue = oldValue;
        }
    }

}
