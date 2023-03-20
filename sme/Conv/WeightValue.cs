using SME;

namespace CNN
{
    public class WeightValue : SimpleProcess
    {
        [InputBus]
        public ValueBus InputValue;
        [InputBus]
        public ValueBus InputWeight;

        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();

        protected override void OnTick()
        {
            // Output should only be updated when the input is valid.
            if (InputValue.enable && InputWeight.enable)
            {
                Output.Value = InputValue.Value * InputWeight.Value;
                Output.enable = true;
            }
            // If Input is last value in slice make plus ctrl know
            Output.LastValue = InputValue.LastValue;
        }
    }
}