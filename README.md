# Bachelorprojekt

This repository has been developed for our bachelor thesis. The main objective has been to build a code base in SME, which can be used to generate VHDL code that imitate convolutional neural networks (CNN) such that they can run on FPGAs as embedded AI applications.

For best understanding of our solution, we strongly advice anyone interested to read the SME documentation as well as our thesis.

Before we could begin creating the SME code base, we needed to understand and verify our knowledge about CNNs and each of the small underlying processes that take place in one.

# # Python

To do this, we created a Python implementation of some general layers in a CNN. All our implementations are compared with their PyTorch equivalent.

```
cd Python
```

The layers which we have implemented are stated in:

```
cd Python/CNN_layers
```

We created a PyTorch CNN trained on MNIST data. The architecture, training as well as testing of the CNN can be seen in
```
cd Python/Tests
```

To verify the correctnes of the Python implemented CNN layers, we recreated the PyTorch network using those layers



# # SME

In the sme/ directory, the implementation for certain CNN layers will be found as well as tests and their results. In the directory sme/CNN, the implementation of various components for different CNN layers will be found.

```
cd sme/Test/
```

leads to the directories where tests on isolated layers have been conducted. All directories are .NET projects on their own. They all have similar Program files that can be used for either configuration tests or tests on CNNSmall tests. In each project, there is also a Python file, that we have used to generate true values that we compare with during testing. The Program files can be set for configuration tests by setting the variable configTest to true, and CNNSmall test by setting it to false. Keep in mind that the tests variable must have a value that is less or equal to the amount of available tests for CNNSmall.

The results for CNNSmall tests are saved in sme/CNNSmall/Tests/<layer_that_has_been_tested>

These results are what is being reported as isolated tests for CNNSmall in the report. To reproduce the results, simply type:
```
dotnet run
```
Keep in mind that it will take a lot of time to go through all 1000 tests and will erase the current saved files.

```
cd sme/CNNSmall/
```

to generate a network test of CNNSmall, simply type:
```
dotnet run
```
If the accumulated network layer errors are wished to be reproduced, set the variable layerOutputs to true. If the network results are wished to be saved, set the variable save to true. Again, this will erase the current files and replace them with the newly computed.

To reproduce the statistics of the layers, run
```
python stats.py
```
