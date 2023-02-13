import numpy as np
import torch

# OBS PADDING AND STRIDING IS NOT IMPLEMENTED

class conv2d():
    def __init__(self, h: int, w: int, n: int = 1, parameters_ = None):
        self.parameters = torch.zeros((n,h,w))
        self.height = h
        self.width = w
        self.numberOutputNodes = n
        if (parameters_ != None):
            assert parameters_.shape() == (n,h,w)
            self.parameters = parameters_
        
    def set_parameters(self,parameters_):
        assert parameters_.shape() == (self.numberOutputNodes,self.height,self.width)
        self.parameters = parameters_

    def get_parameters(self):
        return self.parameters
    
    def forward(self,input):
        try:
            input_shape = input.shape
        except:
            print("The input datatype does not seem to have a shape method")
        output = np.zeros((self.numberOutputNodes,input_shape[0]-self.height+1,
                            input_shape[1]-self.width+1))

        if self.numberOutputNodes > 1:
        # THIS IS GONNA BE SLOW
            for n in range(self.numberOutputNodes):
                for i in range(input_shape[0]-self.height+1):
                    for j in range(input_shape[1]-self.width+1):
                        output[n,i,j] = (input[i:self.height,j:self.width] * 
                                        self.parameters[n]).sum()

        else:
            for i in range(input_shape[0]-self.height+1):
                for j in range(input_shape[1]-self.width+1):
                    output[0,i,j] = (input[j:j+self.width,i:i+self.height] * 
                                    self.parameters).sum()
                                    
        return output