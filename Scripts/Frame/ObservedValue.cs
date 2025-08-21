using System;
using UnityEngine;

namespace TDKToolkit
{
    [System.Serializable]
    public class ObservedValue<T>
    {
        [SerializeField]
        T _value;
        public Action<T, T> onValueChanged;
        public Action<T> valueChanged;
        public ObservedValue() { }
        public ObservedValue(T value)
        {
            _value = value;
        }
        public void ImplicitSetValue(T value) { _value = value; }
        public T Value
        {
            get { return _value; }
            set
            {
                onValueChanged?.Invoke(_value, value);
                valueChanged?.Invoke(value);
                _value = value;
            }

        }



    }
}
