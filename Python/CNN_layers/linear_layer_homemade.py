import numpy as np
import torch


class linear_layer():
    def __init__(self,weights,bias,output_size: int):
        self.weights = weights.detach().numpy()
        self.bias = bias
        self.output_size = output_size

    def __call__(self, input):
        input_size = np.array(input).shape

        # Todo: For loop should also iterate over the batch_size
        for n in range(input_size[0]):
            for i in range(int(self.output_size)):
                if i == 0:
                    tmp = torch.dot(input[n],torch.tensor(self.weights[i], dtype = torch.double)) + torch.tensor(self.bias[i], dtype = torch.double)
                else:
                    tmp = torch.hstack((tmp,(torch.dot(input[n],torch.tensor(self.weights[i], dtype = torch.double)) + torch.tensor(self.bias[i], dtype = torch.double))))
            if n == 0:
                output = tmp
            else:
                output = torch.vstack((output,tmp))
    
        return output
