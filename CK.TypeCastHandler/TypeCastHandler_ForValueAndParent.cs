using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CK.TypeCastHandler
{
    public static partial class HandleTypeExt
    {
        // Custom Classes ----------------------------------------

        public struct ValueAndParentNomad_ForElseIs<TParent>
        {
            public TParent Parent { get; }
            public object Value { get; }
            public bool Handled { get; }

            public ValueAndParentNomad_ForElseIs(TParent parent, object value, bool handled)
            {
                Parent = parent;
                Value = value;
                Handled = handled;
            }


            // Methods - ElseIs<T> -----------------------------------

            public ValueAndParentNomad_ForElseIs<TParent> ElseIs<TObject>(Action<TParent, TObject> action)
            {
                if (Handled || !(Value is TObject))
                    return this;

                action(Parent, (TObject)Value);

                return new ValueAndParentNomad_ForElseIs<TParent>(Parent, Value, true);
            }


            // Methods - Else ----------------------------------------

            public void Else(Action<TParent, object> action)
            {
                if (Handled)
                    return;

                action(Parent, Value);
            }
        }

        public struct ValueAndParentNomad_ForIsNull<TParent>
        {
            public TParent Parent { get; }
            public object Value { get; }
            public bool Handled { get; }

            public ValueAndParentNomad_ForIsNull(TParent parent, object value, bool handled)
            {
                Parent = parent;
                Value = value;
                Handled = handled;
            }


            // Methods - IsNull --------------------------------------

            public ValueAndParentNomad_ForElseIs<TParent> IsNull(Action<TParent> action)
            {
                if (Value == null)
                {
                    action(Parent);

                    return new ValueAndParentNomad_ForElseIs<TParent>(Parent, Value, true);
                }

                return new ValueAndParentNomad_ForElseIs<TParent>(Parent, Value, false);
            }


            // Methods - SkipIfNull ----------------------------------

            public ValueAndParentNomad_ForElseIs<TParent> SkipIfNull()
            {
                var handled = Value == null;

                return new ValueAndParentNomad_ForElseIs<TParent>(Parent, Value, handled);
            }
        }


        // Methods - HandleTypes ---------------------------------

        public static ValueAndParentNomad_ForIsNull<TParent> HandleTypesFor<TParent>(this TParent parent, Func<TParent, object> accessor)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));
            if (accessor == null)
                throw new ArgumentException(nameof(accessor));

            var val = accessor(parent);

            return new ValueAndParentNomad_ForIsNull<TParent>(parent, val, false);
        }
    }
}
