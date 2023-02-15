import numpy as np
from maxpool_homemade import MaxPool

# TEST 1
#
# One batch with two channels 3x3
input_batch1 = [[np.array([
                    [1,2,3,4,5],
                    [0,0,0,0,0],
                    [1,2,3,4,5],
                    [3,9,81,0,0]
                                ]),
          np.array([
                    [1,1,1,1,1],
                    [2,4,16,0,0],
                    [3,9,81,0,0],
                    [1,1,1,1,1]
                                ])]]

kernel_size1 = (2,2)
stride1 = (2,1)

output_batch1 = [[np.array([
                    [2,3,4,5],
                    [9,81,81,5]
                                ]),
          np.array([
                    [4,16,16,1],
                    [9,81,81,1]
                                ])]]

pool1_homemade = MaxPool(kernel_size=kernel_size1, stride=stride1)
out1 = pool1_homemade(input_batch1)
print(out1)
print(output_batch1)