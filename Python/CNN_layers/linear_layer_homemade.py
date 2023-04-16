import numpy as np
import torch

def flatten(input):
    for i in range(len(input)):
        if i == 0:
            output = input[i]
        else:
            output = np.vstack((output,input[i]))
    
    return output.flatten()

class linear_layer():
    def __init__(self,weights,bias,output_size: int):
        self.weights = weights.detach().numpy()
        self.bias = bias.detach().numpy()
        self.output_size = output_size

    def __call__(self, input: np.ndarray, flat: bool = True):
        input_size = np.array(input).shape

        for n in range(input_size[0]):
            if not(flat):
                input_tmp = flatten(input[n])
            else:
                input_tmp = input[n]
            for i in range(int(self.output_size)):
                tmp_weights = self.weights[i]
                if i == 0:
                    sample_output = np.matmul(np.array(input_tmp),tmp_weights) + self.bias[i]
                else:
                    sample_output = np.append(sample_output,np.matmul(np.array(input_tmp),tmp_weights) + self.bias[i])
            if n == 0:
                batch_output = sample_output
            else:
                batch_output = np.vstack((batch_output,sample_output))
    
        return batch_output
