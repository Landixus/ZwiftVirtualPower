var SpeedMeter = function(SensorPin, levelUpPin, levelDownPin, weighting, pulsesPerRev, timeOut, bounceTime)
{
  var rpm = 0;
  var level = 2;
  var timeAct = 0;
  var timeOld = 0;
  var timeDiff = 0;
  var timeLevel = 0;
  var watchdog = null;
  var gpio = require('rpi-gpio');
  var power = [
    [6,12,20,29,40,53,69,79,92,106,121],
    [8,16,26,38,53,68,88,103,120,138,152],
    [9,20,32,47,66,84,107,125,148,172,186],
    [11,23,39,56,79,101,126,150,173,206,219],
    [13,27,45,65,92,117,145,175,202,238,254],
    [15,31,52,75,105,135,166,202,231,275,289],
    [16,35,58,85,118,152,185,226,260,305,332],
    [18,39,65,96,131,169,208,249,289,333,375],
    [19,42,71,104,144,184,227,272,318,361,408],
    [21,46,77,113,157,199,245,295,345,386,442],
    [23,50,84,123,170,216,262,318,372,413,480],
    [24,53,89,131,183,230,279,342,398,441,512],
    [26,56,94,139,196,245,296,365,424,468,548],
    [28,60,101,148,209,261,318,389,449,494,585],
    [30,64,108,158,222,277,337,415,476,518,620],
    [32,68,115,168,235,296,355,439,503,548,658],
    [33,72,122,177,248,312,373,463,530,576,694],
    [35,76,129,187,261,328,390,484,556,606,727],
    [37,79,134,195,274,342,407,507,572,632,763],
    [39,83,140,204,287,354,424,528,598,659,790],
    [40,87,146,213,300,368,442,551,616,689,812],
    [42,91,153,223,313,385,461,574,645,720,840],
    [44,95,160,234,326,401,479,598,673,752,872],
    [47,101,171,246,340,418,501,625,706,788,908]
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
        if (level < 24)
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
    if (idxUpper > 1 && idxUpper <= 12)
    {
      upperVal = power[level - 1][idxUpper - 2];
    }
    else if (idxUpper > 12)
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

