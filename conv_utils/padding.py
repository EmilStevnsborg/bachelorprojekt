import numpy as np

# CUSTOM STRIDE IS NOT IMPLEMENTED
# IN THIS IMPLMENTATION THE KERNEL MUST BE SMALLER THAN THE INPUT
def padding2d(input, kernel_size: tuple, pad_val: int, stride: tuple = None):    

    input_size = input.shape

    if (stride == None):
        stride = kernel_size

    # This should work if stride = kernel_size, however, it does not generalize 
    #height_off = (kernel_size[0] * int(np.ceil(input_size[0]/kernel_size[0]))) % input_size[0]
    #width_off = (kernel_size[1] * int(np.ceil(input_size[1]/kernel_size[1]))) % input_size[0]

    # This should generalize, however, testing is needed
    height_off = (kernel_size[0] + stride[0] * int(np.ceil(input_size[0]-kernel_size[0])/stride[0])) % input_size[0]
    width_off = (kernel_size[1] + stride[0] * int(np.ceil(input_size[1]-kernel_size[1])/stride[1])) % input_size[1]
    
    output_height = None
    output_width = None

    assert input_size[0] >= kernel_size[0] and input_size[1] >= kernel_size[1]

    if (height_off == 0 and width_off == 0):
        return input
    if (height_off != 0):
        output_height = height_off + input_size[0]
    if (width_off != 0):
        output_width = width_off + input_size[1]

    output = np.full((output_height,output_width),pad_val)

    output[int(np.floor(height_off/2)):int(np.floor(height_off/2)) + input_size[0],
                    int(np.floor(width_off/2)):int(np.floor(width_off/2)) + input_size[1]] = input
    
    return output