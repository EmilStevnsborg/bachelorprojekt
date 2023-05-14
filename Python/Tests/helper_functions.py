#importing relevant libraries
from __future__ import annotations
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

# takes a 4-dimensional tensor from the dataloader (batch_size x channel_size x height x width)
# and turns it into a list of lists of (height x width) numpy arrays
def transform_input(input_batch):
    return list(input_batch.detach().numpy())

def compare(x: list[float], y: list[float]):
    """
    compares two lists of batches with same amount of channels and returns the greatest error, the 
    mean error and the variance of the error when comparing x to y

    Args:
        x (list[float]): One part of the values that must be compared 
        y (list[float]): One part of the values that must be compared
    Returns:
        tuple(float,float,float): The maximum error, the mean of the mean error for each channel and
            the mean variance of the error
    """

    batch_size = len(x)

    global_max_error = 0
    global_mean = []
    global_var = []
    global_mean_x = []

    for b in range(batch_size):
        local_max = np.amax(np.absolute(x[b] - y[b]))
        local_mean = np.mean(np.absolute(x[b] - y[b]))
        local_var = np.var(np.absolute(x[b] - y[b])) 
        local_mean_x = np.mean(np.absolute(x[b]))
        global_mean_x.append(local_mean_x)
        global_mean.append(local_mean)
        global_var.append(local_var)
 
        if local_max > global_max_error:
            global_max_error = local_max
    
    return global_max_error, np.mean(global_mean), np.mean(global_var), np.mean(global_mean_x)

def create_conv_homemade(model_conv):
    weights = model_conv.weight
    biases = model_conv.bias.detach().numpy()

    out_c, in_c, r, c = weights.shape
    conv1_filters = []

    for f in range(out_c):
        filter_ = []
        kernels = []

        for kernel in list(weights[f,:,:,:]):
            kernels.append(kernel.detach().numpy())

        filter_.append(kernels)
        filter_.append(biases[f])
        conv1_filters.append(filter_)
    
    return conv_homemade.Conv(filters=conv1_filters, in_channels=in_c)

def create_batchnorm_homemade(model_batchnorm):
    weights = model_batchnorm.weight
    biases = model_batchnorm.bias
    running_mean = model_batchnorm.running_mean
    running_var = model_batchnorm.running_var

    return batchnorm_homemade.BatchNorm(weights=weights, biases=biases, running_mean = running_mean, running_var = running_var)

def create_maxpool_homemade(model_maxpool):
    kernel_size = model_maxpool.kernel_size
    stride = model_maxpool.stride
    if type(model_maxpool.padding) == int:
        padding = (model_maxpool.padding, model_maxpool.padding)
    else:
        padding = model_maxpool.padding
    
    return maxpool_homemade.MaxPool(kernel_size=kernel_size, stride=stride, padding=padding)