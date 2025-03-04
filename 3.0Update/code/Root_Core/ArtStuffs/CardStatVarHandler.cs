using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RootCore.ArtStuffs {
    public class CardStatVarHandler: MonoBehaviour {

		public static Dictionary<string, Func<GameObject,string>> VarMap = new Dictionary<string, Func<GameObject, string>>();



        public void Start() {
			foreach (var key in VarMap.Keys) {
				gameObject.GetComponentsInChildren<TextMeshProUGUI>().ToList().ForEach(text => {
					if(text.text.Contains($"%{key}%"))
					text.text = text.text.Replace($"%{key}%", VarMap[key](gameObject));
                });
			}
		}
		

	
	}
}
