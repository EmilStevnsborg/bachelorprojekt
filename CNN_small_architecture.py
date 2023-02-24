import torch
import torch.nn as nn

class CNNSmall(nn.Module):
    def __init__(self):
        super().__init__()
        self.conv1 = nn.Conv2d(1, 3, (3,3)) 
        self.batchNorm1 = nn.BatchNorm2d(3)
        self.maxPool1 = nn.MaxPool2d((2,2))
        self.conv2 = nn.Conv2d(3, 5, (5,5)) 
        self.batchNorm2 = nn.BatchNorm2d(5)
        self.maxPool2 = nn.MaxPool2d((3,3))
        self.lin = nn.Linear(45,2)

        self.network = nn.Sequential( 
            
            # batch x 1 x 28 x 28
            self.conv1, 
            # batch x 3 x 26 x 26
            self.batchNorm1,
            nn.ELU(),           
            self.maxPool1,
            # batch x 3 x 13 x 13
            self.conv2, 
            # batch x 5 x 9 x 9
            self.batchNorm2,
            nn.ELU(),           
            self.maxPool2,
            # batch x 5 x 3 x 3
            nn.Flatten(),
            # batch x 45
            self.lin
            
        )
    
    def forward(self, x):
        return self.network(x)
