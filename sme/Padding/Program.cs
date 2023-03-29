using SME;

using PadderClass;

using (var sim = new Simulation())
{
    Padder padder = new Padder(0,1,1,0,0f);
    var tester = new PadTester();

    // Connect the buses.
    padder.input = tester.to_network;
    tester.from_network = padder.output;

    // Start the simulation.
    sim.Run();
}