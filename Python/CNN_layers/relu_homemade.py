import numpy as np

class relu():
    def __init__(self):
        """
        Initializes a relu layer

        Relu returns x if x > 0 and 0 otherwise
        """
    
    def __call__(self, input):
        """
        Returns x if x > 0 and 0 otherwise.

        Args:
            input: The input which the ReLU function is supposed to be executed on. Expected shape
            is (sample x channel x height x width).
        """
        input = np.array(input)

        self.input_size = input.shape

        output = np.ndarray(self.input_size)
        
        for i in range(self.input_size[0]):
            for j in range(self.input_size[1]):
                for k in range(self.input_size[2]):
                    for l in range(self.input_size[3]):
                        if input[i,j,k,l]>0:
                            output[i,j,k,l] = input[i,j,k,l]
                        else:
                            output[i,j,k,l] = 0
        
        return output

