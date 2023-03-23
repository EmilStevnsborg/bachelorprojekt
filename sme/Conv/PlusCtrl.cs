using SME;

namespace CNN
{
    public class PlusCtrl : SimpleProcess
    {
        [InputBus]
        public ValueBus InputValue;
        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();

        float buffer = 0;

        protected override void OnTick()
        {
            // Output should only be updated when the input is valid.
            if (InputValue.enable)
            {
                buffer += InputValue.Value;
            }
            // Now we can set the buffer
            if (InputValue.LastValue)
            {
                Output.Value = buffer;
                Output.enable = true;
            }
        }
    }
}