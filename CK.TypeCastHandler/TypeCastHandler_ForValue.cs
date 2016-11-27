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

        public struct ValueNomad_ForElseIs
        {
            public object Value { get; }
            public bool Handled { get; }

            public ValueNomad_ForElseIs(object value, bool handled)
            {
                Value = value;
                Handled = handled;
            }

            // Methods - ElseIs<T> -----------------------------------

            public ValueNomad_ForElseIs ElseIs<TObject>(Action<TObject> action)
            {
                if (Handled || !(Value is TObject))
                    return this;

                var val = (TObject)Value;

                action(val);

                return new ValueNomad_ForElseIs(Value, true);
            }

            // Methods - Else ----------------------------------------

            public void Else(Action<object> action)
            {
                if (Handled)
                    return;

                action(Value);
            }
        }

        public struct ValueNomad_ForIsNull
        {
            public object Value { get; }
            public bool Handled { get; }

            public ValueNomad_ForIsNull(object value, bool handled)
            {
                Value = value;
                Handled = handled;
            }

            // Methods - IsNull --------------------------------------

            public ValueNomad_ForElseIs IsNull(Action action)
            {
                if (Value == null)
                {
                    action();

                    return new ValueNomad_ForElseIs(Value, true);
                }

                return new ValueNomad_ForElseIs(Value, false);
            }


            // Methods - SkipIfNull ----------------------------------

            public ValueNomad_ForElseIs SkipIfNull()
            {
                var handled = Value == null;

                return new ValueNomad_ForElseIs(Value, handled);
            }
        }


        // Methods - HandleTypes ---------------------------------

        public static ValueNomad_ForIsNull HandleTypes(this object value)
        {
            return new ValueNomad_ForIsNull(value, false);
        }
    }
}
