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
