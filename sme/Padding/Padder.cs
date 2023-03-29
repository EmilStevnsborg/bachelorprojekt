using SME;
using CNNPadder;
//using CNN;
using System;

namespace PadderClass
{
    [ClockedProcess]
    public class Padder : SimpleProcess
    {
        [InputBus]
        public ChannelBus input;

        [OutputBus]
        public PaddedChannelBus output = Scope.CreateBus<PaddedChannelBus>();

        private int Right;
        private int Left;
        private int Top;
        private int Bottom;
        private float PadVal;


        public Padder(int right = 0, int left = 0, int top = 0, int bottom = 0, float padVal = 0f)
        {
            Right = right;
            Left = left;
            Top = top;
            Bottom = bottom;
            PadVal = padVal;
        }

        protected override void OnTick()
        {
            output.enable = false;
            output.Height = 29;
            output.Width = 29;


            if (!output.enable && input.enable)
            {
                output.Height = Top + Bottom + input.Height;
                output.Width = Left + Right + input.Width;

                for (int i = 0; i < output.Height; i++)
                {
                    for (int j = 0; j < output.Width; j++)
                    {
                        int idx = i * output.Width + j;

                        if (i < Top || i > input.Height + Top)
                        {
                            output.ArrData[idx] = PadVal;
                        }
                        else if (j < Left || j > input.Width + Left)
                        {
                            output.ArrData[idx] = PadVal;
                        }
                        else
                        {
                            var tmp = input.ArrData[(i - Top) * input.Width + j - Left];
                            output.ArrData[idx] = tmp;
                        }
                    }
                }
            }

            if (!output.enable && input.enable) {output.enable = true;}
                
        }
    }
}

        