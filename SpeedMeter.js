var SpeedMeter = function(SensorPin, levelUpPin, levelDownPin, weighting, pulsesPerRev, timeOut, bounceTime) {
  var rpm = 0;
  var level = 2;
  var timeAct = 0;
  var timeOld = 0;
  var timeDiff = 0;
  var timeLevel = 0;
  var watchdog = null;
  var gpio = require('rpi-gpio');
  var power = [
    [8,16,25,38,50,69,88,103,120,138,152],
    [11,22,34,51,69,91,113,131,154,178,192],
    [12,24,42,64,88,114,145,175,202,238,254],
    [15,31,51,77,107,140,170,206,236,280,294],
    [19,38,60,91,126,163,203,243,283,327,367],
    [20,40,69,105,145,188,228,273,319,362,409],
    [22,48,79,119,164,214,260,310,360,400,458],
    [24,53,88,133,183,237,286,350,406,447,518],
    [28,57,97,146,202,263,318,389,449,494,585],
    [30,64,106,160,221,289,337,415,476,518,620],
    [32,68,115,174,240,312,351,435,499,544,655],
    [33,72,123,186,259,338,293,483,550,596,712],
    [37,79,133,200,278,361,427,527,592,652,783],
    [40,87,142,215,297,387,462,571,636,709,832],
    [42,91,151,228,316,409,485,599,669,744,864],
    [44,95,160,242,335,436,515,633,708,787,905]
   ];
  
  
  weighting = weighting || 0;
  pulsesPerRev = pulsesPerRev || 1;
  timeOut = timeOut || 10000;
  bounceTime = bounceTime || 200;
  
  gpio.on('change', function(channel, value)
  {
    if (channel == SensorPin)
    {
      var timeAct = new Date();
      if ((timeAct - timeOld) > bounceTime)
      {
        if (watchdog)
        {
          clearTimeout(watchdog);
        }
        if (timeOld)
        {
          timeDiff *= weighting;
          timeDiff += (1 - weighting) * (timeAct - timeOld);
          rpm = 60000 / (timeDiff * pulsesPerRev);
        }
        timeOld = timeAct;
        watchdog = setTimeout(function()
        {
          timeOld = 0;
          rpm = 0;
        }, timeOut);
      }
    }
    else if (channel == levelUpPin)
    {
      var timeLevelUp = new Date();
      if ((timeLevelUp - timeLevel) > bounceTime)
      {
        if (level < 16)
        {
          ++level;
        }
        timeLevel = timeLevelUp;
      }
    }
    else if (channel == levelDownPin)
    {
      var timeLevelDown = new Date();
      if ((timeLevelDown - timeLevel) > bounceTime)
      {
        if (level > 1)
        {
          --level;
        }
        timeLevel = timeLevelDown;
      }
    }
  });
  
  gpio.setup(SensorPin, gpio.DIR_IN, gpio.EDGE_RISING);
  gpio.setup(levelUpPin, gpio.DIR_IN, gpio.EDGE_RISING);
  gpio.setup(levelDownPin, gpio.DIR_IN, gpio.EDGE_RISING);
  
  this.getSpeed = function()
  {
    return rpm;
  };
  this.getLevel = function()
  {
    return level;
  };
  this.getPower = function()
  {
    var lowerVal = 0;
    var upperVal = 0;
    var idxLower = Math.floor(rpm / 10);
    var idxUpper = Math.ceil(rpm / 10);
    if (idxLower > 1)
    {
      lowerVal = power[level - 1][idxLower - 2];
    }
    else
    {
      idxLower = 0;
    }
    if (idxUpper > 1 && idxUpper <= 12) //12
    {
      upperVal = power[level - 1][idxUpper - 2];
    }
    else if (idxUpper > 12) //12
    {
      console.log("RPM(", rpm, ") out of range");
      return power[level - 1][10];
    }
    else if (idxUpper <= 1)
    {
      idxUpper = 0;
    }
    if (idxUpper == 0 && idxLower == 0)
    {
      return 0;
    }
    return (upperVal - lowerVal) / (idxUpper * 10 - idxLower * 10) * (rpm - idxLower * 10) + lowerVal;
  };
};
module.exports.SpeedMeter = SpeedMeter;
