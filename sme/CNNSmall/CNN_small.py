import json 
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

#######################################################################################################

torch.manual_seed(0)
torch.set_printoptions(sci_mode=False, precision=20)

sys.path.append('../../Python')
sys.path.append('../../Python/Tests')
from CNN_small_architecture import CNNSmall
from helper_functions import tokenize

# Setting up the original network
#######################################################################################################
model_original = CNNSmall()
model_original.eval()
model_original.load_state_dict(torch.load("../../Python/CNN_small"))
model_original.eval()

# Importing data
#######################################################################################################

MNIST_test = datasets.MNIST(root='../../Python/data', train=False, download=False, 
                            transform=torchvision.transforms.ToTensor())

test_set = [[data[0], tokenize(data[1])] for data in MNIST_test if data[1] in [1,2]]

batch_size = 100

test_loader = DataLoader(test_set, batch_size=batch_size)

conv1_input, labels_test = next(iter(test_loader))

def create_input_json(buffer, computed, samples, dir):
    in_channels, height, width = list(buffer[0,:,:,:].shape)
    out_channels, out_height, out_width = list(computed[0,:,:,:].shape)
    for i in range(1,samples+1):
        d = {}

        d["buffer"] = buffer[i-1,:,:,:].reshape(in_channels, height*width).tolist()
        d["computed"] = computed[i-1,:,:,:].reshape(out_channels, out_height*out_width).tolist()

        input_json = json.dumps(d, indent=4)
        
        with open(dir + "input" + str(i) + ".json", "w") as outfile:
            outfile.write(input_json)

# print(nn.Softmax(dim=1)(model_original(conv1_input)))

# Conv1
#######################################################################################################
conv1 = model_original.conv1
height, width = list(conv1_input[0,0,:,:].shape)

d = {}
d["numInChannels"] = conv1.in_channels
d["numOutChannels"] = conv1.out_channels
d["channelHeight"] = height
d["channelWidth"] = width
d["kernelHeight"] = conv1.kernel_size[0]
d["kernelWidth"] = conv1.kernel_size[1]
d["strideRow"] = conv1.stride[0]
d["strideCol"] = conv1.stride[1]
d["padHeight"] = conv1.padding[0]
d["padWidth"] = conv1.padding[1]
d["padVal"] = 0
d["weights"] = conv1.weight.reshape(conv1.out_channels,conv1.in_channels,conv1.kernel_size[0]*conv1.kernel_size[1]).tolist()
d["biases"] = conv1.bias.tolist()


conv1_output = conv1(conv1_input)

if False:
    conv1_json = json.dumps(d, indent=4)
    with open("Configs/conv1.json", "w") as outfile:
        outfile.write(conv1_json)
    create_input_json(conv1_input, conv1_output, batch_size, "Tests/conv1/")


# BatchNorm1
#######################################################################################################
batchNorm1 = model_original.batchNorm1
height, width = list(conv1_output[0,0,:,:].shape)

d = {}
d["numInChannels"] = conv1.out_channels
d["numOutChannels"] = conv1.out_channels
d["channelHeight"] = height
d["channelWidth"] = width
d["means"] = batchNorm1.running_mean.tolist()
d["vars"] = batchNorm1.running_var.tolist()
d["gammas"] = batchNorm1.weight.tolist()
d["betas"] = batchNorm1.bias.tolist()

batchNorm1_output = batchNorm1(conv1_output)

if False:
    batchNorm1_json = json.dumps(d, indent=4)
    with open("Configs/batchNorm1.json", "w") as outfile:
        outfile.write(batchNorm1_json)
    create_input_json(conv1_output, batchNorm1_output, batch_size, "Tests/batchNorm1/")


# Relu1
#######################################################################################################
relu1 = model_original.relu1
height, width = list(batchNorm1_output[0,0,:,:].shape)

d = {}
d["numInChannels"] = conv1.out_channels
d["numOutChannels"] = conv1.out_channels
d["channelHeight"] = height
d["channelWidth"] = width

relu1_output = relu1(batchNorm1_output)

if False:
    relu1_json = json.dumps(d, indent=4)
    with open("Configs/relu1.json", "w") as outfile:
        outfile.write(relu1_json)
    create_input_json(batchNorm1_output, relu1_output, batch_size, "Tests/relu1/")


# MaxPool1
#######################################################################################################
maxPool1 = model_original.maxPool1
height, width = list(relu1_output[0,0,:,:].shape)

d = {}
d["numInChannels"] = conv1.in_channels
d["numOutChannels"] = conv1.out_channels
d["channelHeight"] = height
d["channelWidth"] = width
d["kernelHeight"] = maxPool1.kernel_size[0]
d["kernelWidth"] = maxPool1.kernel_size[1]
d["strideRow"] = maxPool1.stride[0]
d["strideCol"] = maxPool1.stride[1]
d["padHeight"] = maxPool1.padding
d["padWidth"] = maxPool1.padding
d["padVal"] = 0

maxPool1_output = maxPool1(relu1_output)

if False:
    maxPool1_json = json.dumps(d, indent=4)
    with open("Configs/maxPool1.json", "w") as outfile:
        outfile.write(maxPool1_json)
    create_input_json(relu1_output, maxPool1_output, batch_size, "Tests/maxPool1/")


# Conv2
#######################################################################################################
conv2 = model_original.conv2
height, width = list(maxPool1_output[0,0,:,:].shape)

d = {}
d["numInChannels"] = conv2.in_channels
d["numOutChannels"] = conv2.out_channels
d["channelHeight"] = height
d["channelWidth"] = width
d["kernelHeight"] = conv2.kernel_size[0]
d["kernelWidth"] = conv2.kernel_size[1]
d["strideRow"] = conv2.stride[0]
d["strideCol"] = conv2.stride[1]
d["padHeight"] = conv2.padding[0]
d["padWidth"] = conv2.padding[1]
d["padVal"] = 0
d["weights"] = conv2.weight.reshape(conv2.out_channels,conv2.in_channels,conv2.kernel_size[0]*conv2.kernel_size[1]).tolist()
d["biases"] = conv2.bias.tolist()


conv2_output = conv2(maxPool1_output)

if False:
    conv2_json = json.dumps(d, indent=4)
    with open("Configs/conv2.json", "w") as outfile:
        outfile.write(conv2_json)
    create_input_json(maxPool1_output, conv2_output, batch_size, "Tests/conv2/")


# BatchNorm2
#######################################################################################################
batchNorm2 = model_original.batchNorm2
height, width = list(conv2_output[0,0,:,:].shape)

d = {}
d["numInChannels"] = conv2.out_channels
d["numOutChannels"] = conv2.out_channels
d["channelHeight"] = height
d["channelWidth"] = width
d["means"] = batchNorm2.running_mean.tolist()
d["vars"] = batchNorm2.running_var.tolist()
d["gammas"] = batchNorm2.weight.tolist()
d["betas"] = batchNorm2.bias.tolist()

batchNorm2_output = batchNorm2(conv2_output)

if False:
    batchNorm2_json = json.dumps(d, indent=4)
    with open("Configs/batchNorm2.json", "w") as outfile:
        outfile.write(batchNorm2_json)
    create_input_json(conv2_output, batchNorm2_output, batch_size, "Tests/batchNorm2/")


# Relu2
#######################################################################################################
relu2 = model_original.relu2
height, width = list(batchNorm2_output[0,0,:,:].shape)

d = {}
d["numInChannels"] = conv2.out_channels
d["numOutChannels"] = conv2.out_channels
d["channelHeight"] = height
d["channelWidth"] = width

relu2_output = relu1(batchNorm2_output)

if False:
    relu2_json = json.dumps(d, indent=4)
    with open("Configs/relu2.json", "w") as outfile:
        outfile.write(relu2_json)
    create_input_json(batchNorm2_output, relu2_output, batch_size, "Tests/relu2/")


# MaxPool2
#######################################################################################################
maxPool2 = model_original.maxPool2
height, width = list(relu2_output[0,0,:,:].shape)


d = {}
d["numInChannels"] = conv2.out_channels
d["numOutChannels"] = conv2.out_channels
d["channelHeight"] = height
d["channelWidth"] = width
d["kernelHeight"] = maxPool2.kernel_size[0]
d["kernelWidth"] = maxPool2.kernel_size[1]
d["strideRow"] = maxPool2.stride[0]
d["strideCol"] = maxPool2.stride[1]
d["padHeight"] = maxPool2.padding
d["padWidth"] = maxPool2.padding
d["padVal"] = 0

maxPool2_output = maxPool2(relu2_output)

if False:
    maxPool2_json = json.dumps(d, indent=4)
    with open("Configs/maxPool2.json", "w") as outfile:
        outfile.write(maxPool2_json)
    create_input_json(relu2_output, maxPool2_output, batch_size, "Tests/maxPool2/")


# Linear
#######################################################################################################
lin = model_original.lin
height, width = list(maxPool2_output[0,0,:,:].shape)


d = {}
d["numInChannels"] = conv2.out_channels
d["numOutChannels"] = lin.out_features
d["channelHeight"] = height
d["channelWidth"] = width
d["weights"] = lin.weight.tolist()
d["biases"] = lin.bias.tolist()


lin_output = lin(nn.Flatten()(maxPool2_output)).reshape(batch_size,2,1,1)

if True:
    lin_json = json.dumps(d, indent=4)
    with open("Configs/linear.json", "w") as outfile:
        outfile.write(lin_json)
    create_input_json(maxPool2_output, lin_output, batch_size, "Tests/linear/")