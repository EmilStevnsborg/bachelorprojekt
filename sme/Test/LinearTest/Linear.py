import json 
import torch
import torch.nn as nn
torch.manual_seed(0)
torch.set_printoptions(sci_mode=False, precision=8)

def create_input_json(model, inChannels, outChannels, height, width, samples, dir):
    for i in range(1,samples+1):
        d = {}

        input = torch.rand(1,inChannels*height*width, requires_grad=False)*10-5
        output = model(input)
        buffer = input.reshape(inChannels,height*width).tolist()
        computed = output.reshape(outChannels,1).tolist()

        d["buffer"] = buffer
        d["computed"] = computed

        input_json = json.dumps(d, indent=4)
        
        with open(dir + "input" + str(i) + ".json", "w") as outfile:
            outfile.write(input_json)

# Config 1
####################################################################################################
#
d = {}
d["numInChannels"] = 5
d["numOutChannels"] = 4
d["channelHeight"] = 3
d["channelWidth"] = 7

weights1 = torch.rand(4,5*3*7, requires_grad=False)*10-5
biases1 = torch.rand(4)*10-5
d["weights"] = weights1.tolist()
d["biases"] = biases1.tolist()

config_json = json.dumps(d, indent=4)
lin1 = nn.Linear(5*3*7,4)
lin1.load_state_dict({"weight" : weights1, "bias" : biases1})

if True:
    with open("TestConfig1/config.json", "w") as outfile:
        outfile.write(config_json)
    create_input_json(lin1, 5, 4, 3, 7, 10, "TestConfig1/")


# Config 1
####################################################################################################
#
d = {}
d["numInChannels"] = 10
d["numOutChannels"] = 2
d["channelHeight"] = 5
d["channelWidth"] = 6

weights1 = torch.rand(2,10*5*6, requires_grad=False)*10-5
biases1 = torch.rand(2)*10-5
d["weights"] = weights1.tolist()
d["biases"] = biases1.tolist()

config_json = json.dumps(d, indent=4)
lin1 = nn.Linear(10*5*6,2)
lin1.load_state_dict({"weight" : weights1, "bias" : biases1})

if True:
    with open("TestConfig2/config.json", "w") as outfile:
        outfile.write(config_json)
    create_input_json(lin1, 10, 2, 5, 6, 10, "TestConfig2/")