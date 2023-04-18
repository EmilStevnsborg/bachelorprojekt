import torch
import torch.nn as nn

conv_test1 = nn.Conv2d(1,2,(2,3),(2,1),(0,0),1,1)
weights_test1 = torch.FloatTensor([[[[1,2,3],[4,5,6]]],[[[7,8,9],[10,11,12]]]])
biases_test1 = torch.FloatTensor([0,0])
conv_test1.load_state_dict({"weight" : weights_test1, "bias" : biases_test1})

input1 = torch.FloatTensor([[[[1,2,3,4,5],[6,7,8,9,10],[11,12,13,14,15],[16,17,18,19,20]]]])

print(conv_test1(input1))