using System;
using SME;
using CNN;

class MainClass
{
    public static void Main(string[] args)
    {

        using(var sim = new Simulation())
        {
            // ConvLayer Test
            float[][] weightsOne = { new float[4] {1,1,1,1}, new float[4] {2,2,2,2}};
            float biasOne = 1;
            float[][] weightsTwo = { new float[4] {2,2,2,2}, new float[4] {3,3,3,3}};
            float biasTwo = 2;
            float[][][] weigths = {weightsOne, weightsTwo};
            float[] biases = {biasOne, biasTwo};
            ConvLayer convLayer = new ConvLayer(2,2,weigths,biases,(4,4),(2,2),(2,2),(0,0),0);
            ConvLayerTester convLayerTester = new ConvLayerTester();
            convLayer.Inputs = convLayerTester.Outputs;
            convLayer.PushInputs();
            convLayerTester.Inputs = convLayer.Outputs;

            sim.Run();
        }
    }
}