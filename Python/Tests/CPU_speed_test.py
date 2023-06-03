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

sys.path.append('../Python/Tests')

from CNN_small_architecture import CNNSmall
from Python.CNN_layers import linear_layer_homemade
from Python.CNN_layers import relu_homemade as ReLU
from Tests.helper_functions import tokenize, transform_input, compare, create_conv_homemade, \
                                create_batchnorm_homemade, create_maxpool_homemade
from test_functions import layer_test

import time

def RunTest(batch_size_, iterations_):
    device = "cpu"

    # Importing data
    MNIST_test = datasets.MNIST(root='./data', train=False, download=False, 
                                transform=torchvision.transforms.ToTensor())
    test_set = [[data[0], tokenize(data[1])] for data in MNIST_test if data[1] in [1,2]]
    batch_size = int(batch_size_)
    #test_loader = DataLoader(test_set, batch_size=batch_size, pin_memory=True, pin_memory_device = device)
    test_loader = DataLoader(test_set, batch_size=batch_size)

    # Setting up the original network
    model_original = CNNSmall()
    model_original.eval()
    model_original = model_original.to(device)

    # Setting the path to load the model
    path = "CNN_small"
    load = True

    #torch.cuda.set_device(device) # Include this to assert that it is the correct device
    #print("This experiment is done on the following device:", 
    #      torch.cuda.get_device_properties(torch.cuda.device))

    # Loading the model orginal model
    if load and os.path.isfile(path):
        model_original.load_state_dict(torch.load(path))

    iterations = int(iterations_)

    time_array = np.ndarray(iterations)

    actual_batch_size = next(iter(test_loader))[0].shape[0]

    print("Actual_batch_size", actual_batch_size)

    #warm_up_time_init = time.time()
    for i in range(100):
        model_original(next(iter(test_loader))[0].to(device))
    #warm_up_time_end = time.time()
    #print("Warm up time: ", warm_up_time_end - warm_up_time_init)

    time1 = time.time() 
    for i in range(iterations):
        #model_original(next(iter(test_loader))[0].to(device))
        model_original(next(iter(test_loader))[0].to(device))
    time2 = time.time()
    return (time2 - time1) / (batch_size * iterations)


if __name__ == "__main__":
    print(RunTest(sys.argv[1], sys.argv[2]))
