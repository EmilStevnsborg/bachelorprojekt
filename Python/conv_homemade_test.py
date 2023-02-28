import conv_homemade
import numpy as np

# TEST 1
#
# One batch with one channel 3x2
input_batch1 = [[np.array([
                    [1,1],
                    [1,1],
                    [0,1]
                            ])]]

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

padding1 = (0,1)
stride1 = (1,2)

output_batch1 = [[np.array([
                    [2,2]
                            ]),
                  np.array([
                    [2,2]
                            ]),
                  np.array([
                    [1,2]
                            ]),]]


# TEST 2
#
# One batch with two channels 3x3
input_batch2 = [[np.array([
                    [1,1,1],
                    [1,1,1],
                    [1,1,1]
                            ]),
          np.array([
                    [1,1,1],
                    [1,1,1],
                    [1,1,1]
                            ])]]

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


conv1_homemade = conv_homemade.Conv(filters=filters1, in_channels=1, padding=padding1, stride=stride1)

out_test_1 = conv1_homemade(input_batch=input_batch1)

print(out_test_1)
print(output_batch1)