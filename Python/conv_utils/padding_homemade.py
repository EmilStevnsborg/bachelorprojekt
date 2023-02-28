import numpy as np

def padding(channel2d, padding: tuple, pad_val: int):
    r, c = channel2d.shape
    pr, pc = padding
    new_r = r + 2*pr
    new_c = c + 2*pc
    
    channel2d_padded = np.empty(shape = (new_r, new_c))

    #fill channel2d_padded
    for i in range(new_r):
        for j in range(new_c):
            if (i < pr or j < pc or i >= r+pr or j >= c+pc):
                channel2d_padded[i,j] = pad_val
            else:
                channel2d_padded[i,j] = channel2d[i-pr,j-pc]
    
    return channel2d_padded


# TEST
# 
# channel2d_test = np.array([[1,1,1],[1,1,1],[1,1,1]])
# padding_test = (1,2)
# pad_val_test = 0
# channel2d_padded_test = np.array([[0,0,0,0,0,0,0],[0,0,1,1,1,0,0],[0,0,1,1,1,0,0],[0,0,1,1,1,0,0],[0,0,0,0,0,0,0]])

# print(padding(channel2d_test, padding_test, pad_val_test))
# print(channel2d_padded_test)