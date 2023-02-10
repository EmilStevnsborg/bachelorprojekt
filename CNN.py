import numpy as np

class Conv:
    # each filter contains an array of kernels and an array of biases, where each kernel and bias are dedicated to a single channel
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
            biases = filter[-1]
            for i in range(self.in_channels):
                channel2d = input[i]
                kernel2d = filter[i]
                print(kernel2d)
                intermediates.append(self.execute(channel2d, kernel2d))
            
            out_channel = sum(intermediates) + sum(biases)
            outputs.append(out_channel)
        
        return outputs


    
    def execute(channel2d, kernel2d):
        r, c = channel2d.shape
        kr, kc = kernel2d.shape
        out_channel = np.zeroes((r-kr,c-kc))
        for i in range(r-kr):
            for j in range(c-kc):
                channel2d_slice = channel2d[i:i+kr, j:j+kc]
                ij = np.sum(np.multiply(kernel2d, channel2d_slice))
                out_channel[i,j] = ij
        
        return out_channel





