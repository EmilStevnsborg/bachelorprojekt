import json 
import torch
import torch.nn as nn
torch.manual_seed(0)
torch.set_printoptions(sci_mode=False, precision=20)

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
d["padVal"] = -100

config_json = json.dumps(d, indent=4)
maxPool1 = nn.MaxPool2d((2,3),(2,1),(0,0))

if True:
    with open("TestConfig1/config.json", "w") as outfile:
        outfile.write(config_json)
    create_input_json(maxPool1, 1, 4, 5, 10, "TestConfig1/")

# Config 2
####################################################################################################
#
d = {}
d["numInChannels"] = 10
d["numOutChannels"] = 10
d["channelHeight"] = 3
d["channelWidth"] = 4
d["kernelHeight"] = 1
d["kernelWidth"] = 4
d["strideRow"] = 1
d["strideCol"] = 4
d["padHeight"] = 0
d["padWidth"] = 2
d["padVal"] = -100

config_json = json.dumps(d, indent=4)
maxPool2 = nn.MaxPool2d((1,4),(1,4),(0,2))

if True:
    with open("TestConfig2/config.json", "w") as outfile:
        outfile.write(config_json)
    create_input_json(maxPool2, 10, 3, 4, 10, "TestConfig2/")

# Config 3
####################################################################################################
#
d = {}
d["numInChannels"] = 3
d["numOutChannels"] = 3
d["channelHeight"] = 8
d["channelWidth"] = 6
d["kernelHeight"] = 2
d["kernelWidth"] = 3
d["strideRow"] = 2
d["strideCol"] = 3
d["padHeight"] = 1
d["padWidth"] = 0
d["padVal"] = -100

config_json = json.dumps(d, indent=4)
maxPool3 = nn.MaxPool2d((2,3),(2,3),(1,0))

if True:
    with open("TestConfig3/config.json", "w") as outfile:
        outfile.write(config_json)
    create_input_json(maxPool3, 3, 8, 6, 10, "TestConfig3/")