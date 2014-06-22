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
                Value = value > MaxValue ? MaxValue : value < MinValue ? MinValue : value; //meh turnaryaryaryaryary

                if (previous == Value)
                {
                    return; //don't fire off events if nothing happened...
                }

                if (OnValueChanged != null)
                {
                    OnValueChanged(this, new ConstrainedValueEventArgs(Value, previous));
                }

                if (Value == MaxValue && OnFilled != null)
                {
                    OnFilled(this, new ConstrainedValueEventArgs(Value, previous));
                }
                else if (Value == MinValue && OnDepleted != null)
                {
                    OnDepleted(this, new ConstrainedValueEventArgs(Value, previous));
                }
            }
        }

        public float _maxValue;
        public float MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                var prev = _maxValue;
                MaxValue = value;

                if (OnMaxValueChanged != null)
                {
                    OnMaxValueChanged(this, new ConstrainedValueEventArgs(MaxValue, prev));
                }
            }
        }

        public float _minValue;
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
                    OnMinValueChanged(this, new ConstrainedValueEventArgs(MinValue, prev));
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
