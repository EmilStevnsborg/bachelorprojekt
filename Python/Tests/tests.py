#importing relevant libraries

import numpy as np
import torch
import torch.nn as nn
import torchvision
import torchvision.datasets as datasets
from torch.utils.data import DataLoader, TensorDataset, Subset
import torch.optim as optim
import torch.nn.functional as F
import os
import sys

from CNN_small_architecture import CNNSmall
from CNN_layers import linear_layer_homemade
from CNN_layers import relu_homemade as ReLU
from Tests.helper_functions import tokenize, transform_input, compare, create_conv_homemade, \
                                create_batchnorm_homemade, create_maxpool_homemade
from test_functions import layer_test

def main(batch_size_ : int = 2):
    # Importing data
    MNIST_test = datasets.MNIST(root='./data', train=True, download=True, 
                                transform=torchvision.transforms.ToTensor())
    test_set = [[data[0], tokenize(data[1])] for data in MNIST_test if data[1] in [1,2]]
    batch_size = batch_size_ # batch size must be greater than 1
    test_loader = DataLoader(test_set, batch_size=batch_size)

    # Setting up the original network
    model_original = CNNSmall()
    model_original.eval()

    # Setting the path to load the model
    path = "CNN_small"
    load = True

    # Loading the model orginal model
    if load and os.path.isfile(path):
        model_original.load_state_dict(torch.load(path))

    input_original_test, label_test = next(iter(test_loader))
    input_homemade_test = transform_input(input_batch=input_original_test)

    # Setting up the homemade network
    relu_homemade = ReLU.relu()
    conv1_homemade = create_conv_homemade(model_conv = model_original.conv1)
    batchnorm1_homemade = create_batchnorm_homemade(model_batchnorm = model_original.batchNorm1)
    maxpool1_homemade = create_maxpool_homemade(model_maxpool = model_original.maxPool1)
    conv2_homemade = create_conv_homemade(model_conv = model_original.conv2)
    batchnorm2_homemade = create_batchnorm_homemade(model_batchnorm = model_original.batchNorm2)
    maxpool2_homemade = create_maxpool_homemade(model_original.maxPool2)
    linear_homemade = linear_layer_homemade.linear_layer(model_original.lin.weight,
                                                         model_original.lin.bias,2)
    
    print("\n--------------------- TEST 1 ----------------------")

    print("""\nThis is a test of how each layer performs compared 
to an equivalent Pytorch layer when given, the same 
input.\n""")
    
    # Testing conv1
    _, out_original_conv1 = layer_test(conv1_homemade, model_original.conv1, input_homemade_test,
                                       input_original_test, "Conv1 test: ")

    # Testing batchnorm1
    _, out_original_batchnorm1 = layer_test(batchnorm1_homemade, model_original.batchNorm1,
                                           transform_input(out_original_conv1),out_original_conv1,
                                           "BatchNorm1 test: ")
    
    # Testing relu1
    _, out_original_relu1 = layer_test(relu_homemade, model_original.relu1, 
                                       transform_input(out_original_batchnorm1), 
                                       out_original_batchnorm1, "ReLU1 test: ")
    
    # Testing maxpool1
    _, out_original_maxpool = layer_test(maxpool1_homemade, model_original.maxPool1,
                                         transform_input(out_original_relu1), out_original_relu1,
                                         "MaxPool1 test: ")
    
    # Testing conv2
    _, out_original_conv2 = layer_test(conv2_homemade, model_original.conv2, 
                                       transform_input(out_original_maxpool), out_original_maxpool,
                                       "Conv2 test: ")
    
    # Testing batchnorm 2
    _, out_original_batchnorm2 = layer_test(batchnorm2_homemade, model_original.batchNorm2,
                                            transform_input(out_original_conv2), out_original_conv2,
                                            "BatchNorm2 test: ")

    # Testing relu2
    _, out_original_relu2 = layer_test(relu_homemade, model_original.relu2, 
                                       transform_input(out_original_batchnorm2), 
                                       out_original_batchnorm2, "ReLU2 test: ")
    
    _, out_original_maxpool2 = layer_test(maxpool2_homemade, model_original.maxPool2,
                                         transform_input(out_original_relu2),out_original_relu2,
                                         "MaxPool2 test: ")
    
    out_original_flat = torch.reshape(out_original_maxpool2, (batch_size,45))
    
    # Testing linear
    _, out_original_lin = layer_test(linear_homemade, model_original.lin,
                                     transform_input(out_original_flat), 
                                     out_original_flat,
                                     "Linear test: ")
    

    print("\n--------------------- TEST 2 ----------------------")

    print("""\nThis is a test of how each layer performs compared 
to an equivalent Pytorch layer when each layer gets the 
output of the previous layer - where the homemade layers
#receive input from homemade layers and likewise the PyTorch
#layers receive input from PyTorch layers..\n""")
    
    # Testing conv1
    out_homemade_conv1, out_original_conv1 = layer_test(conv1_homemade, model_original.conv1, 
                                                        input_homemade_test, input_original_test, 
                                                        "Conv1 test: ")

    # Testing batchnorm1
    out_homemade_batchnorm1, out_original_batchnorm1 = layer_test(batchnorm1_homemade, 
                                                                  model_original.batchNorm1,
                                                                  out_homemade_conv1,
                                                                  out_original_conv1, 
                                                                  "BatchNorm1 test: ")
    
    # Testing relu1
    out_homemade_relu1, out_original_relu1 = layer_test(relu_homemade, model_original.relu1, 
                                       out_homemade_batchnorm1, out_original_batchnorm1, 
                                       "ReLU1 test: ")
    
    # Testing maxpool1
    out_homemade_maxpool1, out_original_maxpool = layer_test(maxpool1_homemade, 
                                                             model_original.maxPool1,
                                                             out_homemade_relu1, 
                                                             out_original_relu1,"MaxPool1 test: ")
    
    # Testing conv2
    out_homemade_conv2, out_original_conv2 = layer_test(conv2_homemade, 
                                                        model_original.conv2,
                                                        out_homemade_maxpool1, 
                                                        out_original_maxpool,"Conv2 test: ")
    
    # Testing batchnorm 2
    out_homemade_batchnorm2, out_original_batchnorm2 = layer_test(batchnorm2_homemade, 
                                                                  model_original.batchNorm2,
                                                                  out_homemade_conv2, 
                                                                  out_original_conv2,
                                                                  "BatchNorm2 test: ")

    # Testing relu2
    out_homemade_relu2, out_original_relu2 = layer_test(relu_homemade, model_original.relu2,
                                                        out_homemade_batchnorm2, 
                                                        out_original_batchnorm2, "ReLU2 test: ")
    
    # Testing maxpool2
    out_homemade_maxpool2, out_original_maxpool2 = layer_test(maxpool2_homemade, 
                                                              model_original.maxPool2,
                                                              out_homemade_relu2,out_original_relu2,
                                                              "MaxPool2 test: ")
    
    out_original_flat = torch.reshape(out_original_maxpool2, (batch_size,45))
    
    # Testing linear
    out_homemade_linear, out_original_lin = layer_test(linear_homemade, model_original.lin,
                                                       out_homemade_maxpool2, out_original_flat,
                                                       "Linear test: ", False)



if __name__=='__main__':
    if (int(sys.argv[1]) < 2):
        raise ValueError("The batchsize must be greater than or equal to 2")
    try:
        main(batch_size_ = int(sys.argv[1]))
    except:
        main()