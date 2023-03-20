using SME;

namespace CNN
{
    public class ReluCore : SimpleProcess
    {
        [InputBus]
        public ValueBus Input;

        [OutputBus]
        public ValueBus Output = Scope.CreateBus<ValueBus>();

        protected override void OnTick()
        {
            // The flag can be forwarded.
            Output.enable = Input.enable;
            // Output should only be updated when the input is valid.
            if (Output.enable)
            {
                Output.Value = Input.Value > 0 ? Input.Value : 0;
            }
        }
    }
}