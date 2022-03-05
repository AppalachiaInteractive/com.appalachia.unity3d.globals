using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    namespace Properties
    {
        [Serializable]
        public abstract class ShaderProperty<T, TValue> : AppalachiaBase<T>, IShaderProperty
            where T : ShaderProperty<T, TValue>, new()
        {
            protected ShaderProperty()
            {
            }

            protected ShaderProperty(string propertyName, Object owner) : base(owner)
            {
                _propertyName = propertyName;
                _propertyID = Shader.PropertyToID(_propertyName);
            }

            protected ShaderProperty(string propertyName, TValue value, Object owner) : base(owner)
            {
                _propertyName = propertyName;
                _propertyID = Shader.PropertyToID(_propertyName);
                _value = value;
            }

            #region Fields and Autoproperties

            public AppaEvent<IShaderProperty>.Data ValueChanged;

            [SerializeField, HideInInspector]
            private string _propertyName;

            [SerializeField, HideInInspector]
            private int _propertyID;

            [OnValueChanged(nameof(OnChanged))]
            [SerializeField]
            private TValue _value;

            [NonSerialized] private bool _hasSubcribedToUpdates;

            #endregion

            public TValue Value
            {
                get => _value;
                set
                {
                    /*
                    if (Equals(_value, value))
                    {
                        return;
                    }
                    */

                    _value = value;
                    OnChanged();
                }
            }

            public void ValidatePropertyName(string propertyName)
            {
                using (_PRF_ValidatePropertyName.Auto())
                {
                    if (string.IsNullOrWhiteSpace(_propertyName))
                    {
                        _propertyName = propertyName;
                        SetPropertyID();
                        OnChanged();
                    }
                }
            }

            public bool HasSubcribedToUpdates
            {
                get => _hasSubcribedToUpdates;
                set => _hasSubcribedToUpdates = value;
            }

            /// <inheritdoc />
            protected override async AppaTask Initialize(Initializer initializer)
            {
                await base.Initialize(initializer);

                using (_PRF_Initialize.Auto())
                {
                    if (_propertyName != null)
                    {
                        SetPropertyID();
                    }

                    Changed.Event += OnValueChanged;
                }
            }

            private void OnValueChanged()
            {
                using (_PRF_OnValueChanged.Auto())
                {
                    ValueChanged.RaiseEvent(this);
                }
            }

            private void SetPropertyID()
            {
                using (_PRF_SetPropertyID.Auto())
                {
                    _propertyID = Shader.PropertyToID(_propertyName);
                }
            }

            #region IShaderProperty Members

            public override string Name => _propertyName;

            public int PropertyId
            {
                get
                {
                    using (_PRF_PropertyId.Auto())
                    {
                        if (_propertyID == 0)
                        {
                            SetPropertyID();
                            OnChanged();
                        }

                        return _propertyID;
                    }
                }
            }

            public abstract void ApplyToMaterial(Material m);
            public abstract void InitializeFromMaterial(Material m);

            #endregion

            #region Profiling

            protected static readonly ProfilerMarker _PRF_ApplyToMaterial =
                new ProfilerMarker(_PRF_PFX + nameof(ApplyToMaterial));

            protected static readonly ProfilerMarker _PRF_InitializeFromMaterial =
                new ProfilerMarker(_PRF_PFX + nameof(InitializeFromMaterial));

            private static readonly ProfilerMarker _PRF_OnValueChanged =
                new ProfilerMarker(_PRF_PFX + nameof(OnValueChanged));

            private static readonly ProfilerMarker _PRF_PropertyId = new ProfilerMarker(_PRF_PFX + nameof(PropertyId));

            private static readonly ProfilerMarker _PRF_SetPropertyID =
                new ProfilerMarker(_PRF_PFX + nameof(SetPropertyID));

            private static readonly ProfilerMarker _PRF_ValidatePropertyName =
                new ProfilerMarker(_PRF_PFX + nameof(ValidatePropertyName));

            #endregion
        }
    }
}
