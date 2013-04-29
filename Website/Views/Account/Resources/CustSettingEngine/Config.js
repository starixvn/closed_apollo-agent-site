// Test PT: M99RM02001
var pt = {
    Min: 0,
    Max: 60,
    Preset: 30,
    Value: 35,
    Upline:
    {
        Max: 40,
        Min: 0,
        Preset: 55,
        Value: 15
    }
};

var commissionSettingData = {
    CommissionItems: [{
        Group: "A",
        SportId: 1,
        BetTypeId: 1,
        Name: "Group A",
        Discount: 0.5,
        DiscountA: 0.15,
        DiscountB: 0.25,
        DiscountC: 0.35,
        DiscountD: 0.45,
        Upline:{
                DiscountA: 0.252,
                DiscountB: 0.35,
                DiscountC: 0.45,
                DiscountD: 0.55
             }
     }, {
        Group: "",
        Name: "1X2",
        Discount: 0.2,
        DiscountA: 0.2,
        Upline: {
            DiscountA: 0.3
        }
    }, {
        Group: "",
        Name: "Ohters",
        Discount: 0.5,
        DiscountA: 0.5,
        Upline: {
            DiscountA: 0.8
        }
    }, {
        Group: "",
        Name: "Number Game",
        Discount: 0.5,
        DiscountA: 0.5,
        Upline: {
            DiscountA: 0.8
        }
    }],
    UserGroup: "A"
};

var bettingLimits2 = {
    "bettingLimitItems": [{
        "SportId": 1,
        "Name": "Soccer",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000,
        "MinBetMax": 11.0000,
        "MaxBetMin": 700.0000,
        
        "MaxPerMatchMin": 10000.0000
    }, {
        "SportId": 2,
        "Name": "Basketball",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000,
        "MinBetMax": 11.0000,
        "MaxBetMin": 700.0000,
        
        "MaxPerMatchMin": 10000.0000
    }, {
        "SportId": 3,
        "Name": "Football",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000,
        "MinBetMax": 11.0000,
        "MaxBetMin": 700.0000,
        
        "MaxPerMatchMin": 10000.0000
    }, {
        "SportId": 5,
        "Name": "Tennis",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000,
        "MinBetMax": 11.0000,
        "MaxBetMin": 700.0000,
        
        "MaxPerMatchMin": 10000.0000
    }, {
        "SportId": 8,
        "Name": "Baseball",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000,
        "MinBetMax": 11.0000,
        "MaxBetMin": 700.0000,
        
        "MaxPerMatchMin": 10000.0000
    }, {
        "SportId": 10,
        "Name": "Golf",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000,
        "MinBetMax": 11.0000,
        "MaxBetMin": 700.0000,
        
        "MaxPerMatchMin": 10000.0000
    }, {
        "SportId": 11,
        "Name": "Moto Sports",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000,
        "MinBetMax": 11.0000,
        "MaxBetMin": 700.0000,
        
        "MaxPerMatchMin": 10000.0000
    }, {
        "SportId": 99,
        "Name": "Other Sports",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000,
        "MinBetMax": 11.0000,
        "MaxBetMin": 700.0000,
        
        "MaxPerMatchMin": 10000.0000
    }, {
        "SportId": 99,
        "Name": "Mix Sports Parlay",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000,
        "MinBetMax": 11.0000,
        "MaxBetMin": 700.0000,
        
        "MaxPerMatchMin": 10000.0000
    }
//    , {
//        "SportId": 161,
//        "Name": "Number Game",
//        "MinBet": 10.0000,
//        "MaxBet": 100.0000,
//        "MaxPerMatch": 1000.0000,
//        "MaxPerBall": 1000.0000,
//        "MinBetMax": 10.0000,
//        "MaxBetMin": 100.0000,
//        
//        "MaxPerMatchMin": 1000.0000,
//        "MaxPerBallMin": 1000.0000
//    }
    ]
};
var bettingLimits = {
    "bettingLimitItems": [{
        "SportId": 1,
        "Name": "Soccer",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000
    }, {
        "SportId": 2,
        "Name": "Basketball",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000
    }, {
        "SportId": 3,
        "Name": "Football",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000
    }, {
        "SportId": 5,
        "Name": "Tennis",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000
    }, {
        "SportId": 8,
        "Name": "Baseball",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000
    }, {
        "SportId": 10,
        "Name": "Golf",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000
    }, {
        "SportId": 11,
        "Name": "Moto Sports",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000
    }, {
        "SportId": 99,
        "Name": "Other Sports",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000
    }, {
        "SportId": 99,
        "Name": "Mix Sports Parlay",
        "MinBet": 11.0000,
        "MaxBet": 700.0000,
        "MaxPerMatch": 10000.0000
    }, {
        "SportId": 161,
        "Name": "Number Game",
        "MinBet": 10.0000,
        "MaxBet": 100.0000,
        "MaxPerMatch": 1000.0000,
        "MaxPerBall": 1000.0000
    }]
};
var grid = {
    Name: 'ptGrId1',
    SportTypes: [{
        Id: 1,
        Name: 'Soccer',
        IsLive: false,
        Collapsed: false,
        BetTypeGroups: [{
            Id: 1,
            BetTypes: [{
                Id: 1,
                Name: 'Handicap',
                Pt: pt
            }, {
                Id: 3,
                Name: 'Over/Under',
                MinWidth: 140,
                Pt: pt
            }, {
                Id: 7,
                Name: '1st Hdp',
                MinWidth: 140,
                Pt: pt
            }, {
                Id: 8,
                Name: '1st OU',
                MinWidth: 140,
                Pt: pt
            }, {
                Id: 2,
                Name: 'Odd/Even',
                MinWidth: 140,
                Pt: pt
            }]
        }, {
            Id: 2,
            BetTypes: [{
                Id: 5,
                Name: '1X2',
                Pt: pt
            }, {
                Id: 4,
                Name: 'Correct Score',
                MinWidth: 140,
                Pt: pt
            }, {
                Id: 6,
                Name: 'Total Goal',
                MinWidth: 140,
                Pt: pt
            }, {
                Id: 9,
                Name: 'Mix Parlay',
                MinWidth: 140,
                Pt: pt
            }, {
                Id: 10,
                Name: 'Outright',
                MinWidth: 140,
                Pt: pt
            }]
        }]
    },
    {
        Id: 1,
        Name: 'Soccer',
        Collapsed: false,
        IsLive: true,

        BetTypeGroups: [{
            BetTypes: [{
                Id: 1,
                Name: 'Handicap',
                Pt: pt
            }, {
                Id: 3,
                Name: 'Over/Under',
                MinWidth: 140,
                Pt: pt
            }, {
                Id: 7,
                Name: '1st Hdp',
                MinWidth: 140,
                Pt: pt
            }, {
                Id: 8,
                Name: '1st OU',
                MinWidth: 140,
                Pt: pt
            }]
        }]
    }, {
        Id: 2,
        Name: 'Basketball',
        Collapsed: false,
        IsLive: false,
        BetTypeGroups: [{
            BetTypes: [{
                Id: 1,
                Name: 'Handicap',
                Pt: pt
            }, {
                Id: 3,
                Name: 'Over/Under',
                Pt: pt
            }, {
                Id: 2,
                Name: 'Odd/Even',
                Pt: pt
            }, {
                Id: 20,
                Name: 'Money Line',
                Pt: pt
            }]
        }]
    }, {
        Id: 3,
        Name: 'Football',
        IsLive: false,

        BetTypeGroups: [{
            BetTypes: [{
                Id: 1,
                Name: 'Handicap',
                Pt: pt
            }, {
                Id: 3,
                Name: 'Over/Under',
                Pt: pt
            }, {
                Id: 2,
                Name: 'Odd/Even',
                Pt: pt
            }, {
                Id: 20,
                Name: 'Money Line',
                Pt: pt
            }]
        }]
    }, {
        Id: 5,
        Name: 'Tennis',
        IsLive: false,

        BetTypeGroups: [{
            BetTypes: [{
                Id: 1,
                Name: 'Handicap',
                Pt: pt
            }, {
                Id: 3,
                Name: 'Over/Under',
                Pt: pt
            }, {
                Id: 2,
                Name: 'Odd/Even',
                Pt: pt
            }, {
                Id: 20,
                Name: 'Money Line',
                Pt: pt
            }]
        }]
    }, {
        Id: 8,
        Name: 'Baseball',
        IsLive: false,

        BetTypeGroups: [{
            BetTypes: [{
                Id: 1,
                Name: 'Handicap',
                Pt: pt
            }, {
                Id: 3,
                Name: 'Over/Under',
                Pt: pt
            }, {
                Id: 20,
                Name: 'Money Line',
                Pt: pt
            }]
        }]
    }, {
        Id: 10,
        Name: 'Golf',
        IsLive: false,

        BetTypeGroups: [{
            BetTypes: [{
                Id: 1,
                Name: 'Handicap',
                Pt: pt
            }, {
                Id: 3,
                Name: 'Over/Under',
                Pt: pt
            }, {
                Id: 2,
                Name: 'Odd/Even',
                Pt: pt
            }, {
                Id: 20,
                Name: 'Money Line',
                Pt: pt
            }]
        }]
    }, {
        Id: 11,
        Name: 'Moto Sports',
        IsLive: false,

        BetTypeGroups: [{
            BetTypes: [{
                Id: 1,
                Name: 'Handicap',
                Pt: pt
            }, {
                Id: 20,
                Name: 'Money Line',
                Pt: pt
            }]
        }]
    }, {
        Id: 99,
        Name: 'Other Sports',
        IsLive: false,
        BetTypeGroups: [{
            BetTypes: [{
                Id: 1,
                Name: 'Handicap',
                Pt: pt
            }, {
                Id: 3,
                Name: 'Over/Under',
                Pt: pt
            }, {
                Id: 2,
                Name: 'Odd/Even',
                Pt: pt
            }, {
                Id: 20,
                Name: 'Money Line',
                Pt: pt
            }]
        }]
    }, {
        Id: 99,
        Name: 'Mix Sports Parlay',
        IsLive: false,

        BetTypeGroups: [{
            BetTypes: [{
                Id: 9,
                Name: 'Mix Parlay',
                Pt: pt,
                MinWidth: 140
            }]
        }]
    }
//    , {
//        Id: 161,
//        Name: 'Number Game',
//        IsLive: false,
//        BetTypeGroups: [{
//            BetTypes: [{
//                Id: 81,
//                Name: '1st/Last O/U',
//                Pt: pt
//            }, {
//                Id: 83,
//                Name: '1st/Last O/E',
//                MinWidth: 140,
//                Pt: pt
//            }, {
//                Id: 88,
//                Name: 'Warrior',
//                MinWidth: 140,
//                Pt: pt
//            }, {
//                Id: 86,
//                Name: 'Odd/Even',
//                MinWidth: 140,
//                Pt: pt
//            }]
//        }]
//    },
//     {
//         Id: 161,
//         Name: 'Number Game',
//         IsLive: true,
//         BetTypeGroups: [{
//             BetTypes: [{
//                 Id: 85,
//                 Name: 'Over/Under',
//                 Pt: pt
//             }, {
//                 Id: 86,
//                 Name: 'Odd/Even',
//                 MinWidth: 140,
//                 Pt: pt
//             }, {
//                 Id: 87,
//                 Name: 'High/Low',
//                 MinWidth: 140,
//                 Pt: pt
//             }, {
//                 Id: 89,
//                 Name: 'Next Combo',
//                 MinWidth: 140,
//                 Pt: pt
//             }, {
//                 Id: 90,
//                 Name: 'Number Group',
//                 MinWidth: 140,
//                 Pt: pt
//             }]
//         }]
//     }
]
}