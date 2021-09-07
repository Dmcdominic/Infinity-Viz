using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Viz Structure/Scene SO")]
public class SceneSO : VizSO {
	public DisplaySO background;
	public List<ItemSO> items;
	public LightingSO lighting;
	public CameraSO camera;
	public PostProcessingSO postProcessing;
}
