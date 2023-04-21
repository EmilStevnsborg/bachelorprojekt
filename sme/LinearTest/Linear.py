import torch
import torch.nn as nn
torch.manual_seed(0)
torch.set_printoptions(sci_mode=False)

# Config 1
####################################################################################################
#
# 5 input channels of size 3x7, 4 outputchannels
lin1 = nn.Linear(5*3*7,4)
weights11 = torch.rand(5*3*7, requires_grad=False)
weights12 = torch.rand(5*3*7, requires_grad=False)
weights13 = torch.rand(5*3*7, requires_grad=False)
weights14 = torch.rand(5*3*7, requires_grad=False)

# print(weights11)
# print(weights12)
# print(weights13)
# print(weights14)

weights1 = torch.cat((weights11,weights12,weights13,weights14)).reshape(4,5*3*7)

biases1 = torch.FloatTensor([1,9,2,5])
lin1.load_state_dict({"weight" : weights1, "bias" : biases1})

input1_11 = torch.rand(3*7, requires_grad=False)
input1_12 = torch.rand(3*7, requires_grad=False)
input1_13 = torch.rand(3*7, requires_grad=False)
input1_14 = torch.rand(3*7, requires_grad=False)
input1_15 = torch.rand(3*7, requires_grad=False)

input1_1 = torch.cat((input1_11,input1_12,input1_13,input1_14,input1_15))
print("Config3, Test1")
print(lin1(input1_1))

#!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

input1_21 = torch.rand(3*7, requires_grad=False)
input1_22 = torch.rand(3*7, requires_grad=False)
input1_23 = torch.rand(3*7, requires_grad=False)
input1_24 = torch.rand(3*7, requires_grad=False)
input1_25 = torch.rand(3*7, requires_grad=False)

input1_2 = torch.cat((input1_21,input1_22,input1_23,input1_24,input1_25))
print("Config3, Test2")
print(input1_2.reshape(5,3,7))
print(lin1(input1_2))