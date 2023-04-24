import json 
import torch
import torch.nn as nn
torch.manual_seed(0)
torch.set_printoptions(sci_mode=False, precision=8)

def create_input_json(model, inChannels, height, width, samples, dir):
    for i in range(1,samples+1):
        d = {}

        input = torch.rand(1,inChannels,height,width, requires_grad=False)*10-5
        output = model(input)
        out_height, out_width = output.shape[2:]
        buffer = input.reshape(inChannels,height*width).tolist()
        computed = output.reshape(inChannels,out_height*out_width).tolist()

        d["buffer"] = buffer
        d["computed"] = computed

        input_json = json.dumps(d, indent=4)
        
        with open(dir + "input" + str(i) + ".json", "w") as outfile:
            outfile.write(input_json)

# Config 1
####################################################################################################
#
d = {}
d["numInChannels"] = 1
d["numOutChannels"] = 1
d["channelHeight"] = 4
d["channelWidth"] = 5
d["kernelHeight"] = 2
d["kernelWidth"] = 3
d["strideRow"] = 2
d["strideCol"] = 1
d["padHeight"] = 0
d["padWidth"] = 0
d["padVal"] = 0

config_json = json.dumps(d, indent=4)
maxPool1 = nn.MaxPool2d((2,3),(2,1),(0,0))

if True:
    with open("TestConfig1/config.json", "w") as outfile:
        outfile.write(config_json)
    create_input_json(maxPool1, 1, 4, 5, 10, "TestConfig1/")