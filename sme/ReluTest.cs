using SME;
using System;

namespace CNN 
{
    public class ReLUTest : SimpleProcess
    {
        [InputBus]
        public Pixel Input;

        protected override void OnTick()
        {
            Console.WriteLine("RELU TEST");
            Console.WriteLine(Input.Value);
        }
    }
}