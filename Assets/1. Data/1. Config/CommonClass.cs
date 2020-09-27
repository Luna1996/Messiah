namespace Messiah.JsonData {
  using System;

  [Serializable]
  public class Effect {
    public int[] trigger; // 触发函数参数
    public int[] effect; // 效果函数参数
  }
}