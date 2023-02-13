import conv_homemade
import numpy as np

# One channel 3x3
input1 = [np.array([
                    [1,2,3],
                    [4,5,6],
                    [7,8,9]
                            ])]

# three filters, one kernel 3x2 one bias for each filter
filters1 = [
            # In one filter one kernel (implying in_channel = 1)
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

padding1 = 0
stride1 = 0

# Two channels 3x3
input2 = [np.array([
                    [1,2,3],
                    [4,5,6],
                    [7,8,9]
                            ]),
          np.array([
                    [1,2,3],
                    [4,5,6],
                    [7,8,9]
                            ])]

# three filters, two kernels 2x2 one bias for each filter
filters2 = [
            # In one filter two kernels (implying in_channel = 2)
            [[np.array([
                    [2,2],
                    [0,0]
                            ]),
              np.array([
                     [0,0],
                     [1,1]
                             ])
                                ], 1],
            [[np.array([
                    [1,1],
                    [0,0]
                            ]),
              np.array([
                     [0,0],
                     [1,1]
                             ])
                                ], 0]]


conv1_homemade = conv_homemade.Conv(in_channels=1, padding=0, stride=0, filters=filters2)

out = conv1_homemade(input=input1)

print(out)