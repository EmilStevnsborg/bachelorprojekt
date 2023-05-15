#importing relevant libraries
import numpy as np
import pandas as pd
from Tests.helper_functions import transform_input, compare
from CNN_layers.linear_layer_homemade import flatten

def layer_test(homemade_layer, original_layer, input_homemade, input_original, description, 
               flat: bool = True, dataframe : pd.DataFrame = pd.DataFrame()):
    # Making sure the input to the linear layer has the correct shape (it must be flat for each samp
    # le in the batch)
    if (not(flat)):
        for i in range(len(input_homemade)):
            if i == 0:
                input_homemade_ = np.array(flatten(input_homemade[i]))
            else:
                input_homemade_ = np.vstack((input_homemade_,flatten(input_homemade[i])))
    else:
        input_homemade_ = input_homemade
    # homemade conv1 filter on test
    out_homemade = homemade_layer(input_homemade_)
    # original conv1 filter on test
    out_original = original_layer(input_original)

    print(description)
    max_error,mean_error,var_error,mean_values,mean_rrmse = compare(out_homemade, 
                                             transform_input(input_batch = out_original))
    print("    max error: ", max_error)
    print("    mean error: ", mean_error)
    print("    variance of errors: ", var_error)
    print("    mean values: ", mean_values)
    print("    mean RRMSE: ", mean_rrmse)

    if dataframe.empty:
        data_ = {description: [max_error, mean_error, var_error,mean_values,mean_rrmse]}
        dataframe = pd.DataFrame(data = data_, index = ['max','mean','var','mean values',
                                                        'mean RRMSE'])
    else:
        dataframe.insert(len(dataframe.columns),description,[max_error,mean_error,var_error,mean_values,mean_rrmse])

    return out_homemade,out_original,dataframe