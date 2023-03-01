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

from CNN_small_architecture import CNNSmall
from CNN_layers import conv_homemade
from CNN_layers import maxpool_homemade
from CNN_layers import batchnorm_homemade
from CNN_layers import linear_layer_homemade
from CNN_layers import elu_homemade

def tokenize(num):
    if num == 1:
        return torch.tensor(np.array([1., 0.]))
    else:
        return torch.tensor(np.array([0., 1.]))

MNIST_test = datasets.MNIST(root='./data', train=True, download=True, transform=torchvision.transforms.ToTensor())
test_set = [[data[0], tokenize(data[1])] for data in MNIST_test if data[1] in [1,2]]

batch_size = 2
test_loader = DataLoader(test_set, batch_size=batch_size)