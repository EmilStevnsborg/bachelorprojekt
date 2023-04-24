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
        buffer = input.reshape(inChannels,height*width).tolist()
        computed = output.reshape(inChannels,height*width).tolist()

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
d["numOutChannels"] = 5
d["channelHeight"] = 5
d["channelWidth"] = 4

means1 = torch.rand(5)*10-5
vars1 = torch.rand(5)*10-5
gammas1 = torch.rand(5)*10-5
betas1 = torch.rand(5)*10-5

d["means"] = means1.tolist()
d["vars"] = vars1.tolist()
d["gammas"] = gammas1.tolist()
d["betas"] = betas1.tolist()

config_json = json.dumps(d, indent=4)
batchNorml1 = nn.BatchNorm2d(5)

batchNorml1.load_state_dict({"running_mean" : means1, "running_var" : vars1, "weight" : gammas1, "bias" : betas1})

if True:
    with open("TestConfig1/config.json", "w") as outfile:
        outfile.write(config_json)
    create_input_json(batchNorml1, 5, 5, 4, 10, "TestConfig1/")