using System.Threading.Tasks;
using SME;
using SME.Components;

namespace CNN
{
    public class ConvKernelTester : SimulationProcess
    {
        [InputBus]
        public Pixel Product;
        [OutputBus]
        public Channel Input = Scope.CreateOrLoadBus<Channel>();
        [OutputBus]
        public TrueDualPortMemory<double>.IControlA weightRamctrl;

        private int kernelHeight;
        private int kernelWidth;

        public ConvKernelTester(int kernelHeight, int kernelWidth)
        {
            this.kernelHeight = kernelHeight;
            this.kernelWidth = kernelWidth;
        }
        public override async Task Run()
        {
            await ClockAsync();
            
            for (int i = 0; i < kernelHeight*kernelWidth; i++)
            {
                weightRamctrl.Address = i;
                weightRamctrl.Data = (double) i;
                weightRamctrl.IsWriting = true;
                weightRamctrl.Enabled = true;
                await ClockAsync();
            }
            weightRamctrl.Enabled = false;
            await ClockAsync();

            // 2x2 input channel
            Input.Height = 2;
            Input.Width = 2;
            for (int i = 0; i < Input.Height; i++)
            {
                for (int j = 0; j < Input.Width; j++)
                {
                    Input.Data[i*Input.Width+j] = (double) i;
                }
            }

        }
    }
}