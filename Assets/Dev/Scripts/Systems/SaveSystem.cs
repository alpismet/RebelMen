using UnityEngine;

namespace Dev.Scripts.Systems
{
	public static class SaveSystem
	{
		//---------------------------------------------------------------------------------
		public static void Load<T>(T data)
		{
			if (!PlayerPrefs.HasKey(data.ToString()))
			{
				Save(data);
				return;
			}

			string dataString = PlayerPrefs.GetString(data.ToString());
			JsonUtility.FromJsonOverwrite(dataString, data);
		}


		//---------------------------------------------------------------------------------
		public static void Save<T>(T data)
		{
			string dataString = JsonUtility.ToJson(data);
			PlayerPrefs.SetString(data.ToString(), dataString);
		}
	}
}
