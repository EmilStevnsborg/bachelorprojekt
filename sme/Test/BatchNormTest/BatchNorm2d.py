import json 
import torch
import torch.nn as nn
torch.manual_seed(0)
torch.set_printoptions(sci_mode=False, precision=20)

def create_input_json(model, inChannels, height, width, samples, dir):
    model.eval()
    for i in range(1,samples+1):
        d = {}

        input = torch.rand(1,inChannels,height,width, requires_grad=False)*10-5
        output = model(input)
        buffer = input.reshape(inChannels,height*width).tolist()
        computed = output.reshape(inChannels,height*width).tolist()

        d["buffer"] = buffer
        d["computed"] = computed

        input_json = json.dumps(d, indent=4)
        
        with open(dir + "input" + str(i) + ".json", "w+") as outfile:
            outfile.write(input_json)

# Config 1
####################################################################################################
#
d = {}
d["numInChannels"] = 5
d["numOutChannels"] = 5
d["channelHeight"] = 5
d["channelWidth"] = 4

means1 = torch.rand(5)*10-5
vars1 = torch.rand(5)*25
gammas1 = torch.rand(5)*10-5
betas1 = torch.rand(5)*10-5

d["means"] = means1.tolist()
d["vars"] = vars1.tolist()
d["gammas"] = gammas1.tolist()
d["betas"] = betas1.tolist()

config_json = json.dumps(d, indent=4)
batchNorm1 = nn.BatchNorm2d(5)

batchNorm1.load_state_dict({"running_mean" : means1, "running_var" : vars1, "weight" : gammas1, "bias" : betas1})

if True:
    with open("TestConfig1/config.json", "w+") as outfile:
        outfile.write(config_json)
    create_input_json(batchNorm1, 5, 5, 4, 10, "TestConfig1/")

# Config 3
####################################################################################################
#
d = {}
d["numInChannels"] = 10
d["numOutChannels"] = 10
d["channelHeight"] = 3
d["channelWidth"] = 9

means2 = torch.rand(10)*10-5
vars2 = torch.rand(10)*25
gammas2 = torch.rand(10)*10-5
betas2 = torch.rand(10)*10-5

d["means"] = means2.tolist()
d["vars"] = vars2.tolist()
d["gammas"] = gammas2.tolist()
d["betas"] = betas2.tolist()

config_json = json.dumps(d, indent=4)
batchNorm2 = nn.BatchNorm2d(10)

batchNorm2.load_state_dict({"running_mean" : means2, "running_var" : vars2, "weight" : gammas2, "bias" : betas2})

if True:
    with open("TestConfig2/config.json", "w+") as outfile:
        outfile.write(config_json)
    create_input_json(batchNorm2, 10, 3, 9, 10, "TestConfig2/")

# Config 3
####################################################################################################
#
d = {}
d["numInChannels"] = 20
d["numOutChannels"] = 20
d["channelHeight"] = 5
d["channelWidth"] = 11

means3 = torch.rand(20)*10-5
vars3 = torch.rand(20)*25
gammas3 = torch.rand(20)*10-5
betas3 = torch.rand(20)*10-5

d["means"] = means3.tolist()
d["vars"] = vars3.tolist()
d["gammas"] = gammas3.tolist()
d["betas"] = betas3.tolist()

config_json = json.dumps(d, indent=4)
batchNorm3 = nn.BatchNorm2d(20)

batchNorm3.load_state_dict({"running_mean" : means3, "running_var" : vars3, "weight" : gammas3, "bias" : betas3})

if True:
    with open("TestConfig3/config.json", "w+") as outfile:
        outfile.write(config_json)
    create_input_json(batchNorm3, 20, 5, 11, 10, "TestConfig3/")