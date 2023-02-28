import numpy as np

class ELU():

    def __init__(self, alpha=1.0):
        self.alpha = alpha
    
    def __call__(self, input_batch):
        output_batch = []
        for batch in input_batch:

            out_channels = []
            for channel2d in batch:

                out_channel = self.elu(channel2d=channel2d)
                out_channels.append(out_channel)

            output_batch.append(out_channels)
        
        return output_batch
    
    def elu(self, channel2d):
        r, c = channel2d.shape
        out_channel2d = np.zeros((r, c))
        for i in range(r):
            
            for j in range(c):

                x = channel2d[i,j]
                if x > 0.0:
                    out_channel2d[i,j] = x
                else:
                    out_channel2d[i,j] = self.alpha * (np.exp(x) - 1.0)
        
        return out_channel2d