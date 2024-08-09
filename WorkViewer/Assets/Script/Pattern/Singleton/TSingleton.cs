public abstract class TSingleton<T> where T : TSingleton<T>, new()
{
    static T _instance;
    public static bool IsLoad;
    public void Initialize()
    {
        if (!IsLoad)
        {
            T t = Instance;
        }
    }
    protected abstract void OnInitialize();
    public static T Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            _instance = new T();
            _instance.OnInitialize();
            IsLoad = true;
            return _instance;
        }
    }
}
