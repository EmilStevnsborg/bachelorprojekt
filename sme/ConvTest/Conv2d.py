import json 
import torch
import torch.nn as nn
torch.manual_seed(0)
torch.set_printoptions(sci_mode=False, precision=8)
    

def create_input_json(model, inChannels, outChannels, height, width, samples, dir):
    for i in range(1,samples+1):
        d = {}

        input = torch.rand(1,inChannels,height,width, requires_grad=False)
        output = model(input)
        out_height, out_width = output.shape[2:]
        buffer = input.reshape(inChannels,height*width).tolist()
        computed = output.reshape(outChannels,out_height*out_width).tolist()

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
d["numOutChannels"] = 2
d["channelHeight"] = 4
d["channelWidth"] = 5
d["kernelHeight"] = 2
d["kernelWidth"] = 3
d["strideRow"] = 2
d["strideCol"] = 1
d["padHeight"] = 0
d["padWidth"] = 0
d["padVal"] = 0

weights1 = torch.rand(2,1,2,3, requires_grad=False)
biases1 = torch.rand(2)
d["weights"] = weights1.reshape(2,1,2*3).tolist()
d["biases"] = biases1.tolist()

config_json = json.dumps(d, indent=4)
conv1 = nn.Conv2d(1,2,(2,3),(2,1),(0,0))
conv1.load_state_dict({"weight" : weights1, "bias" : biases1})

if False:
    with open("TestConfig1/config.json", "w") as outfile:
        outfile.write(config_json)
    create_input_json(conv1, 1, 2, 4, 5, 10, "TestConfig1/")

# Config 2
####################################################################################################
#

d = {}
d["numInChannels"] = 3
d["numOutChannels"] = 2
d["channelHeight"] = 4
d["channelWidth"] = 5
d["kernelHeight"] = 2
d["kernelWidth"] = 3
d["strideRow"] = 2
d["strideCol"] = 1
d["padHeight"] = 2
d["padWidth"] = 1
d["padVal"] = 0

weights2 = torch.rand(2,3,2,3, requires_grad=False)
biases2 = torch.rand(2)
d["weights"] = weights2.reshape(2,3,2*3).tolist()
d["biases"] = biases2.tolist()

config_json = json.dumps(d, indent=4)
conv2 = nn.Conv2d(3,2,(2,3),(2,1),(2,1))
conv2.load_state_dict({"weight" : weights2, "bias" : biases2})

if False:
    with open("TestConfig2/config.json", "w") as outfile:
        outfile.write(config_json)
    create_input_json(conv2, 3, 2, 4, 5, 10, "TestConfig2/")


# Config 3
####################################################################################################
#

d = {}
d["numInChannels"] = 5
d["numOutChannels"] = 2
d["channelHeight"] = 5
d["channelWidth"] = 4
d["kernelHeight"] = 5
d["kernelWidth"] = 4
d["strideRow"] = 1
d["strideCol"] = 4
d["padHeight"] = 3
d["padWidth"] = 2
d["padVal"] = 0

weights3 = torch.rand(2,5,5,4, requires_grad=False)
biases3 = torch.rand(2)
d["weights"] = weights3.reshape(2,5,5*4).tolist()
d["biases"] = biases3.tolist()

config_json = json.dumps(d, indent=4)
conv3 = nn.Conv2d(5,2,(5,4),(1,4),(3,2))
conv3.load_state_dict({"weight" : weights3, "bias" : biases3})

if True:
    with open("TestConfig3/config.json", "w") as outfile:
        outfile.write(config_json)
    create_input_json(conv3, 5, 2, 5, 4, 10, "TestConfig3/")




