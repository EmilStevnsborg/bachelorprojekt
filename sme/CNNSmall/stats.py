import ast
import json 
import numpy as np
import pandas as pd
from sklearn.metrics import accuracy_score

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

def analysis_network():
    with open("Tests/Network/output.json", "r") as file:
        data = json.load(file)
    
    true = np.array(data["True"])
    pred = np.array(data["Pred"])

    true_class = []
    pred_class = []
    
    for i in range(0, 10, 2):
        true_val = true[i: i + 2]
        pred_val = pred[i: i + 2]
        loss = true_val - pred_val

        true_class.append(np.argmax(true_val))
        pred_class.append(np.argmax(pred_val))

        if (np.argmax(true_val) != np.argmax(pred_val)):

            print("Index: {}, True value: {}, Pred value: {}, loss: {}".format(i,true_val, pred_val, loss))
    
    acc = accuracy_score(true_class, pred_class)

    dataframe = pd.DataFrame({"" : ["accuracy"]})
    dataframe.loc[len(dataframe)] = acc

    return dataframe


layers = ["conv1","batchNorm1","relu1","maxPool1","conv2","batchNorm2","relu2","maxPool2","linear","softmax"]

# layers_df = analysis(layers)
# print("Stats for the layers isolated")
# # print(layers_df.to_latex(index=False))
# print(layers_df)
# print("\n")

print("Accuracy of class predictions of SME implementation in relation to the PyTorch implementation")
print(analysis_network())
print(analysis(["Network"]))


