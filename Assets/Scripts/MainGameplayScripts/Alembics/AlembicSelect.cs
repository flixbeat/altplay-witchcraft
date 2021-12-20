using UnityEngine;

[System.Serializable]
public partial class AlembicSelect
{
	private static AlembicsInstanceCheck _mgr;
	public static AlembicsInstanceCheck mgr{
		get{
			if(!_mgr) _mgr = AlembicsInstanceCheck.Instance;
			return _mgr;
		}
	}
	
	public int index;
	
	public GameObject GetReference(){
		return mgr.GetReference(index);
	}
	
	public bool IsAlembic(){
		return mgr.IsAlembic(index);
	}
}