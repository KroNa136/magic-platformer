using System;

public static class UnityObjectExtensions
{
    public static T Bind<T>(this T obj, Action action) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke();

        return obj;
    }

    public static T Bind<T, TArg>(this T obj, Action<TArg> action, TArg arg) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(arg);

        return obj;
    }

    public static T Bind<T, TArg1, TArg2>(this T obj, Action<TArg1, TArg2> action, TArg1 arg1, TArg2 arg2) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(arg1, arg2);

        return obj;
    }

    public static T Bind<T, TArg1, TArg2, TArg3>(this T obj, Action<TArg1, TArg2, TArg3> action, TArg1 arg1, TArg2 arg2, TArg3 arg3) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(arg1, arg2, arg3);

        return obj;
    }

    public static T Bind<T, TArg1, TArg2, TArg3, TArg4>(this T obj, Action<TArg1, TArg2, TArg3, TArg4> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(arg1, arg2, arg3, arg4);

        return obj;
    }

    public static T Bind<T, TArg1, TArg2, TArg3, TArg4, TArg5>(this T obj, Action<TArg1, TArg2, TArg3, TArg4, TArg5> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(arg1, arg2, arg3, arg4, arg5);

        return obj;
    }

    public static T Bind<T, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(this T obj, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);

        return obj;
    }

    public static T Bind<T, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(this T obj, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);

        return obj;
    }

    public static T Bind<T, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(this T obj, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

        return obj;
    }

    public static T Bind<T>(this T obj, Action<T> action) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(obj);

        return obj;
    }

    public static T Bind<T, TArg>(this T obj, Action<T, TArg> action, TArg arg) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(obj, arg);

        return obj;
    }

    public static T Bind<T, TArg1, TArg2>(this T obj, Action<T, TArg1, TArg2> action, TArg1 arg1, TArg2 arg2) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(obj, arg1, arg2);

        return obj;
    }

    public static T Bind<T, TArg1, TArg2, TArg3>(this T obj, Action<T, TArg1, TArg2, TArg3> action, TArg1 arg1, TArg2 arg2, TArg3 arg3) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(obj, arg1, arg2, arg3);

        return obj;
    }

    public static T Bind<T, TArg1, TArg2, TArg3, TArg4>(this T obj, Action<T, TArg1, TArg2, TArg3, TArg4> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(obj, arg1, arg2, arg3, arg4);

        return obj;
    }

    public static T Bind<T, TArg1, TArg2, TArg3, TArg4, TArg5>(this T obj, Action<T, TArg1, TArg2, TArg3, TArg4, TArg5> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(obj, arg1, arg2, arg3, arg4, arg5);

        return obj;
    }

    public static T Bind<T, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(this T obj, Action<T, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(obj, arg1, arg2, arg3, arg4, arg5, arg6);

        return obj;
    }

    public static T Bind<T, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(this T obj, Action<T, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(obj, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

        return obj;
    }

    public static T Bind<T, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(this T obj, Action<T, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8) where T : UnityEngine.Object
    {
        if (obj != null)
            action?.Invoke(obj, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

        return obj;
    }
}
