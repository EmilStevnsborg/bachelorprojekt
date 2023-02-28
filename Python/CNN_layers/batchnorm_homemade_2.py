import numpy as np
import sys
import math

class BatchNorm:
    def __init__(self, weights, biases, running_mean, running_var):
        self.weights = weights
        self.biases = biases
        self.running_mean = running_mean
        self.running_var = running_var
    
    def __call__(self, input):
        # output_batch is a list
        # input is supposed to be [NxCxHxW]

        self.size = np.array(input).shape

        for input,i in zip(input,range(len(input))):
            if i == 0:
                output_batch = self.batchnorm_operation(input, self.running_mean, self.running_var)
            # input is a list of batches with a list of channels where each channel is a 2d numpy array
            else:
                tmp = self.batchnorm_operation(input, self.running_mean, self.running_var)
                output_batch = np.vstack((output_batch, tmp))
        
        return output_batch
                
    def batchnorm_operation(self, channel2d, mean, var):
        k, r, c = np.array(channel2d).shape

        # biased estimator
        out_channel = np.zeros((k,r,c))


        for k_ in range(k):
            for i in range(r):
                for j in range(c):
                    out_channel[k_,i,j] = (np.array(channel2d)[k_,i,j] - mean[k_]) / math.sqrt(var[k_] + sys.float_info.min)# * self.weights[k_]# + self.biases[k_] 
        
        return np.array([out_channel])