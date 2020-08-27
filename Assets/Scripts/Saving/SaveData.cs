﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SaveData {

	private static SaveData _current;
	public static SaveData current{
		get {
			if (_current == null) {
				_current = new SaveData ();
				_current.objects = new List<ObjectData>();
			}
			return _current;
		}
		set
		{
			if(value != null)
			{
				_current = value;
			}
		}
		
	}

	public List<ObjectData> objects;

}