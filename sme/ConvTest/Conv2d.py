import torch
import torch.nn as nn
torch.manual_seed(0)

# Config 1
####################################################################################################
#
conv1 = nn.Conv2d(1,2,(2,3),(2,1),(0,0))
weights1 = torch.FloatTensor([[
                                     [[1,2,3],
                                      [4,5,6]]
                                              ],
                                    [
                                     [[7,8,9],
                                      [10,11,12]]
                                                 ]])
biases1 = torch.FloatTensor([0,0])
conv1.load_state_dict({"weight" : weights1, "bias" : biases1})
input1_1 = torch.FloatTensor([[[
                                [1,2,3,4,5],
                                [6,7,8,9,10],
                                [11,12,13,14,15],
                                [16,17,18,19,20]]
                                                 ]])
# print("Config1, Test1")
# print(conv1(input1_1))

#!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

input1_2 = torch.rand(1,1,4,5, requires_grad=False)
print("Config1, Test2")
print(input1_2)
print(conv1(input1_2))

# Config 2
####################################################################################################
#
conv2 = nn.Conv2d(3,2,(2,3),(2,1),(2,1))
weights2 = torch.FloatTensor([[
                                    [[1,2,3],
                                     [4,5,6]],
                                    [[7,8,9],
                                     [10,11,12]],
                                     [[13,14,15],
                                      [16,17,18]]
                                                 ],
                                   [
                                    [[-1,2,3],
                                     [-4,5,6]],
                                    [[7,-8,9],
                                     [10,-11,12]],
                                     [[13,14,-15],
                                      [16,17,-18]]
                                                  ]])
biases2 = torch.FloatTensor([1,1])
conv2.load_state_dict({"weight" : weights2, "bias" : biases2})

input2_1 = torch.FloatTensor([[[
                                    [1, 1, 1, 1, 1],
                                    [2, 2, 2, 2, 2],
                                    [3, 3, 3, 3, 3],
                                    [4, 4, 4, 4, 4]],
                                [
                                    [-1,-2,-3,-4,-5],
                                    [-9,-8,-7,-2,-1],
                                    [1, 2, 3, 4, 5],
                                    [6, 7, 8, 9, 0]],
                                [
                                    [1, 1, 1, 1, 1],
                                    [2, 2, 2, 2, 2],
                                    [1, 2, 3, 4, 5],
                                    [6, 7, 8, 9, 0]]
                                                    ]])
# print(input2_1.shape)
# print("Config2 Test1")
# print(conv2(input2_1))



# Config 3
####################################################################################################
#
conv3 = nn.Conv2d(1,2,(2,3),(2,1),(0,0))
weights3 = torch.rand(2,1,2,3, requires_grad=False)
biases3 = torch.rand(2, requires_grad=False)
conv3.load_state_dict({"weight" : weights3, "bias" : biases3})



