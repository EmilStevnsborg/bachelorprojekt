import torch
import torch.nn as nn
torch.manual_seed(0)
torch.set_printoptions(sci_mode=False, precision=8)

# Config 1
####################################################################################################
#
maxPool1 = nn.MaxPool2d((2,3),(2,1),(0,1))
input1_1 = torch.rand(1,4,6,4, requires_grad=False)
print("Config1, Test1")
print(input1_1)
print(maxPool1(input1_1))