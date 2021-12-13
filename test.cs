using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

// Token: 0x020004D7 RID: 1239
public class RidingStatusData
{
	// Token: 0x1700032D RID: 813
	// (get) Token: 0x06001EE4 RID: 7908 RVA: 0x000BE5E9 File Offset: 0x000BC7E9
	public List<float> Powerlst
	{
		get
		{
			return this.powerlst;
		}
	}

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x06001EE5 RID: 7909 RVA: 0x000BE5F1 File Offset: 0x000BC7F1
	public List<float> SpeedList
	{
		get
		{
			return this.speedlst;
		}
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x06001EE6 RID: 7910 RVA: 0x000BE5F9 File Offset: 0x000BC7F9
	public List<float> CadenceList
	{
		get
		{
			return this.cadencelst;
		}
	}

	// Token: 0x17000330 RID: 816
	// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x000BE601 File Offset: 0x000BC801
	public List<float> HeartList
	{
		get
		{
			return this.heartlst;
		}
	}

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06001EE8 RID: 7912 RVA: 0x000BE609 File Offset: 0x000BC809
	public List<float> DistanceList
	{
		get
		{
			return this.distancelst;
		}
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06001EE9 RID: 7913 RVA: 0x000BE611 File Offset: 0x000BC811
	public List<long> TimeList
	{
		get
		{
			return this.timelst;
		}
	}

	// Token: 0x17000333 RID: 819
	// (get) Token: 0x06001EEA RID: 7914 RVA: 0x000BE619 File Offset: 0x000BC819
	public List<int> KeyPointIndexesList
	{
		get
		{
			return this.keyPointIndexes;
		}
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06001EEB RID: 7915 RVA: 0x000BE621 File Offset: 0x000BC821
	public List<int> CoachTimeList
	{
		get
		{
			return this.coachTimelst;
		}
	}

	// Token: 0x06001EEC RID: 7916 RVA: 0x000BE62C File Offset: 0x000BC82C
	public RidingStatusData()
	{
		this.EmptyData();
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06001EED RID: 7917 RVA: 0x000BE68A File Offset: 0x000BC88A
	public float AccumulateDistance
	{
		get
		{
			if (this.distancelst == null || this.distancelst.Count <= 0)
			{
				return 0f;
			}
			return this.distancelst[this.distancelst.Count - 1];
		}
	}

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06001EEE RID: 7918 RVA: 0x000BE6C0 File Offset: 0x000BC8C0
	// (set) Token: 0x06001EEF RID: 7919 RVA: 0x000BE6C7 File Offset: 0x000BC8C7
	public static string[] FtpBikeColorStr
	{
		get
		{
			return RidingStatusData.ftpBikeColorStr;
		}
		set
		{
			RidingStatusData.ftpBikeColorStr = value;
		}
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06001EF0 RID: 7920 RVA: 0x000BE6CF File Offset: 0x000BC8CF
	// (set) Token: 0x06001EF1 RID: 7921 RVA: 0x000BE6D6 File Offset: 0x000BC8D6
	public static string[] FtpSpinColorStr
	{
		get
		{
			return RidingStatusData.ftpSpinColorStr;
		}
		set
		{
			RidingStatusData.ftpSpinColorStr = value;
		}
	}

	// Token: 0x06001EF2 RID: 7922 RVA: 0x000BE6DE File Offset: 0x000BC8DE
	public float GetAvgPowerByType(AvgPowerType type)
	{
		return this.GetAVGPower((int)type);
	}

	// Token: 0x06001EF3 RID: 7923 RVA: 0x000BE6E7 File Offset: 0x000BC8E7
	private float GetAVGPower(int time)
	{
		if (!this.avgPowerDict.ContainsKey(time))
		{
			return -1f;
		}
		return this.avgPowerDict[time];
	}

	// Token: 0x06001EF4 RID: 7924 RVA: 0x000BE70C File Offset: 0x000BC90C
	private void CaculateAvgPower(float power, int time)
	{
		if (!this.avgPowerDict.ContainsKey(time))
		{
			this.avgPowerDict[time] = -1f;
			this.avgPowerSumDict[time] = 0f;
			this.avgPowerHeadDict[time] = -1;
		}
		List<float> list = this.powerlst;
		int count = list.Count;
		Dictionary<int, float> dictionary = this.avgPowerSumDict;
		dictionary[time] += power;
		if (this.avgPowerHeadDict[time] >= 0)
		{
			dictionary = this.avgPowerSumDict;
			dictionary[time] -= list[this.avgPowerHeadDict[time]];
		}
		if (count >= time)
		{
			this.avgPowerHeadDict[time] = count - time;
			float num = this.avgPowerSumDict[time] / (float)time;
			if (num > this.avgPowerDict[time])
			{
				this.avgPowerDict[time] = num;
			}
		}
	}

	// Token: 0x06001EF5 RID: 7925 RVA: 0x000BE7F8 File Offset: 0x000BC9F8
	public float AvgCadence(bool isLapData = false)
	{
		float num = 0f;
		int num2;
		int num3;
		int num4;
		if (isLapData)
		{
			num2 = (this.lapDataDic.ContainsKey(this.lapCount) ? this.lapDataDic[this.lapCount].preListCount : 0);
			num3 = (this.lapDataDic.ContainsKey(this.lapCount) ? this.lapDataDic[this.lapCount].listCount : 0);
			num4 = num3 - num2;
		}
		else
		{
			num2 = 0;
			num3 = this.cadencelst.Count;
			num4 = num3 - num2;
		}
		for (int i = num2; i < num3; i++)
		{
			num += this.cadencelst[i];
		}
		if (num4 > 0)
		{
			num /= (float)num4;
		}
		return num;
	}

	// Token: 0x06001EF6 RID: 7926 RVA: 0x000BE8AC File Offset: 0x000BCAAC
	public float AvgPower(bool isLapData = false)
	{
		if (!this.startCalPower)
		{
			for (int i = 0; i < this.powerlst.Count; i++)
			{
				if (this.powerlst[i] > 0.5f)
				{
					this.startCalPower = true;
					this.startCalIndex = i;
					break;
				}
			}
		}
		if (!this.startCalPower)
		{
			return 0f;
		}
		float num = 0f;
		int num2 = 0;
		int num3;
		int num4;
		int num5;
		if (isLapData)
		{
			num3 = (this.lapDataDic.ContainsKey(this.lapCount) ? this.lapDataDic[this.lapCount].preListCount : 0);
			num4 = (this.lapDataDic.ContainsKey(this.lapCount) ? this.lapDataDic[this.lapCount].listCount : 0);
			num5 = num4 - num3;
		}
		else
		{
			num3 = this.startCalIndex;
			num4 = this.powerlst.Count;
			num5 = num4 - num3;
		}
		for (int j = num3; j < num4; j++)
		{
			float num6 = this.powerlst[j];
			num += num6;
			if (num6 <= 0f)
			{
				num2++;
			}
		}
		if (num5 - num2 > 0)
		{
			num /= (float)num5;
		}
		return num;
	}

	// Token: 0x06001EF7 RID: 7927 RVA: 0x000BE9D4 File Offset: 0x000BCBD4
	public float AvgHeartRate(bool isLapData = false)
	{
		float num = 0f;
		int num2 = 0;
		int num3;
		int num4;
		int num5;
		if (isLapData)
		{
			num3 = (this.lapDataDic.ContainsKey(this.lapCount) ? this.lapDataDic[this.lapCount].preListCount : 0);
			num4 = (this.lapDataDic.ContainsKey(this.lapCount) ? this.lapDataDic[this.lapCount].listCount : 0);
			num5 = num4 - num3;
		}
		else
		{
			num3 = 0;
			num4 = this.heartlst.Count;
			num5 = num4 - num3;
		}
		for (int i = num3; i < num4; i++)
		{
			float num6 = this.heartlst[i];
			num += num6;
			if (num6 <= 0f)
			{
				num2++;
			}
		}
		if (num5 - num2 > 0)
		{
			num /= (float)num5;
		}
		return num;
	}

	// Token: 0x06001EF8 RID: 7928 RVA: 0x000BEAA0 File Offset: 0x000BCCA0
	public float AvgSpeed(bool isLapData = false)
	{
		float num = 0f;
		if (isLapData)
		{
			int num2 = this.lapDataDic.ContainsKey(this.lapCount) ? this.lapDataDic[this.lapCount].preListCount : 0;
			int num3 = this.lapDataDic.ContainsKey(this.lapCount) ? this.lapDataDic[this.lapCount].listCount : 0;
			float num4 = 0f;
			int num5 = num3 - num2;
			for (int i = num2; i < num3; i++)
			{
				float num6 = this.speedlst[i];
				num4 += num6;
			}
			if (num5 > 0)
			{
				num4 /= (float)num5;
			}
			return num4;
		}
		if (this.distancelst.Count > 0)
		{
			num = this.distancelst[this.distancelst.Count - 1];
		}
		float num7 = this.ridingTime;
		if (num7 > 0f && num > 0f)
		{
			return num / num7;
		}
		return 0f;
	}

	// Token: 0x06001EF9 RID: 7929 RVA: 0x000BEBA2 File Offset: 0x000BCDA2
	public float GetCalorie()
	{
		return this.calorie;
	}

	// Token: 0x06001EFA RID: 7930 RVA: 0x000BEBAC File Offset: 0x000BCDAC
	public float GetMusicTrainingCalorie()
	{
		float num = this.GetCalorie();
		if (!Mathf.Approximately(num, 0f))
		{
			return num;
		}
		float num2;
		float num3;
		float num4;
		float num5;
		if (GameController.manager.userInfo.sex == 1)
		{
			num2 = -55.0969f;
			num3 = 0.6309f;
			num4 = 0.1988f;
			num5 = 0.2017f;
		}
		else
		{
			num2 = -20.4022f;
			num3 = 0.4472f;
			num4 = 0.1263f;
			num5 = 0.074f;
		}
		return Mathf.Max((float)((double)(num2 + num3 * BikeManager.manager.statsData.AvgHeartRate(false) + num4 * GameController.manager.userInfo.weight + num5 * (float)GameController.manager.userInfo.age) * BikeManager.manager.GetPlayer().HeartRateTime / 60.0 / 4.185999870300293), 0f);
	}

	// Token: 0x06001EFB RID: 7931 RVA: 0x000BEC80 File Offset: 0x000BCE80
	private float GetCalorieByHeart(float heart)
	{
		float num;
		float num2;
		float num3;
		float num4;
		if (GameController.manager.userInfo.sex == 1)
		{
			num = -55.0969f;
			num2 = 0.6309f;
			num3 = 0.1988f;
			num4 = 0.2017f;
		}
		else
		{
			num = -20.4022f;
			num2 = 0.4472f;
			num3 = -0.1263f;
			num4 = 0.074f;
		}
		return Mathf.Max((num + num2 * heart + num3 * GameController.manager.userInfo.weight + num4 * (float)GameController.manager.userInfo.age) / 60f / 4.186f, 0f);
	}

	// Token: 0x06001EFC RID: 7932 RVA: 0x000BED14 File Offset: 0x000BCF14
	public void AddOne(float power, float speed, float cadence, float heartRate, float acc_dis, float curElevation, long timestamp, bool keyPoint = false, int coachRidingTime = 0)
	{
		this.powerlst.Add(power);
		this.speedlst.Add(speed);
		this.cadencelst.Add(cadence);
		this.heartlst.Add(heartRate);
		this.distancelst.Add(acc_dis);
		this.timelst.Add(timestamp);
		this.coachTimelst.Add(coachRidingTime);
		int num = this.speedlst.Count - 1;
		if (num == 0 && this.speedlst[num] > 0f)
		{
			this.ridingTime = 1f;
		}
		else if (num > 0 && this.speedlst[num] > 0f && this.speedlst[num - 1] > 0f)
		{
			this.ridingTime += (float)(this.timelst[num] - this.timelst[num - 1]) / 1000f;
		}
		if (keyPoint)
		{
			this.keyPointIndexes.Add(this.powerlst.Count - 1);
		}
		this.calorie += this.CalculateInstantCalorie(power, heartRate, cadence);
		this.UpdateMaxData(power, speed, cadence, heartRate);
		this.CalNp(power);
		this.CalculateElevation(curElevation);
		this.CaculateAvgPower(power, 5);
		this.CaculateAvgPower(power, 60);
		this.CaculateAvgPower(power, 240);
		this.CaculateAvgPower(power, 300);
		this.CaculateAvgPower(power, 480);
		this.CaculateAvgPower(power, 1200);
	}

	// Token: 0x06001EFD RID: 7933 RVA: 0x000BEE94 File Offset: 0x000BD094
	private float CalculateInstantCalorie(float power, float heartRate, float cadence)
	{
		if (this.calorieSource == CalorieType.None)
		{
			if (power > 0f)
			{
				this.calorieSource = CalorieType.Power;
			}
			else if (heartRate > 0f)
			{
				this.calorieSource = CalorieType.Heart;
			}
			else if (cadence > 0f)
			{
				this.calorieSource = CalorieType.Cadence;
			}
		}
		float result = 0f;
		if (this.calorieSource == CalorieType.Power)
		{
			result = power / 990f;
		}
		else if (this.calorieSource == CalorieType.Heart)
		{
			result = this.GetCalorieByHeart(heartRate);
		}
		else if (this.calorieSource == CalorieType.Cadence)
		{
			result = this.CalculatePowerByCadence(cadence) / 990f;
		}
		return result;
	}

	// Token: 0x06001EFE RID: 7934 RVA: 0x000BEF20 File Offset: 0x000BD120
	private void CalculateElevation(float curElevation)
	{
		if (this.firstCalElevation)
		{
			this.latestElevation = curElevation;
			this.firstCalElevation = false;
			return;
		}
		if (Mathf.Abs(curElevation - this.latestElevation) > 3.5f)
		{
			if (curElevation > this.latestElevation)
			{
				this.totalElevation += 3.5f;
			}
			else
			{
				this.totalDrop += 3.5f;
			}
			this.latestElevation = curElevation;
		}
	}

	// Token: 0x06001EFF RID: 7935 RVA: 0x000BEF90 File Offset: 0x000BD190
	public void ClearElevation()
	{
		this.firstCalElevation = true;
		this.latestElevation = -1f;
		this.totalElevation = 0f;
	}

	// Token: 0x06001F00 RID: 7936 RVA: 0x000BEFB0 File Offset: 0x000BD1B0
	private float CalculatePowerByCadence(float cadence)
	{
		if (cadence < 40f)
		{
			return 0f;
		}
		float num = Mathf.Clamp(cadence, 40f, 120f);
		return 8f * num / 9.55f;
	}

	// Token: 0x06001F01 RID: 7937 RVA: 0x000BEFEC File Offset: 0x000BD1EC
	private void CalNp(float power)
	{
		if (power > 0.5f)
		{
			this.np_started = true;
		}
		if (!this.np_started)
		{
			return;
		}
		this.powerlst_for_NP.Add(power);
		if (this.powerlst_for_NP.Count < 30)
		{
			return;
		}
		float num = 0f;
		int num2 = this.powerlst_for_NP.Count - 1;
		int i = 30;
		while (i > 0)
		{
			num += this.powerlst_for_NP[num2];
			i--;
			num2--;
		}
		num = Mathf.Pow(num / 30f, 4f);
		int count = this.avgPowerLst.Count;
		this.avgPowerLst.Add(num);
		this.NP4 = (this.NP4 * (float)count + num) / (float)this.avgPowerLst.Count;
		this.NP = Mathf.Pow(this.NP4, 0.25f);
	}

	// Token: 0x06001F02 RID: 7938 RVA: 0x000BF0C0 File Offset: 0x000BD2C0
	public void EmptyData()
	{
		this.powerlst_for_NP = new List<float>();
		this.avgPowerLst = new List<float>();
		this.NP4 = 0f;
		this.NP = 0f;
		this.np_started = false;
		this.powerlst = new List<float>();
		this.speedlst = new List<float>();
		this.cadencelst = new List<float>();
		this.heartlst = new List<float>();
		this.distancelst = new List<float>();
		this.timelst = new List<long>();
		this.keyPointIndexes = new List<int>();
		this.coachTimelst = new List<int>();
		this.maxSpeed = 0f;
		this.maxPower = 0f;
		this.maxCadence = 0f;
		this.maxHeart = 0f;
		this.calorie = 0f;
		this.calorieSource = CalorieType.None;
		this.firstCalElevation = true;
		this.latestElevation = -1f;
		this.totalElevation = 0f;
		this.avgPowerSumDict.Clear();
		this.avgPowerDict.Clear();
		this.avgPowerHeadDict.Clear();
		this.lapCount = 0;
		this.lapDataDic.Clear();
		this.lapMaxCadence = -2.14748365E+09f;
		this.lapMaxHeartRate = -2.14748365E+09f;
		this.lapMaxPower = -2.14748365E+09f;
		this.lapMaxSpeed = -2.14748365E+09f;
	}

	// Token: 0x06001F03 RID: 7939 RVA: 0x000BF214 File Offset: 0x000BD414
	public void StartLap(long timeStamp)
	{
		GameController.manager.trainingMan.stepStart = false;
		this.lapCount++;
		LapData lapData = new LapData();
		lapData.netTime = timeStamp;
		this.lapDataDic[this.lapCount] = lapData;
	}

	// Token: 0x06001F04 RID: 7940 RVA: 0x000BF260 File Offset: 0x000BD460
	public void EndLap(long timeStamp)
	{
		if (this.lapDataDic.ContainsKey(this.lapCount))
		{
			LapData lapData = this.lapDataDic[this.lapCount];
			lapData.listCount = this.powerlst.Count;
			try
			{
				lapData.lapDistance = this.distancelst[this.distancelst.Count - 1];
			}
			catch (Exception)
			{
				lapData.lapDistance = 0f;
			}
			lapData.netTime = timeStamp - lapData.netTime;
			lapData.lapElvation = this.totalElevation;
			lapData.lapRidingTime = this.ridingTime;
			lapData.lapDrop = this.totalDrop;
			lapData.calerie = this.calorie;
			lapData.preListCount = (this.lapDataDic.ContainsKey(this.lapCount - 1) ? this.lapDataDic[this.lapCount - 1].listCount : 0);
			lapData.lapMaxCadence = this.lapMaxCadence;
			lapData.lapMaxHeartRate = this.lapMaxHeartRate;
			lapData.lapMaxPower = this.lapMaxPower;
			lapData.lapMaxSpeed = this.lapMaxSpeed;
			this.lapDataDic[this.lapCount] = lapData;
			this.lapMaxCadence = -2.14748365E+09f;
			this.lapMaxHeartRate = -2.14748365E+09f;
			this.lapMaxPower = -2.14748365E+09f;
			this.lapMaxSpeed = -2.14748365E+09f;
		}
	}

	// Token: 0x06001F05 RID: 7941 RVA: 0x000BF3C8 File Offset: 0x000BD5C8
	public void RestoreEndLap(long timestamp, float distance, float elvation, float ridingTime, float lapDrop, float calerie, float maxCadence, float maxHeartRate, float maxPower, float maxSpeed)
	{
		if (!this.lapDataDic.ContainsKey(this.lapCount))
		{
			return;
		}
		LapData lapData = this.lapDataDic[this.lapCount];
		lapData.listCount = this.powerlst.Count;
		lapData.lapDistance = (this.lapDataDic.ContainsKey(this.lapCount - 1) ? (this.lapDataDic[this.lapCount - 1].lapDistance + distance) : distance);
		lapData.netTime = timestamp - lapData.netTime;
		lapData.lapElvation = (this.lapDataDic.ContainsKey(this.lapCount - 1) ? (this.lapDataDic[this.lapCount - 1].lapElvation + elvation) : elvation);
		lapData.lapRidingTime = (this.lapDataDic.ContainsKey(this.lapCount - 1) ? (this.lapDataDic[this.lapCount - 1].lapRidingTime + ridingTime) : ridingTime);
		lapData.lapDrop = (this.lapDataDic.ContainsKey(this.lapCount - 1) ? (this.lapDataDic[this.lapCount - 1].lapDrop + lapDrop) : (this.lapDataDic.ContainsKey(this.lapCount - 1) ? (this.lapDataDic[this.lapCount - 1].lapRidingTime + ridingTime) : ridingTime));
		lapData.calerie = (this.lapDataDic.ContainsKey(this.lapCount - 1) ? (this.lapDataDic[this.lapCount - 1].calerie + calerie) : calerie);
		lapData.preListCount = (this.lapDataDic.ContainsKey(this.lapCount - 1) ? this.lapDataDic[this.lapCount - 1].listCount : 0);
		lapData.lapMaxCadence = maxCadence;
		lapData.lapMaxHeartRate = maxHeartRate;
		lapData.lapMaxPower = maxPower;
		lapData.lapMaxSpeed = maxSpeed;
		this.lapDataDic[this.lapCount] = lapData;
		this.lapMaxCadence = -2.14748365E+09f;
		this.lapMaxHeartRate = -2.14748365E+09f;
		this.lapMaxPower = -2.14748365E+09f;
		this.lapMaxSpeed = -2.14748365E+09f;
	}

	// Token: 0x06001F06 RID: 7942 RVA: 0x000BF600 File Offset: 0x000BD800
	public void UpdateMaxData(float power, float speed, float cadence, float heartRate)
	{
		this.lapMaxPower = Mathf.Max(power, this.lapMaxPower);
		this.lapMaxSpeed = Mathf.Max(speed, this.lapMaxSpeed);
		this.lapMaxCadence = Mathf.Max(cadence, this.lapMaxCadence);
		this.lapMaxHeartRate = Mathf.Max(heartRate, this.lapMaxHeartRate);
		this.maxPower = Mathf.Max(power, this.maxPower);
		this.maxSpeed = Mathf.Max(speed, this.maxSpeed);
		this.maxCadence = Mathf.Max(cadence, this.maxCadence);
		this.maxHeart = Mathf.Max(heartRate, this.maxHeart);
	}

	// Token: 0x06001F07 RID: 7943 RVA: 0x000BF6A0 File Offset: 0x000BD8A0
	public float LapDrop()
	{
		if (this.lapDataDic.ContainsKey(this.lapCount - 1))
		{
			return this.lapDataDic[this.lapCount].lapDrop - this.lapDataDic[this.lapCount - 1].lapDrop;
		}
		if (!this.lapDataDic.ContainsKey(this.lapCount))
		{
			return 0f;
		}
		return this.lapDataDic[this.lapCount].lapDrop;
	}

	// Token: 0x06001F08 RID: 7944 RVA: 0x000BF721 File Offset: 0x000BD921
	public long LapTotalTime()
	{
		if (!this.lapDataDic.ContainsKey(this.lapCount))
		{
			return 0L;
		}
		return this.lapDataDic[this.lapCount].netTime;
	}

	// Token: 0x06001F09 RID: 7945 RVA: 0x000BF750 File Offset: 0x000BD950
	public float LapAccumulateDistance()
	{
		if (this.lapDataDic.ContainsKey(this.lapCount - 1))
		{
			return this.lapDataDic[this.lapCount].lapDistance - this.lapDataDic[this.lapCount - 1].lapDistance;
		}
		if (this.distancelst.Count < 1)
		{
			return 0f;
		}
		return this.distancelst[this.distancelst.Count - 1];
	}

	// Token: 0x06001F0A RID: 7946 RVA: 0x000BF7D0 File Offset: 0x000BD9D0
	public float LapRidingTime()
	{
		if (!this.lapDataDic.ContainsKey(this.lapCount - 1))
		{
			return this.ridingTime;
		}
		return this.lapDataDic[this.lapCount].lapRidingTime - this.lapDataDic[this.lapCount - 1].lapRidingTime;
	}

	// Token: 0x06001F0B RID: 7947 RVA: 0x000BF828 File Offset: 0x000BDA28
	public float LapCalerie()
	{
		if (!this.lapDataDic.ContainsKey(this.lapCount - 1))
		{
			return this.calorie;
		}
		return this.lapDataDic[this.lapCount].calerie - this.lapDataDic[this.lapCount - 1].calerie;
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x000BF880 File Offset: 0x000BDA80
	public float LapMaxSpeed()
	{
		if (this.lapDataDic.ContainsKey(this.lapCount))
		{
			return this.lapDataDic[this.lapCount].lapMaxSpeed;
		}
		return 0f;
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x000BF8B1 File Offset: 0x000BDAB1
	public float LapMaxCadence()
	{
		if (this.lapDataDic.ContainsKey(this.lapCount))
		{
			return this.lapDataDic[this.lapCount].lapMaxCadence;
		}
		return 0f;
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x000BF8E2 File Offset: 0x000BDAE2
	public float LapMaxPower()
	{
		if (this.lapDataDic.ContainsKey(this.lapCount))
		{
			return this.lapDataDic[this.lapCount].lapMaxPower;
		}
		return 0f;
	}

	// Token: 0x06001F0F RID: 7951 RVA: 0x000BF913 File Offset: 0x000BDB13
	public float LapMaxHeartRate()
	{
		if (this.lapDataDic.ContainsKey(this.lapCount))
		{
			return this.lapDataDic[this.lapCount].lapMaxHeartRate;
		}
		return 0f;
	}

	// Token: 0x06001F10 RID: 7952 RVA: 0x000BF944 File Offset: 0x000BDB44
	public float GetTotalTime()
	{
		if (BikeManager.manager.startRidingTime == 0L)
		{
			return 0f;
		}
		return (float)(NetworkController.manager.GetServerTime(Time.fixedTime) - BikeManager.manager.startRidingTime) / 1000f + BikeManager.manager.stMan.lastRacingTime;
	}

	// Token: 0x06001F11 RID: 7953 RVA: 0x000BF994 File Offset: 0x000BDB94
	public float CheckPowerLimit(float power)
	{
		float num = power;
		if ((GameController.manager.CurMatchType != MatchType.Workout && GameController.manager.CurMatchType != MatchType.Match && GameController.manager.CurMatchType != MatchType.TeamWorkout) || (GameController.manager.CurMatchType == MatchType.Match && GameController.manager.activityCtrlMan.matchStart))
		{
			if (BikeManager.manager.limitChecker.ShouldRecreate)
			{
				BikeManager.manager.limitChecker = new LimitChecker(GameController.manager.userInfo.ability, true);
			}
			BikeManager.manager.limitChecker.CheckState(num);
			if (BikeManager.manager.limitChecker.LimitWKG > 0f && num > GameController.manager.userInfo.weight * BikeManager.manager.limitChecker.LimitWKG)
			{
				num = GameController.manager.userInfo.weight * BikeManager.manager.limitChecker.LimitWKG;
			}
		}
		return num;
	}

	// Token: 0x06001F12 RID: 7954 RVA: 0x000BFA87 File Offset: 0x000BDC87
	public static float GetHeartRateById(long id)
	{
		if (id == 35285L)
		{
			return GameController.manager.trainingMan.avgHeartRateForTest;
		}
		return 0f;
	}

	// Token: 0x06001F13 RID: 7955 RVA: 0x000BFAA8 File Offset: 0x000BDCA8
	public static Color GetHeartRateColor(float r)
	{
		if (GameController.manager.userInfo.mhrMode == HeartRateType.MaxHeartRate)
		{
			if (r <= 0.5f)
			{
				return Util.GetColorFromString(RidingStatusData.heartMHRColorStr[0]);
			}
			if (r <= 0.6f)
			{
				return Util.GetColorFromString(RidingStatusData.heartMHRColorStr[1]);
			}
			if (r <= 0.7f)
			{
				return Util.GetColorFromString(RidingStatusData.heartMHRColorStr[2]);
			}
			if (r <= 0.8f)
			{
				return Util.GetColorFromString(RidingStatusData.heartMHRColorStr[3]);
			}
			if (r <= 0.9f)
			{
				return Util.GetColorFromString(RidingStatusData.heartMHRColorStr[4]);
			}
			return Util.GetColorFromString(RidingStatusData.heartMHRColorStr[5]);
		}
		else
		{
			if (r <= 0.82f)
			{
				return Util.GetColorFromString(RidingStatusData.heartLTHRColorStr[0]);
			}
			if (r <= 0.89f)
			{
				return Util.GetColorFromString(RidingStatusData.heartLTHRColorStr[1]);
			}
			if (r <= 0.94f)
			{
				return Util.GetColorFromString(RidingStatusData.heartLTHRColorStr[2]);
			}
			if (r <= 1f)
			{
				return Util.GetColorFromString(RidingStatusData.heartLTHRColorStr[3]);
			}
			if (r <= 1.03f)
			{
				return Util.GetColorFromString(RidingStatusData.heartLTHRColorStr[4]);
			}
			if (r <= 1.06f)
			{
				return Util.GetColorFromString(RidingStatusData.heartLTHRColorStr[5]);
			}
			return Util.GetColorFromString(RidingStatusData.heartLTHRColorStr[6]);
		}
	}

	// Token: 0x06001F14 RID: 7956 RVA: 0x000BFBC8 File Offset: 0x000BDDC8
	public static Color GetFTPColor(float r)
	{
		if (BLEBridge.manager.connectMode == ConnectMode.Bicycle)
		{
			if (r > 1.51f)
			{
				return Util.GetColorFromString(RidingStatusData.FtpBikeColorStr[6]);
			}
			if (r > 1.21f)
			{
				return Util.GetColorFromString(RidingStatusData.FtpBikeColorStr[5]);
			}
			if (r > 1.06f)
			{
				return Util.GetColorFromString(RidingStatusData.FtpBikeColorStr[4]);
			}
			if (r > 0.91f)
			{
				return Util.GetColorFromString(RidingStatusData.FtpBikeColorStr[3]);
			}
			if (r > 0.76f)
			{
				return Util.GetColorFromString(RidingStatusData.FtpBikeColorStr[2]);
			}
			if (r > 0.56f)
			{
				return Util.GetColorFromString(RidingStatusData.FtpBikeColorStr[1]);
			}
			return Util.GetColorFromString(RidingStatusData.FtpBikeColorStr[0]);
		}
		else
		{
			if (r > 1.06f)
			{
				return Util.GetColorFromString(RidingStatusData.FtpSpinColorStr[8]);
			}
			if (r > 0.99f)
			{
				return Util.GetColorFromString(RidingStatusData.FtpSpinColorStr[7]);
			}
			if (r > 0.91f)
			{
				return Util.GetColorFromString(RidingStatusData.FtpSpinColorStr[6]);
			}
			if (r > 0.84f)
			{
				return Util.GetColorFromString(RidingStatusData.FtpSpinColorStr[5]);
			}
			if (r > 0.76f)
			{
				return Util.GetColorFromString(RidingStatusData.FtpSpinColorStr[4]);
			}
			if (r > 0.66f)
			{
				return Util.GetColorFromString(RidingStatusData.FtpSpinColorStr[3]);
			}
			if (r > 0.56f)
			{
				return Util.GetColorFromString(RidingStatusData.FtpSpinColorStr[2]);
			}
			if (r > 0.45f)
			{
				return Util.GetColorFromString(RidingStatusData.FtpSpinColorStr[1]);
			}
			return Util.GetColorFromString(RidingStatusData.FtpSpinColorStr[0]);
		}
	}

	// Token: 0x06001F15 RID: 7957 RVA: 0x000BFD24 File Offset: 0x000BDF24
	public float CalculatePowerBySpeed(float value)
	{
		float num = GameController.manager.userInfo.BWD / 1000f;
		value = value * num * 3.6f;
		float num2 = value * value;
		float num3 = value * value * value * 9E-05f + num2 * 0.10452f + value * 2.81125f + 0.52773f;
		if (num3 < 5f)
		{
			num3 = 0f;
		}
		return num3;
	}

	// Token: 0x06001F16 RID: 7958 RVA: 0x000BFD8C File Offset: 0x000BDF8C
	public int CheckHeartRate(MBike player, bool checkCadence)
	{
		int result = 0;
		if (checkCadence && BikeManager.manager.GetPlayer().Cadence <= 0f)
		{
			return 0;
		}
		float num = GameController.manager.userInfo.GetHeartRate();
		if (player.HeartRate > num)
		{
			this.heartRateDangerTimer += Time.deltaTime;
		}
		if (player.HeartRate < num * 0.9f)
		{
			this.heartRateSafeTimer += Time.deltaTime;
		}
		if (player.HeartRate > num * 0.8f && player.HeartRate < num)
		{
			this.heartRateMaxTimer += Time.deltaTime;
		}
		else
		{
			this.heartRateMaxTimer = 0f;
		}
		if (this.heartRateDangerTimer >= 10f)
		{
			result = 1;
		}
		if (this.heartRateMaxTimer >= 30f)
		{
			this.heartRateMaxTimer = 0f;
			result = 2;
		}
		if (this.heartRateSafeTimer > 3f)
		{
			this.heartRateSafeTimer = 0f;
			this.heartRateDangerTimer = 0f;
			result = 3;
		}
		return result;
	}

	// Token: 0x04002169 RID: 8553
	private List<float> powerlst;

	// Token: 0x0400216A RID: 8554
	private List<float> speedlst;

	// Token: 0x0400216B RID: 8555
	private List<float> cadencelst;

	// Token: 0x0400216C RID: 8556
	private List<float> heartlst;

	// Token: 0x0400216D RID: 8557
	private List<float> distancelst;

	// Token: 0x0400216E RID: 8558
	private List<long> timelst;

	// Token: 0x0400216F RID: 8559
	private List<int> keyPointIndexes;

	// Token: 0x04002170 RID: 8560
	private List<int> coachTimelst;

	// Token: 0x04002171 RID: 8561
	public float maxSpeed;

	// Token: 0x04002172 RID: 8562
	public float maxPower;

	// Token: 0x04002173 RID: 8563
	public float maxCadence;

	// Token: 0x04002174 RID: 8564
	public float maxHeart;

	// Token: 0x04002175 RID: 8565
	public float ridingTime;

	// Token: 0x04002176 RID: 8566
	public bool firstCalElevation = true;

	// Token: 0x04002177 RID: 8567
	public float latestElevation = -1f;

	// Token: 0x04002178 RID: 8568
	public float totalElevation;

	// Token: 0x04002179 RID: 8569
	public float totalDrop;

	// Token: 0x0400217A RID: 8570
	private Dictionary<int, float> avgPowerSumDict = new Dictionary<int, float>();

	// Token: 0x0400217B RID: 8571
	private Dictionary<int, float> avgPowerDict = new Dictionary<int, float>();

	// Token: 0x0400217C RID: 8572
	private Dictionary<int, int> avgPowerHeadDict = new Dictionary<int, int>();

	// Token: 0x0400217D RID: 8573
	private List<float> powerlst_for_NP;

	// Token: 0x0400217E RID: 8574
	private List<float> avgPowerLst;

	// Token: 0x0400217F RID: 8575
	private float NP4;

	// Token: 0x04002180 RID: 8576
	public float NP;

	// Token: 0x04002181 RID: 8577
	private bool np_started;

	// Token: 0x04002182 RID: 8578
	public int lapCount;

	// Token: 0x04002183 RID: 8579
	public Dictionary<int, LapData> lapDataDic = new Dictionary<int, LapData>();

	// Token: 0x04002184 RID: 8580
	public float lapMaxPower;

	// Token: 0x04002185 RID: 8581
	public float lapMaxSpeed;

	// Token: 0x04002186 RID: 8582
	private float lapMaxCadence;

	// Token: 0x04002187 RID: 8583
	private float lapMaxHeartRate;

	// Token: 0x04002188 RID: 8584
	private bool startCalPower;

	// Token: 0x04002189 RID: 8585
	private int startCalIndex = -1;

	// Token: 0x0400218A RID: 8586
	public CalorieType calorieSource;

	// Token: 0x0400218B RID: 8587
	public float calorie;

	// Token: 0x0400218C RID: 8588
	private static string[] heartMHRColorStr = new string[]
	{
		"#CCCCCCFF",
		"#999999FF",
		"#3EBFFFFF",
		"#86E638FF",
		"#FFCD00FF",
		"#F84E3DFF"
	};

	// Token: 0x0400218D RID: 8589
	private static string[] heartLTHRColorStr = new string[]
	{
		"#999999FF",
		"#3EBFFFFF",
		"#86E638FF",
		"#FFCD00FF",
		"#FF8A1AFF",
		"#F84E3DFF",
		"#BA2FCAFF"
	};

	// Token: 0x0400218E RID: 8590
	private static string[] ftpBikeColorStr = new string[]
	{
		"#999999FF",
		"#3EBFFFFF",
		"#86E638FF",
		"#FFCD00FF",
		"#FF8A1AFF",
		"#F84E3DFF",
		"#BA2FCAFF"
	};

	// Token: 0x0400218F RID: 8591
	private static string[] ftpSpinColorStr = new string[]
	{
		"#999999FF",
		"#3EBFFFFF",
		"#3E78FFFF",
		"#86E638FF",
		"#2FC22AFF",
		"#FFCD00FF",
		"#FF8A1AFF",
		"#F84E3DFF",
		"#BA2FCAFF"
	};

	// Token: 0x04002190 RID: 8592
	private float heartRateDangerTimer;

	// Token: 0x04002191 RID: 8593
	private float heartRateSafeTimer;

	// Token: 0x04002192 RID: 8594
	private float heartRateMaxTimer;
}
