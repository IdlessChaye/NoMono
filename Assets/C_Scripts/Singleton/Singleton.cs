using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Singleton<T> where T : class {
    private static T instance;

    static Singleton() {
        return;
    }

    public static void Create() {
        instance = (T)Activator.CreateInstance(typeof(T), true);
    }

    public static T Instance {
        get {
            return instance;
        }
    }

    public static void Destroy() {
        instance = null;
    }
}
