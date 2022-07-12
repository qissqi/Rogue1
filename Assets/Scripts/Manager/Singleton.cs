using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:Singleton<T>
{
	private static T instance;
	public static T Instance
	{
		get { return instance; }
	}

    

    //����Ϊ���������Թ��죬���ڱ�������

    protected virtual void Awake()
	{
		
		if (instance != null)
			Destroy(gameObject);
		else
			instance = (T)this;
	}
	//�˷���������ʱΪ��Ӧ�Ľű������䵥��

	protected virtual void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}
	//��gameobject������ʱ�����䵥��
}
