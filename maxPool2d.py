import numpy as np
from conv_utils.padding import padding2d


class MaxPool2d():
    
    def __init__(self, kernel_size : tuple, stride : tuple = None, padding : int = 0):

        # Inspired by torch.nn.MaxPool2d - if stride is not set then it will be set to the size
        # of the kernel

        if (stride == None):
            print("DEN ER BLEVET SAT")
            self.stride = kernel_size
        else:
            print("DEN KOMMER OGSÅ HERIND")
            self.stride = stride

        self.kernel_size = kernel_size
        self.padding_value = padding
        

    def __call__(self, input):
        
        # The input is supposed to be [number of inputs x channels x h x w]

        number_of_inputs = input.shape[0]
        channels = input.shape[1]

        test_sample = padding2d(
            input[0,0,:,:],
            kernel_size = self.kernel_size,
            pad_val = self.padding_value,
            stride = self.stride)

        # The recast should be safe! TESTS NEEDED!
        height_iterations = int((test_sample.shape[0] - self.kernel_size[0]) / self.stride[0]) + 1
        width_iterations = int((test_sample.shape[1] - self.kernel_size[1]) / self.stride[1]) + 1

        output = np.zeros((number_of_inputs,channels,height_iterations,width_iterations))

        for i in range(number_of_inputs):
            for j in range(channels):
                output[i,j] = self.maxPoolOnInstance(
                    input[i,j,:,:], 
                    height_iterations, 
                    width_iterations)
        
        return output


    def maxPoolOnInstance(self, input, h, w):

        input_padded = padding2d(
            input = input, 
            kernel_size = self.kernel_size,
            pad_val = self.padding_value,
            stride = self.stride)

        output = np.zeros((h,w))

        for i in range(w):
            for j in range(h):
                output[i,j] = np.amax(
                    input_padded[j:j+self.kernel_size[0], 
                    i:i+self.kernel_size[1]])

        return output.transpose()