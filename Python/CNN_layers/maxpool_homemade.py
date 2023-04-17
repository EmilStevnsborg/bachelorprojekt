import numpy as np
from conv_utils.padding_homemade import padding

class MaxPool():
    
    def __init__(self, kernel_size: tuple, stride: tuple = None, padding: tuple = (0,0), pad_val: int = 0):

        # Inspired by torch.nn.MaxPool2d - if stride is not set then it will be set to the size
        # of the kernel
        if (stride == None):
            self.stride = kernel_size
        else:
            self.stride = stride

        self.kernel_size = kernel_size
        self.padding = padding
        self.pad_val = pad_val
    
    # Current implementation: kernel_size, stride used in self.pool_operation 
    # and padding applied before must correlate with the dimensions of the input (height x width)
    # This means that no padding or removal of columns/rows is done automatically
    def __call__(self, input_batch):

        # The input is a list of lists of (height x width) numpy arrays
        # In the order of: batches -> channels -> (height x width)

        output_batch = []
        for input in input_batch:
            # input is a list of batches with a list of channels where each channel is a 2d numpy array
            in_channels = len(input)
            output = []
            for i in range(in_channels):
                # apply padding to input
                channel2d = padding(channel2d=input[i], padding=self.padding, pad_val=0)
                out_channel = self.pool_operation(channel2d=channel2d)
                output.append(out_channel)
            
            output_batch.append(output)
        
        return output_batch
    
    def pool_operation(self, channel2d):

        r, c = channel2d.shape
        kr, kc = self.kernel_size
        stride_r, stride_c = self.stride

        out_r = int((r + 2 * self.padding[0] - (kr - 1) -1) / stride_r + 1)
        out_c = int((c + 2 * self.padding[1] - (kc - 1) -1) / stride_c + 1)

        out_channel = np.zeros((out_r, out_c))

        for i in range(out_r):
            for j in range(out_c):
                # stride are factors that push the indentation of the slice
                channel2d_slice = channel2d[i * stride_r: i * stride_r + kr, j * stride_c: j * stride_c + kc]
                ij = np.amax(channel2d_slice)
                out_channel[i,j] = ij
        
        return out_channel