import numpy as np


class linear_layer():
    def __init__(self,weights,bias,output_size: int):
        self.weights = weights
        self.bias = bias
        self.output_size = output_size

    def __call__(self, input):
        output = []

        for i in range(self.output_size):
            output += np.sum(input[i,:] * self.weights[i,:]) + self.bias[i]
    
        return output
