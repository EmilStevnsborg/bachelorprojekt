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
batch_size = 10 # batch size must be greater than 1
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
    create_input_json(conv1_input, conv1_output, batch_size, "Tests/Conv1/")



#######################################################################################################