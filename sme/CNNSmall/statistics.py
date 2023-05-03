import ast
import json 
import numpy as np

def relative_mean_error(true, pred):
    num = np.mean(np.absolute(true-pred))
    den = np.sum(np.absolute(pred))
    return num/den


def relative_root_mean_squared_error(true, pred):
    num = np.mean(np.square(true - pred))
    den = np.sum(np.square(pred))
    squared_error = num/den
    rrmse_loss = np.sqrt(squared_error)
    return rrmse_loss


def analysis(layer:str):
    with open("Tests/" + layer + "/output.json", "r") as file:
        data = json.load(file)

    true = np.array(data["True"])
    pred = np.array(data["Pred"])

    print(layer.upper() + "-statistics")
    print("RME: " + str(relative_mean_error(true,pred)))
    print("RRMSE: " + str(relative_root_mean_squared_error(true,pred)) + "\n")


analysis("conv1")
analysis("batchNorm1")
analysis("relu1")
analysis("maxPool1")
analysis("conv2")
analysis("batchNorm2")
analysis("relu2")
analysis("maxPool2")
analysis("linear")