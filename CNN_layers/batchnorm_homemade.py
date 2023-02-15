import numpy as np

class BatchNorm:
    def __init__(self, weights, biases):
        self.weights = weights
        self.biases = biases
    
    def __call__(self, input_batch):
        # output_batch is a list
        output_batch = []
        for input in input_batch:
            # input is a list of batches with a list of channels where each channel is a 2d numpy array
            input_channels = len(input)
            output = []
            
            for i in range(input_channels):
                channel2d = input[i]
                out_channel = self.batchnorm_operation(channel2d=channel2d, index=i)
                output.append(out_channel)
            
            output_batch.append(output)
        
        return output_batch
                
    def batchnorm_operation(self, channel2d, index: int):
        r, c = channel2d.shape
        mean = np.mean(channel2d)
        # biased estimator
        std = np.std(channel2d)
        weight = self.weights[index]
        bias = self.biases[index]

        out_channel = np.zeros((r,c))

        for i in range(r):
            for j in range(c):
                ij = (channel2d[i,j] - mean) / (std) * weight + bias
                out_channel[i,j] = ij
        
        return out_channel
