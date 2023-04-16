#importing relevant libraries

from Tests.helper_functions import transform_input, compare

def layer_test(homemade_layer, original_layer, input_homemade, input_original, description):
    # homemade conv1 filter on test
    out_homemade = homemade_layer(input_homemade)
    # original conv1 filter on test
    out_original = original_layer(input_original)

    print(description)
    max_error,mean_error,var_error = compare(out_homemade, 
                                             transform_input(input_batch = out_original))
    print("    max error: ", max_error)
    print("    mean error: ", mean_error)
    print("    variance of errors: ", var_error)

    return out_homemade,out_original