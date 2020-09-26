using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test {
  public class BasicTest {
    [Test]
    public unsafe void BinaryCast() {
      int a = 101;
      int* pa = &a;
      float* pa_f = (float*)pa;
      int* pa_i = (int*)pa_f;
      Debug.Log(*pa_f);
      Debug.Log(*pa_i);
    }

    [Test]
    public void RemoveEmptyDictionary() {
      object a = new Dictionary<int, int>();
      Dictionary<int, int> t;
      for (int i = 0; i < 10000; i++)
        t = (Dictionary<int, int>)a;

    }

    [Test]
    public void RemoveEmptyDictionary02() {
      Dictionary<int, int> a = new Dictionary<int, int>();
      Dictionary<int, int> t;
      for (int i = 0; i < 10000; i++)
        t = (Dictionary<int, int>)a;

    }
  }
}
