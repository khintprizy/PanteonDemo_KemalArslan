using UnityEngine;

public class SingletonMaker<T> : MonoBehaviour where T : SingletonMaker<T> {

	protected static SingletonMaker<T> mInstance {
		get {
			if(!_mInstance)
			{
				T [] managers = Object.FindSceneObjectsOfType(typeof(T)) as T[];
				if(managers.Length != 0)
				{
					if(managers.Length == 1)
					{
						_mInstance = managers[0];
						return _mInstance;
					} else {
						Debug.LogError("You have more than one " + typeof(T).Name + " in the scene. You only need 1, it's a singleton!");
						foreach(T manager in managers)
						{
							Destroy(manager.gameObject);
						}
					}
				}
			}
			return _mInstance;
		} set {
			_mInstance = value as T;
		}
	}
	private static T _mInstance;
    public static T Instance
    {
        get
        {
            return ((T)mInstance);
        }
        set
        {
            mInstance = value;
        }
    }
}
