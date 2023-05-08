import ast
import json 
import numpy as np
import pandas as pd

def relative_root_mean_squared_error(true, pred):
    num = np.sum(np.square(true - pred))
    den = np.sum(np.square(pred))
    squared_error = num/den
    rrmse_loss = np.sqrt(squared_error)
    return rrmse_loss


def analysis(layers:str):
    dataframe = pd.DataFrame({"" : ["mean", "var", "max", "rrmse"]})
    for layer in layers:
        with open("Tests/" + layer + "/output.json", "r") as file:
            data = json.load(file)

        true = np.array(data["True"])
        pred = np.array(data["Pred"])

        loss = np.absolute(true-pred)

        mean = np.mean(loss)
        var = np.var(loss)
        max = np.max(loss)
        rrmse = relative_root_mean_squared_error(true,pred)

        dataframe[layer] = [mean,var,max,rrmse]
    
    return dataframe

def accuracy_newtork():
    with open("Tests/Network/output.json", "r") as file:
        data = json.load(file)
    
    true = np.array(data["True"])
    pred = np.array(data["Pred"])

    correct = len((np.argmax(true) == np.argmax(pred)))
    wrong   = len(true) - correct

    print(correct)
    print(wrong)


layers = ["conv1","batchNorm1","relu1","maxPool1","conv2","batchNorm2","relu2","maxPool2","linear","softmax"]

# layers_df = analysis(layers)
# print("Stats for the layers isolated")
# print(layers_df.to_latex(index=False))
# print("\n")

# accuracy_newtork()


