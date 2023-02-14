import numpy as np
from conv_utils.padding_homemade import padding

class Conv:
    # each filter contains a list of an array of kernels and a bias, where each kernel is dedicated to a single channel and the bias to the filter
    # kernels and channels will be numpy 2d arrays
    def __init__(self, filters, in_channels: int, padding: tuple = (0,0), stride: tuple = (1,1)):
        self.in_channels = in_channels
        self.padding = padding
        self.stride = stride
        self.filters = filters
    
    def __call__(self, input_batch):
        # output_batch is a list
        output_batch = []
        for input in input_batch:
            # input is a list of batches with a list of channels where each channel is a 2d numpy array
            input_channels = len(input)
            output = []

            if input_channels != self.in_channels:
                raise Exception("channels in input != expected channels")
            
            for filter in self.filters:
                intermediates = []
                kernels = filter[0]
                bias = filter[1]
                for i in range(self.in_channels):
                    # apply padding to input
                    channel2d = padding(channel2d=input[i], padding=self.padding, pad_val=0)
                    kernel2d = kernels[i]
                    intermediates.append(self.kernel_operation(channel2d, kernel2d))
                
                out_channel = sum(intermediates) + bias
                output.append(out_channel)
            
            output_batch.append(output)
        
        return output_batch


    
    def kernel_operation(self, channel2d, kernel2d):
        r, c = channel2d.shape
        kr, kc = kernel2d.shape
        stride_r, stride_c = self.stride

        out_r = r - (kr - 1) - (stride_r - 1)
        out_c = c - (kc - 1) - (stride_c - 1)

        out_channel = np.zeros((out_r, out_c))

        for i in range(out_r):
            for j in range(out_c):
                # stride are factors that push the indentation of the slice
                channel2d_slice = channel2d[i * stride_r: i * stride_r + kr, j * stride_c: j * stride_c + kc]
                ij = np.sum(np.multiply(kernel2d, channel2d_slice))
                out_channel[i,j] = ij
        
        return out_channel





