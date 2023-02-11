import conv_homemade
import numpy as np

# One channel 3x3
input = [np.array([
                    [1,2,3],
                    [4,5,6],
                    [7,8,9]
                            ])]

# three filters one kernel 3x2 one bias for each kernel
filters1 = [
            # one filter with one kernel (implying in_channel = 1) and bias
            [[np.array([
                    [1,1],
                    [0,0],
                    [0,0]
                            ])], 1],
            [[np.array([
                    [0,0],
                    [1,1],
                    [0,0]
                            ])], 1],
            [[np.array([
                    [0,0],
                    [0,0],
                    [1,1]
                            ])], 1]]

# three filters one kernel 2x2 one bias for each kernel
filters2 = [
            # one filter with one kernel (implying in_channel = 1) and bias
            [[np.array([
                    [1,1],
                    [0,0]
                            ])], 1],
            [[np.array([
                    [0,0],
                    [1,1]
                            ])], 1],
            [[np.array([
                    [0,0],
                    [0,0]
                            ])], 1]]


conv1_homemade = conv_homemade.Conv(in_channels=1, filters=filters2)

out = conv1_homemade(input=input)

print(out)