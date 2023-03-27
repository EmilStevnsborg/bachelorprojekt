using SME;
using CNN;
using System;

// Much of this test is reused from the simple-relu test
public class PadTester : SimulationProcess
{
    [InputBus]
    public PaddedChannelBus from_network;

    [OutputBus]
    public ChannelBus to_network = Scope.CreateBus<ChannelBus>();

    public async override System.Threading.Tasks.Task Run()
    {
        // Setup the values to test
        float[,]
            test_values = new float[28,28],
            expected = new float[29,29];
        System.Random rng = new System.Random();
        for (int i = 0; i < 28; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                float value = (float) (rng.NextDouble());
                test_values[i,j] = value;
                expected[i + 1,j + 1] = value;
            }
        }
        to_network.Height = 28;
        to_network.Width = 28;

        // Simulation processes always wait a single clock cycle to initialize
        // the network.
        await ClockAsync();

        // Pack the test values onto the bus.
        //to_network.enable = true;

        to_network.enable = true;

        for (int i = 0; i < 28; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                to_network.ArrData[i * 28 + j] = test_values[i,j];
            }
        }

        

        await ClockAsync();

        // Ensure that the test data isn't read again.
        to_network.enable = false;
        
        await ClockAsync();

        // Read the values back from the network.
        float[,] computed = new float[29,29];

        // Wait until there is something valid on the bus.
        while (!from_network.enable) await ClockAsync();

        for (int i = 0; i < from_network.Height; i++)
        {
            for (int j = 0; j < from_network.Width; j++)
                computed[i,j] = from_network.ArrData[i * from_network.Width + j];
        }
        
        await ClockAsync();

        // Verify that the output from the network matches the expected result.
        for (int i = 0; i < 29; i++)
        {
            for (int j = 0; j < 29; j++)
            {
                System.Diagnostics.Debug.Assert(
                    Math.Abs(expected[i,j] - computed[i,j]) < 1e-7,
                    $"At index [{i}, {j}], {expected[i,j]} != {computed[i,j]}"
                );
            }
        }

        // Output to terminal to indicate everything went ok. If not, the
        // earlier asserts should halt the program.
        Console.WriteLine("Tester finished");
    }
}