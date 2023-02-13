import numpy as np

# CUSTOM STRIDE IS NOT IMPLEMENTED
def padding2d(input, kernel_size: tuple, pad_val: int):
    
    input_size = input.shape
    height_off = input_size[0] % kernel_size[0]
    width_off = input_size[1] % kernel_size[1]
    output_height = None
    output_width = None

    if (height_off == 0 and width_off == 0):
        return input
    if (height_off != 0):
        output_height = input_size[0] % kernel_size[0] + input_size[0]
    if (width_off != 0):
        output_width = input_size[1] % kernel_size[1] + input_size[1]

    output = np.full((output_height,output_width),pad_val)
    output[int(np.floor(height_off/2)):int(np.floor(height_off/2)) + input_size[0],
                    int(np.floor(width_off/2)):int(np.floor(width_off/2)) + input_size[1]] = input
    
    return output