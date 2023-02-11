import numpy as np

class Conv:
    # each filter contains a list of an array of kernels and a bias, where each kernel is dedicated to a single channel and the bias to the filter
    # kernels and channels will be numpy 2d arrays
    def __init__(self, in_channels:int, filters):
        self.in_channels = in_channels
        self.filters = filters
    
    def __call__(self, input):
        # input is a list of channels where each channel is a 2d numpy array
        input_channels = len(input)
        outputs = []

        if input_channels != self.in_channels:
            raise Exception("channels in input != expected channels")
        
        for filter in self.filters:
            intermediates = []
            kernels = filter[0]
            bias = filter[1]
            for i in range(self.in_channels):
                channel2d = input[i]
                kernel2d = kernels[i]
                intermediates.append(self.execute(channel2d, kernel2d))
            
            out_channel = sum(intermediates) + bias
            outputs.append(out_channel)
        
        return outputs


    
    def execute(self, channel2d, kernel2d):
        r, c = channel2d.shape
        kr, kc = kernel2d.shape
        out_channel = np.zeros((r-kr+1,c-kc+1))
        for i in range(r-kr+1):
            for j in range(c-kc+1):
                channel2d_slice = channel2d[i:i+kr, j:j+kc]
                ij = np.sum(np.multiply(kernel2d, channel2d_slice))
                out_channel[i,j] = ij
        
        return out_channel





